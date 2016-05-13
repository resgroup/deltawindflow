using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clerc2011
{
    public class AngleCorrelationCalculator : IAngleCorrelationCalculator
    {
        public double Calculate(double deltaTheta)
        {
            return System.Math.Max(1 - System.Math.Abs(deltaTheta) / 90.0, 0);
        }
    }
}
