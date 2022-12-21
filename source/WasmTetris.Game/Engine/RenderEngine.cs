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
        _activeGameObjects.Remove(gameObject);
    }

    public async void DrawImageAsync(string imageUrl, float posX, float posY)
    {
        await _jsReference!.InvokeVoidAsync("drawImage", imageUrl, (int)posX, (int)posY);
    }


    [JSInvokable]
    public void UpdateGameObjects(float deltaTime)
    {
        //Console.WriteLine(_activeGameObjects.Count);
        var activeItems = _activeGameObjects.ToArray();

        foreach (var gameObject in activeItems)
        {
            gameObject.Update(this, deltaTime);
        }

        foreach (var gameObject in activeItems)
        {
            gameObject.Render(this);
        }
    }
}
