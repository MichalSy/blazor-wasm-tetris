namespace WasmTetris.Game.BaseGameObject;

public class RenderObject<T>
    where T : class
{
    public required string Type { get; set; }
    public int PositionX { get; set; }
    public int PositionY { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }

    public required T Data { get; set; }
}
