using CSharpPaint.Editors.Shapes;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace CSharpPaint.Visitors
{
    public class SizeVisitor : IVisitor
    {
        public SizeVisitor(Editor editor, Canvas canvas, MouseWheelEventArgs e)
        {
            this.editor = editor;
            this.canvas = canvas;
            this.e = e;
        }

        private readonly Editor editor;
        private readonly Canvas canvas;
        private readonly MouseWheelEventArgs e;

        public void Visit(BaseShape baseShape)
        {
            Handle_Sizing(e, baseShape);
        }

        private void Handle_Sizing(MouseWheelEventArgs e, BaseShape? shapeToSize)
        {
            if (editor.isSizing && shapeToSize?.shape != null)
            {
                // Adjust the width and height based on the mouse wheel delta
                double delta = e.Delta / 120.0;
                double scaleFactor = 1.1;

                double newWidth = shapeToSize.shape.Width * Math.Pow(scaleFactor, delta);
                double newHeight = shapeToSize.shape.Height * Math.Pow(scaleFactor, delta);

                shapeToSize.shape.Width = Math.Max(newWidth, 1);
                shapeToSize.shape.Height = Math.Max(newHeight, 1);
            }
        }
    }
}
