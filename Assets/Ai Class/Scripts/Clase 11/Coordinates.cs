using System.Collections.Generic;

namespace Clase11
{
    public struct Coordinates
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Coordinates Up    => new(X, Y + 1);
        public Coordinates Down  => new(X, Y - 1);
        public Coordinates Right => new(X + 1, Y);
        public Coordinates Left  => new(X - 1, Y);
        public Coordinates UpRight   => new(X + 1, Y + 1);
        public Coordinates UpLeft    => new(X - 1, Y + 1);
        public Coordinates DownRight => new(X + 1, Y - 1);
        public Coordinates DownLeft  => new(X - 1, Y - 1);

        public List<Coordinates> GetAllAdyacentDirections => new() { Up, Left, Down, Right };
        public List<Coordinates> GetAllDiagonalDirections => new() { UpRight, UpLeft, DownRight, DownLeft };

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X},{Y})";
    }
}
