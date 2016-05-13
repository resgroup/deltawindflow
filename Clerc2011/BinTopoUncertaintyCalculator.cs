using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clerc2011
{
    public class BinTopoUncertaintyCalculator : IBinTopoUncertaintyCalculator
    {
        private ICoefficientOfVariationCalculator _coefficientOfVariationCalculator;
        private ICorrelationCalculator _correlationCalculator;

        public BinTopoUncertaintyCalculator(ICoefficientOfVariationCalculator coefficientOfVariationCalculator, ICorrelationCalculator correlationCalculator)
        {
            _coefficientOfVariationCalculator = coefficientOfVariationCalculator;
            _correlationCalculator = correlationCalculator;
        }

        private double Distance(double x1, double y1, double x2, double y2)
        {
            return System.Math.Sqrt(System.Math.Pow(x1 - x2, 2) + System.Math.Pow(y1 - y2, 2));
        }

        private double GetAngleDifference(double dir1, double dir2)
        {
            double diff = dir1 - dir2;
            if (diff > 180)
                return diff - 360;
            else if (diff < -180)
                return diff + 360;
            else
                return diff;

        }

        public double CalculateVariance(BinInformation bin)
        {
            return _coefficientOfVariationCalculator.Calculate(Distance(bin.Turbine.X, bin.Turbine.Y, bin.Mast.X, bin.Mast.Y), bin.SpeedUp);
        }

        private double CalculateCoefficient(BinInformation bin1, BinInformation bin2)
        {
            return _correlationCalculator.Calculate(GetAngleDifference(bin1.Direction, bin2.Direction),
                                                    Distance(bin1.Mast.X, bin1.Mast.Y, bin2.Mast.X, bin2.Mast.Y),
                                                    Distance(bin1.Turbine.X, bin1.Turbine.Y, bin2.Turbine.X, bin2.Turbine.Y));
        }

        public double Calculate(BinInformation bin1, BinInformation bin2, double variance1, double variance2)
        {
            return CalculateCoefficient(bin1, bin2) * variance1 * variance2 * bin1.SensitivityFactor * bin2.SensitivityFactor;
        }

        public double Calculate(BinInformation bin1, BinInformation bin2)
        {
            return Calculate(bin1, bin2, CalculateVariance(bin1), CalculateVariance(bin2));
        }
    }
}
