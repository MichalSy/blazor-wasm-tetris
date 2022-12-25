using System.Data;
using WasmTetris.Game.Engine;

namespace WasmTetris.Game.BaseGameObject;

public class ImageGameObject : GameObject
{
    public string ImageUrl { get; init; } = default!;

    public override void Render(IRenderEngine renderEngine)
    {
        renderEngine.AddDrawImageToRender(ImageUrl, (int)PositionY, (int)PositionX);
    }
}
