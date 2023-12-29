using CSharpPaint.Commands;
using CSharpPaint.Compositions;
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
        private bool keyIsDown;

        public Composite group;

        public MainWindow()
        {
            InitializeComponent();
            editor = new Editor
                (canvas,
                rectangleRadioButton,
                ellipseRadioButton);
            invoker = new Invoker();

            group = new Composite(invoker, editor);
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (groupingRadioButton.IsChecked == true && e.RightButton == MouseButtonState.Pressed)
            {
                rightMouseButtonDown = true;
                group.MoveOperation(new Point());
                return;
            }

            if (e.RightButton == MouseButtonState.Pressed)
            {
                rightMouseButtonDown = true;
                shapeIsConnectedToMouse = editor.Check_For_Drag_Connect(e);
            }
            else if (e.MiddleButton == MouseButtonState.Pressed)
            {
                middleMouseButtonDown = true;
                shapeIsConnectedToMouse = editor.Check_For_Sizing_Connect(e);

                if (shapeIsConnectedToMouse && editor.isSizing)
                {
                    var sizeCommand = new SizeCommand(editor);
                    invoker.Execute(sizeCommand);

                    shapeIsConnectedToMouse = false;
                }
                return;
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                leftMouseButtonDown = true;

                if (groupingRadioButton.IsChecked == true)
                {
                    editor.Check_For_Grouping_Connect(e, group);
                    return;
                } 
            }

            editor.Start_Drawing(sender, e);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            editor.Handle_Movement(sender, e);
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (groupingRadioButton.IsChecked == true)
            {
                return;
            }

            editor.Handle_Sizing(e, editor.GetCurrentShape());
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (groupingRadioButton.IsChecked == true && leftMouseButtonDown)
            {
                leftMouseButtonDown = false;
                return;
            }

            if (rightMouseButtonDown)
            {
                if (groupingRadioButton.IsChecked == true)
                {
                    rightMouseButtonDown = false;
                    editor.Stop_Moving();
                    shapeIsConnectedToMouse = false;
                    return;
                }

                rightMouseButtonDown = false;
                editor.Stop_Moving();
                if (shapeIsConnectedToMouse)
                {
                    invoker.Execute(new MoveCommand(editor));
                    shapeIsConnectedToMouse = false;
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
                shapeIsConnectedToMouse = false;
            }
        }     

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Z && Keyboard.Modifiers == ModifierKeys.Control && !leftMouseButtonDown && !keyIsDown)
            {
                keyIsDown = true;
                invoker.Undo();
            }
            else if (e.Key == Key.Y && Keyboard.Modifiers == ModifierKeys.Control && !leftMouseButtonDown && !keyIsDown)
            {
                keyIsDown = true;
                invoker.Redo();
            }
            else if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control && !leftMouseButtonDown && !keyIsDown)
            {
                keyIsDown = true;
                editor.Save();
            }
            else if (e.Key == Key.PageUp && !leftMouseButtonDown && !keyIsDown)
            {
                keyIsDown = true;
                group.SizeOperation(10);
            }
            else if (e.Key == Key.PageDown && !leftMouseButtonDown && !keyIsDown)
            {
                keyIsDown = true;
                group.SizeOperation(-10);
            }
        }

        private void Canvas_KeyUp(object sender, KeyEventArgs e)
        {
            keyIsDown = false;
        }
    }
}
