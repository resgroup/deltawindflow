using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace RES.CSVReader.Test
{
    [TestFixture]
    public class StringSplitterTest
    {
        [Test]
        public void BasicTest()
        {
            string test = "Test1,Test2,Test3,Test4";
            StringSplitter splitter = new StringSplitter(',', '"');
            List<string> result = splitter.Split(test, true);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("Test1", result[0]);
            Assert.AreEqual("Test2", result[1]);
            Assert.AreEqual("Test3", result[2]);
            Assert.AreEqual("Test4", result[3]);
        }

        [Test]
        public void Spaces()
        {
            string test = "Test1, Test2, Test3, Test4";
            StringSplitter splitter = new StringSplitter(',', '"', true);
            List<string> result = splitter.Split(test, true);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("Test1", result[0]);
            Assert.AreEqual("Test2", result[1]);
            Assert.AreEqual("Test3", result[2]);
            Assert.AreEqual("Test4", result[3]);
        }

        [Test]
        public void QuotesAndSpaces()
        {
            string test = "Test1, \" Test2 \",Test3,Test4";
            StringSplitter splitter = new StringSplitter(',', '"', true);
            List<string> result = splitter.Split(test, true);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("Test1", result[0]);
            Assert.AreEqual(" Test2 ", result[1]);
            Assert.AreEqual("Test3", result[2]);
            Assert.AreEqual("Test4", result[3]);
        }

        [Test]
        public void WithQuotes()
        {
            string test = "Test1,\"Test2\",Test3,Test4";
            StringSplitter splitter = new StringSplitter(',', '"');
            List<string> result = splitter.Split(test, true);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("Test1", result[0]);
            Assert.AreEqual("Test2", result[1]);
            Assert.AreEqual("Test3", result[2]);
            Assert.AreEqual("Test4", result[3]);
        }

        [Test]
        public void WithQuotesAroundComma()
        {
            string test = "Test1,\"Test2,Test3\",Test4";
            StringSplitter splitter = new StringSplitter(',', '"');
            List<string> result = splitter.Split(test, true);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Test1", result[0]);
            Assert.AreEqual("Test2,Test3", result[1]);
            Assert.AreEqual("Test4", result[2]);
        }

        [Test]
        public void WithQuotesAtStart()
        {
            string test = "\"Test1,Test2\",Test3,Test4";
            StringSplitter splitter = new StringSplitter(',', '"');
            List<string> result = splitter.Split(test, true);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Test1,Test2", result[0]);
            Assert.AreEqual("Test3", result[1]);
            Assert.AreEqual("Test4", result[2]);
        }

        [Test]
        public void WithQuotesAtEnd()
        {
            string test = "Test1,Test2,\"Test3,Test4\"";
            StringSplitter splitter = new StringSplitter(',', '"');
            List<string> result = splitter.Split(test, true);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Test1", result[0]);
            Assert.AreEqual("Test2", result[1]);
            Assert.AreEqual("Test3,Test4", result[2]);
        }

        [Test]
        public void TwoTypesOfSplitter()
        {
            string test1 = "Test1,Test2,\"Test3,Test4\"";
            string test2 = "Test1;Test2;\"Test3;Test4\"";
            StringSplitter splitter = new StringSplitter(new char[] { ',', ';' }, '"');
            List<string> result = splitter.Split(test1, true);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Test1", result[0]);
            Assert.AreEqual("Test2", result[1]);
            Assert.AreEqual("Test3,Test4", result[2]);
            result = splitter.Split(test2, true);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Test1", result[0]);
            Assert.AreEqual("Test2", result[1]);
            Assert.AreEqual("Test3;Test4", result[2]);
        }

        [Test]
        public void TrimTest()
        {
            string test = "Test1,  Test2  ,\"Test3,Test4\"";
            StringSplitter splitter = new StringSplitter(',', '"', true);
            StringSplitter splitter2 = new StringSplitter(',', '"', false);
            List<string> result = splitter.Split(test, true);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Test1", result[0]);
            Assert.AreEqual("Test2", result[1]);
            Assert.AreEqual("Test3,Test4", result[2]);
            result = splitter2.Split(test, true);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Test1", result[0]);
            Assert.AreEqual("  Test2  ", result[1]);
            Assert.AreEqual("Test3,Test4", result[2]);
        }
    }
}
