using WasmTetris.Game.BaseGameObject;
using WasmTetris.Game.Engine;

namespace WasmTetris.Game.GameObjects;

public class StartGameUIGameObject : GameObject
{
    private int _positionX = 100;
    private int _positionY = 100;
    private int _messageBoxWidth = 300;
    private int _messageBoxHeight = 100;
    private int _fontSize = 22;
    

    public override void Render(IRenderEngine renderEngine)
    {
        renderEngine.AddDrawFillRectToRender(_positionX, _positionY, _messageBoxWidth, _messageBoxHeight, "#000", 0.5f);
        renderEngine.AddDrawStrokeRectToRender(_positionX, _positionY, _messageBoxWidth, _messageBoxHeight, "#3260a8", 2, 1, 10);

        renderEngine.AddDrawTextToRender(_positionX + (_messageBoxWidth / 2), _positionY + (_messageBoxHeight / 2), "PRESS KEY TO START", "#3260a8", "center", _fontSize, 1, 5);
    }

    public void SetCenterPoint(int fieldCenterX, int fieldCenterY, int fieldWidth)
    {
        _messageBoxWidth = (int)Math.Round(fieldWidth * .8f);
        _messageBoxHeight = (int)Math.Round((_messageBoxWidth / 300f) * 100f);

        _fontSize = (int)Math.Round((_messageBoxWidth / 300f) * 22f);

        _positionX = fieldCenterX - (_messageBoxWidth / 2);
        _positionY = fieldCenterY - (_messageBoxHeight / 2);
    }
}
