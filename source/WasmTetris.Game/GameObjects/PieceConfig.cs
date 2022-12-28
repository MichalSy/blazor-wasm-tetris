using System.Drawing;

namespace WasmTetris.Game.GameObjects;

public record PieceConfig(bool CanRotate, IEnumerable<Point> Blocks);
