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
    private int _fieldTopMargin = 120;

    private int _fieldLinesX = 10;
    private int _fieldLinesY = 25;
    private int _fieldWidth = 300;
    private int _fieldHeight = 500;

    private int _gameWidth = 300;
    private int _gameHeight = 600;

    private int nextCheckTime;
    private DateTime _lastInsert = DateTime.MinValue;

    private FieldGameObject _fieldGO = new();
    private PlayerPieceGameObject? _currentPlayerPiece;


    public GameManager(IRenderEngine renderEngine)
    {
        _renderEngine = renderEngine ?? throw new ArgumentNullException(nameof(renderEngine));
        _renderEngine.OnWindowSizeChanged += RenderEngine_OnWindowSizeChanged;
        _renderEngine.OnTouchStarted += RenderEngine_OnTouchStarted;
    }




    // Recalculate game size
    private void RenderEngine_OnWindowSizeChanged(object? sender, Size windowSize)
    {
        int maxFieldWidth = windowSize.Width - (_fieldHorzMargin * 2);
        int maxPieceWidth = maxFieldWidth / _fieldLinesX;
        _pieceWidth = Math.Min(maxPieceWidth, _maxPieceWidth);

        int maxFieldHeight = windowSize.Height - _fieldTopMargin;
        int maxPieceHeight = maxFieldHeight / _fieldLinesY;

        _pieceWidth = Math.Min(maxPieceWidth, maxPieceHeight);

        _fieldWidth = _pieceWidth * _fieldLinesX;
        _fieldHeight = _pieceWidth * _fieldLinesY;
        _gameWidth = _fieldWidth + (_fieldHorzMargin * 2);
        _gameHeight = _fieldHeight + _fieldTopMargin;

        _renderEngine.SetCanvasSizeAsync(new Size(_gameWidth, _gameHeight));

        _fieldGO.SetFieldSetup(_fieldHorzMargin, _fieldTopMargin, _fieldLinesX, _fieldLinesY, _pieceWidth, _pieceWidth);

        //Console.WriteLine($"Game: {_gameWidth}x{_gameHeight}, Field: {_fieldWidth}x{_fieldHeight},  Piece: {_pieceWidth}");
    }

    private void RenderEngine_OnTouchStarted(object? sender, Point e)
    {
        if (e.X < _gameWidth / 2)
        {
            _currentPlayerPiece?.MoveLeft();
        }
        else
        {
            _currentPlayerPiece?.MoveRight();
        }
    }

    public override void Update(IRenderEngine renderEngine, float time)
    {


        if (_currentPlayerPiece == null || _currentPlayerPiece.IsDestroyed)
        {
            _currentPlayerPiece = new PlayerPieceGameObject(_fieldGO);
            _currentPlayerPiece.SetFieldSetup(_fieldHorzMargin, _fieldTopMargin, _fieldLinesX, _pieceWidth, _pieceWidth);
            renderEngine.AddGameObject(_currentPlayerPiece);
        }

        //if (nextCheckTime == 0 || _lastInsert.AddMilliseconds(nextCheckTime) < DateTime.UtcNow)
        //{
        //    nextCheckTime = _random.Next(5000, 6000);

        //    var nn = new PlayerPieceGameObject(_fieldGO);
        //    nn.SetFieldSetup(_fieldHorzMargin, _fieldTopMargin, _fieldLinesX, _pieceWidth, _pieceWidth);
        //    renderEngine.AddGameObject(nn);

        //    _lastInsert = DateTime.UtcNow;
        //}
    }

    public override void Render(IRenderEngine renderEngine)
    {
        _fieldGO.Render(renderEngine);

    }
}
