using CSharpPaint.Editors.Shapes;
using System.Collections.Generic;

namespace CSharpPaint.Commands
{
    public class SizeGroupCommand : Command
    {
        public SizeGroupCommand
            (Editor Editor,
            List<BaseShape> shapes,
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
        List<BaseShape> shapes;
        private readonly List<double> leafWidthsBeforeSizing;
        private readonly List<double> leafHeightsBeforeSizing;
        private readonly List<double> leafWidthsAfterSizing;
        private readonly List<double> leafHeightsAfterSizing;

        public override void Execute()
        {
            int i = 0;

            foreach (var shape in shapes)
            {
                shape.shape!.Width = leafWidthsAfterSizing[i];
                shape.shape!.Height = leafHeightsAfterSizing[i];
                editor.Finalize_Sizing(shape);
                i++;
            }
        }

        public override void Undo()
        {
            int i = 0;

            foreach (var shape in shapes)
            {
                shape.shape!.Width = leafWidthsBeforeSizing[i];
                shape.shape!.Height = leafHeightsBeforeSizing[i];
                editor.Finalize_Sizing(shape);
                i++;
            }
        }
    }
}
