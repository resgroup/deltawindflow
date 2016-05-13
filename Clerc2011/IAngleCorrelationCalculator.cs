using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clerc2011
{
    public interface IAngleCorrelationCalculator
    {
        double Calculate(double deltaTheta);
    }
}
