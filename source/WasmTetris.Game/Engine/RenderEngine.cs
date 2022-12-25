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
}
