using WasmTetris.Game.Engine;

namespace WasmTetris.Game.BaseGameObject;

public class GameObject
{
    public float PositionX { get; set; } = 0;
    public float PositionY { get; set; } = 0;

    public int Width { get; set; }
    public int Height { get; set; }

    public bool IsDestroyed { get; set; }

    public virtual void Update(IRenderEngine renderEngine, float time)
    {
    }

    public virtual void Render(IRenderEngine renderEngine)
    {
    }
}
