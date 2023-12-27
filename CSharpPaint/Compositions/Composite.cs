using CSharpPaint.Commands;
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
            var shapes = new List<Shape>();
            var leafPositions = new List<Point>();
            var leafPositionsBeforeMoving = new List<Point>();
            var leafPositionsAfterMoving = new List<Point>();

            foreach (var component in this.children)
            {
                if (!component.IsComposite())
                {
                    shapes.Add(((Leaf)component).shape);

                    var leaf = (Leaf)component;
                    var leafPoint = new Point(Canvas.GetLeft(leaf.shape), Canvas.GetTop(leaf.shape));
                    leafPositions.Add(leafPoint);
                    leafPositionsBeforeMoving.Add(leafPoint);
                    continue;
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
                    var leafPoint = new Point(Canvas.GetLeft(leaf.shape), Canvas.GetTop(leaf.shape));
                    leafPositionsAfterMoving.Add(leafPoint);
                }
            }

            invoker.Execute(new MoveGroupCommand(editor, shapes, leafPositionsBeforeMoving, leafPositionsAfterMoving));
        }

        public override void SizeOperation()
        {
            foreach (var component in this.children)
            {
                // Implement groupsizing
            }
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
