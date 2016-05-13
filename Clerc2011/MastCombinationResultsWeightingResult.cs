using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clerc2011
{
    public struct MastWeighting
    {
        public MastWeighting(Mast mast, double resultsWeighting) : this()
        {
            Mast = mast;
            ResultsWeighting = resultsWeighting;
        }

        public Mast Mast { get; set; }
        public double ResultsWeighting { get; set; }
    }
}
