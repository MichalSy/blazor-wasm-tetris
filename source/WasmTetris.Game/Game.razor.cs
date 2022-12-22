using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WasmTetris.Game.BaseGameObject;
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

        //renderEngine.AddGameObject(new RectWithBorderGameObject
        //{
        //    Color = "#ff0000",
        //    Width = 32,
        //    Height = 32,
        //    PositionX = 100,
        //    PositionY = 100
        //});
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            renderEngine.StartRenderEngineAsync();
        }
    }
}
