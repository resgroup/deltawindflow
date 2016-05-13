using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clerc2011
{
    public class DistanceCorrelationCalculator : IDistanceCorrelationCalculator
    {
        private static double _rootTwo = System.Math.Sqrt(2);
        private double _lengthScale;

        public DistanceCorrelationCalculator(double lengthScale)
        {
            _lengthScale = lengthScale;
        }

        public double Calculate(params double[] distances)
        {
            var total = 0.0;
            foreach (var distance in distances)
            {
                total += System.Math.Exp(-_rootTwo * distance / _lengthScale);
            }
            return total / 2.0;
        }
    }
}
