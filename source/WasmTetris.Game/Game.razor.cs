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

        RenderEngine.AddSoundAsset("drop.ogg");
        RenderEngine.AddSoundAsset("rotate.ogg");
        RenderEngine.AddSoundAsset("move.ogg");
        RenderEngine.AddSoundAsset("bg.ogg");

        RenderEngine.AddGameObject(new GameManager(RenderEngine));

    }

    protected override async void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            await RenderEngine.StartRenderEngineAsync();

            RenderEngine.PlaySound("bg.ogg", 0.1f, true);
        }
    }
}
