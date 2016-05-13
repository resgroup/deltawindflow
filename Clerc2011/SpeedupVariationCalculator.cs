using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clerc2011
{
    public class SpeedupVariationCalculator : ISpeedupVariationCalculator
    {
        private double _a;

        public SpeedupVariationCalculator(double a)
        {
            _a = a;
        }

        public double Calculate(double speedUp)
        {
            return 2 * _a * System.Math.Abs(speedUp - 1) / (speedUp + 1);
        }
    }
}
