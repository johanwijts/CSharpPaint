using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace CSharpPaint.Compositions
{
    public class Leaf : Component
    {
        public Leaf(Shape shape, Canvas canvas, MouseButtonEventArgs e)
        {
            this.shape = shape;
            this.canvas = canvas;
            this.e = e;
        }

        public Shape shape;
        private readonly Canvas canvas;
        private readonly MouseButtonEventArgs e;

        public override void MoveOperation(Point compositeMiddle)
        {
            Point currentPosition = e.GetPosition(canvas);
            double offsetX = currentPosition.X - compositeMiddle.X;
            double offsetY = currentPosition.Y - compositeMiddle.Y;

            Canvas.SetLeft(shape, Canvas.GetLeft(shape) + offsetX);
            Canvas.SetTop(shape, Canvas.GetTop(shape) + offsetY);
        }

        public override void SizeOperation(double sizeDifference)
        {
            var newWidth = shape.Width + sizeDifference;
            var newHeight = shape.Height + sizeDifference;

            if (newWidth < 0 || newHeight < 0)
            {
                return;
            }

            shape.Width = newWidth;
            shape.Height = newHeight;
        }

        public override bool IsComposite()
        {
            return false;
        }
    }
}
