using CSharpPaint.Commands;
using CSharpPaint.Invokers;
using System.Windows;
using System.Windows.Input;

namespace CSharpPaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Editor editor;
        private readonly Invoker invoker;

        private bool rightMouseButtonDown;
        private bool middleMouseButtonDown;
        private bool leftMouseButtonDown;
        private bool shapeIsConnectedToMouse;

        public MainWindow()
        {
            InitializeComponent();
            editor = new Editor(canvas, rectangleRadioButton, ellipseRadioButton);
            invoker = new Invoker();
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                rightMouseButtonDown = true;
                shapeIsConnectedToMouse = editor.Check_For_Drag_Connect(e);
            }
            else if (e.MiddleButton == MouseButtonState.Pressed)
            {
                middleMouseButtonDown = true;
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                leftMouseButtonDown = true;
            }
            editor.Start_Drawing(sender, e);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            editor.Handle_Movement(sender, e);
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            editor.Handle_Sizing(sender, e);
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (rightMouseButtonDown)
            {
                rightMouseButtonDown = false;
                editor.Stop_Moving();
                if (shapeIsConnectedToMouse)
                {
                    invoker.Execute(new MoveCommand(editor));
                }
            }
            else if (middleMouseButtonDown)
            {
                middleMouseButtonDown = false;
            }
            else if (leftMouseButtonDown)
            {
                leftMouseButtonDown = false;
                editor.Stop_Drawing();
                invoker.Execute(new DrawCommand(editor));
            }
        }     

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Z && Keyboard.Modifiers == ModifierKeys.Control)
            {
                invoker.Undo();
            }
            else if (e.Key == Key.Y && Keyboard.Modifiers == ModifierKeys.Control)
            {
                invoker.Redo();
            }
        }

    }
}
