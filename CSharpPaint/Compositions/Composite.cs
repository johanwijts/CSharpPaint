using CSharpPaint.Commands;
using CSharpPaint.Editors.Shapes;
using CSharpPaint.Invokers;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace CSharpPaint.Compositions
{
    public class Composite : Component
    {
        public Composite(Invoker invoker, Editor editor)
        {
            this.invoker = invoker;
            this.editor = editor;
        }

        private readonly Invoker invoker;
        private readonly Editor editor;
        protected readonly List<Component> children = new List<Component>();

        public override void MoveOperation(Point compositeMiddle)
        {
            if (this.children.Count == 0)
            {
                return;
            }

            var shapes = new List<BaseShape>();
            var leafPositions = new List<Point>();
            var leafPositionsBeforeMoving = new List<Point>();
            var leafPositionsAfterMoving = new List<Point>();

            foreach (var component in this.children)
            {
                if (!component.IsComposite())
                {
                    shapes.Add(((Leaf)component).shape);

                    var leaf = (Leaf)component;
                    var leafPoint = new Point(Canvas.GetLeft(leaf.shape.shape), Canvas.GetTop(leaf.shape.shape));
                    leafPositions.Add(leafPoint);
                    leafPositionsBeforeMoving.Add(leafPoint);
                }
            }

            var leafMiddleX = 0.0;
            var leafMiddleY = 0.0;

            foreach (var leafPosition in leafPositions)
            {
                leafMiddleX += leafPosition.X;
                leafMiddleY += leafPosition.Y;
            }

            leafMiddleX /= leafPositions.Count;
            leafMiddleY /= leafPositions.Count;

            compositeMiddle = new Point(leafMiddleX, leafMiddleY);

            foreach (var component in this.children)
            {
                if (!component.IsComposite())
                {
                    component.MoveOperation(compositeMiddle);
                    var leaf = (Leaf)component;
                    var leafPoint = new Point(Canvas.GetLeft(leaf.shape.shape), Canvas.GetTop(leaf.shape.shape));
                    leafPositionsAfterMoving.Add(leafPoint);
                }
            }

            invoker.Execute(new MoveGroupCommand(editor, shapes, leafPositionsBeforeMoving, leafPositionsAfterMoving));
        }

        public override void SizeOperation(double sizeDifference)
        {
            if (this.children.Count == 0)
            {
                return;
            }

            var shapes = new List<BaseShape>();
            var leafWidthsBeforeSizing = new List<double>();
            var leafHeightsBeforeSizing = new List<double>();
            var leafWidthsAfterSizing = new List<double>();
            var leafHeightsAfterSizing = new List<double>();

            foreach (var component in this.children)
            {
                if (!component.IsComposite())
                {
                    shapes.Add(((Leaf)component).shape);

                    var leaf = (Leaf)component;
                    leafWidthsBeforeSizing.Add(leaf.shape.shape!.Width);
                    leafHeightsBeforeSizing.Add(leaf.shape.shape!.Height);
                } 
            }

            foreach (var component in this.children)
            {
                if (!component.IsComposite())
                {
                    component.SizeOperation(sizeDifference);
                    var leaf = (Leaf)component;
                    leafWidthsAfterSizing.Add(leaf.shape.shape!.Width);
                    leafHeightsAfterSizing.Add(leaf.shape.shape!.Height);
                }
            }

            invoker.Execute(
                new SizeGroupCommand(
                    editor, 
                    shapes,
                    leafWidthsBeforeSizing,
                    leafHeightsBeforeSizing,
                    leafWidthsAfterSizing,
                    leafHeightsAfterSizing));
        }

        public override void Add(Component component)
        {
            this.children.Add(component);
        }

        public override void Remove(Component component)
        {
            this.children.Remove(component);
        }

        public override bool IsComposite()
        {
            return true;
        }
    }
}
