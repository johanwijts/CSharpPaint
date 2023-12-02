using System.Windows.Input;

namespace CSharpPaint.Commands
{
    public class DrawCommand : Command
    {
        public DrawCommand(Editor Editor, object Sender, MouseButtonEventArgs E)
        {
            this.editor = Editor;
            this.sender = Sender;
            this.e = E;
        }

        private readonly Editor editor;
        private readonly object sender;
        private readonly MouseButtonEventArgs e;

        public override void Execute()
        {
            editor.Editor_MouseDown(sender, e);
        }

        public override void Undo()
        {
            editor.canvas.Children.RemoveAt(editor.canvas.Children.Count - 1);
        }
    }
}
