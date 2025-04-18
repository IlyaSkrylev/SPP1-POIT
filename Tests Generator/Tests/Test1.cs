using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestClassGenerator
{
    [TestClass]
    public class TestGeneratorTests
    {
        private readonly string _inputDirectory = @"D:\Works\6 semestr\SPP\SPP1-POIT\Tests Generator\Test Classes\Classes";
        private string _outputDirectory;

        [TestInitialize]
        public void TestInitialize()
        {
            _outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "OutputFiles");
            Directory.CreateDirectory(_outputDirectory);
        }

        [TestMethod]
        public async Task GenerateTestsAsync_CreatesTestFiles()
        {
            var inputFiles = Directory.GetFiles(_inputDirectory, "*.cs");
            Assert.IsTrue(inputFiles.Length > 0, "Исходные файлы не найдены в указанной папке.");

            var generator = new TestGenerator();

            await generator.GenerateTestsAsync(inputFiles, _outputDirectory);

            var generatedFiles = Directory.GetFiles(_outputDirectory, "*Tests.cs");
            Assert.IsTrue(generatedFiles.Length > 0, "В папке вывода не созданы тестовые файлы.");

            foreach (var file in generatedFiles)
            {
                string content = File.ReadAllText(file);
                Assert.IsTrue(content.Contains("[TestClass]"),
                    $"Файл {Path.GetFileName(file)} не содержит атрибут [TestClass].");
            }
        }


        [TestMethod]
        public async Task GenerateTestsAsync_NoInputFiles_DoesNotThrow()
        {
            var inputFiles = new string[0];
            var generator = new TestGenerator();

            await generator.GenerateTestsAsync(inputFiles, _outputDirectory);

            var generatedFiles = Directory.GetFiles(_outputDirectory, "*Tests.cs");
            Assert.AreEqual(0, generatedFiles.Length, "Не должно быть создано тестовых файлов.");
        }
    }
}