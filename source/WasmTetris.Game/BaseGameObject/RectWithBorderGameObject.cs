using WasmTetris.Game.Engine;

namespace WasmTetris.Game.BaseGameObject;

public class RectWithBorderGameObject : GameObject
{
    public string Color { get; init; } = default!;

    public override void Render(IRenderEngine renderEngine)
    {
        renderEngine.AddDrawRectWithBorderToRender(Color, (int)PositionY, (int)PositionX, Width, Height);
    }
}
