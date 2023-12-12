using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CSharpPaint
{
    public class Editor
    {
        public Editor(Canvas Canvas, RadioButton RectangleRadioButton, RadioButton EllipseRadioButton)
        {
            this.canvas = Canvas;
            this.rectangleRadioButton = RectangleRadioButton;
            this.ellipseRadioButton = EllipseRadioButton;
        }

        public Canvas canvas;
        private readonly RadioButton rectangleRadioButton;
        private readonly RadioButton ellipseRadioButton;

        public bool isSizing;
        private bool isDrawing;
        private Point startPoint;
        private Shape? currentShape;
        private (double offsetX, double offsetY) shapePositionOffsetAfterMoving;
        private (double width, double height) shapeSizeBeforeSizing;
        private bool isDragging;
        private Point lastMousePosition;

        public Shape? GetCurrentShape()
        {
            return currentShape;
        }

        public (double offsetX, double offsetY) GetShapePositionOffsetAfterMoving()
        {
            return shapePositionOffsetAfterMoving;
        }

        public (double width, double height) GetShapeSizeBeforeSizing()
        {
            return shapeSizeBeforeSizing;
        }

        public void Start_Drawing(object sender, MouseButtonEventArgs e)
        {
            Stop_Sizing();

            if (currentShape != null)
            {
                currentShape.Stroke = Brushes.Black;
            }

            if (e.RightButton == MouseButtonState.Pressed)
            {
                return;
            }

            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                return;
            }

            isDrawing = true;
            startPoint = e.GetPosition(canvas);

            if (rectangleRadioButton.IsChecked == true)
            {
                currentShape = new Rectangle
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                };
            }
            else if (ellipseRadioButton.IsChecked == true)
            {
                currentShape = new Ellipse
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                };
            }

            if (currentShape != null)
            {
                Canvas.SetLeft(currentShape, startPoint.X);
                Canvas.SetTop(currentShape, startPoint.Y);
                canvas.Children.Add(currentShape);
            }
        }

        public void Handle_Movement(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Handle_Dragging(e);
            }
            else if (isDrawing && currentShape != null)
            {
                Handle_Drawing(e);
            }
        }

        public void Handle_Sizing(object sender, MouseWheelEventArgs e)
        {
            if (isSizing && currentShape != null)
            {
                // Adjust the width and height based on the mouse wheel delta
                double delta = e.Delta / 120.0;
                double scaleFactor = 1.1;

                double newWidth = currentShape.Width * Math.Pow(scaleFactor, delta);
                double newHeight = currentShape.Height * Math.Pow(scaleFactor, delta);

                currentShape.Width = Math.Max(newWidth, 1);
                currentShape.Height = Math.Max(newHeight, 1);
            }
        }

        public void Stop_Drawing()
        {
            isDrawing = false;
        }

        public void Stop_Moving()
        {
            isDragging = false;
            shapePositionOffsetAfterMoving.offsetX -= Canvas.GetLeft(currentShape);
            shapePositionOffsetAfterMoving.offsetY -= Canvas.GetTop(currentShape);
        }

        public void Stop_Sizing()
        {
            isSizing = false;
        }

        public void Finalize_Drawing(Shape shape)
        {
            canvas.Children.Remove(shape);
            canvas.Children.Add(shape);
        }

        public void Finalize_Moving(Shape shape)
        {
            canvas.Children.Remove(shape);
            canvas.Children.Add(shape);
        }

        public void Finalize_Sizing(Shape shape)
        {
            canvas.Children.Remove(shape);
            canvas.Children.Add(shape);
        }

        private void Handle_Dragging(MouseEventArgs e)
        {
            Point currentPosition = e.GetPosition(canvas);
            double offsetX = currentPosition.X - lastMousePosition.X;
            double offsetY = currentPosition.Y - lastMousePosition.Y;

            Canvas.SetLeft(currentShape, Canvas.GetLeft(currentShape) + offsetX);
            Canvas.SetTop(currentShape, Canvas.GetTop(currentShape) + offsetY);

            lastMousePosition = currentPosition;
        }

        private void Handle_Drawing(MouseEventArgs e)
        {
            Point currentPoint = e.GetPosition(canvas);
            double width = currentPoint.X - startPoint.X;
            double height = currentPoint.Y - startPoint.Y;

            double left = startPoint.X;
            double top = startPoint.Y;

            if (width < 0)
            {
                left = currentPoint.X;
                width = Math.Abs(width);
            }

            if (height < 0)
            {
                top = currentPoint.Y;
                height = Math.Abs(height);
            }

            Canvas.SetLeft(currentShape, left);
            Canvas.SetTop(currentShape, top);

            if (currentShape is Rectangle)
            {
                (currentShape as Rectangle)!.Width = width;
                (currentShape as Rectangle)!.Height = height;
            }
            else if (currentShape is Ellipse)
            {
                (currentShape as Ellipse)!.Width = width;
                (currentShape as Ellipse)!.Height = height;
            }
        }

        public bool Check_For_Drag_Connect(MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition(canvas);
            var hitTestResult = VisualTreeHelper.HitTest(canvas, mousePosition);

            if (hitTestResult != null)
            {
                if (hitTestResult.VisualHit is Shape)
                {
                    currentShape = (Shape)hitTestResult.VisualHit;
                    shapePositionOffsetAfterMoving = 
                        (Canvas.GetLeft(currentShape), Canvas.GetTop(currentShape));
                    isDragging = true;
                    lastMousePosition = mousePosition;
                    return true;
                }
            }
            return false;
        }

        public bool Check_For_Sizing_Connect(MouseButtonEventArgs e)
        {
            if (currentShape != null)
            {
                currentShape.Stroke = Brushes.Black;
            }

            Point mousePosition = e.GetPosition(canvas);
            var hitTestResult = VisualTreeHelper.HitTest(canvas, mousePosition);

            if (hitTestResult != null)
            {
                if (hitTestResult.VisualHit is Shape)
                {
                    if (currentShape is Rectangle)
                    {
                        shapeSizeBeforeSizing.width = (currentShape as Rectangle)!.Width;
                        shapeSizeBeforeSizing.height = (currentShape as Rectangle)!.Height;
                    }
                    else if (currentShape is Ellipse)
                    {
                        shapeSizeBeforeSizing.width = (currentShape as Ellipse)!.Width;
                        shapeSizeBeforeSizing.height = (currentShape as Ellipse)!.Height;
                    }

                    currentShape = (Shape)hitTestResult.VisualHit;
                    currentShape.Stroke = Brushes.Blue;
                    isSizing = true;
                    lastMousePosition = mousePosition;
                    return true;
                }
            }
            Stop_Sizing();
            return false;
        }
    }
}
