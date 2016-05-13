using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clerc2011
{
    public interface ICorrelationCalculator
    {
        double Calculate(double deltaTheta, double mastMastDistance, double turbineTurbineDistance);
    }
}
