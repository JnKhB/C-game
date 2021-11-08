using System;

namespace RollerCoasterTycoon.Model
{
    public partial class AmusementPark
    {
        public struct Position
        {
            public Position(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; set; }
            public int Y { get; set; }

            public override bool Equals(object obj)
            {
                return obj is Position position &&
                       X == position.X &&
                       Y == position.Y;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }

            public override string ToString()
            {
                return "(" + X + ", " + Y + ")";
            }

            public static bool operator ==(Position p1, Position p2)
            {
                return p1.Equals(p2);
            }

            public static bool operator !=(Position p1, Position p2)
            {
                return !p1.Equals(p2);
            }
        }

    }
}
