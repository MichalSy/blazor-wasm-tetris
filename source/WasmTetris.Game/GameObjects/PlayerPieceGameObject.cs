using System;
using System.Drawing;
using WasmTetris.Game.BaseGameObject;
using WasmTetris.Game.Engine;

namespace WasmTetris.Game.GameObjects;

public class PlayerPieceGameObject : GameObject
{
    private static Random _random = new((int)DateTime.Now.Ticks);
    private int _fieldPositionX;
    private int _fieldPositionY;
    private int _fieldLinesX;
    private int _pieceWidth;
    private int _pieceHeight;

    private int _piecePositionX;
    private int _piecePositionY;

    private string _color = string.Format("#{0:X6}", _random.Next(0x1000000));

    private IEnumerable<Point> _currentPiece = PieceTypes.AllTypes.First();

    private DateTime _lastRotate = DateTime.UtcNow;

    private DateTime _lifeSince = DateTime.UtcNow;

    private int _nextRotateDelay = _random.Next(200, 600);


    private int _posXIndex = 8;


    public PlayerPieceGameObject()
    {
        var allPieces = PieceTypes.AllTypes.ToArray();
        _currentPiece = allPieces[_random.Next(0, allPieces.Length - 1)];

        RotateRight();
    }

    internal void SetFieldSetup(int fieldPositionX, int fieldPositionY, int fieldLinesX, int pieceWidth, int pieceHeight)
    {
        _fieldPositionX = fieldPositionX;
        _fieldPositionY = fieldPositionY;
        _fieldLinesX = fieldLinesX;
        _pieceWidth = pieceWidth;
        _pieceHeight = pieceHeight;

        _posXIndex = _random.Next(0, _fieldLinesX);
    }

    private void RotateRight()
    {
        var oldPositions = _currentPiece.ToArray();
        List<Point> newPos = new();

        foreach (var piece in _currentPiece)
        {
            newPos.Add(new Point(-piece.Y, piece.X));
        }
        _currentPiece = newPos;
    }

    private void RotateLeft()
    {
        var oldPositions = _currentPiece.ToArray();
        List<Point> newPos = new();

        foreach (var piece in _currentPiece)
        {
            newPos.Add(new Point(piece.Y, -piece.X));
        }
        _currentPiece = newPos;
    }

    public override void Update(IRenderEngine renderEngine, float time)
    {
        //if (_lastRotate.AddMilliseconds(_nextRotateDelay) < DateTime.UtcNow)
        //{
        //    RotateRight();
        //    _lastRotate = DateTime.UtcNow;
        //    _nextRotateDelay = _random.Next(200, 600);
        //}

        int xdiff = _posXIndex + _currentPiece.Min(p => p.X);
        if (xdiff < 0)
        {
            _posXIndex -= xdiff;
        }
        xdiff = _fieldLinesX - 1 - (_posXIndex + _currentPiece.Max(p => p.X));
        if (xdiff < 0)
        {
            _posXIndex += xdiff;
        }

        _piecePositionX = _posXIndex * _pieceWidth;
        _piecePositionY += (int)Math.Ceiling(20 * time);

        if (_lifeSince.AddSeconds(6) < DateTime.UtcNow)
        {
            renderEngine.RemoveGameObject(this);
        }
    }

    public override void Render(IRenderEngine renderEngine)
    {
        foreach (var piece in _currentPiece)
        {
            var posX = _fieldPositionX + _piecePositionX + (piece.X * _pieceWidth);
            var posY = _fieldPositionY + _piecePositionY + (piece.Y * _pieceWidth);
            renderEngine.AddDrawRectWithBorderToRender(_color, posX, posY, _pieceWidth, _pieceHeight);
        }


    }


}
