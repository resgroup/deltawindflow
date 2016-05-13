using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clerc2011
{
    public interface IDistanceCorrelationCalculator
    {
        double Calculate(params double[] distances);
    }
}
