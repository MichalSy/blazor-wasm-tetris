using System.ComponentModel;
using Microsoft.JSInterop;
using WasmTetris.Game.BaseGameObject;

namespace WasmTetris.Game.Engine;

public class RenderEngine : IRenderEngine
{
    private readonly IJSRuntime _jsRuntime;
    private IJSObjectReference? _jsReference;

    private List<string> _imageAssetUrls = new();

    private List<GameObject> _activeGameObjects = new();

    private List<object> _nextRenderStack = new();

    public RenderEngine(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
    }

    public void AddImageAsset(string imageAssetUrl) => _imageAssetUrls.Add(imageAssetUrl);

    public async void StartRenderEngineAsync()
    {
        _jsReference = await _jsRuntime.InvokeAsync<IJSObjectReference>("WasmTetris.createRenderEngineInstance", DotNetObjectReference.Create(this));
        await _jsReference.InvokeVoidAsync("loadImages", _imageAssetUrls);
        await _jsReference.InvokeVoidAsync("startEngine", _imageAssetUrls);
    }

    public void AddGameObject(GameObject gameObject)
    {
        //Console.WriteLine(_activeGameObjects.Count);
        _activeGameObjects.Add(gameObject);
    }

    public void RemoveGameObject(GameObject gameObject)
    {
        gameObject.IsDestroyed = true;
        _activeGameObjects.Remove(gameObject);
    }

    public void DrawImageAsync(string imageUrl, int posX, int posY)
    {
        _nextRenderStack.Add(new RenderObject<RenderImageObject>
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

    public void DrawRectWithBorder(string htmlColor, int posX, int posY, int width, int height)
    {
        _nextRenderStack.Add(new RenderObject<RenderRectWithBorderObject>
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

    [JSInvokable]
    public async void ResizeGame(int width, int height)
    {
        width = 400;
        height = 600;

        await _jsReference!.InvokeVoidAsync("setCanvasSize", width, height);
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


        await _jsReference!.InvokeVoidAsync("drawObjects", _nextRenderStack);
        _nextRenderStack.Clear();
    }
}
