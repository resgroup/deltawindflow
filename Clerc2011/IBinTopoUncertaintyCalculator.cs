using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clerc2011
{
    public interface IBinTopoUncertaintyCalculator
    {
        double CalculateVariance(BinInformation bin);
        double Calculate(BinInformation bin1, BinInformation bin2, double variance1, double variance2);
    }
}
