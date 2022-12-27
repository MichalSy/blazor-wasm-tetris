﻿using System.Drawing;
using WasmTetris.Game.BaseGameObject;

namespace WasmTetris.Game.Engine;

public interface IRenderEngine
{
    event EventHandler<Size>? OnWindowSizeChanged;
    event EventHandler<Point>? OnTouchStarted;

    void AddGameObject(GameObject gameObject);
    void AddImageAsset(string imageAssetUrl);
    void AddDrawImageToRender(string imageUrl, int posX, int posY);
    void AddDrawRectWithBorderToRender(string htmlColor, int posX, int posY, int width, int height);
    void RemoveGameObject(GameObject gameObject);
    void StartRenderEngineAsync();
    void SetCanvasSizeAsync(Size newSize);
    void AddDrawStrokeRectToRender(int posX, int posY, int width, int height, string htmlColor, int lineWidth = 1, float alpha = 1, float shadowBlur = 0);
    void AddDrawLineToRender(int posX, int posY, int posEndX, int posEndY, string htmlColor, int lineWidth = 1, float alpha = 1);
    bool IsKeyDown(int keyCode);
    void AddDrawFillRectToRender(int posX, int posY, int width, int height, string htmlColor, float alpha = 1, float shadowBlur = 0);
}