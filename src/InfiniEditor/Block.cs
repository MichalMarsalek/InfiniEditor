using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InfiniEditor
{
    public class Block
    {
        public Roles Role;
        public int Group;
        public int Type;
        public Vec Position;
        public int RelativeFacing;
        public int State;
        public Dictionary<Direction, int> Decals;
        public Direction Facing
        {
            get
            {
                return new Direction[]
                {
                    Direction.PosZ,
                    Direction.NegZ,
                    Direction.NegX,
                    Direction.PosX
                }[RelativeFacing];
            }
            set
            {
                RelativeFacing = new Dictionary<Direction, int>
                {
                    {Direction.PosZ, 0 },
                    {Direction.NegZ, 1 },
                    {Direction.NegX, 2 },
                    {Direction.PosX, 3 }
                }[value];
            }
        }
        public int? this[Direction f]
        {
            get
            {
                if (Decals.ContainsKey(f))
                {
                    return Decals[f];
                }
                return null;
            }
            set
            {
                if(value == null)
                {
                    Decals.Remove(f);
                }
                else
                {
                    Decals[f] = (int)value;
                }
            }
        }



        public Block(int type, Direction facing = Direction.NegX, Roles role = Roles.World, int group = 0, int state = 0, Dictionary<Direction, int> decals = null)
        {
            Role = role;
            Group = group;
            Type = type;
            Facing = facing;
            State = state;
            Decals = decals == null ? new Dictionary<Direction, int>() : decals;
        }
        public Block(BinaryReader stream, Roles role, int group)
        {
            Role = role;
            Group = group;
            Type = stream.ReadInt16();
            int x = stream.ReadInt16();
            int y = stream.ReadInt16();
            int z = stream.ReadInt16();
            Position = new Vec(x, y, z);
            RelativeFacing = stream.ReadByte();
            State = stream.ReadByte();
            int ndecals = stream.ReadByte();
            Decals = new Dictionary<Direction, int>();
            for (int j = 0; j < ndecals; j++)
            {
                Direction facing = (Direction)stream.ReadByte();
                if (facing.Axis() != Axis.Y)
                {
                    int rot = Facing.Angle();
                    facing = facing.Rotate(rot);
                }
                int type = stream.ReadInt16();
                Decals.Add(facing, type);
            }
        }
        public Block(XElement xEl, Roles role, int group)
        {
            Group = group;
            Type = (int)xEl.Attribute("Type");
            int x = (int)xEl.Attribute("X");
            int y = (int)xEl.Attribute("Y");
            int z = (int)xEl.Attribute("Z");
            Position = new Vec(x, y, z);
            Facing = ((string)xEl.Attribute("Facing")).XMLNameToDirection();
            State = xEl.Attribute("State") == null ? 0 : (int)xEl.Attribute("State");
            Decals = new Dictionary<Direction, int>();
            foreach(XElement e in xEl.Elements("Decal"))
            {
                Direction facing = ((string)xEl.Attribute("Facing")).XMLNameToDirection();
                int type = ((int)xEl.Attribute("Type"));
                Decals.Add(facing, type);
            }
        }
        public Block(Block old)
        {
            Role = old.Role;
            Type = old.Type;
            Position = old.Position;
            RelativeFacing = old.RelativeFacing;
            State = old.State;
            Decals = new Dictionary<Direction, int>(old.Decals);
        }

        public override string ToString()
        {
            return "{" + Type + " " + Position.ToString() + " " + RelativeFacing + ", " + State + String.Join(", ", Decals) + "}";
        }

        public void SaveToStream(BinaryWriter stream)
        {
            stream.Write(BitConverter.GetBytes((short)Type));
            stream.Write(BitConverter.GetBytes((short)Position.X));
            stream.Write(BitConverter.GetBytes((short)Position.Y));
            stream.Write(BitConverter.GetBytes((short)Position.Z));
            stream.Write((byte)RelativeFacing);
            stream.Write((byte)State);
            stream.Write((byte)Decals.Count());
            foreach (var pair in Decals)
            {
                if (pair.Value >= 0)
                {
                    Direction facing = pair.Key;
                    if (facing.Axis() != Axis.Y)
                    {
                        int rot = Facing.Angle();
                        facing = facing.Rotate(-rot);
                    }
                    stream.Write((byte)facing);
                    stream.Write(BitConverter.GetBytes((short)pair.Value));
                }
            }
        }

        public XElement ToXElement()
        {
            return new XElement("Block",
                new XAttribute("Type", Type),
                new XAttribute("X", Position.X),
                new XAttribute("Y", Position.Y),
                new XAttribute("Z", Position.Z),
                new XAttribute("Facing", Facing.ToXMLName()),
                State != 0 ? new XAttribute("State", State) : null,
                from decal in Decals.Where(p => p.Value >= 0) select
                    new XElement("Decal",
                        new XAttribute("Type", decal.Value),
                        new XAttribute("Facing", decal.Key.ToXMLName())
                    )
            );
        }

        public void RotateWithDecals(int rot)
        {
            RotateWithoutDecals(rot);
            var decals = new Dictionary<Direction, int>();
            foreach (var decal in Decals)
            {
                decals.Add(decal.Key.Rotate(rot), decal.Value);
            }
            Decals = decals;
        }

        public void RotateWithoutDecals(int rot)
        {
            Facing = Facing.Rotate(rot);
        }

        public void Replace(int from, int to)
        {
            if (Type == from)
            {
                Type = to;
            }
        }

        public void Flip(Axis axis, BlockInfo block)
        {
            Position |= axis;

            if (axis == Axis.Y)
            {
                if (block.Symmetries.ContainsKey("Y"))
                {
                    int with = block.Symmetries["Y"].With;
                    if (with != -1)
                    {
                        Type = with;
                    }
                }
            }
            if (axis == Axis.X)
            {
                if (block.Symmetries.ContainsKey("X"))
                {
                    int with = block.Symmetries["X"].With;
                    if(with != -1)
                    {
                        Type = with;
                    }
                }
                if (block.Symmetries.ContainsKey("Z"))
                {
                    int with = block.Symmetries["Z"].With;
                    if (with != -1)
                    {
                        Type = with;
                    }

                }
            }
            if (axis == Axis.Z)
            {
                if (block.Symmetries.ContainsKey("Z"))
                {
                    int with = block.Symmetries["Z"].With;
                    if (with != -1)
                    {
                        Type = with;
                    }
                }
            }
            Facing = Facing.Flip(axis);
            var decals = new Dictionary<Direction, int>();
            foreach (var decal in Decals)
            {
                decals.Add(decal.Key.Flip(axis), decal.Value);
            }
            Decals = decals;
        }

        public enum Roles { World, In, Out }
    }
}
