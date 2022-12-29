using System.Drawing;
using WasmTetris.Game.BaseGameObject;
using WasmTetris.Game.Engine;

namespace WasmTetris.Game.GameObjects;

public class FieldGameObject : GameObject
{
    private int _fieldPositionX;
    private int _fieldPositionY;
    private int _fieldWidth;
    private int _fieldHeight;
    private int _fieldLinesX;
    private int _fieldLinesY;
    private int _pieceWidth;
    private int _pieceHeight;

    private List<PieceMapData[]> _fieldMap = new ();

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

        if (_fieldMap.Count == 0)
        {
            _fieldMap = Enumerable.Range(0, _fieldLinesY).Select(y => new PieceMapData[_fieldLinesX]).ToList();
        }
    }

    internal void ClearField()
    {
        _fieldMap = Enumerable.Range(0, _fieldLinesY).Select(y => new PieceMapData[_fieldLinesX]).ToList();
    }

    public bool IsFieldPositionEmpty(int positionX, int positionY)
    {
        if (positionX < 0 | positionY < 0 | positionX >= _fieldLinesX)
        {
            return true;
        }

        if (positionY >= _fieldLinesY)
        {
            return false;
        }

        return _fieldMap[positionY][positionX] == null;
    }

    public void SetFieldData(int positionIndexX, int poisitionIndexY, IEnumerable<Point> pieces, string color)
    {
        //Console.WriteLine($"PosI_X: {positionIndexX}, PosI_Y: {poisitionIndexY}, Color: {color}");
        foreach (var currentPiece in pieces)
        {
            var pieceIndexX = positionIndexX + currentPiece.X;
            var pieceIndexY = poisitionIndexY + currentPiece.Y;
            if (pieceIndexX >= 0 && pieceIndexY >= 0)
            {
                //Console.WriteLine($"PosI_X: {positionIndexX + currentPiece.X}, PosI_Y: {poisitionIndexY + currentPiece.Y}, Color: {color}");
                _fieldMap[poisitionIndexY + currentPiece.Y][positionIndexX + currentPiece.X] = new PieceMapData(color);
            }
        }
    }

    public override void Update(IRenderEngine renderEngine, float time)
    {
        var fullLines = _fieldMap.Where(y => y.All(x => x is not null));
        if (fullLines.Any())
        {
            var removeCount = _fieldMap.RemoveAll(y => y.All(x => x is not null));
            switch (removeCount)
            {
                case 1:
                    renderEngine.PlaySound("line1.ogg");
                    break;

                case 2:
                    renderEngine.PlaySound("line2.ogg");
                    break;

                case 3:
                    renderEngine.PlaySound("line3.ogg");
                    break;

                case 4:
                    renderEngine.PlaySound("line4.ogg");
                    break;

                default:
                    renderEngine.PlaySound("line5.ogg");
                    break;
            }
            _fieldMap.InsertRange(0, Enumerable.Range(0, removeCount).Select(y => new PieceMapData[_fieldLinesX]));
        }
    }

    public override void Render(IRenderEngine renderEngine)
    {
        renderEngine.AddDrawFillRectToRender(_fieldPositionX, _fieldPositionY, _fieldWidth, _fieldHeight, "#000", 0.9f);

        for (int i = 1; i < _fieldLinesX; i++)
        {
            renderEngine.AddDrawLineToRender(_fieldPositionX + (i * _pieceWidth), _fieldPositionY, _fieldPositionX + (i * _pieceWidth), _fieldPositionY + _fieldHeight, "#3260a8", 2, .2f);
        }

        for (int i = 1; i < _fieldLinesY; i++)
        {
            renderEngine.AddDrawLineToRender(_fieldPositionX, _fieldPositionY + (i * _pieceHeight), _fieldPositionX + _fieldWidth, _fieldPositionY + (i * _pieceHeight), "#3260a8", 2, .2f);
        }

        renderEngine.AddDrawStrokeRectToRender(_fieldPositionX, _fieldPositionY, _fieldWidth, _fieldHeight, "#3260a8", 2, 1, 10);

        for (int y = 0; y < _fieldLinesY; y++)
        {
            for (int x = 0; x < _fieldLinesX; x++)
            {
                if (_fieldMap[y][x] != null)
                {
                    renderEngine.AddDrawFillRectToRender(_fieldPositionX + (x * _pieceWidth) + 1, _fieldPositionY + (y * _pieceHeight) + 1, _pieceWidth - 2, _pieceHeight - 2, _fieldMap[y][x].Color, 1, 0);
                }
            }
        }
    }

    
}
