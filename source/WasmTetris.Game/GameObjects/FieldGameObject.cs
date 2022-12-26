using System;
using System.Drawing;
using WasmTetris.Game.BaseGameObject;
using WasmTetris.Game.Engine;

namespace WasmTetris.Game.GameObjects;

public class FieldGameObject : GameObject
{
    private static Random _random = new((int)DateTime.Now.Ticks);
    private int _fieldPositionX;
    private int _fieldPositionY;
    private int _fieldWidth;
    private int _fieldHeight;
    private int _fieldLinesX;
    private int _fieldLinesY;
    private int _pieceWidth;
    private int _pieceHeight;

    private PieceMapData[,] _fieldMap = new PieceMapData[0,0];

    internal void SetFieldSetup(int fieldPositionX, int fieldPositionY, int fieldLinesX, int fieldLinesY, int pieceWidth, int pieceHeight)
    {
        _fieldPositionX = fieldPositionX;
        _fieldPositionY = fieldPositionY;
        _fieldLinesX = fieldLinesX;
        _fieldLinesY = fieldLinesY;
        _pieceWidth = pieceWidth;
        _pieceHeight = pieceHeight;

        _fieldWidth = _fieldLinesX * _pieceWidth;
        _fieldHeight = _fieldLinesY * _pieceHeight;

        _fieldMap = new PieceMapData[_fieldLinesY, _fieldLinesX];


        _fieldMap[12, 5] = new PieceMapData("#BB2212");
    }

    public bool IsFieldPositionEmpty(int positionX, int positionY)
    {
        if (positionX < 0 | positionY < 0 | positionX >= _fieldLinesX | positionY >= _fieldLinesY)
        {
            return true;
        }

        return _fieldMap[positionY, positionX] == null;
    }

    public override void Render(IRenderEngine renderEngine)
    {
        for (int i = 1; i < _fieldLinesX; i++)
        {
            renderEngine.AddDrawLineToRender(_fieldPositionX + (i * _pieceWidth), _fieldPositionY, _fieldPositionX + (i * _pieceWidth), _fieldPositionY + _fieldHeight, "#3260a8", 2, 0.05f);
        }

        for (int i = 1; i < _fieldLinesY; i++)
        {
            renderEngine.AddDrawLineToRender(_fieldPositionX, _fieldPositionY + (i * _pieceHeight), _fieldPositionX + _fieldWidth, _fieldPositionY + (i * _pieceHeight), "#3260a8", 2, 0.05f);
        }

        renderEngine.AddDrawStrokeRectToRender(_fieldPositionX, _fieldPositionY, _fieldWidth, _fieldHeight, "#3260a8", 2);

        for (int y = 0; y < _fieldLinesY; y++)
        {
            for (int x = 0; x < _fieldLinesX; x++)
            {
                if (_fieldMap[y,x] != null)
                {
                    renderEngine.AddDrawRectWithBorderToRender(_fieldMap[y, x].color, _fieldPositionX + (x * _pieceWidth), _fieldPositionY + (y * _pieceHeight), _pieceWidth, _pieceHeight);
                }
            }
        }
    }
}
