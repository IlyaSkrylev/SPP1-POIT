using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringFormatting; 
using System;
using System.Collections.Generic; 

namespace StringFormatting.Tests 
{
    public class User
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string[] Orders { get; }
        public List<string> Tags { get; } 

        public User(string fn, string ln, string[] orders = null, List<string> tags = null)
        {
            FirstName = fn;
            LastName = ln;
            Orders = orders ?? Array.Empty<string>();
            Tags = tags ?? new List<string>();
        }

    }

    public class TestDataWithField
    {
        public string PublicField = "InitialFieldValue";
        public readonly int ReadOnlyField = 123; 
    }

    public struct Point
    {
        public int X { get; }
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public override string ToString() => $"({X}, {Y})";
    }


    [TestClass]
    public class StringFormatterTests
    {
        private readonly IStringFormatter _formatter = StringFormatter.Shared;


        [TestMethod]
        public void Format_WithValidProperties_ReturnsFormattedString()
        {
            var user = new User("John", "Doe");
            var template = "Hello, {FirstName} {LastName}!";
            var expected = "Hello, John Doe!";

            var result = _formatter.Format(template, user);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Format_WithValidFields_ReturnsFormattedString()
        {
            var data = new TestDataWithField { PublicField = "TestingFields" };
            var template = "Field Value: {PublicField}, ReadOnly: {ReadOnlyField}";
            var expected = "Field Value: TestingFields, ReadOnly: 123";

            var result = _formatter.Format(template, data);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Format_WithStruct_ReturnsFormattedString()
        {
            var point = new Point(10, 20);
            var template = "Point: ({X}, {Y})"; 
            var expected = "Point: (10, 20)";

            var result = _formatter.Format(template, point);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Format_TemplateWithoutPlaceholders_ReturnsTemplateItself()
        {
            var user = new User("Test", "User");
            var template = "This is a literal string.";
            var expected = "This is a literal string.";

            var result = _formatter.Format(template, user);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Format_EmptyTemplate_ReturnsEmptyString()
        {
            var user = new User("Test", "User");
            var template = "";
            var expected = "";

            var result = _formatter.Format(template, user);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Format_WithEscapedBraces_ReturnsStringWithLiteralBraces()
        {
            var user = new User("Test", "User");
            var template = "This {{is}} not a {{placeholder}}, but this {FirstName} is.";
            var expected = "This {is} not a {placeholder}, but this Test is.";

            var result = _formatter.Format(template, user);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Format_WithArrayIndexer_ReturnsCorrectElement()
        {
            var user = new User("Index", "User", orders: new[] { "OrderA", "OrderB", "OrderC" });
            var template = "First: {Orders[0]}, Third: {Orders[2]}";
            var expected = "First: OrderA, Third: OrderC";

            var result = _formatter.Format(template, user);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Format_WithListIndexer_ReturnsCorrectElement()
        {
            var user = new User("List", "User", tags: new List<string> { "tag1", "tag2" });
            var template = "Tag 1: {Tags[0]}, Tag 2: {Tags[1]}";
            var expected = "Tag 1: tag1, Tag 2: tag2";

            var result = _formatter.Format(template, user);

            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void Format_WithNullPropertyValue_ThrowsNullReferenceException()
        {
            var user = new User("NullTest", null); 
            var template = "Hello, {FirstName} {LastName}";

            Assert.ThrowsException<NullReferenceException>(() => _formatter.Format(template, user));
        }


        [TestMethod]
        public void Format_NullTemplate_ThrowsArgumentNullException()
        {
            var user = new User("Test", "User");
            Assert.ThrowsException<ArgumentNullException>(() => _formatter.Format(null, user));
        }

        [TestMethod]
        public void Format_NullTarget_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _formatter.Format("template", null));
        }

        [TestMethod]
        public void Format_UnclosedPlaceholder_ThrowsFormatException()
        {
            var user = new User("Test", "User");
            var template = "Hello, {FirstName";
            Assert.ThrowsException<FormatException>(() => _formatter.Format(template, user));
        }

        [TestMethod]
        public void Format_UnexpectedClosingBrace_ThrowsFormatException()
        {
            var user = new User("Test", "User");
            var template = "Hello, FirstName}";
            Assert.ThrowsException<FormatException>(() => _formatter.Format(template, user));
        }

        [TestMethod]
        public void Format_EmptyPlaceholder_ThrowsFormatException()
        {
            var user = new User("Test", "User");
            var template = "Hello, {}";
            Assert.ThrowsException<FormatException>(() => _formatter.Format(template, user));
        }

        [TestMethod]
        public void Format_WhitespacePlaceholder_ThrowsFormatException()
        {
            var user = new User("Test", "User");
            var template = "Hello, {        }";
            Assert.ThrowsException<FormatException>(() => _formatter.Format(template, user));
        }

        [TestMethod]
        public void Format_MissingMember_ThrowsMissingMemberException()
        {
            var user = new User("Test", "User");
            var template = "Hello, {NonExistentProperty}";
            Assert.ThrowsException<MissingMemberException>(() => _formatter.Format(template, user));
        }

        [TestMethod]
        public void Format_MissingMemberForIndexer_ThrowsMissingMemberException()
        {
            var user = new User("Test", "User");
            var template = "Hello, {NonExistentProperty[0]}";
            Assert.ThrowsException<MissingMemberException>(() => _formatter.Format(template, user));
        }

        [TestMethod]
        public void Format_InvalidIndexerSyntax_MissingClosingBracket_ThrowsFormatException()
        {
            var user = new User("Test", "User", orders: new[] { "a" });
            var template = "Order: {Orders[0";
            Assert.ThrowsException<FormatException>(() => _formatter.Format(template, user));
        }

        [TestMethod]
        public void Format_InvalidIndexerSyntax_NonNumericIndex_ThrowsFormatException()
        {
            var user = new User("Test", "User", orders: new[] { "a" });
            var template = "Order: {Orders[abc]}";
            Assert.ThrowsException<FormatException>(() => _formatter.Format(template, user));
        }

        [TestMethod]
        public void Format_ArrayIndexer_IndexOutOfRange_ThrowsIndexOutOfRangeException()
        {
            var user = new User("Test", "User", orders: new[] { "one" });
            var template = "Order: {Orders[1]}"; 

            Assert.ThrowsException<IndexOutOfRangeException>(() => _formatter.Format(template, user));
        }
    }
}