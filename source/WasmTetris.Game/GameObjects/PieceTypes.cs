using System;
using System.Drawing;

namespace WasmTetris.Game.GameObjects;

public static class PieceTypes
{
    private static readonly Random _random = new((int)DateTime.Now.Ticks);
    public static PieceConfig GetRandomPiece() => AllTypes.Skip(_random.Next(0, AllTypes.Count())).First();

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

        new PieceConfig(true, new[] // W
        {
            new Point(-1, 1),
            new Point(0, 1),
            new Point(0, 0),
            new Point(1, 0),
            new Point(1, -1),
        }),

        //   X
        //  XXX
        new PieceConfig(true, new[] 
        {
            new Point(-1, 0),
            new Point(0, 0),
            new Point(1, 0),
            new Point(0, -1),
        }),

        //  XX
        //   XX
        new PieceConfig(true, new[]
        {
            new Point(-1, -1),
            new Point(0, -1),
            new Point(0, 0),
            new Point(1, 0),
        }),

        //   XX
        //  XX
        new PieceConfig(true, new[]
        {
            new Point(-1, 1),
            new Point(0, 1),
            new Point(0, 0),
            new Point(1, 0),
        }),

        ////  XXXXXX
        //new PieceConfig(true, new[]
        //{
        //    new Point(-2, 0),
        //    new Point(-1, 0),
        //    new Point(0, 0),
        //    new Point(1, 0),
        //    new Point(2, 0),
        //}),

        //  XXX
        new PieceConfig(true, new[]
        {
            new Point(-1, 0),
            new Point(0, 0),
            new Point(1, 0),
        }),

        //  X
        //  X
        //  X
        //  X
        //  X
        new PieceConfig(true, new[]
        {
            new Point(0, -2),
            new Point(0, -1),
            new Point(0, 0),
            new Point(0, 1),
            new Point(0, 2),
        }),

        //  X
        //  X
        //  X
        new PieceConfig(true, new[]
        {
            new Point(0, -1),
            new Point(0, 0),
            new Point(0, 1),
        }),

        //  XX
        //  XX
        new PieceConfig(false, new[]
        {
            new Point(-1, -1),
            new Point(0, -1),
            new Point(-1, 0),
            new Point(0, 0),
        }),

        // XX
        //  X
        //  X
        new PieceConfig(true, new[]
        {
            new Point(-1, -1),
            new Point(0, -1),
            new Point(0, 0),
            new Point(0, 1),
        }),

        //  XX
        //  X
        //  X
        new PieceConfig(true, new[]
        {
            new Point(0, -1),
            new Point(1, -1),
            new Point(0, 0),
            new Point(0, 1),
        }),
    };
}
