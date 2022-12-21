using WasmTetris.Game.BaseGameObject;

namespace WasmTetris.Game.Engine;

public interface IRenderEngine
{
    void AddGameObject(GameObject gameObject);
    void AddImageAsset(string imageAssetUrl);
    void DrawImageAsync(string imageUrl, float posX, float posY);
    void RemoveGameObject(GameObject gameObject);
    void StartRenderEngineAsync();
}