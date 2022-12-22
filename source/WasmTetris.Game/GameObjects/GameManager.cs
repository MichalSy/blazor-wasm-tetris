using WasmTetris.Game.BaseGameObject;
using WasmTetris.Game.Engine;

namespace WasmTetris.Game.GameObjects;

public class GameManager : GameObject
{
    private static Random _random = new((int)DateTime.Now.Ticks);
    private int nextCheckTime;

    private PlayerPieceGameObject? _playerPieceGameObject;

    private DateTime _lastInsert = DateTime.MinValue;

    public override void Update(IRenderEngine renderEngine, float time)
    {
        //if (_playerPieceGameObject is null || _playerPieceGameObject.IsDestroyed)
        //{
        //    _playerPieceGameObject = new();
        //    renderEngine.AddGameObject(_playerPieceGameObject);
        //}

        //_playerPieceGameObject.SetFieldSetup(200, 200, 32, 32);

        if (nextCheckTime == 0 || _lastInsert.AddMilliseconds(nextCheckTime) < DateTime.UtcNow)
        {
            nextCheckTime = _random.Next(300, 600);
            
                var nn = new PlayerPieceGameObject();
                nn.SetFieldSetup(150, 100, 32, 32);
                renderEngine.AddGameObject(nn);
            
            //Console.WriteLine("Add");
            //renderEngine.AddGameObject(new PlayerGO());
            //renderEngine.AddGameObject(new PlayerGO());
            //renderEngine.AddGameObject(new PlayerGO());
            //renderEngine.AddGameObject(new PlayerGO());
            //renderEngine.AddGameObject(new PlayerGO());

            _lastInsert = DateTime.UtcNow;
        }
    }
}
