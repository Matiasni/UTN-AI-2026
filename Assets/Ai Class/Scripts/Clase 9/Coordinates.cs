using System.Collections.Generic;

public struct Coordinates
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Coordinates Up    => new(X, Y + 1);
    public Coordinates Down  => new(X, Y - 1);
    public Coordinates Right => new(X + 1, Y);
    public Coordinates Left  => new(X - 1, Y);

    public List<Coordinates> GetAllAdyacentDirections => new() { Up, Left, Down, Right };

    public Coordinates(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString() => $"({X},{Y})";
}
