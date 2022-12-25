using System.Drawing;
using WasmTetris.Game.BaseGameObject;

namespace WasmTetris.Game.Engine;

public interface IRenderEngine
{
    event EventHandler<Size>? OnWindowSizeChanged;

    void AddGameObject(GameObject gameObject);
    void AddImageAsset(string imageAssetUrl);
    void AddDrawImageToRender(string imageUrl, int posX, int posY);
    void AddDrawRectWithBorderToRender(string htmlColor, int posX, int posY, int width, int height);
    void RemoveGameObject(GameObject gameObject);
    void StartRenderEngineAsync();
    void SetCanvasSizeAsync(Size newSize);
}