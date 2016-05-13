using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clerc2011
{
    public class MastToTurbineVariationCalculator : IMastToTurbineVariationCalculator
    {
        private double _lamda;
        private double _lengthScale;

        public MastToTurbineVariationCalculator(double lamda, double lengthScale)
        {
            _lamda = lamda;
            _lengthScale = lengthScale;
        }

        public double Calculate(double distanceMastTurbine)
        {
            return _lamda * (1 - System.Math.Exp(-distanceMastTurbine / _lengthScale));
        }
    }
}
