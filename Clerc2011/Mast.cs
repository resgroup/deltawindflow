using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clerc2011
{
    public struct Mast
    {
        public Mast(double x, double y) : this() { X = x; Y = y; }

        public double X { get; set; }
        public double Y { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Mast)
            {
                var other = (Mast)obj;
                return other.X == X && other.Y == Y;
            }
            else
                return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}
