using System.Windows.Controls;
using System.Windows.Shapes;

namespace CSharpPaint.Commands
{
    public class MoveCommand : Command
    {
        public MoveCommand(Editor Editor)
        {
            this.editor = Editor;
            this.currentShape = Editor.GetCurrentShape();
            this.shapePositionOffsetAfterMoving = Editor.GetShapePositionOffsetAfterMoving();
        }

        private readonly Editor editor;
        private readonly Shape? currentShape;
        private readonly (double offsetX, double offsetY) shapePositionOffsetAfterMoving;
        private bool isAlreadyExecuted;

        public override void Execute()
        {
            if (currentShape == null)
            {
                return;
            }

            if (isAlreadyExecuted)
            {
                Canvas.SetLeft(currentShape, Canvas.GetLeft(currentShape) - shapePositionOffsetAfterMoving.offsetX);
                Canvas.SetTop(currentShape, Canvas.GetTop(currentShape) - shapePositionOffsetAfterMoving.offsetY);
            }

            isAlreadyExecuted = true;
            editor.Finalize_Moving(currentShape);
        }

        public override void Undo()
        {
            Canvas.SetLeft(currentShape, Canvas.GetLeft(currentShape) + shapePositionOffsetAfterMoving.offsetX);
            Canvas.SetTop(currentShape, Canvas.GetTop(currentShape) + shapePositionOffsetAfterMoving.offsetY);
        }
    }
}
