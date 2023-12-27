using System.Collections.Generic;
using System.Windows.Shapes;

namespace CSharpPaint.Commands
{
    public class SizeGroupCommand : Command
    {
        public SizeGroupCommand
            (Editor Editor,
            List<Shape> shapes,
            List<double> leafWidthsBeforeSizing,
            List<double> leafHeightsBeforeSizing,
            List<double> leafWidthsAfterSizing,
            List<double> leafHeightsAfterSizing)
        {
            this.editor = Editor;
            this.shapes = shapes;
            this.leafWidthsBeforeSizing = leafWidthsBeforeSizing;
            this.leafHeightsBeforeSizing = leafHeightsBeforeSizing;
            this.leafWidthsAfterSizing = leafWidthsAfterSizing;
            this.leafHeightsAfterSizing = leafHeightsAfterSizing;
        }

        private readonly Editor editor;
        List<Shape> shapes;
        private readonly List<double> leafWidthsBeforeSizing;
        private readonly List<double> leafHeightsBeforeSizing;
        private readonly List<double> leafWidthsAfterSizing;
        private readonly List<double> leafHeightsAfterSizing;

        public override void Execute()
        {
            int i = 0;

            foreach (var shape in shapes)
            {
                shape.Width = leafWidthsAfterSizing[i];
                shape.Height = leafHeightsAfterSizing[i];
                editor.Finalize_Sizing(shape);
                i++;
            }
        }

        public override void Undo()
        {
            int i = 0;

            foreach (var shape in shapes)
            {
                shape.Width = leafWidthsBeforeSizing[i];
                shape.Height = leafHeightsBeforeSizing[i];
                editor.Finalize_Sizing(shape);
                i++;
            }
        }
    }
}
