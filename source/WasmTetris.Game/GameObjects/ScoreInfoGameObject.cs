using WasmTetris.Game.BaseGameObject;
using WasmTetris.Game.Engine;

namespace WasmTetris.Game.GameObjects;

public class ScoreInfoGameObject : GameObject
{
    private int _fontSize = 22;

    private int _timePositionX = 100;
    private int _timePositionY = 10;
    private int _timeBoxWidth = 300;
    private int _timeBoxHeight = 100;

    private int _scorePositionX = 100;
    private int _scorePositionY = 10;
    private int _scoreBoxWidth = 300;
    private int _scoreBoxHeight = 100;


    private DateTime? _startTime;
    private TimeSpan _displayTime = TimeSpan.Zero;
    private int _displayScore = 0;

    public void SetFieldPosition(int fieldX, int fieldWidth)
    {
        _fontSize = (int)Math.Round(fieldWidth * .035f);

        _timePositionX = fieldX;
        _timeBoxWidth = (int)Math.Round(fieldWidth * .3f);
        _timeBoxHeight = (int)Math.Round(fieldWidth * .085f);

        _scoreBoxWidth = (int)Math.Round(fieldWidth * .43f);
        _scorePositionX = fieldX + fieldWidth - _scoreBoxWidth;
        _scoreBoxHeight = (int)Math.Round(fieldWidth * .085f);

    }

    public void Reset()
    {
        _displayTime = TimeSpan.Zero;
        _displayScore = 0;
        _startTime = null;
    }

    public void SetStartTime()
    {
        _startTime = DateTime.UtcNow;
    }

    public override void Update(IRenderEngine renderEngine, float time)
    {
        if (_startTime is not null)
        {
            _displayTime = DateTime.UtcNow - _startTime.Value;
        }
    }

    public override void Render(IRenderEngine renderEngine)
    {
        renderEngine.AddDrawFillRectToRender(_timePositionX, _timePositionY, _timeBoxWidth, _timeBoxHeight, "#000", 0.9f);
        renderEngine.AddDrawStrokeRectToRender(_timePositionX, _timePositionY, _timeBoxWidth, _timeBoxHeight, "#3260a8", 2, 1, 10);
        renderEngine.AddDrawTextToRender(_timePositionX + (_timeBoxWidth / 2), _timePositionY + (_timeBoxHeight / 2), $"TIME: {_displayTime:mm\\:ss}", "#3260a8", "center", _fontSize, 1, 5);

        renderEngine.AddDrawFillRectToRender(_scorePositionX, _scorePositionY, _scoreBoxWidth, _scoreBoxHeight, "#000", 0.9f);
        renderEngine.AddDrawStrokeRectToRender(_scorePositionX, _scorePositionY, _scoreBoxWidth, _scoreBoxHeight, "#3260a8", 2, 1, 10);
        renderEngine.AddDrawTextToRender(_scorePositionX + (_scoreBoxWidth / 2), _scorePositionY + (_scoreBoxHeight / 2), $"SCORE: {_displayScore:00000000}", "#3260a8", "center", _fontSize, 1, 5);
    }
}
