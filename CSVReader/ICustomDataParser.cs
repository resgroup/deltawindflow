using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RES.CSVReader
{
    public interface ICustomDataParser<T>
    {
        T Parse(string value);
    }
}
