using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniEditor
{
    public class Symmetry
    {
        public Vec NormalVector { get; private set; }
        public int With { get; private set; }
        public Symmetry(string axis)
        {
            NormalVector = Parse(axis);
            With = -1;
        }
        public Symmetry(string axis, int with)
        {
            NormalVector = Parse(axis);
            With = with;
        }

        private Vec Parse(string axis)
        {
            int x = 0, y = 0, z = 0;
            int sign = 1;
            foreach (char chr in axis.ToLower())
            {
                if (chr == '-')
                {
                    sign = -1;
                }
                else if (chr == 'x')
                {
                    x = sign;
                    sign = 1;
                }
                else if (chr == 'y')
                {
                    y = sign;
                    sign = 1;
                }
                else if (chr == 'z')
                {
                    z = sign;
                    sign = 1;
                }
            }
            return new Vec(x, y, z);
        }

        public string VectorToString()
        {
            string ret = "";
            if(NormalVector.X == -1)
            {
                ret += "-X";
            }
            else if (NormalVector.X == 1)
            {
                ret += "X";
            }
            if (NormalVector.Y == -1)
            {
                ret += "-Y";
            }
            else if (NormalVector.Y == 1)
            {
                ret += "Y";
            }
            if (NormalVector.Z == -1)
            {
                ret += "-Z";
            }
            else if (NormalVector.Z == 1)
            {
                ret += "Z";
            }
            return ret;
        }

        public override string ToString()
        {
            return VectorToString() + (With == -1 ? "" : ":" + With);
        }

        public string ToString(bool decal)
        {
            return VectorToString() + (With == -1 ? "" : " (with " + (decal ? "decal " : "block ") + With + ")");
        }
    }
}
