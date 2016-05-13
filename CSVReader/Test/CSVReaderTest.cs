using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace RES.CSVReader.Test
{
    [TestFixture]
    public class CSVReaderTest
    {
        [Test]
        public void TestCSVBasic()
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
            Assert.AreEqual("Column1", reader.Headers[0]);
            Assert.AreEqual("Column2", reader.Headers[1]);
            Assert.AreEqual("Column3", reader.Headers[2]);

            Assert.AreEqual(3, reader[0].Length);
            Assert.AreEqual("Data1-1", reader[0][0]);
            Assert.AreEqual("Data1-2", reader[0][1]);
            Assert.AreEqual("Data1-3", reader[0][2]);

            Assert.AreEqual(3, reader["Column1"].Length);
            Assert.AreEqual("Data1-1", reader["Column1"][0]);
            Assert.AreEqual("Data1-2", reader["Column1"][1]);
            Assert.AreEqual("Data1-3", reader["Column1"][2]);

            Assert.AreEqual(3, reader[1].Length);
            Assert.AreEqual("Data2-1", reader[1][0]);
            Assert.AreEqual("Data2-2", reader[1][1]);
            Assert.AreEqual("Data2-3", reader[1][2]);

            Assert.AreEqual(3, reader["Column2"].Length);
            Assert.AreEqual("Data2-1", reader["Column2"][0]);
            Assert.AreEqual("Data2-2", reader["Column2"][1]);
            Assert.AreEqual("Data2-3", reader["Column2"][2]);

            Assert.AreEqual(3, reader[2].Length);
            Assert.AreEqual("Data3-1", reader[2][0]);
            Assert.AreEqual("Data3-2", reader[2][1]);
            Assert.AreEqual("Data3-3", reader[2][2]);

            Assert.AreEqual(3, reader["Column3"].Length);
            Assert.AreEqual("Data3-1", reader["Column3"][0]);
            Assert.AreEqual("Data3-2", reader["Column3"][1]);
            Assert.AreEqual("Data3-3", reader["Column3"][2]);
        }

        [Test]
        public void TestCSVSpaces()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            writer.WriteLine("Column1, Column2, Column3");
            writer.WriteLine("Data1-1, Data2-1, Data3-1");
            writer.WriteLine("Data1-2, Data2-2, Data3-2");
            writer.WriteLine("Data1-3, Data2-3, Data3-3");

            stream.Position = 0;

            CSVReader reader = new CSVReader(stream);

            Assert.AreEqual(3, reader.RowCount);

            Assert.AreEqual(3, reader.Headers.Length);
            Assert.AreEqual("Column1", reader.Headers[0]);
            Assert.AreEqual("Column2", reader.Headers[1]);
            Assert.AreEqual("Column3", reader.Headers[2]);

            Assert.AreEqual(3, reader[0].Length);
            Assert.AreEqual("Data1-1", reader[0][0]);
            Assert.AreEqual("Data1-2", reader[0][1]);
            Assert.AreEqual("Data1-3", reader[0][2]);

            Assert.AreEqual(3, reader["Column1"].Length);
            Assert.AreEqual("Data1-1", reader["Column1"][0]);
            Assert.AreEqual("Data1-2", reader["Column1"][1]);
            Assert.AreEqual("Data1-3", reader["Column1"][2]);

            Assert.AreEqual(3, reader[1].Length);
            Assert.AreEqual("Data2-1", reader[1][0]);
            Assert.AreEqual("Data2-2", reader[1][1]);
            Assert.AreEqual("Data2-3", reader[1][2]);

            Assert.AreEqual(3, reader["Column2"].Length);
            Assert.AreEqual("Data2-1", reader["Column2"][0]);
            Assert.AreEqual("Data2-2", reader["Column2"][1]);
            Assert.AreEqual("Data2-3", reader["Column2"][2]);

            Assert.AreEqual(3, reader[2].Length);
            Assert.AreEqual("Data3-1", reader[2][0]);
            Assert.AreEqual("Data3-2", reader[2][1]);
            Assert.AreEqual("Data3-3", reader[2][2]);

            Assert.AreEqual(3, reader["Column3"].Length);
            Assert.AreEqual("Data3-1", reader["Column3"][0]);
            Assert.AreEqual("Data3-2", reader["Column3"][1]);
            Assert.AreEqual("Data3-3", reader["Column3"][2]);
        }

        [Test]
        public void TestCSVSingleDataLine()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            writer.WriteLine("Column1,Column2,Column3");
            writer.WriteLine("Data1-1,Data2-1,Data3-1");

            stream.Position = 0;

            CSVReader reader = new CSVReader(stream);

            Assert.AreEqual(1, reader.RowCount);

            Assert.AreEqual(3, reader.Headers.Length);
            Assert.AreEqual("Column1", reader.Headers[0]);
            Assert.AreEqual("Column2", reader.Headers[1]);
            Assert.AreEqual("Column3", reader.Headers[2]);

            Assert.AreEqual(1, reader[0].Length);
            Assert.AreEqual("Data1-1", reader[0][0]);

            Assert.AreEqual(1, reader["Column1"].Length);
            Assert.AreEqual("Data1-1", reader["Column1"][0]);

            Assert.AreEqual(1, reader[1].Length);
            Assert.AreEqual("Data2-1", reader[1][0]);

            Assert.AreEqual(1, reader["Column2"].Length);
            Assert.AreEqual("Data2-1", reader["Column2"][0]);

            Assert.AreEqual(1, reader[2].Length);
            Assert.AreEqual("Data3-1", reader[2][0]);

            Assert.AreEqual(1, reader["Column3"].Length);
            Assert.AreEqual("Data3-1", reader["Column3"][0]);
        }

        [Test]
        public void TestCSVBlankLine()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            writer.WriteLine("Column1,Column2,Column3");
            writer.WriteLine("Data1-1,Data2-1,Data3-1");
            writer.WriteLine("Data1-2,Data2-2,Data3-2");
            writer.WriteLine("Data1-3,Data2-3,Data3-3");
            writer.WriteLine("");

            stream.Position = 0;

            CSVReader reader = new CSVReader(stream);

            Assert.AreEqual(3, reader.RowCount);

            Assert.AreEqual(3, reader.Headers.Length);
            Assert.AreEqual("Column1", reader.Headers[0]);
            Assert.AreEqual("Column2", reader.Headers[1]);
            Assert.AreEqual("Column3", reader.Headers[2]);

            Assert.AreEqual(3, reader[0].Length);
            Assert.AreEqual("Data1-1", reader[0][0]);
            Assert.AreEqual("Data1-2", reader[0][1]);
            Assert.AreEqual("Data1-3", reader[0][2]);

            Assert.AreEqual(3, reader["Column1"].Length);
            Assert.AreEqual("Data1-1", reader["Column1"][0]);
            Assert.AreEqual("Data1-2", reader["Column1"][1]);
            Assert.AreEqual("Data1-3", reader["Column1"][2]);

            Assert.AreEqual(3, reader[1].Length);
            Assert.AreEqual("Data2-1", reader[1][0]);
            Assert.AreEqual("Data2-2", reader[1][1]);
            Assert.AreEqual("Data2-3", reader[1][2]);

            Assert.AreEqual(3, reader["Column2"].Length);
            Assert.AreEqual("Data2-1", reader["Column2"][0]);
            Assert.AreEqual("Data2-2", reader["Column2"][1]);
            Assert.AreEqual("Data2-3", reader["Column2"][2]);

            Assert.AreEqual(3, reader[2].Length);
            Assert.AreEqual("Data3-1", reader[2][0]);
            Assert.AreEqual("Data3-2", reader[2][1]);
            Assert.AreEqual("Data3-3", reader[2][2]);

            Assert.AreEqual(3, reader["Column3"].Length);
            Assert.AreEqual("Data3-1", reader["Column3"][0]);
            Assert.AreEqual("Data3-2", reader["Column3"][1]);
            Assert.AreEqual("Data3-3", reader["Column3"][2]);
        }

        [Test]
        public void TestCSVCommaLine()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            writer.WriteLine("Column1,Column2,Column3");
            writer.WriteLine("Data1-1,Data2-1,Data3-1");
            writer.WriteLine("Data1-2,Data2-2,Data3-2");
            writer.WriteLine("Data1-3,Data2-3,Data3-3");
            writer.WriteLine(",,");

            stream.Position = 0;

            CSVReader reader = new CSVReader(stream);

            Assert.AreEqual(3, reader.RowCount);

            Assert.AreEqual(3, reader.Headers.Length);
            Assert.AreEqual("Column1", reader.Headers[0]);
            Assert.AreEqual("Column2", reader.Headers[1]);
            Assert.AreEqual("Column3", reader.Headers[2]);

            Assert.AreEqual(3, reader[0].Length);
            Assert.AreEqual("Data1-1", reader[0][0]);
            Assert.AreEqual("Data1-2", reader[0][1]);
            Assert.AreEqual("Data1-3", reader[0][2]);

            Assert.AreEqual(3, reader["Column1"].Length);
            Assert.AreEqual("Data1-1", reader["Column1"][0]);
            Assert.AreEqual("Data1-2", reader["Column1"][1]);
            Assert.AreEqual("Data1-3", reader["Column1"][2]);

            Assert.AreEqual(3, reader[1].Length);
            Assert.AreEqual("Data2-1", reader[1][0]);
            Assert.AreEqual("Data2-2", reader[1][1]);
            Assert.AreEqual("Data2-3", reader[1][2]);

            Assert.AreEqual(3, reader["Column2"].Length);
            Assert.AreEqual("Data2-1", reader["Column2"][0]);
            Assert.AreEqual("Data2-2", reader["Column2"][1]);
            Assert.AreEqual("Data2-3", reader["Column2"][2]);

            Assert.AreEqual(3, reader[2].Length);
            Assert.AreEqual("Data3-1", reader[2][0]);
            Assert.AreEqual("Data3-2", reader[2][1]);
            Assert.AreEqual("Data3-3", reader[2][2]);

            Assert.AreEqual(3, reader["Column3"].Length);
            Assert.AreEqual("Data3-1", reader["Column3"][0]);
            Assert.AreEqual("Data3-2", reader["Column3"][1]);
            Assert.AreEqual("Data3-3", reader["Column3"][2]);
        }

        [Test]
        public void TestCSVNoHeaders()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            writer.WriteLine("Data1-1,Data2-1,Data3-1");
            writer.WriteLine("Data1-2,Data2-2,Data3-2");
            writer.WriteLine("Data1-3,Data2-3,Data3-3");

            stream.Position = 0;

            CSVReader reader = new CSVReader(stream, false);

            Assert.AreEqual(3, reader.RowCount);

            Assert.AreEqual(3, reader[0].Length);
            Assert.AreEqual("Data1-1", reader[0][0]);
            Assert.AreEqual("Data1-2", reader[0][1]);
            Assert.AreEqual("Data1-3", reader[0][2]);

            Assert.AreEqual(3, reader[1].Length);
            Assert.AreEqual("Data2-1", reader[1][0]);
            Assert.AreEqual("Data2-2", reader[1][1]);
            Assert.AreEqual("Data2-3", reader[1][2]);

            Assert.AreEqual(3, reader[2].Length);
            Assert.AreEqual("Data3-1", reader[2][0]);
            Assert.AreEqual("Data3-2", reader[2][1]);
            Assert.AreEqual("Data3-3", reader[2][2]);
        }

        [Test]
        public void TestCSVQuotes()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            writer.WriteLine("\"Column,1\",Column2,Column3");
            writer.WriteLine("\"Data1,1\",Data2-1,Data3-1");
            writer.WriteLine("Data1-2,Data2-2,Data3-2");
            writer.WriteLine("Data1-3,Data2-3,Data3-3");

            stream.Position = 0;

            CSVReader reader = new CSVReader(stream);

            Assert.AreEqual(3, reader.RowCount);

            Assert.AreEqual(3, reader.Headers.Length);
            Assert.AreEqual("Column,1", reader.Headers[0]);
            Assert.AreEqual("Column2", reader.Headers[1]);
            Assert.AreEqual("Column3", reader.Headers[2]);

            Assert.AreEqual(3, reader[0].Length);
            Assert.AreEqual("Data1,1", reader[0][0]);
            Assert.AreEqual("Data1-2", reader[0][1]);
            Assert.AreEqual("Data1-3", reader[0][2]);

            Assert.AreEqual(3, reader["Column,1"].Length);
            Assert.AreEqual("Data1,1", reader["Column,1"][0]);
            Assert.AreEqual("Data1-2", reader["Column,1"][1]);
            Assert.AreEqual("Data1-3", reader["Column,1"][2]);

            Assert.AreEqual(3, reader[1].Length);
            Assert.AreEqual("Data2-1", reader[1][0]);
            Assert.AreEqual("Data2-2", reader[1][1]);
            Assert.AreEqual("Data2-3", reader[1][2]);

            Assert.AreEqual(3, reader["Column2"].Length);
            Assert.AreEqual("Data2-1", reader["Column2"][0]);
            Assert.AreEqual("Data2-2", reader["Column2"][1]);
            Assert.AreEqual("Data2-3", reader["Column2"][2]);

            Assert.AreEqual(3, reader[2].Length);
            Assert.AreEqual("Data3-1", reader[2][0]);
            Assert.AreEqual("Data3-2", reader[2][1]);
            Assert.AreEqual("Data3-3", reader[2][2]);

            Assert.AreEqual(3, reader["Column3"].Length);
            Assert.AreEqual("Data3-1", reader["Column3"][0]);
            Assert.AreEqual("Data3-2", reader["Column3"][1]);
            Assert.AreEqual("Data3-3", reader["Column3"][2]);
        }

        [Test]
        [ExpectedException(typeof(CSVReader.DataNotRectangularException), ExpectedMessage = "Error:  Expecting 3 lines, found 2 lines, in line \"Data1-2,Data2-2\"")]
        public void TestCSVMissingData()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            writer.WriteLine("\"Column,1\",Column2,Column3");
            writer.WriteLine("\"Data1,1\",Data2-1,Data3-1");
            writer.WriteLine("Data1-2,Data2-2");
            writer.WriteLine(",Data2-3,Data3-3");

            stream.Position = 0;

            CSVReader reader = new CSVReader(stream);
        }

        [Test]
        [ExpectedException(typeof(CSVReader.DataNotRectangularException), ExpectedMessage = "Error:  Expecting 2 lines, found 3 lines, in line \"Data1-1,Data2-1,Data3-1\"")]
        public void TestCSVMissingHeader()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            writer.WriteLine("Column1,Column2");
            writer.WriteLine("Data1-1,Data2-1,Data3-1");
            writer.WriteLine("Data1-2,Data2-2,Data3-2");
            writer.WriteLine("Data1-3,Data2-3,Data3-3");

            stream.Position = 0;

            CSVReader reader = new CSVReader(stream);
        }

        [Test]
        public void TestCsvFixedSizeHeader()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            writer.WriteLine("Some Header line");
            writer.WriteLine("Parameter: Value");
            writer.WriteLine("Etc");
            writer.WriteLine("");
            writer.WriteLine("Column1,Column2,Column3");
            writer.WriteLine("Data1-1,Data2-1,Data3-1");
            writer.WriteLine("Data1-2,Data2-2,Data3-2");
            writer.WriteLine("Data1-3,Data2-3,Data3-3");

            stream.Position = 0;

            CSVReader reader = new CSVReader(stream,4,true);

            Assert.AreEqual(3, reader.RowCount);

            Assert.AreEqual(3, reader.Headers.Length);
            Assert.AreEqual("Column1", reader.Headers[0]);
            Assert.AreEqual("Column2", reader.Headers[1]);
            Assert.AreEqual("Column3", reader.Headers[2]);

            Assert.AreEqual(3, reader[0].Length);
            Assert.AreEqual("Data1-1", reader[0][0]);
            Assert.AreEqual("Data1-2", reader[0][1]);
            Assert.AreEqual("Data1-3", reader[0][2]);

            Assert.AreEqual(3, reader["Column1"].Length);
            Assert.AreEqual("Data1-1", reader["Column1"][0]);
            Assert.AreEqual("Data1-2", reader["Column1"][1]);
            Assert.AreEqual("Data1-3", reader["Column1"][2]);

            Assert.AreEqual(3, reader[1].Length);
            Assert.AreEqual("Data2-1", reader[1][0]);
            Assert.AreEqual("Data2-2", reader[1][1]);
            Assert.AreEqual("Data2-3", reader[1][2]);

            Assert.AreEqual(3, reader["Column2"].Length);
            Assert.AreEqual("Data2-1", reader["Column2"][0]);
            Assert.AreEqual("Data2-2", reader["Column2"][1]);
            Assert.AreEqual("Data2-3", reader["Column2"][2]);

            Assert.AreEqual(3, reader[2].Length);
            Assert.AreEqual("Data3-1", reader[2][0]);
            Assert.AreEqual("Data3-2", reader[2][1]);
            Assert.AreEqual("Data3-3", reader[2][2]);

            Assert.AreEqual(3, reader["Column3"].Length);
            Assert.AreEqual("Data3-1", reader["Column3"][0]);
            Assert.AreEqual("Data3-2", reader["Column3"][1]);
            Assert.AreEqual("Data3-3", reader["Column3"][2]);
        }

        [Test]
        public void TestCsvDynamicHeader()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            writer.WriteLine("Some Header line");
            writer.WriteLine("Parameter: Value");
            writer.WriteLine("Etc");
            writer.WriteLine("");
            writer.WriteLine("Column1,Column2,Column3");
            writer.WriteLine("Data1-1,Data2-1,Data3-1");
            writer.WriteLine("Data1-2,Data2-2,Data3-2");
            writer.WriteLine("Data1-3,Data2-3,Data3-3");

            stream.Position = 0;

            CSVReader reader = new CSVReader(stream, new string[] {"Column1","Column2","Column3"});

            Assert.AreEqual(3, reader.RowCount);

            Assert.AreEqual(3, reader.Headers.Length);
            Assert.AreEqual("Column1", reader.Headers[0]);
            Assert.AreEqual("Column2", reader.Headers[1]);
            Assert.AreEqual("Column3", reader.Headers[2]);

            Assert.AreEqual(3, reader[0].Length);
            Assert.AreEqual("Data1-1", reader[0][0]);
            Assert.AreEqual("Data1-2", reader[0][1]);
            Assert.AreEqual("Data1-3", reader[0][2]);

            Assert.AreEqual(3, reader["Column1"].Length);
            Assert.AreEqual("Data1-1", reader["Column1"][0]);
            Assert.AreEqual("Data1-2", reader["Column1"][1]);
            Assert.AreEqual("Data1-3", reader["Column1"][2]);

            Assert.AreEqual(3, reader[1].Length);
            Assert.AreEqual("Data2-1", reader[1][0]);
            Assert.AreEqual("Data2-2", reader[1][1]);
            Assert.AreEqual("Data2-3", reader[1][2]);

            Assert.AreEqual(3, reader["Column2"].Length);
            Assert.AreEqual("Data2-1", reader["Column2"][0]);
            Assert.AreEqual("Data2-2", reader["Column2"][1]);
            Assert.AreEqual("Data2-3", reader["Column2"][2]);

            Assert.AreEqual(3, reader[2].Length);
            Assert.AreEqual("Data3-1", reader[2][0]);
            Assert.AreEqual("Data3-2", reader[2][1]);
            Assert.AreEqual("Data3-3", reader[2][2]);

            Assert.AreEqual(3, reader["Column3"].Length);
            Assert.AreEqual("Data3-1", reader["Column3"][0]);
            Assert.AreEqual("Data3-2", reader["Column3"][1]);
            Assert.AreEqual("Data3-3", reader["Column3"][2]);
        }

        [Test]
        [ExpectedException(typeof(CSVReader.HeadersNotFoundException), ExpectedMessage = "Could not find header line containing: \"Tom\", \"Dick\", \"Harry\",")]
        public void TestCsvDynamicHeaderFail()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            writer.WriteLine("Some Header line");
            writer.WriteLine("Parameter: Value");
            writer.WriteLine("Etc");
            writer.WriteLine("");
            writer.WriteLine("Column1,Column2,Column3");
            writer.WriteLine("Data1-1,Data2-1,Data3-1");
            writer.WriteLine("Data1-2,Data2-2,Data3-2");
            writer.WriteLine("Data1-3,Data2-3,Data3-3");

            stream.Position = 0;

            CSVReader reader = new CSVReader(stream, new string[] { "Tom","Dick","Harry" });
        }
    }
}
