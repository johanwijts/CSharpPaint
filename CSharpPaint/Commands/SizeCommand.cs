using System.Windows.Shapes;

namespace CSharpPaint.Commands
{
    public class SizeCommand : Command
    {
        public SizeCommand(Editor Editor)
        {
            this.editor = Editor;
            this.currentShape = Editor.GetCurrentShape();
            this.shapeSizeBeforeSizing = Editor.GetShapeSizeBeforeSizing();
        }

        private readonly Editor editor;
        private readonly Shape? currentShape;
        private readonly (double offsetX, double offsetY) shapeSizeBeforeSizing;

        public override void Execute()
        {
            if (currentShape == null)
            {
                return;
            }

            editor.Finalize_Sizing(currentShape);
        }

        public override void Undo()
        {
            if (currentShape == null)
            {
                return;
            }

            currentShape.Width = shapeSizeBeforeSizing.offsetX;
            currentShape.Height = shapeSizeBeforeSizing.offsetY;
        }
    }
}
