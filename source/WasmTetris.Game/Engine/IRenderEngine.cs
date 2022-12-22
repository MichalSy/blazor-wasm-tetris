using WasmTetris.Game.BaseGameObject;

namespace WasmTetris.Game.Engine;

public interface IRenderEngine
{
    void AddGameObject(GameObject gameObject);
    void AddImageAsset(string imageAssetUrl);
    void DrawImageAsync(string imageUrl, int posX, int posY);
    void DrawRectWithBorder(string htmlColor, int posX, int posY, int width, int height);
    void RemoveGameObject(GameObject gameObject);
    void StartRenderEngineAsync();
}