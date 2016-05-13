using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RES.CSVReader
{
    public class StringSplitter
    {
        #region Private Variables

        private char[] _splitters;
        private char _quote;
        private bool _trim;

        #endregion

        #region Constructors

        public StringSplitter(char splitter, char quote) : this(splitter, quote, false) { }
            
        public StringSplitter(char splitter, char quote, bool trim) : this(new char[] { splitter }, quote, trim) { }

        public StringSplitter(char[] splitters, char quote) : this(splitters, quote, false) { }

        public StringSplitter(char[] splitters, char quote, bool trim)
        {
            _splitters = splitters;
            _quote = quote;
            _trim = trim;
        }

        #endregion

        #region Static Methods

        public static List<string> Split(string stringToSplit, bool ignourEmpty, char[] splitters, char quote, bool trim)
        {
            return new StringSplitter(splitters, quote, trim).Split(stringToSplit, ignourEmpty);
        }

        public static List<string> Split(string stringToSplit, bool ignourEmpty, char splitter, char quote, bool trim)
        {
            return new StringSplitter(splitter, quote, trim).Split(stringToSplit, ignourEmpty);
        }

        #endregion

        #region Public Methods

        public List<string> Split(string stringToSplit, bool ignoreEmpty)
        {
            int position;
            string firstString;
            List<string> strings = new List<string>();
            bool atEnd = false;
            if (_trim) stringToSplit = stringToSplit.Trim(' ');
            if (stringToSplit.StartsWith(_quote.ToString()))
            {
                position = -1;
                foreach (char splitter in _splitters)
                {
                    int splitterPos = stringToSplit.IndexOf(_quote.ToString() + splitter.ToString(), 1);
                    if (splitterPos > -1 && (position == -1 || splitterPos < position))
                    {
                        position = splitterPos;
                    }
                }
                if (position == -1)
                {
                    atEnd = true;
                    firstString = stringToSplit.Substring(1, stringToSplit.Length - 2);
                }
                else
                {
                    firstString = stringToSplit.Substring(1, position - 1);
                    position++;
                }
            }
            else
            {
                position = -1;
                foreach (char splitter in _splitters)
                {
                    int splitterPos = stringToSplit.IndexOf(splitter);
                    if (splitterPos > -1 && (position == -1 || splitterPos < position))
                    {
                        position = splitterPos;
                    }
                }
                if (position == -1)
                {
                    atEnd = true;
                    firstString = stringToSplit;
                }
                else
                    firstString = stringToSplit.Substring(0, position);
                if (_trim)
                    firstString = firstString.Trim();
            }
            if (!(ignoreEmpty && String.IsNullOrEmpty(firstString)))
                strings.Add(firstString);
            if (!atEnd)
            {
                string secondString = stringToSplit.Substring(position + 1, stringToSplit.Length - position - 1);
                strings.AddRange(Split(secondString, ignoreEmpty));
            }
            return strings;
        }

        #endregion
    }
}
