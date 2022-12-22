using WasmTetris.Game.BaseGameObject;
using WasmTetris.Game.Engine;

namespace WasmTetris.Game.GameObjects;

public class GameManager : GameObject
{
    private DateTime _lastInsert = DateTime.MinValue;

    public override void Update(IRenderEngine renderEngine, float time)
    {
        if (_lastInsert.AddMilliseconds(30) < DateTime.UtcNow)
        {
            //Console.WriteLine("Add");
            renderEngine.AddGameObject(new PlayerGO());
            renderEngine.AddGameObject(new PlayerGO());
            renderEngine.AddGameObject(new PlayerGO());
            renderEngine.AddGameObject(new PlayerGO());
            renderEngine.AddGameObject(new PlayerGO());

            _lastInsert = DateTime.UtcNow;
        }
    }
}
