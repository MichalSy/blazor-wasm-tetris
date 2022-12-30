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

        RenderEngine.AddSoundAsset("start.ogg");
        RenderEngine.AddSoundAsset("drop.ogg");
        RenderEngine.AddSoundAsset("rotate.ogg");
        RenderEngine.AddSoundAsset("move.ogg");
        RenderEngine.AddSoundAsset("bg.ogg");
        RenderEngine.AddSoundAsset("line1.ogg");
        RenderEngine.AddSoundAsset("line2.ogg");
        RenderEngine.AddSoundAsset("line3.ogg");
        RenderEngine.AddSoundAsset("line4.ogg");
        RenderEngine.AddSoundAsset("line5.ogg");

        RenderEngine.AddGameObject(new GameManager(RenderEngine));

    }

    protected override async void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            await RenderEngine.StartRenderEngineAsync();

            RenderEngine.PlaySound("bg.ogg", 0.4f, true);
        }
    }
}
