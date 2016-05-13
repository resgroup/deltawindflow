using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clerc2011
{
    public struct BinInformation
    {
        public Turbine Turbine { get; set; }
        public Mast Mast { get; set; }
        public double Direction { get; set; }
        public double SpeedUp { get; set; }
        public double SensitivityFactor { get; set; }
        public double BinEnergy { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is BinInformation)
            {
                var bin = (BinInformation)obj;
                return bin.Direction == Direction &&
                       bin.SpeedUp == SpeedUp &&
                       bin.SensitivityFactor == SensitivityFactor &&
                       bin.BinEnergy == BinEnergy &&
                       bin.Turbine.Equals(Turbine) &&
                       bin.Mast.Equals(Mast);
            }
            else
                return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Turbine.GetHashCode() ^ Mast.GetHashCode() ^ Direction.GetHashCode() ^ SpeedUp.GetHashCode() ^ SensitivityFactor.GetHashCode() ^ BinEnergy.GetHashCode();
        }
    }
}
