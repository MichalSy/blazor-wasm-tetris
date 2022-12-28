using Microsoft.AspNetCore.Components;
using WasmTetris.Game.Engine;
using WasmTetris.Game.GameObjects;

namespace WasmTetris.Game;

public partial class Game
{
    [Inject]
    public required IRenderEngine RenderEngine { get; set; }

    protected override void OnInitialized()
    {
        RenderEngine.AddImageAsset("images/ChestBlue.png");
        RenderEngine.AddImageAsset("images/tile.png");

        RenderEngine.AddGameObject(new GameManager(RenderEngine));

    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            RenderEngine.StartRenderEngineAsync();
        }
    }
}
