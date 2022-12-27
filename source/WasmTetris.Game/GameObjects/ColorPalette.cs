using System.Drawing;

namespace WasmTetris.Game.GameObjects;

public static class ColorPalette
{
    private static Random _random = new((int)DateTime.Now.Ticks);
    public static string[] AllColors => new[]
    {
        "#d400e8",
        "#a813fb",
        "#6c66fb",
        "#339ffc",
        "#3ecfea",
        "#45edcf",
        "#49ff9e",
        "#48ff5e",
        "#fff44f",
        "#f7585a",
        "#f7009d",
        "#f700d2"
    };

    public static string GetRandomColor()
    {
        return AllColors[_random.Next(0, AllColors.Length)];
    }
}
