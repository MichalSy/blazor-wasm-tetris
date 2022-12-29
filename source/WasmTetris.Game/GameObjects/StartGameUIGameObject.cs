using WasmTetris.Game.BaseGameObject;
using WasmTetris.Game.Engine;

namespace WasmTetris.Game.GameObjects;

public class StartGameUIGameObject : GameObject
{
    private int _positionX = 100;
    private int _positionY = 100;
    private readonly int _messageBoxWidth = 300;
    private readonly int _messageBoxHeight = 100;

    public override void Render(IRenderEngine renderEngine)
    {
        renderEngine.AddDrawFillRectToRender(_positionX, _positionY, _messageBoxWidth, _messageBoxHeight, "#000", 0.5f);
        renderEngine.AddDrawStrokeRectToRender(_positionX, _positionY, _messageBoxWidth, _messageBoxHeight, "#3260a8", 2, 1, 10);

        renderEngine.AddDrawTextToRender(_positionX + (_messageBoxWidth / 2), _positionY + (_messageBoxHeight / 2), "PRESS KEY TO START", "#3260a8", "center", 1, 5);
    }

    public void SetCenterPoint(int fieldCenterX, int fieldCenterY)
    {
        _positionX = fieldCenterX - (_messageBoxWidth / 2);
        _positionY = fieldCenterY - (_messageBoxHeight / 2);
    }
}
