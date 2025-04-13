using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Faker.Tests
{

    [TestClass]
    public class FakerTests
    {
        [TestMethod]
        public void Create_Int_ReturnsRandomInt()
        {
            var _faker = new Faker.Faker();
            var result = _faker.Create<int>();
            Assert.IsInstanceOfType(result, typeof(int));
        }

        [TestMethod]
        public void Create_Double_ReturnsRandomDouble()
        {
            var _faker = new Faker.Faker();
            var result = _faker.Create<double>();
            Assert.IsInstanceOfType<double>(result);
        }

        [TestMethod]
        public void Create_String_ReturnsRandomString()
        {
            var _faker = new Faker.Faker();
            var result = _faker.Create<string>();
            Assert.IsInstanceOfType<string>(result);
            Assert.IsTrue(result.Length >= 5 && result.Length <= 100);
        }

        [TestMethod]
        public void Create_DateTime_ReturnsRandomDateTime()
        {
            var _faker = new Faker.Faker();
            var result = _faker.Create<DateTime>();
            Assert.IsInstanceOfType<DateTime>(result);
            Assert.IsTrue(result >= new DateTime(1970, 1, 1) && result <= DateTime.Today);
        }

        [TestMethod]
        public void Create_User_FillsProperties()
        {
            var _faker = new Faker.Faker();
            var user = _faker.Create<User>();
            Assert.IsNotNull(user);
            Assert.IsNotNull(user.Name);
            Assert.IsNotNull(user.Age);
        }

        [TestMethod]
        public void Create_ListOfUsers_ReturnsNonEmptyList()
        {
            var _faker = new Faker.Faker();
            var users = _faker.Create<List<User>>();
            Assert.IsNotNull(users);
            Assert.IsTrue(users.Count > 0);
            foreach (var user in users)
            {
                Assert.IsNotNull(user.Name);
                Assert.IsNotNull(user.Age);
            }
        }

        [TestMethod]
        public void Create_ListOfLists_ReturnsNestedLists()
        {
            var _faker = new Faker.Faker();
            var lists = _faker.Create<List<List<User>>>();
            Assert.IsNotNull(lists);
            Assert.IsTrue(lists.Count > 0);
            foreach (var list in lists)
            {
                Assert.IsTrue(list.Count > 0);
            }
        }

        [TestMethod]
        public void Create_CyclicDependency()
        {
            var _faker = new Faker.Faker();
            var a = _faker.Create<A>();
            Assert.IsNotNull(a);
            Assert.IsNotNull(a.B);
            Assert.IsNotNull(a.B.C);
        }

        [TestMethod]
        public void Create_ClassWithMultipleConstructors()
        {
            var _faker = new Faker.Faker();
            var obj = _faker.Create<ClassWithMultipleConstructors>();
            Assert.IsNotNull(obj);
            Assert.IsNotNull(obj.Name);
            Assert.IsNotNull(obj.Age);
            Assert.IsTrue(obj.GetFlag()); 
        }
    }


    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class A
    {
        public B B { get; set; }
    }

    public class B
    {
        public C C { get; set; }
    }

    public class C
    {
        public A A { get; set; }
    }

    public class ClassWithPrivateConstructor
    {
        private ClassWithPrivateConstructor() { }
    }

    public class ClassWithMultipleConstructors
    {
        public string Name { get; set; }
        public int Age { get; set; }
        private bool InitializedWithTwoParameters;

        public ClassWithMultipleConstructors()
        {
            InitializedWithTwoParameters = false;
        }

        public ClassWithMultipleConstructors(string name, int age)
        {
            Name = name;
            Age = age;
            InitializedWithTwoParameters = true;
        }

        public ClassWithMultipleConstructors(string name)
        {
            Name = name;
            InitializedWithTwoParameters = false;
        }

        public bool GetFlag()
        {
            return InitializedWithTwoParameters;
        }
    }
}