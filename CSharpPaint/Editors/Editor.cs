﻿using CSharpPaint.Compositions;
using CSharpPaint.Editors.Shapes;
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
        public Editor(Canvas Canvas,RadioButton RectangleRadioButton, RadioButton EllipseRadioButton)
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
        private BaseShape currentShape = new BaseShape();
        private (double offsetX, double offsetY) shapePositionOffsetAfterMoving;
        private (double width, double height) shapeSizeBeforeSizing;
        private bool isDragging;
        private Point lastMousePosition;

        public BaseShape? GetCurrentShape()
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

            if (currentShape?.shape != null)
            {
                currentShape.shape.Stroke = Brushes.Black;
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
                currentShape!.shape = new Rectangle
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                };
            }
            else if (ellipseRadioButton.IsChecked == true)
            {
                currentShape!.shape = new Ellipse
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                };
            }

            if (currentShape?.shape != null)
            {
                Canvas.SetLeft(currentShape.shape, startPoint.X);
                Canvas.SetTop(currentShape.shape, startPoint.Y);
                canvas.Children.Add(currentShape.shape);
            }
        }

        public void Handle_Movement(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Handle_Dragging(e, currentShape!);
            }
            else if (isDrawing && currentShape?.shape != null)
            {
                Handle_Drawing(e);
            }
        }

        public void Handle_Sizing(object sender, MouseWheelEventArgs e, BaseShape? shapeToSize)
        {
            if (isSizing && shapeToSize?.shape != null)
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

        public void Handle_Dragging(MouseEventArgs e, BaseShape shapeToMove)
        {
            Point currentPosition = e.GetPosition(canvas);
            double offsetX = currentPosition.X - lastMousePosition.X;
            double offsetY = currentPosition.Y - lastMousePosition.Y;

            Canvas.SetLeft(shapeToMove.shape, Canvas.GetLeft(shapeToMove.shape) + offsetX);
            Canvas.SetTop(shapeToMove.shape, Canvas.GetTop(shapeToMove.shape) + offsetY);

            lastMousePosition = currentPosition;
        }

        public void Handle_Group_Dragging(MouseButtonEventArgs e, BaseShape shapeToMove)
        {
            Point mousePosition = e.GetPosition(canvas);

            shapePositionOffsetAfterMoving =
                (Canvas.GetLeft(currentShape!.shape), Canvas.GetTop(currentShape.shape));
            isDragging = true;
            lastMousePosition = mousePosition;

            Handle_Dragging(e, shapeToMove);
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

            Canvas.SetLeft(currentShape?.shape, left);
            Canvas.SetTop(currentShape?.shape, top);

            if (currentShape?.shape is Rectangle)
            {
                (currentShape?.shape as Rectangle)!.Width = width;
                (currentShape?.shape as Rectangle)!.Height = height;
            }
            else if (currentShape?.shape is Ellipse)
            {
                (currentShape?.shape as Ellipse)!.Width = width;
                (currentShape?.shape as Ellipse)!.Height = height;
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
                    currentShape!.shape = (Shape)hitTestResult.VisualHit;
                    shapePositionOffsetAfterMoving = 
                        (Canvas.GetLeft(currentShape?.shape), Canvas.GetTop(currentShape?.shape));
                    isDragging = true;
                    lastMousePosition = mousePosition;
                    return true;
                }
            }
            return false;
        }

        public bool Check_For_Sizing_Connect(MouseButtonEventArgs e)
        {
            if (currentShape?.shape != null)
            {
                currentShape!.shape.Stroke = Brushes.Black;
            }

            Point mousePosition = e.GetPosition(canvas);
            var hitTestResult = VisualTreeHelper.HitTest(canvas, mousePosition);

            if (hitTestResult != null)
            {
                if (hitTestResult.VisualHit is Shape)
                {
                    if (currentShape?.shape is Rectangle)
                    {
                        shapeSizeBeforeSizing.width = (currentShape?.shape as Rectangle)!.Width;
                        shapeSizeBeforeSizing.height = (currentShape?.shape as Rectangle)!.Height;
                    }
                    else if (currentShape?.shape is Ellipse)
                    {
                        shapeSizeBeforeSizing.width = (currentShape?.shape as Ellipse)!.Width;
                        shapeSizeBeforeSizing.height = (currentShape?.shape as Ellipse)!.Height;
                    }

                    currentShape!.shape = (Shape)hitTestResult.VisualHit;
                    currentShape!.shape.Stroke = Brushes.Blue;
                    isSizing = true;
                    lastMousePosition = mousePosition;
                    return true;
                }
            }
            Stop_Sizing();
            return false;
        }

        public void Check_For_Grouping_Connect(MouseButtonEventArgs e, Composite group)
        {
            Point mousePosition = e.GetPosition(canvas);
            var hitTestResult = VisualTreeHelper.HitTest(canvas, mousePosition);

            if (hitTestResult != null)
            {
                if (hitTestResult.VisualHit is Shape)
                {
                    Shape leafShape;
                    leafShape = (Shape)hitTestResult.VisualHit;
                    leafShape.Stroke = Brushes.Green;

                    group.Add(new Leaf(new BaseShape(leafShape), canvas, e));
                }
            }
        }

        public void Stop_Drawing()
        {
            isDrawing = false;
        }

        public void Stop_Moving()
        {
            isDragging = false;
            shapePositionOffsetAfterMoving.offsetX -= Canvas.GetLeft(currentShape!.shape);
            shapePositionOffsetAfterMoving.offsetY -= Canvas.GetTop(currentShape!.shape);
        }

        public void Stop_Sizing()
        {
            isSizing = false;
        }

        public void Finalize_Drawing(BaseShape shape)
        {
            currentShape = new BaseShape(currentShape.shape);
            canvas.Children.Remove(shape.shape);
            canvas.Children.Add(shape.shape);
        }

        public void Finalize_Moving(BaseShape shape)
        {
            currentShape = new BaseShape(currentShape.shape);
            canvas.Children.Remove(shape.shape);
            canvas.Children.Add(shape.shape);
        }

        public void Finalize_Sizing(BaseShape shape)
        {
            currentShape = new BaseShape(currentShape.shape);
            canvas.Children.Remove(shape.shape);
            canvas.Children.Add(shape.shape);
        }
    }
}