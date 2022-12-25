using Microsoft.AspNetCore.Components;
using WasmTetris.Game.Engine;
using WasmTetris.Game.GameObjects;

namespace WasmTetris.Game;

public partial class Game
{
    [Inject]
    public required IRenderEngine renderEngine { get; set; }

    protected override void OnInitialized()
    {
        renderEngine.AddImageAsset("images/ChestBlue.png");
        renderEngine.AddImageAsset("images/tile.png");

        renderEngine.AddGameObject(new GameManager());

    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            renderEngine.StartRenderEngineAsync();
        }
    }
}
