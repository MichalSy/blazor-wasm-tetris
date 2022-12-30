using System.Drawing;
using WasmTetris.Game.BaseGameObject;

namespace WasmTetris.Game.Engine;

public interface IRenderEngine
{
    event EventHandler<Size>? OnWindowSizeChanged;
    event EventHandler<Point>? OnTouchStarted;
    event EventHandler<int>? OnKeyDown;

    void AddGameObject(GameObject gameObject);
    void AddImageAsset(string imageAssetUrl);
    void AddDrawImageToRender(string imageUrl, int posX, int posY);
    void AddDrawRectWithBorderToRender(string htmlColor, int posX, int posY, int width, int height);
    void RemoveGameObject(GameObject gameObject);
    Task StartRenderEngineAsync();
    void SetCanvasSizeAsync(Size newSize);
    void AddDrawStrokeRectToRender(int posX, int posY, int width, int height, string htmlColor, int lineWidth = 1, float alpha = 1, float shadowBlur = 0);
    void AddDrawLineToRender(int posX, int posY, int posEndX, int posEndY, string htmlColor, int lineWidth = 1, float alpha = 1);
    bool IsKeyDown(int keyCode);
    void AddDrawFillRectToRender(int posX, int posY, int width, int height, string htmlColor, float alpha = 1, float shadowBlur = 0);
    void PlaySound(string soundFileName, float volume = 1, bool loop = false);
    void AddSoundAsset(string soundAssetUrl);
    void AddDrawTextToRender(int posX, int posY, string text, string htmlColor, string textAlign = "left", int fontSize = 15, float alpha = 1, float shadowBlur = 0);
}