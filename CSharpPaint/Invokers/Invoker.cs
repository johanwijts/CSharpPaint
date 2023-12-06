using CSharpPaint.Commands;
using System.Collections.Generic;

namespace CSharpPaint.Invokers
{
    public class Invoker
    {
        private readonly Stack<Command> undoStack = new Stack<Command>();
        private readonly Stack<Command> redoStack = new Stack<Command>();

        public void Execute(Command command)
        {
            command.Execute();
            undoStack.Push(command);
        }

        public void Undo()
        {
            if (undoStack.Count == 0)
            {
                return;
            }

            Command command = undoStack.Pop();
            command.Undo();
            redoStack.Push(command);
        }

        public void Redo()
        {
            if (redoStack.Count == 0)
            {
                return;
            }

            Command command = redoStack.Pop();
            command.Execute();
            undoStack.Push(command);
        }
    }
}
