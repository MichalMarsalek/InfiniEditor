using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniEditor
{
    public struct BoundingBox
    {
        public Vec Min { get; private set; }
        public Vec Max { get; private set; }

        public BoundingBox(Vec min, Vec max)
        {
            Min = min;
            Max = max;
        }

        public Vec Size
        {
            get
            {
                return Max - Min + new Vec(1, 1, 1);
            }
        }

        public string SizeToString()
        {
            return Size.X + " × " + Size.Y + " × " + Size.Z;
        }

        public override string ToString()
        {
            return Min + " --> " + Max;
        }

        public static bool operator ==(BoundingBox a, BoundingBox b)
        {
            return a.Min == b.Min && a.Max == b.Max;
        }

        public static bool operator !=(BoundingBox a, BoundingBox b)
        {
            return !(a == b);
        }
    }
}
