using System.Drawing;
using WasmTetris.Game.BaseGameObject;
using WasmTetris.Game.Engine;

namespace WasmTetris.Game.GameObjects;

public class PlayerPieceGameObject : GameObject
{
    private static readonly Random _random = new((int)DateTime.Now.Ticks);
    private int _fieldPositionX;
    private int _fieldPositionY;
    private int _fieldLinesX;
    private int _pieceWidth;
    private int _pieceHeight;

    private int _piecePositionX;
    private int _piecePositionY;

    private readonly string _color = ColorPalette.GetRandomColor();

    private PieceConfig _currentPiece = PieceTypes.AllTypes.First();

    private int _posXIndex = 5;
    private readonly IRenderEngine _renderEngine;
    private readonly FieldGameObject _fieldGameObject;

    private readonly List<Point> _checkMapFields = new();

    private bool _showCollisionLines = false;

    private readonly float _movingSpeed = 10;
    private float _movingMultiplyer = 1;

    private bool _needMoveRight;
    private bool _needMoveLeft;

    public PlayerPieceGameObject(IRenderEngine renderEngine, FieldGameObject fieldGameObject)
    {
        var allPieces = PieceTypes.AllTypes.ToArray();
        _currentPiece = allPieces[_random.Next(0, allPieces.Length - 1)];
        _renderEngine = renderEngine;
        _fieldGameObject = fieldGameObject ?? throw new ArgumentNullException(nameof(fieldGameObject));
    }

    internal void SetFieldSetup(int fieldPositionX, int fieldPositionY, int fieldLinesX, int pieceWidth, int pieceHeight)
    {
        _fieldPositionX = fieldPositionX;
        _fieldPositionY = fieldPositionY;
        _fieldLinesX = fieldLinesX;
        _pieceWidth = pieceWidth;
        _pieceHeight = pieceHeight;

        _movingMultiplyer = _pieceWidth / 32f;
    }

    private void DestroyMyself(IRenderEngine renderEngine)
    {
        renderEngine.PlaySound("drop.ogg", 1f);
        _fieldGameObject.SetFieldData(_posXIndex, (int)Math.Ceiling(_piecePositionY / (float)_pieceHeight), _currentPiece.Blocks, _color);
        renderEngine.RemoveGameObject(this);
    }

    public void RotateRight()
    {
        if (!_currentPiece.CanRotate)
            return;

        _currentPiece = _currentPiece with { Blocks = _currentPiece.Blocks.Select(p => new Point(-p.Y, p.X)) };
        _renderEngine.PlaySound("rotate.ogg");
    }

    public void RotateLeft()
    {
        if (!_currentPiece.CanRotate)
            return;

        _currentPiece = _currentPiece with { Blocks = _currentPiece.Blocks.Select(p => new Point(p.Y, -p.X)) };
        _renderEngine.PlaySound("rotate.ogg");
    }

    public void MoveLeft()
    {
        _needMoveLeft = true;
    }

    public void MoveRight()
    {
        _needMoveRight = true;
    }

    public override void Update(IRenderEngine renderEngine, float time)
    {
        var isDebugMode = _showCollisionLines = renderEngine.IsKeyDown(32);

        if (!isDebugMode)
        {
            _checkMapFields.Clear();
        }

        // check left and right bounds
        int xdiff = _posXIndex + _currentPiece.Blocks.Min(p => p.X);
        if (xdiff < 0)
        {
            _posXIndex -= xdiff;
        }
        xdiff = _fieldLinesX - 1 - (_posXIndex + _currentPiece.Blocks.Max(p => p.X));
        if (xdiff < 0)
        {
            _posXIndex += xdiff;
        }
        _piecePositionX = _posXIndex * _pieceWidth;

        var newPosY = _piecePositionY;

        if (!isDebugMode) // space
        {
            newPosY = _piecePositionY + (int)Math.Ceiling(_movingSpeed * _movingMultiplyer * time);
        }

        if (renderEngine.IsKeyDown(40)) // arrow down
        {
            if (isDebugMode) // space
            {
                newPosY = _piecePositionY + (int)Math.Ceiling((_movingSpeed / 3) * _movingMultiplyer * time);
            }
            else
            {
                newPosY = _piecePositionY + (int)Math.Ceiling(90f * _movingMultiplyer * time);
            }
        }

        // Next position Y index
        var newPosYIndex = (int)Math.Ceiling(newPosY / (float)_pieceHeight);

        // If right movement is needed, it is first checked if this is possible at all
        if (_needMoveRight
                && IsPieceMovementPossible(_posXIndex + 1, newPosYIndex, true)
                && IsPieceMovementPossible(_posXIndex + 1, (int)Math.Floor(_piecePositionY / (float)_pieceHeight), true))
        {
            _posXIndex += 1;
            renderEngine.PlaySound("move.ogg");
        }
        _needMoveRight = false;

        // If left movement is needed, it is first checked if this is possible at all
        if (_needMoveLeft
            && IsPieceMovementPossible(_posXIndex - 1, newPosYIndex, true)
            && IsPieceMovementPossible(_posXIndex - 1, (int)Math.Floor(_piecePositionY / (float)_pieceHeight), true))
        {
            _posXIndex -= 1;
            renderEngine.PlaySound("move.ogg");
        }
        _needMoveLeft = false;

        if (newPosY != _piecePositionY)
        {
            // check falling position
            if (!IsPieceMovementPossible(_posXIndex, newPosYIndex))
            {
                DestroyMyself(renderEngine);
                return;
            }

            _piecePositionY = newPosY;
        }
    }

    private bool IsPieceMovementPossible(int posYIndex, int posXIndex, bool addDebugCheckFields = false)
    {
        foreach (var piece in _currentPiece.Blocks)
        {
            var checkIndexX = posYIndex + piece.X;
            var checkIndexY = posXIndex + piece.Y;
            if (!_fieldGameObject.IsFieldPositionEmpty(checkIndexX, checkIndexY))
            {
                return false;
            }
            if (addDebugCheckFields)
            {
                _checkMapFields.Add(new Point(checkIndexX, checkIndexY));
            }
        }
        return true;
    }

    public override void Render(IRenderEngine renderEngine)
    {
        foreach (var piece in _currentPiece.Blocks)
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
                renderEngine.AddDrawStrokeRectToRender(posX, posY, _pieceWidth, _pieceHeight, "#76ea7c", 2, 0.5f);
            }
        }
    }


}
