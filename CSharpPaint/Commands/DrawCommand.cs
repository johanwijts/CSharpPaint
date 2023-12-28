using CSharpPaint.Editors.Shapes;

namespace CSharpPaint.Commands
{
    public class DrawCommand : Command
    {
        public DrawCommand(Editor Editor)
        {
            this.editor = Editor;
            this.currentShape = Editor.GetCurrentShape();
        }

        private readonly Editor editor;
        private readonly BaseShape? currentShape;

        public override void Execute()
        {
            if (currentShape == null)
            {
                return;
            }

            editor.Finalize_Drawing(currentShape);
        }

        public override void Undo()
        {
            editor.canvas.Children.RemoveAt(editor.canvas.Children.Count - 1);
        }
    }
}
