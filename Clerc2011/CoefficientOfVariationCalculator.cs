using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clerc2011
{
    public class CoefficientOfVariationCalculator : ICoefficientOfVariationCalculator
    {
        private IMastToTurbineVariationCalculator _mastToTurbineVariationCalculator;
        private ISpeedupVariationCalculator _speedupVariationCalculator;

        public CoefficientOfVariationCalculator(IMastToTurbineVariationCalculator mastToTurbineVariationCalculator, ISpeedupVariationCalculator speedupVariationCalculator)
        {
            _mastToTurbineVariationCalculator = mastToTurbineVariationCalculator;
            _speedupVariationCalculator = speedupVariationCalculator;
        }

        public double Calculate(double mastTurbineSeparation, double speedUp)
        {
            return System.Math.Sqrt(System.Math.Pow(_mastToTurbineVariationCalculator.Calculate(mastTurbineSeparation), 2) + System.Math.Pow(_speedupVariationCalculator.Calculate(speedUp), 2));
        }
    }
}
