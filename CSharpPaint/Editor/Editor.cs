using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System;

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
        private RadioButton rectangleRadioButton;
        private RadioButton ellipseRadioButton;

        private bool isSizing;
        private bool isDrawing;
        private Point startPoint;
        private Shape? currentShape;
        private bool isDragging;
        private Point lastMousePosition;

        public void Editor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isSizing = false;

            if (currentShape != null)
            {
                currentShape.Stroke = Brushes.Black;
            }

            if (e.RightButton == MouseButtonState.Pressed)
            {
                HandleRightMouseButtonDown(e);
                return;
            }

            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                HandleMiddleMouseButtonDown(e);
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

        public void Editor_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                HandleDragging(e);
            }
            else if (isDrawing && currentShape != null)
            {
                HandleDrawing(e);
            }
        }

        public void Editor_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (isSizing)
            {
                HandleSizing(e);
            }
        }

        public void Editor_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isDrawing)
            {
                isDrawing = false;
            }

            if (isDragging)
            {
                isDragging = false;
            }
        }

        private void HandleDragging(MouseEventArgs e)
        {
            Point currentPosition = e.GetPosition(canvas);
            double offsetX = currentPosition.X - lastMousePosition.X;
            double offsetY = currentPosition.Y - lastMousePosition.Y;

            Canvas.SetLeft(currentShape, Canvas.GetLeft(currentShape) + offsetX);
            Canvas.SetTop(currentShape, Canvas.GetTop(currentShape) + offsetY);

            lastMousePosition = currentPosition;
        }

        private void HandleDrawing(MouseEventArgs e)
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

        private void HandleSizing(MouseWheelEventArgs e)
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

        private void HandleRightMouseButtonDown(MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition(canvas);
            var hitTestResult = VisualTreeHelper.HitTest(canvas, mousePosition);

            if (hitTestResult != null)
            {
                if (hitTestResult.VisualHit is Shape)
                {
                    currentShape = (Shape)hitTestResult.VisualHit;
                    isDragging = true;
                    lastMousePosition = mousePosition;
                }
            }
        }

        private void HandleMiddleMouseButtonDown(MouseButtonEventArgs e)
        {
            isSizing = false;

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
                    currentShape = (Shape)hitTestResult.VisualHit;
                    currentShape.Stroke = Brushes.Blue;
                    isSizing = true;
                    lastMousePosition = mousePosition;
                }
            }
        }
    }
}
