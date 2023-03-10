using System.Drawing;
using WasmTetris.Game.BaseGameObject;
using WasmTetris.Game.Engine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace WasmTetris.Game.GameObjects;

public class GameManager : GameObject
{
    private readonly IRenderEngine _renderEngine;

    private static readonly int _fieldHorzMargin = 40;
    private static readonly int _maxPieceWidth = 32;

    private int _pieceWidth = 16;
    private readonly int _fieldTopMargin = 60;

    private readonly int _fieldLinesX = 11;
    private readonly int _fieldLinesY = 25;
    private int _fieldWidth = 300;
    private int _fieldHeight = 500;

    private int _gameWidth = 300;
    private int _gameHeight = 600;

    private readonly FieldGameObject _fieldGO;
    private readonly StartGameUIGameObject _startGameUI = new();
    private readonly ScoreInfoGameObject _scoreInfoUI = new();
    private PlayerPieceGameObject? _currentPlayerPiece;

    private bool _isGameRunning = false;

    private int _currentGameLines = 0;
    public int CurrentGameLines => _currentGameLines;

    private DateTime? _currentGameStart;
    private int _currentGameScore = 0;
    private TimeSpan _currentGameTime = TimeSpan.Zero;

    

    public GameManager(IRenderEngine renderEngine)
    {
        _fieldGO = new(this);
        _renderEngine = renderEngine ?? throw new ArgumentNullException(nameof(renderEngine));
        _renderEngine.OnWindowSizeChanged += RenderEngine_OnWindowSizeChanged;
        _renderEngine.OnTouchStarted += RenderEngine_OnTouchStarted;
        _renderEngine.OnKeyDown += RenderEngine_OnKeyDown;
        _renderEngine.OnKeyUp += RenderEngine_OnKeyUp;
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

        _startGameUI.SetCenterPoint(_fieldHorzMargin + (_fieldWidth / 2), _fieldTopMargin + (_fieldHeight / 2), _fieldWidth);
        _scoreInfoUI.SetFieldPosition(_fieldHorzMargin, _fieldWidth);
        //Console.WriteLine($"Game: {_gameWidth}x{_gameHeight}, Field: {_fieldWidth}x{_fieldHeight},  Piece: {_pieceWidth}");
    }

    private void RenderEngine_OnTouchStarted(object? sender, Point e)
    {
        if (!_isGameRunning)
        {
            StartGame();
            return;
        }

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
        if (!_isGameRunning)
        {
            StartGame();
            return;
        }

        switch (keyCode)
        {
            case 27: // esc
                GameOver();
                return;

            case 17: // crtl
                _currentPlayerPiece?.RotateLeft();
                return;

            case 37: // Arrow Left
                _currentPlayerPiece?.MoveLeft();
                return;

            case 38: // Arrow Up
                _currentPlayerPiece?.RotateRight();
                return;

            case 39: // Arrow Right
                _currentPlayerPiece?.MoveRight();
                return;

            case 40: // Arrow Down
                _currentPlayerPiece?.SetTurbo(true);
                return;
        }
    }

    private void RenderEngine_OnKeyUp(object? sender, int keyCode)
    {
        if (!_isGameRunning)
        {
            return;
        }

        switch (keyCode)
        {
            case 40: // Arrow Down
                _currentPlayerPiece?.SetTurbo(false);
                return;
        }
    }

    private void StartGame()
    {
        _currentGameStart = null;
        _currentGameLines = 0;
        _currentGameScore = 0;
        _scoreInfoUI.Reset();

        _isGameRunning = true;
        _currentGameStart = DateTime.UtcNow;
        _renderEngine.PlaySound("start.ogg", 0.2f);
    }

    internal void GameOver()
    {
        _isGameRunning = false;
        if (_currentPlayerPiece is not null)
        {
            _renderEngine.RemoveGameObject(_currentPlayerPiece);
        }
        _fieldGO.ClearField();
        
    }

    public void AddScore(int addPoints, int addLines = 0)
    {
        _currentGameScore += addPoints;
        if (addLines > 0)
        {
            _currentGameLines += addLines;
        }
    }

    public override void Update(IRenderEngine renderEngine, float time)
    {
        if (_isGameRunning)
        {
            _fieldGO.Update(renderEngine, time);
            
            if ((_currentPlayerPiece == null || _currentPlayerPiece.IsDestroyed))
            {
                if (!Enumerable.Range(0, _fieldLinesX).All(i => _fieldGO.IsFieldPositionEmpty(i, 0)))
                {
                    GameOver();
                    return;
                }

                _currentPlayerPiece = new PlayerPieceGameObject(renderEngine, this, _fieldGO);
                _currentPlayerPiece.SetFieldSetup(_fieldHorzMargin, _fieldTopMargin, _fieldLinesX, _pieceWidth, _pieceWidth);
                renderEngine.AddGameObject(_currentPlayerPiece);
            }

            if (_currentGameStart is not null)
            {
                _currentGameTime = DateTime.UtcNow - _currentGameStart.Value;
            }

            _scoreInfoUI.SetValues(_currentGameTime, _currentGameLines, _currentGameScore);
        } 
    }

    public override void Render(IRenderEngine renderEngine)
    {
        _fieldGO.Render(renderEngine);
        _scoreInfoUI.Render(renderEngine);

        if (!_isGameRunning)
        {
            _startGameUI.Render(renderEngine);
        }
    }

    
}
