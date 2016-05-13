using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clerc2011
{
    public class TopoUncertaintyCalculator
    {
        private IBinTopoUncertaintyCalculator _binTopoUncertaintyCalculator;

        private List<BinInformation> _values = new List<BinInformation>();
        private IDictionary<Turbine, IEnumerable<MastWeighting>> _mastWeightings;

        public TopoUncertaintyCalculator(double lamda,
                                         double lengthScale,
                                         double a,
                                         IDictionary<Turbine, IEnumerable<MastWeighting>> mastWeightings,
                                         bool offshore)
            : this(new BinTopoUncertaintyCalculator(
                        new CoefficientOfVariationCalculator(
                            new MastToTurbineVariationCalculator(lamda, lengthScale),
                            offshore ? (ISpeedupVariationCalculator)new OffshoreSpeedupVariationCalculator() : new SpeedupVariationCalculator(a)),
                        new CorrelationCalculator(
                            new AngleCorrelationCalculator(),
                            new DistanceCorrelationCalculator(lengthScale))),
                   mastWeightings)
        {
        }

        public TopoUncertaintyCalculator(IBinTopoUncertaintyCalculator binTopoUncertaintyCalculator, IDictionary<Turbine, IEnumerable<MastWeighting>> mastWeightings)
        {
            _binTopoUncertaintyCalculator = binTopoUncertaintyCalculator;
            _mastWeightings = mastWeightings;
        }

        private double GetMastWeight(Turbine turbine, Mast mast)
        {
            return GetMastWeight(_mastWeightings[turbine], mast);
        }

        private double GetMastWeight(IEnumerable<MastWeighting> mastWeightings, Mast mast)
        {
            if (mastWeightings.Count(w => w.Mast.Equals(mast)) != 1)
                return 0;
            else
                return mastWeightings.Single(w => w.Mast.Equals(mast)).ResultsWeighting;
        }

        private double CalculateUncertainty(IEnumerable<BinInformation> values)
        {
            var total = 0.0;

            BinInformation[] bins = values.ToArray();

            int count = bins.Length;

            double[] mastWeights = new double[count];
            double[] variance = new double[count];

            for (int i = 0; i < count; i++)
            {
                if (bins[i].BinEnergy > 0 && bins[i].SensitivityFactor > 0)
                {
                    mastWeights[i] = GetMastWeight(bins[i].Turbine, bins[i].Mast);
                    if (mastWeights[i] > 0)
                    {
                        variance[i] = _binTopoUncertaintyCalculator.CalculateVariance(bins[i]);
                        if (variance[i] > 0)
                        {
                            for (int j = 0; j <= i; j++)
                            {
                                if (bins[j].BinEnergy > 0 && mastWeights[j] > 0 && bins[j].SensitivityFactor > 0 && variance[j] > 0)
                                {
                                    var singleResult = _binTopoUncertaintyCalculator.Calculate(bins[i], bins[j], variance[i], variance[j]) * bins[i].BinEnergy * bins[j].BinEnergy * mastWeights[i] * mastWeights[j];
                                    if (i != j)
                                        singleResult *= 2;

                                    total += singleResult;
                                }
                            }
                        }
                    }
                }
            }
            return System.Math.Sqrt(total);
        }

        public double Energy
        {
            get
            {
                var total = 0.0;
                foreach (var bin1 in _values)
                    total += bin1.BinEnergy * GetMastWeight(bin1.Turbine, bin1.Mast);

                return total;
            }
        }

        public double Uncertainty
        {
            get
            {
                return CalculateUncertainty(_values);
            }
        }

        public void AddBin(BinInformation binInformation)
        {
            lock (this)
            {
                _values.Add(binInformation);
            }
        }

        public double PerTurbineUncertainty(Turbine turbine)
        {
            return CalculateUncertainty(_values.Where(x => x.Turbine.Equals(turbine)));
        }
    }
}
