using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clerc2011
{
    public class OffshoreSpeedupVariationCalculator : ISpeedupVariationCalculator
    {
        #region ISpeedupVariationCalculator Members

        public double Calculate(double speedUp)
        {
            return 0; // No speedup variation offshore
        }

        #endregion
    }
}
