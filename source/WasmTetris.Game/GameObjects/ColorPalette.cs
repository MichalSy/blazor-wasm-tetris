namespace WasmTetris.Game.GameObjects;

public static class ColorPalette
{
    private static readonly Random _random = new((int)DateTime.Now.Ticks);
    public static string GetRandomColor() => AllColors.Skip(_random.Next(0, AllColors.Count())).First();

    public static IEnumerable<string> AllColors => new[]
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
}
