using System.Drawing;
using Microsoft.JSInterop;
using WasmTetris.Game.BaseGameObject;

namespace WasmTetris.Game.Engine;

public class RenderEngine : IRenderEngine
{
    private readonly IJSRuntime _jsRuntime;
    private IJSObjectReference? _jsReference;

    private readonly List<string> _imageAssetsPreloadUrls = new();
    private readonly List<GameObject> _activeGameObjects = new();
    private readonly List<object> _nextRenderObjectStack = new();

    public event EventHandler<Size>? OnWindowSizeChanged;

    private bool[] _keyDownCache = new bool[200];

    public RenderEngine(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
    }

    public void AddImageAsset(string imageAssetUrl) => _imageAssetsPreloadUrls.Add(imageAssetUrl);

    public async void StartRenderEngineAsync()
    {
        _jsReference = await _jsRuntime.InvokeAsync<IJSObjectReference>("WasmTetris.createRenderEngineInstance", DotNetObjectReference.Create(this));
        await _jsReference.InvokeVoidAsync("loadImages", _imageAssetsPreloadUrls);
        await _jsReference.InvokeVoidAsync("startEngine");
    }

    public bool IsKeyDown(int keyCode) => _keyDownCache[keyCode];

    public void AddGameObject(GameObject gameObject) => _activeGameObjects.Add(gameObject);
    public void RemoveGameObject(GameObject gameObject)
    {
        gameObject.IsDestroyed = true;
        _activeGameObjects.Remove(gameObject);
    }

    public void AddDrawImageToRender(string imageUrl, int posX, int posY)
    {
        _nextRenderObjectStack.Add(new RenderObject<RenderImageObject>
        {
            Type = "Image",
            PositionX = posX,
            PositionY = posY,
            Data = new RenderImageObject
            {
                ImageUrl = imageUrl
            }
        });
    }
    public void AddDrawRectWithBorderToRender(string htmlColor, int posX, int posY, int width, int height)
    {
        _nextRenderObjectStack.Add(new RenderObject<RenderRectWithBorderObject>
        {
            Type = "RectWithBorder",
            PositionX = posX,
            PositionY = posY,
            Width = width,
            Height = height,
            Data = new RenderRectWithBorderObject
            {
                Color = htmlColor
            }
        });
    }

    public void AddDrawStrokeRectToRender(int posX, int posY, int width, int height, string htmlColor, int lineWidth = 1, float alpha = 1)
    {
        _nextRenderObjectStack.Add(new RenderObject<object>
        {
            Type = "StrokeRect",
            PositionX = posX,
            PositionY = posY,
            Width = width,
            Height = height,
            Data = new
            {
                Color = htmlColor,
                LineWidth = lineWidth,
                Alpha = alpha
            }
        });
    }

    public void AddDrawLineToRender(int posX, int posY, int posEndX, int posEndY, string htmlColor, int lineWidth = 1, float alpha = 1)
    {
        _nextRenderObjectStack.Add(new RenderObject<object>
        {
            Type = "Line",
            PositionX = posX,
            PositionY = posY,
            Width = 0,
            Height = 0,
            Data = new
            {
                Color = htmlColor,
                LineWidth = lineWidth,
                Alpha = alpha,
                PositionEndX = posEndX,
                PositionEndY = posEndY
            }
        });
    }


    public async void SetCanvasSizeAsync(Size newSize)
    {
        await _jsReference!.InvokeVoidAsync("setCanvasSize", newSize.Width, newSize.Height);
    }

    [JSInvokable]
    public async void SetWindowSize(int width, int height)
    {
        if (OnWindowSizeChanged is not null)
        {
            OnWindowSizeChanged.Invoke(this, new Size(width, height));
        }
        else
        {
            await _jsReference!.InvokeVoidAsync("setCanvasSize", width, height);
        }
    }

    [JSInvokable]
    public async void UpdateGameObjects(float deltaTime)
    {
        var activeItems = _activeGameObjects.ToArray();

        foreach (var gameObject in activeItems)
        {
            gameObject.Update(this, deltaTime);
        }

        foreach (var gameObject in activeItems)
        {
            gameObject.Render(this);
        }


        await _jsReference!.InvokeVoidAsync("drawObjects", _nextRenderObjectStack);
        _nextRenderObjectStack.Clear();
    }

    [JSInvokable]
    public void SendKeyUpdate(string eventType, int keyCode)
    {
        switch(eventType)
        {
            case "keydown":
                _keyDownCache[keyCode] = true;
                return;

            case "keyup":
                _keyDownCache[keyCode] = false;
                return;
        }
    }
}
