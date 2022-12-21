using WasmTetris.Game.BaseGameObject;
using WasmTetris.Game.Engine;

namespace WasmTetris.Game.GameObjects;

public class PlayerGO : ImageGameObject
{
    private DateTime _lifeSince = DateTime.UtcNow;

    public PlayerGO()
    {
        ImageUrl = "images/ChestBlue.png";
        PositionX = new Random().Next(0, 400);
        PositionY = new Random().Next(0, 400);
    }

    public override void Update(IRenderEngine renderEngine, float time)
    {
        PositionX += (int)Math.Round(10 * time);
        PositionY += (int)Math.Round(10 * time);

        if (_lifeSince.AddSeconds(3) < DateTime.UtcNow)
        {
            renderEngine.RemoveGameObject(this);
        }
    }

}
