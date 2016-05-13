using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;

using RES.CSVReader;

namespace RES.CSVReader.Test
{
    public struct StringWrapper
    {
        public string Value;
        public StringWrapper(string value)
        {
            Value = value;
        }
    }

    public class TrivialCustomDataParser
        : ICustomDataParser<StringWrapper>
    {
        public StringWrapper Parse(string value)
        {
            return new StringWrapper(value);
        }
    }


    [TestFixture]
    public class CSVReaderCustomParserTest
    {
        [Test]
        public void CustomParsingTest()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            writer.WriteLine("Column1,Column2,Column3");
            writer.WriteLine("Data1-1,Data2-1,Data3-1");
            writer.WriteLine("Data1-2,Data2-2,Data3-2");
            writer.WriteLine("Data1-3,Data2-3,Data3-3");

            stream.Position = 0;

            CSVReader reader = new CSVReader(stream);

            Assert.AreEqual(3, reader.RowCount);
            Assert.AreEqual(3, reader.Headers.Length);

            List<StringWrapper> list = reader.GetCustomTypeList(new TrivialCustomDataParser(), 0);

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("Data1-1", list[0].Value);
            Assert.AreEqual("Data1-2", list[1].Value);
            Assert.AreEqual("Data1-3", list[2].Value);

            list = reader.GetCustomTypeList(new TrivialCustomDataParser(), "Column2");

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("Data2-1", list[0].Value);
            Assert.AreEqual("Data2-2", list[1].Value);
            Assert.AreEqual("Data2-3", list[2].Value);
        }
    }
}