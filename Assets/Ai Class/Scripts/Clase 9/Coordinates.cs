using System.Collections.Generic;

namespace Clase9
{
    public struct Coordinates
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Coordinates Up    => new(X, Y + 1);
        public Coordinates Down  => new(X, Y - 1);
        public Coordinates Right => new(X + 1, Y);
        public Coordinates Left  => new(X - 1, Y);

        public List<Coordinates> GetAllAdyacentDirections => new() { Up, Left, Down, Right };

        // Otra forma de escribir lo mismo usando un iterador:
        //public IEnumerable<Coordinates> GetAllAdyacentDirections
        //{
        //    get
        //    {
        //        yield return Up;
        //        yield return Left;
        //        yield return Down;
        //        yield return Right;
        //    }
        //}

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X},{Y})";
    }
}
