using CSharpPaint.Editors.Shapes;
using System.Windows.Controls;

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
        private readonly BaseShape? currentShape;
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
                Canvas.SetLeft(currentShape.shape, Canvas.GetLeft(currentShape.shape) - shapePositionOffsetAfterMoving.offsetX);
                Canvas.SetTop(currentShape.shape, Canvas.GetTop(currentShape.shape) - shapePositionOffsetAfterMoving.offsetY);
            }

            isAlreadyExecuted = true;
            editor.Finalize_Moving(currentShape);
        }

        public override void Undo()
        {
            if (currentShape == null)
            {
                return;
            }

            Canvas.SetLeft(currentShape.shape, Canvas.GetLeft(currentShape.shape) + shapePositionOffsetAfterMoving.offsetX);
            Canvas.SetTop(currentShape.shape, Canvas.GetTop(currentShape.shape) + shapePositionOffsetAfterMoving.offsetY);
        }
    }
}
