using System;
using System.Drawing;
using System.IO.Pipelines;
using System.Net.NetworkInformation;
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

    private string _color = ColorPalette.GetRandomColor();

    private IEnumerable<Point> _currentPiece = PieceTypes.AllTypes.First();

    private DateTime _lastRotate = DateTime.UtcNow;

    private DateTime _lifeSince = DateTime.UtcNow;

    private int _nextRotateDelay = _random.Next(200, 600);


    private int _posXIndex = 4;
    private readonly FieldGameObject _fieldGameObject;

    private readonly List<Point> _checkMapFields = new();

    private bool _lastLeftKeyPressed;
    private bool _lastRightKeyPressed;

    private bool _showCollisionLines = false;

    public PlayerPieceGameObject(FieldGameObject fieldGameObject)
    {
        var allPieces = PieceTypes.AllTypes.ToArray();
        _currentPiece = allPieces[_random.Next(0, allPieces.Length - 1)];

        RotateRight();
        _fieldGameObject = fieldGameObject ?? throw new ArgumentNullException(nameof(fieldGameObject));
    }

    internal void SetFieldSetup(int fieldPositionX, int fieldPositionY, int fieldLinesX, int pieceWidth, int pieceHeight)
    {
        _fieldPositionX = fieldPositionX;
        _fieldPositionY = fieldPositionY;
        _fieldLinesX = fieldLinesX;
        _pieceWidth = pieceWidth;
        _pieceHeight = pieceHeight;

        //_posXIndex = _random.Next(0, _fieldLinesX);
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

    public void MoveLeft()
    {
        _posXIndex -= 1;
    }
    public void MoveRight()
    {
        _posXIndex += 1;
    }

    public override void Update(IRenderEngine renderEngine, float time)
    {
        _checkMapFields.Clear();

        if (renderEngine.IsKeyDown(37) && !_lastLeftKeyPressed)
        {
            _posXIndex -= 1;
        }
        if (renderEngine.IsKeyDown(39) && !_lastRightKeyPressed)
        {
            _posXIndex += 1;
        }
        _lastLeftKeyPressed = renderEngine.IsKeyDown(37);
        _lastRightKeyPressed = renderEngine.IsKeyDown(39);

        // check left and right bounds
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

        var newPosY = _piecePositionY;


        newPosY = _piecePositionY + (int)Math.Ceiling(15 * time);

        if (renderEngine.IsKeyDown(38))
        {
            newPosY = _piecePositionY + (int)Math.Ceiling(10 * time);
        }
        if (renderEngine.IsKeyDown(40))
        {
            newPosY = _piecePositionY + (int)Math.Ceiling(10 * time);
        }




        if (newPosY != _piecePositionY)
        {
            // check falling position
            var newPosYIndex = (int)Math.Ceiling(newPosY / (float)_pieceHeight);
            //Console.WriteLine($"FY: {_fieldPositionY}, PP_Y:{_piecePositionY}, NP_Y: {newPosY}, NP_IY: {newPosYIndex}, PH: {_pieceHeight}");

            var allChecksTrue = true;
            foreach (var piece in _currentPiece)
            {
                var checkIndexX = _posXIndex + piece.X;
                var checkIndexY = newPosYIndex + piece.Y;
                if (!_fieldGameObject.IsFieldPositionEmpty(checkIndexX, checkIndexY))
                {
                    allChecksTrue = false;
                    _fieldGameObject.SetFieldData(_posXIndex, (int)Math.Ceiling(_piecePositionY / (float)_pieceHeight), _currentPiece, _color);
                    renderEngine.RemoveGameObject(this);
                    break;
                }
                _checkMapFields.Add(new Point(_posXIndex + piece.X, newPosYIndex + piece.Y));
            }

            if (allChecksTrue)
            {
                _piecePositionY = newPosY;
            }
        }



        //if (_lifeSince.AddSeconds(10) < DateTime.UtcNow)
        //{
        //    renderEngine.RemoveGameObject(this);
        //}
    }

    public override void Render(IRenderEngine renderEngine)
    {
        foreach (var piece in _currentPiece)
        {
            var posX = _fieldPositionX + _piecePositionX + (piece.X * _pieceWidth);
            var posY = _fieldPositionY + _piecePositionY + (piece.Y * _pieceWidth);
            renderEngine.AddDrawStrokeRectToRender(posX + 1, posY + 1, _pieceWidth - 2, _pieceHeight - 2, _color, 1, 1, 30);
            renderEngine.AddDrawFillRectToRender(posX + 1, posY + 1, _pieceWidth - 2, _pieceHeight - 2, _color, 0.3f, 15);
        }

        if (_showCollisionLines)
        {
            foreach (var checkField in _checkMapFields)
            {
                var posX = _fieldPositionX + (checkField.X * _pieceWidth);
                var posY = _fieldPositionY + (checkField.Y * _pieceWidth);
                renderEngine.AddDrawStrokeRectToRender(posX, posY, _pieceWidth, _pieceHeight, "#FF3322", 2, 0.5f);
            }
        }
    }


}
