using System.Drawing;

namespace WasmTetris.Game.GameObjects;

public static class PieceTypes
{
    public static IEnumerable<PieceConfig> AllTypes => new[]
    {
        new PieceConfig(true, new[] // T
        {
            new Point(-1, -1),
            new Point(0, -1),
            new Point(1, -1),
            new Point(0, 0),
            new Point(0, 1),
        }),
        new PieceConfig(true, new[] // Z
        {
            new Point(-1, -1),
            new Point(0, -1),
            new Point(0, 0),
            new Point(0, 1),
            new Point(1, 1),
        }),
        new PieceConfig(true, new[] // W
        {
            new Point(-1, 1),
            new Point(0, 1),
            new Point(0, 0),
            new Point(1, 0),
            new Point(1, -1),
        }),
        new PieceConfig(true, new[]
        {
            new Point(-1, 0),
            new Point(0, 0),
            new Point(1, 0),
            new Point(0, -1),
        }),
        new PieceConfig(true, new[]
        {
            new Point(-1, -1),
            new Point(0, -1),
            new Point(0, 0),
            new Point(1, 0),
        }),
        new PieceConfig(true, new[]
        {
            new Point(-1, 1),
            new Point(0, 1),
            new Point(0, 0),
            new Point(1, 0),
        }),
        new PieceConfig(true, new[]
        {
            new Point(0, -2),
            new Point(0, -1),
            new Point(0, 0),
            new Point(0, 1),
            new Point(0, 2),
        }),
        new PieceConfig(false, new[]
        {
            new Point(-1, -1),
            new Point(0, -1),
            new Point(-1, 0),
            new Point(0, 0),
        }),
        new PieceConfig(true, new[]
        {
            new Point(-1, -1),
            new Point(0, -1),
            new Point(0, 0),
            new Point(0, 1),
        })
    };
}
