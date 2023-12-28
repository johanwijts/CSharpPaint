using CSharpPaint.Editors.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CSharpPaint.Compositions
{
    public class Leaf : Component
    {
        public Leaf(BaseShape shape, Canvas canvas, MouseButtonEventArgs e)
        {
            this.shape = shape;
            this.canvas = canvas;
            this.e = e;
        }

        public BaseShape shape;
        private readonly Canvas canvas;
        private readonly MouseButtonEventArgs e;

        public override void MoveOperation(Point compositeMiddle)
        {
            Point currentPosition = e.GetPosition(canvas);
            double offsetX = currentPosition.X - compositeMiddle.X;
            double offsetY = currentPosition.Y - compositeMiddle.Y;

            Canvas.SetLeft(shape.shape, Canvas.GetLeft(shape.shape) + offsetX);
            Canvas.SetTop(shape.shape, Canvas.GetTop(shape.shape) + offsetY);
        }

        public override void SizeOperation(double sizeDifference)
        {
            var newWidth = shape.shape!.Width + sizeDifference;
            var newHeight = shape.shape!.Height + sizeDifference;

            if (newWidth < 0 || newHeight < 0)
            {
                return;
            }

            shape.shape!.Width = newWidth;
            shape.shape!.Height = newHeight;
        }

        public override bool IsComposite()
        {
            return false;
        }
    }
}
