using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace CSharpPaint.Commands
{
    public class MoveGroupCommand : Command
    {
        public MoveGroupCommand(Editor Editor, List<Shape> shapes, List<Point> leafPositionsBeforeMoving, List<Point> leafPositionsAfterMoving)
        {
            this.editor = Editor;
            this.shapes = shapes;
            this.leafPositionsBeforeMoving = leafPositionsBeforeMoving;
            this.leafPositionsAfterMoving = leafPositionsAfterMoving;
        }

        private readonly Editor editor;
        List<Shape> shapes;
        private readonly List<Point> leafPositionsBeforeMoving;
        private readonly List<Point> leafPositionsAfterMoving;

        public override void Execute()
        {
            int i = 0;

            foreach (var shape in shapes)
            {
                Canvas.SetLeft(shape, leafPositionsAfterMoving[i].X);
                Canvas.SetTop(shape, leafPositionsAfterMoving[i].Y);
                editor.Finalize_Moving(shape);
                i++;
            }
        }

        public override void Undo()
        {
            int i = 0;

            foreach (var shape in shapes)
            {
                Canvas.SetLeft(shape, leafPositionsBeforeMoving[i].X);
                Canvas.SetTop(shape, leafPositionsBeforeMoving[i].Y);
                editor.Finalize_Moving(shape);
                i++;
            }
        }
    }
}
