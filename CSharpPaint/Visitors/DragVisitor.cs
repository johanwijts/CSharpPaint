using CSharpPaint.Editors.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CSharpPaint.Visitors
{
    public class DragVisitor : IVisitor
    {
        public DragVisitor(Editor editor, Canvas canvas, MouseEventArgs e)
        {
            this.editor = editor;
            this.canvas = canvas;
            this.e = e;
        }

        private readonly Editor editor;
        private readonly Canvas canvas;
        private readonly MouseEventArgs e;

        public void Visit(BaseShape baseShape)
        {
            Handle_Dragging(e, baseShape);
        }

        public void Handle_Dragging(MouseEventArgs e, BaseShape shapeToMove)
        {
            Point currentPosition = e.GetPosition(canvas);
            double offsetX = currentPosition.X - editor.GetlastMousePosition().X;
            double offsetY = currentPosition.Y - editor.GetlastMousePosition().Y;

            Canvas.SetLeft(shapeToMove.shape, Canvas.GetLeft(shapeToMove.shape) + offsetX);
            Canvas.SetTop(shapeToMove.shape, Canvas.GetTop(shapeToMove.shape) + offsetY);

            editor.SetlastMousePosition(currentPosition);
        }
    }
}
