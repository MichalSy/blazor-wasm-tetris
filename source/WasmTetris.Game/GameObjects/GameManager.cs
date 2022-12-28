using System.Drawing;
using WasmTetris.Game.BaseGameObject;
using WasmTetris.Game.Engine;

namespace WasmTetris.Game.GameObjects;

public class GameManager : GameObject
{
    private readonly IRenderEngine _renderEngine;

    private static readonly int _fieldHorzMargin = 60;
    private static readonly int _maxPieceWidth = 32;

    private int _pieceWidth = 16;
    private readonly int _fieldTopMargin = 120;

    private readonly int _fieldLinesX = 11;
    private readonly int _fieldLinesY = 25;
    private int _fieldWidth = 300;
    private int _fieldHeight = 500;

    private int _gameWidth = 300;
    private int _gameHeight = 600;

    private readonly FieldGameObject _fieldGO = new();
    private PlayerPieceGameObject? _currentPlayerPiece;


    public GameManager(IRenderEngine renderEngine)
    {
        _renderEngine = renderEngine ?? throw new ArgumentNullException(nameof(renderEngine));
        _renderEngine.OnWindowSizeChanged += RenderEngine_OnWindowSizeChanged;
        _renderEngine.OnTouchStarted += RenderEngine_OnTouchStarted;
        _renderEngine.OnKeyDown += RenderEngine_OnKeyDown;
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

    private void RenderEngine_OnKeyDown(object? sender, int keyCode)
    {
        if (_currentPlayerPiece is null)
            return;

        switch (keyCode)
        {
            case 17: // crtl
                _currentPlayerPiece.RotateLeft();
                return;

            case 37: // Arrow Left
                _currentPlayerPiece.MoveLeft();
                return;

            case 38: // Arrow Up
                _currentPlayerPiece.RotateRight();
                return;

            case 39: // Arrow Right
                _currentPlayerPiece.MoveRight();
                return;
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
    }

    public override void Render(IRenderEngine renderEngine)
    {
        _fieldGO.Render(renderEngine);

    }
}
