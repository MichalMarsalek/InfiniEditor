using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniEditor
{
    public struct Vec
    {
        public int X;
        public int Y;
        public int Z;
        public Vec(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vec(Direction dir)
        {
            X = dir == Direction.PosX ? 1 : dir == Direction.NegX ? -1 : 0;
            Y = dir == Direction.PosY ? 1 : dir == Direction.NegY ? -1 : 0;
            Z = dir == Direction.PosZ ? 1 : dir == Direction.NegZ ? -1 : 0;
        }

        public override string ToString()
        {
            return "[" + X + ", " + Y + ", " + Z + "]";
        }

        public static Vec operator +(Vec v1, Vec v2)
        {
            return new Vec(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vec operator -(Vec v1, Vec v2)
        {
            return new Vec(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vec operator ^(Vec v, int rot)
        {
            int x = v.X;
            int z = v.Z;
            rot = rot.Mod(4);
            for(int i = 0; i < rot; i++)
            {
                int temp = x;
                x = -z;
                z = temp;
            }
            return new Vec(x, v.Y, z);
        }

        public static Vec operator *(Vec v, int q)
        {
            return new Vec(v.X * q, v.Y * q, v.Z * q);
        }

        public static Vec operator *(Vec v, Vec q)
        {
            return new Vec(v.X * q.X, v.Y * q.Y, v.Z * q.Z);
        }

        public static Vec operator |(Vec v, Axis axis)
        {
            return v * new Vec(axis == Axis.X ? -1 : 1, axis == Axis.Y ? -1 : 1, axis == Axis.Z ? -1 : 1);
        }

        public static bool operator ==(Vec v1, Vec v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        }
        public static bool operator !=(Vec v1, Vec v2)
        {
            return !(v1 == v2);
        }

        public override bool Equals(object obj)
        {
            return GetHashCode().Equals(obj.GetHashCode());
        }

        public override int GetHashCode()
        {
            return new { X, Y, Z }.GetHashCode();
        }
    }

    public enum Direction { PosX = 0, NegX = 1, PosY = 2, NegY = 3, PosZ = 4, NegZ = 5}
    public enum Axis { X, Y, Z }

    public static class DirectionRelatedOperations
    {
        public static Direction Opposite(this Direction dir)
        {
            return (Direction)(2 * (int)dir.Axis() + (1 + (int)dir) % 2);
        }

        public static Axis Axis(this Direction dir)
        {
            return (Axis)(((int)dir) / 2 );
        }

        public static Direction Rotate(this Direction dir, int rot)
        {
            if(dir.Axis() == InfiniEditor.Axis.Y)
            {
                return dir;
            }
            int angle = dir.Angle();
            angle = (angle + rot).Mod(4);
            return angle.AngleToDirection();
        }

        public static int Angle(this Direction dir)
        {
            return new Dictionary<Direction, int> { { Direction.PosY, -1 }, { Direction.NegY, -2 }, { Direction.PosZ, 0 }, { Direction.NegX, 1 }, { Direction.NegZ, 2 }, { Direction.PosX, 3 } }[dir];
        }
        public static Direction AngleToDirection(this int angle)
        {
            return new Dictionary<int, Direction> { { -1, Direction.PosY }, { -2, Direction.NegY }, { 0, Direction.PosZ }, { 1, Direction.NegX }, { 2, Direction.NegZ }, { 3, Direction.PosX } }[angle];
        }

        public static Direction Flip(this Direction dir, Axis axis)
        {
            return dir.Axis() == axis ? dir.Opposite() : dir;
        }

        public static Direction BlockDirection2WorldDirection(this Direction dir)
        {
            return new Direction[] { Direction.PosZ, Direction.NegZ, Direction.NegX, Direction.PosX }[(int)dir];
        }

        public static int WorldDirection2BlockDirection(this Direction dir)
        {
            return new Dictionary<int, int> { { 4, 0}, { 5, 1 }, { 1, 2 }, { 0, 3 }}[(int)dir];
        }

        public static int Difference(this Direction a, Direction b)
        {
            if(a.Angle() == -1 || b.Angle() == -1)
            {
                return -1;
            }
            return (a.Angle() - b.Angle()).Mod(4);
        }

        public static string ToXMLName(this Direction dir)
        {
            Dictionary<Direction, string> dict = new Dictionary<Direction, string> {
                { Direction.NegX, "-X"},
                { Direction.PosX, "+X"},
                { Direction.NegY, "-Y"},
                { Direction.PosY, "+Y"},
                { Direction.NegZ, "-Z"},
                { Direction.PosZ, "+Z"},
            };
            return dict[dir];
        }
        public static Direction XMLNameToDirection(this string dir)
        {
            Dictionary<string, Direction> dict = new Dictionary<string, Direction> {
                { "-X", Direction.NegX},
                { "+X", Direction.PosX},
                { "-Y", Direction.NegY},
                { "+Y", Direction.PosY},
                { "-Z", Direction.NegZ},
                { "+Z", Direction.PosZ},
            };
            return dict[dir];
        }

        public static int Mod(this int x, int m)
        {
            return (x % m + m) % m;
        }

    }
}
