using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace RES.CSVReader
{
    public class CSVReader
    {
        private static readonly CultureInfo _culture = new CultureInfo("en-GB");

        public class HeadersNotFoundException
            : Exception
        {
            public HeadersNotFoundException(string message)
                : base(message)
            { }
        }

        public class ParsingException
            : Exception
        {
            public string FailedAt
            {
                get;
                private set;
            }

            public Type Type
            {
                get;
                private set;
            }

            public ParsingException(string failedAt, Type type)
                : base(string.Format("Error:  \"{0}\" is not a valid {1}.  Check the format of the csv file", failedAt,type.ToString()))
            {
                FailedAt = failedAt;
                Type = type;
            }
        }

        public class DataNotRectangularException
            : Exception
        {
            public string ErrorLine
            {
                get;
                private set;
            }

            public int? Expecting
            {
                get;
                private set;
            }

            public DataNotRectangularException()
                : base("Data not in rectangular form")
            {
                ErrorLine = null;
                Expecting = null;
            }

            public DataNotRectangularException(string errorLine, int expecting, int found)
                : base(string.Format("Error:  Expecting {0} lines, found {1} lines, in line \"{2}\"", expecting,found,errorLine))
            {
                ErrorLine = errorLine;
                Expecting = expecting;
            }
        }

        #region Private Variables

        private List<string> _headers = null;
        private List<List<string>> _data;
        private Dictionary<int, string[]> _columnCache = new Dictionary<int,string[]>();
        private StringSplitter Splitter = new StringSplitter(',', '"', true);

        #endregion

        #region Public Properties

        public int RowCount
        {
            get
            {
                return _data.Count;
            }
        }

        public string[] this[string column]
        {
            get
            {
                if (_headers == null)
                {
                    throw new Exception("No row headers available");
                }
                int index = _headers.IndexOf(column);
                if (index == -1)
                {
                    throw new Exception(string.Format("{0} not found in collection", column));
                }
                return this[index];
            }
        }

        public string[] this[int column]
        {
            get
            {
                if (!_columnCache.ContainsKey(column))
                {
                    List<string> cells = new List<string>();
                    foreach (List<string> row in _data)
                    {
                        if (column < row.Count)
                        {
                            cells.Add(row[column]);
                        }
                        else
                        {
                            cells.Add("");
                        }
                    }
                    _columnCache.Add(column, cells.ToArray());
                }
                return _columnCache[column];
            }
        }

        public string[] Headers
        {
            get
            {
                return _headers.ToArray();
            }
        }

        #endregion

        #region Constructors

        public CSVReader(Stream input)
            :this(input,true)
        {    
        }

        public CSVReader(Stream input, bool containsColumnHeaders)
        {
            ReadData(new StreamReader(input), containsColumnHeaders);
        }

        public CSVReader(Stream input, int ignoreLinesOfHeader)
            :this(input,ignoreLinesOfHeader,true)
        {
        }

        public CSVReader(Stream input, int ignoreLinesOfHeader, bool containsColumnHeaders)
            : this(input, ignoreLinesOfHeader, containsColumnHeaders,null)
        {
        }

        public CSVReader(Stream input, int ignoreLinesOfHeader, bool containsColumnHeaders, Encoding encoding)
        {
            StreamReader reader;
            if(encoding == null)
                reader = new StreamReader(input);
            else
                reader = new StreamReader(input, encoding);

            string header;
            for (int i = 0; i < ignoreLinesOfHeader; i++)
                header = reader.ReadLine();

            ReadData(reader, containsColumnHeaders);
        }

        public CSVReader(Stream input, string[] headerLineContains)
        {
            StreamReader reader = new StreamReader(input);

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                List<string> headers = new List<string>();
                foreach (string s in line.Split(new char[] { ',' }))
                    headers.Add(s.Trim());

                bool found = true;
                foreach (string s in headerLineContains)
                {
                    if (!headers.Contains(s.Trim()))
                    {
                        found = false;
                        break;
                    }
                }
                if (!found)
                    continue;

                _headers = Splitter.Split(line, false);
                ReadData(reader, false);
                return;
            }
            string msg = "Could not find header line containing: ";
            foreach (string s in headerLineContains)
                msg += string.Format("\"{0}\", ",s);
            msg = msg.Remove(msg.Length-1);
            throw new HeadersNotFoundException(msg);
        }

        private void ReadData(StreamReader reader, bool containsColumnHeaders)
        {
            _data = new List<List<string>>();
            int expectedColumns;
            string line;

            if (containsColumnHeaders)
            {
                _headers = Splitter.Split(reader.ReadLine(), false);
                expectedColumns = _headers.Count;
                line = reader.ReadLine();
            }
            else
            {
                line = reader.ReadLine();
                expectedColumns = Splitter.Split(line, false).Count;
            }

            bool gotLine = true;

            while (!reader.EndOfStream || gotLine)
            {
                if (gotLine)
                    gotLine = false;
                else
                    line = reader.ReadLine();

                if (line != "")
                {
                    List<string> items = Splitter.Split(line, false);
                    if (!AllStringsEmpty(items))
                    {
                        if (items.Count != expectedColumns)
                            throw new DataNotRectangularException(line, expectedColumns, items.Count);
                         
                        _data.Add(Splitter.Split(line, false));
                    }
                }
            }
        }

        private bool AllStringsEmpty(List<string> strings)
        {
            foreach (string item in strings)
            {
                if (!string.IsNullOrEmpty(item))
                    return false;
            }
            return true;
        }

        #endregion

        private List<DateTime> GetDateTimeList(string[] column)
        {
            List<DateTime> list = new List<DateTime>();
            foreach (string dateTimeString in column)
            {
                try
                {
                    list.Add(DateTime.Parse(dateTimeString));
                }
                catch
                {
                    throw new ParsingException(dateTimeString, typeof(DateTime));
                }
            }
            return list;
        }

        public List<DateTime> GetDateTimeList(string columnName)
        {
            return GetDateTimeList(this[columnName]);
        }

        public List<DateTime> GetDateTimeList(int columnIndex)
        {
            return GetDateTimeList(this[columnIndex]);
        }

        protected List<double> GetDoubleList(string[] column)
        {
            List<double> list = new List<double>();
            foreach (string doubleString in column)
            {
                try
                {
                    if (string.IsNullOrEmpty(doubleString))
                        list.Add(double.NaN);
                    else
                        list.Add(double.Parse(doubleString, _culture));
                }
                catch
                {
                    throw new ParsingException(doubleString, typeof(double));
                }
            }
            return list;
        }

        public List<double> GetDoubleList(string columnName)
        {
            return GetDoubleList(this[columnName]);
        }

        public List<double> GetDoubleList(int columnIndex)
        {
            return GetDoubleList(this[columnIndex]);
        }

        protected List<int> GetIntList(string[] column)
        {
            List<int> list = new List<int>();
            foreach (string intString in column)
            {
                try
                {
                    list.Add(int.Parse(intString));
                }
                catch
                {
                    throw new ParsingException(intString, typeof(int));
                }
            }
            return list;
        }

        public List<int> GetIntList(string columnName)
        {
            return GetIntList(this[columnName]);
        }

        public List<int> GetIntList(int columnIndex)
        {
            return GetIntList(this[columnIndex]);
        }

        private List<T> GetCustomTypeList<T>(ICustomDataParser<T> parser, string[] column)
        {
            List<T> list = new List<T>();
            foreach (string s in column)
            {
                list.Add(parser.Parse(s));
            }
            return list;
        }

        public List<T> GetCustomTypeList<T>(ICustomDataParser<T> parser, string columnName)
        {
            return GetCustomTypeList<T>(parser, this[columnName]);
        }

        public List<T> GetCustomTypeList<T>(ICustomDataParser<T> parser, int columnIndex)
        {
            return GetCustomTypeList<T>(parser, this[columnIndex]);
        }
    }
}
