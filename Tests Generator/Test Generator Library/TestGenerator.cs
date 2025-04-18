using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks.Dataflow;
using System.Reflection;

namespace TestClassGenerator
{
    public class TestGenerator
    {
        private const int MaxConcurrentFileLoads = 5;
        private const int MaxConcurrentTestGenerations = 10;
        private const int MaxConcurrentFileWrites = 5;

        public async Task GenerateTestsAsync(IEnumerable<string> inputFiles, string outputDirectory)
        {
            var writeBlock = new ActionBlock<(string testClass, string className)>(async tuple =>
            {
                var (testClass, className) = tuple;
                var outputFile = Path.Combine(outputDirectory, $"{className}Tests.cs");
                await Task.Run(() => File.WriteAllText(outputFile, testClass));
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = MaxConcurrentFileWrites });

            var generateBlock = new ActionBlock<ClassDeclarationSyntax>(async classDeclaration =>
            {
                var testClass = GenerateTestClass(classDeclaration);
                await writeBlock.SendAsync((testClass, classDeclaration.Identifier.Text));
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = MaxConcurrentTestGenerations });

            var loadBlock = new ActionBlock<string>(async filePath =>
            {
                var code = await Task.Run(() => File.ReadAllText(filePath));
                var compilationUnit = CSharpSyntaxTree.ParseText(code).GetCompilationUnitRoot();
                var classes = compilationUnit.DescendantNodes().OfType<ClassDeclarationSyntax>();
                foreach (var classDeclaration in classes)
                {
                    await generateBlock.SendAsync(classDeclaration);
                }
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = MaxConcurrentFileLoads });

            foreach (var file in inputFiles)
            {
                await loadBlock.SendAsync(file);
            }

            loadBlock.Complete();
            await loadBlock.Completion;

            generateBlock.Complete();
            await generateBlock.Completion;

            writeBlock.Complete();
            await writeBlock.Completion;
        }

        private string GenerateTestClass(ClassDeclarationSyntax classDeclaration)
        {
            var className = classDeclaration.Identifier.Text;
            var methods = classDeclaration.Members
                .OfType<MethodDeclarationSyntax>()
                .Where(m => m.Modifiers.Any(SyntaxKind.PublicKeyword))
                .ToList();

            var namespaceDeclaration = classDeclaration.FirstAncestorOrSelf<NamespaceDeclarationSyntax>();
            var namespaceName = namespaceDeclaration?.Name.ToString() ?? "GeneratedTests";

            var constructor = classDeclaration.Members
                .OfType<ConstructorDeclarationSyntax>()
                .FirstOrDefault();

            var dependencies = constructor?.ParameterList.Parameters
                .Where(p => p.Type is IdentifierNameSyntax id && id.Identifier.Text.StartsWith("I"))
                .ToList() ?? new List<ParameterSyntax>();

            var simpleParameters = constructor?.ParameterList.Parameters
                .Where(p => !(p.Type is IdentifierNameSyntax id && id.Identifier.Text.StartsWith("I")))
                .ToList() ?? new List<ParameterSyntax>();

            var dependencyFields = string.Join(Environment.NewLine, dependencies.Select(d =>
                $"private Mock<{d.Type}> _{CamelCase(d.Identifier.Text)}Mock;"));

            var parameterFields = string.Join(Environment.NewLine, simpleParameters.Select((p, index) =>
                $"{(index == 0 ? "" : "        ")}private {p.Type} _{CamelCase(p.Identifier.Text)};"));

            var dependencySetups = string.Join(Environment.NewLine + "        ",
                dependencies.Select(d =>
                    $"_{CamelCase(d.Identifier.Text)}Mock = new Mock<{d.Type}>();"));

            var parameterSetups = string.Join(Environment.NewLine + "        ",
                simpleParameters.Select((p, index) =>
                    $"{(index == 0 ? "" : "    ")}_{CamelCase(p.Identifier.Text)} = {GetDefaultValueExpression(p.Type)};"));

            var constructorParams = new List<string>();
            constructorParams.AddRange(simpleParameters.Select(p => $"_{CamelCase(p.Identifier.Text)}"));
            constructorParams.AddRange(dependencies.Select(d => $"_{CamelCase(d.Identifier.Text)}Mock.Object"));

            var constructorParamsString = string.Join(", ", constructorParams);

            var initializeMethod = $@"
        [TestInitialize]
        public void Initialize()
        {{  
            {parameterSetups}
            {dependencySetups}
            _{CamelCase(className)}UnderTest = new {className}({constructorParamsString});
        }}";

            var classUnderTestField = $"private {className} _{CamelCase(className)}UnderTest;";

            var testMethods = string.Join(Environment.NewLine, methods.Select(GenerateTestMethod));

            var testClassBuilder = $@"
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using {namespaceName};

namespace {namespaceName}.Tests
{{
    [TestClass]
    public class {className}Tests
    {{
        {classUnderTestField}
        {dependencyFields}
        {parameterFields}
        {initializeMethod}

        {testMethods}
    }}
}}";

            return testClassBuilder;
        }

        private string GenerateTestMethod(MethodDeclarationSyntax method)
        {
            var methodName = method.Identifier.Text;
            var overloadIndex = GetMethodOverloadIndex(method);

            var parameters = method.ParameterList.Parameters;
            var arrangeSection = string.Join(Environment.NewLine + "            ",
                parameters.Select(p => $"{p.Type} {p.Identifier.Text} = {GetDefaultValueExpression(p.Type)};"));

            var methodCallArgs = string.Join(", ", parameters.Select(p => p.Identifier.Text));

            string actSection;
            string assertSection;

            var parentClass = method.Parent as ClassDeclarationSyntax;
            var classNameCamelCase = parentClass != null
                ? CamelCase(parentClass.Identifier.Text)
                : "classUnderTest";

            if (method.ReturnType.ToString() == "void")
            {
                actSection = $"_{classNameCamelCase}UnderTest.{methodName}({methodCallArgs});";
                assertSection = "Assert.Fail(\"autogenerated\");";
            }
            else
            {
                actSection = $"{method.ReturnType} actual = _{classNameCamelCase}UnderTest.{methodName}({methodCallArgs});";
                assertSection = $"{method.ReturnType} expected = {GetDefaultValueExpression(method.ReturnType)};\n            Assert.AreEqual(expected, actual);\n            Assert.Fail(\"autogenerated\");";
            }

            return $@"
        [TestMethod]
        public void {methodName}{overloadIndex}Test()
        {{
            {(arrangeSection.Count() == 0 ? "" : arrangeSection + "\n\n            ")}{actSection}

            {assertSection}
        }}";
        }

        private string CamelCase(string identifier)
        {
            return char.ToLowerInvariant(identifier[0]) + identifier.Substring(1);
        }

        private string GetDefaultValueExpression(TypeSyntax type)
        {
            var typeName = type.ToString();

            switch (typeName)
            {
                case "int":
                case "long":
                case "short":
                case "byte":
                case "float":
                case "double":
                case "decimal":
                    return "0";
                case "string":
                    return "\"\"";
                case "bool":
                    return "false";
                default:
                    return "null";
            }
        }

        private int GetMethodOverloadIndex(MethodDeclarationSyntax method)
        {
            var methodsWithSameName = method.Parent.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Where(m => m.Identifier.Text == method.Identifier.Text)
                .ToList();
            var index = methodsWithSameName.IndexOf(method);
            return index >= 0 ? index + 1 : 0;
        }
    }
}