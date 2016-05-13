using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clerc2011
{
    public class CorrelationCalculator : ICorrelationCalculator
    {
        private IAngleCorrelationCalculator _angleCorrelationCalculator;
        private IDistanceCorrelationCalculator _distanceCorrelationCalculator;

        public CorrelationCalculator(IAngleCorrelationCalculator angleCorrelationCalculator, IDistanceCorrelationCalculator distanceCorrelationCalculator)
        {
            _angleCorrelationCalculator = angleCorrelationCalculator;
            _distanceCorrelationCalculator = distanceCorrelationCalculator;
        }

        public double Calculate(double deltaTheta, double mastMastDistance, double turbineTurbineDistance)
        {
            return _angleCorrelationCalculator.Calculate(deltaTheta) * _distanceCorrelationCalculator.Calculate(mastMastDistance, turbineTurbineDistance);
        }
    }
}
