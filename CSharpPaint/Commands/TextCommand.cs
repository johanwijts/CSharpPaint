using CSharpPaint.Decorators;
using CSharpPaint.Editors.Shapes;
using System.Windows.Controls;

namespace CSharpPaint.Commands
{
    public class TextCommand : Command
    {
        public TextCommand(Editor Editor)
        {
            this.editor = Editor;
            this.currentShape = Editor.GetCurrentShape();
        }

        private readonly Editor editor;
        private BaseShape? currentShape;
        public IShape? newDecoratedShape;

        public override void Execute()
        {
            if (currentShape == null)
            {
                return;
            }

            newDecoratedShape = new TextDecorator(currentShape);
            TextBlock textBlock = new TextBlock();
            textBlock.FontSize = 9;
            textBlock.Text = newDecoratedShape.GetDescription();
            Canvas.SetTop(textBlock, Canvas.GetTop(currentShape.shape));
            Canvas.SetLeft(textBlock, Canvas.GetLeft(currentShape.shape));
            editor.canvas.Children.Add(textBlock);
        }

        public override void Undo()
        {
            editor.canvas.Children.RemoveAt(editor.canvas.Children.Count - 1);
        }
    }
}
