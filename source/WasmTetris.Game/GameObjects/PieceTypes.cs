using System.Drawing;

namespace WasmTetris.Game.GameObjects;

public static class PieceTypes
{
    public static IEnumerable<IEnumerable<Point>> AllTypes => new[]
    {
        new[] // T
        {
            new Point(-1, -1),
            new Point(0, -1),
            new Point(1, -1),
            new Point(0, 0),
            new Point(0, 1),
        },
        new[] // Z
        {
            new Point(-1, -1),
            new Point(0, -1),
            new Point(0, 0),
            new Point(0, 1),
            new Point(1, 1),
        },
        new[] // W
        {
            new Point(-1, 1),
            new Point(0, 1),
            new Point(0, 0),
            new Point(1, 0),
            new Point(1, -1),
        },
        new[]
        {
            new Point(-1, 0),
            new Point(0, 0),
            new Point(1, 0),
            new Point(0, -1),
        },
        new[]
        {
            new Point(-1, -1),
            new Point(0, -1),
            new Point(0, 0),
            new Point(1, 0),
        },
        new[]
        {
            new Point(-1, 1),
            new Point(0, 1),
            new Point(0, 0),
            new Point(1, 0),
        },
        new[]
        {
            new Point(0, -2),
            new Point(0, -1),
            new Point(0, 0),
            new Point(0, 1),
            new Point(0, 2),
        },
        new[]
        {
            new Point(-1, -1),
            new Point(0, -1),
            new Point(-1, 0),
            new Point(0, 0),
        },
        new[]
        {
            new Point(-1, -1),
            new Point(0, -1),
            new Point(0, 0),
            new Point(0, 1),
        }
        
    };
}
