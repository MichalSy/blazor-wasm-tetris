using System.Drawing;
using WasmTetris.Game.BaseGameObject;
using WasmTetris.Game.Engine;

namespace WasmTetris.Game.GameObjects;

public class GameManager : GameObject
{
    private static readonly Random _random = new((int)DateTime.Now.Ticks);
    private readonly IRenderEngine _renderEngine;

    private static readonly int _fieldHorzMargin = 60;
    private static readonly int _maxPieceWidth = 32;


    private int _pieceWidth = 16;
    private int _fieldTopMargin = 100;
    private int _fieldWidth = 300;
    private int _fieldHeight = 500;
    private int _fieldLinesX = 12;
    private int _fieldLinesY = 25;
    private int _gameWidth = 300;
    private int _gameHeight = 600;

    private int nextCheckTime;
    private DateTime _lastInsert = DateTime.MinValue;
    

    public GameManager(IRenderEngine renderEngine)
    {
        _renderEngine = renderEngine ?? throw new ArgumentNullException(nameof(renderEngine));
        _renderEngine.OnWindowSizeChanged += RenderEngine_OnWindowSizeChanged;
    }


    // Recalculate game size
    private void RenderEngine_OnWindowSizeChanged(object? sender, Size windowSize)
    {
        int maxFieldWidth = windowSize.Width - (_fieldHorzMargin * 2);
        int maxPieceWidth = maxFieldWidth / 12;

        _pieceWidth = Math.Min(maxPieceWidth, _maxPieceWidth);
        _fieldTopMargin = (_pieceWidth * 5) + 100;

        _fieldWidth = _pieceWidth * _fieldLinesX;
        _fieldHeight = _pieceWidth * _fieldLinesY;
        _gameWidth = _fieldWidth + (_fieldHorzMargin * 2);
        _gameHeight = _fieldHeight + _fieldTopMargin;

        _renderEngine.SetCanvasSizeAsync(new Size(_gameWidth, _gameHeight));

        Console.WriteLine($"Game: {_gameWidth}x{_gameHeight}, Field: {_fieldWidth}x{_fieldHeight},  Piece: {_pieceWidth}");
    }

    public override void Update(IRenderEngine renderEngine, float time)
    {
        if (nextCheckTime == 0 || _lastInsert.AddMilliseconds(nextCheckTime) < DateTime.UtcNow)
        {
            nextCheckTime = _random.Next(300, 600);

            var nn = new PlayerPieceGameObject();
            nn.SetFieldSetup(_fieldHorzMargin, _fieldTopMargin, _pieceWidth, _pieceWidth);
            renderEngine.AddGameObject(nn);

            _lastInsert = DateTime.UtcNow;
        }
    }
}
