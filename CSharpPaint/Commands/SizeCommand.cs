using CSharpPaint.Editors.Shapes;

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
        private BaseShape? currentShape;
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
            if (currentShape?.shape == null)
            {
                return;
            }

            currentShape.shape.Width = shapeSizeBeforeSizing.offsetX;
            currentShape.shape.Height = shapeSizeBeforeSizing.offsetY;
        }

        public void SetCurrentShape(BaseShape shape)
        {
            currentShape = shape;
        }
    }
}
