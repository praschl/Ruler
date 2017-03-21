using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MiP.Ruler.Annotations;
using MiP.Ruler.Commands;

namespace MiP.Ruler
{
    // Yup, some parts here are event handlers, and some are commands.
    // I've used what worked better and simpler for each use case.
    // Many things like mouseevents cant be expressed as commands properly
    // and some other work better as commands like Keybindings.

    public partial class MainWindow : INotifyPropertyChanged
    {
        private const int ResizingBoxSize = 10;

        private SizingBox _currentResizingBox;
        private Point _lastClickPosition;

        private Point _oldWindowPosition;
        private Vector _oldWindowSize;

        private SizingBox[] _sizingBoxes;
        private bool _statusDoubleClicked;
        private bool _statusResizing;
        private bool _showPercentages;

        public MainWindow()
        {
            InitializeComponent();

            InitializeSizingBoxes();
        }

        public ICommand CloseCommand => new CloseCommand(this);

        public ICommand ClearRulerLinesCommand => new ClearRulerLinesCommand(this);

        public ICommand ToggleOrientationCommand => new SwitchOrientationCommand(this, true);
        public ICommand SwitchHorizontalCommand => new SwitchOrientationCommand(this, Orientation.Horizontal);
        public ICommand SwitchVerticalCommand => new SwitchOrientationCommand(this, Orientation.Vertical);
        public ICommand ShowAboutWindowCommand => new ShowAboutWindowCommand(this);

        public ICommand TogglePercentageCommand => new TogglePercentageCommand(this);

        public Config Config { get; } = Config.Instance;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void ClearLines()
        {
            _rulerLineDisplay.ClearRulerLines();
        }

        public void TogglePercentages()
        {
            _rulerLineDisplay.TogglePercentages();
        }

        private void MainWindow_OnMouseEnter(object sender, MouseEventArgs e)
        {
            _rulerLineDisplay.SetCurrentVisible(true);
        }

        private void MainWindow_OnMouseLeave(object sender, MouseEventArgs e)
        {
            _rulerLineDisplay.SetCurrentVisible(false);
        }

        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _oldWindowPosition = new Point(Left, Top);
            _oldWindowSize = new Vector(Width, Height);
            _lastClickPosition = e.GetPosition(this);

            if (_currentResizingBox.Cursor == Cursors.Arrow)
            {
                DragMove();
            }
            else
            {
                CaptureMouse();
                _statusResizing = true;
                _rulerLineDisplay.SetCurrentVisible(false);
            }
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_statusResizing)
                CheckSizingBox(e);

            if (_statusResizing)
                DoResizing(e);
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RecalculateSizingBoxes();
            if (!Config.LockOrientationOnResize)
                Config.Orientation = Width > Height ? Orientation.Horizontal : Orientation.Vertical;
            _rulerLineDisplay.ParentSizeChanged();
        }

        private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();

            _statusResizing = false;
            _rulerLineDisplay.SetCurrentVisible(true);

            var newPosition = new Point(Left, Top);
            var newSize = new Vector(Width, Height);

            if ((newPosition == _oldWindowPosition) && (_oldWindowSize == newSize) && !_statusDoubleClicked)
                _rulerLineDisplay.AddNewRulerLine(_lastClickPosition);

            _statusDoubleClicked = false;
        }

        private void MainWindow_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _rulerLineDisplay.RemoveLast();

            var position = e.GetPosition(this);

            SwitchDirection(position);

            _statusDoubleClicked = true;
        }

        private void MainWindow_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var delta = e.Delta > 0 ? 0.1 : -0.1;

            var newOpacity = Opacity + delta;

            if (newOpacity > 1.0)
                newOpacity = 1.0;
            if (newOpacity < 0.1)
                newOpacity = 0.1;

            Opacity = newOpacity;
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers == ModifierKeys.Alt) && (e.SystemKey != Key.F4))
            {
                e.Handled = true;
                return;
            }

            // easier to implement than with a huge load of different key bindings
            var pixel = 1;
            if (Keyboard.IsKeyDown(Key.LeftShift) | Keyboard.IsKeyDown(Key.RightShift))
                pixel = 5;
            if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl))
                pixel = 25;

            if (e.Key == Key.Left)
                Left -= pixel;
            if (e.Key == Key.Right)
                Left += pixel;
            if (e.Key == Key.Up)
                Top -= pixel;
            if (e.Key == Key.Down)
                Top += pixel;
        }
        
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            AboutWindow.CloseWindow();
        }

        public void SwitchDirection(Point pos)
        {
            var left = Left + pos.X - pos.Y;
            var top = Top + pos.Y - pos.X;
            var width = Height;
            var height = Width;

            Left = left;
            Top = top;
            Width = width;
            Height = height;

            Config.Orientation = Config.Orientation == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;
        }

        private void InitializeSizingBoxes()
        {
            var width = Width - 2*ResizingBoxSize; // width of middle boxes (top, center, bottom)
            var height = Height - 2*ResizingBoxSize; // height of middle boxes (left, middle right)
            var right = Width - ResizingBoxSize; // x of right boxes (right-top, right-middle, right-bottom)
            var bottom = Height - ResizingBoxSize; // y of bottom boxes (left-bottom, middle-bottom, right-bottom)

            _sizingBoxes = new[]
            {
                new SizingBox {Rect = new Rect(ResizingBoxSize, ResizingBoxSize, width, height), Cursor = Cursors.Arrow}, // center
                new SizingBox {Rect = new Rect(0, 0, ResizingBoxSize, ResizingBoxSize), Cursor = Cursors.SizeNWSE, SizeLeft = true, SizeTop = true},
                new SizingBox {Rect = new Rect(ResizingBoxSize, 0, width, ResizingBoxSize), Cursor = Cursors.SizeNS, SizeTop = true},
                new SizingBox {Rect = new Rect(right, 0, ResizingBoxSize, ResizingBoxSize), Cursor = Cursors.SizeNESW, SizeRight = true, SizeTop = true},
                new SizingBox {Rect = new Rect(right, ResizingBoxSize, ResizingBoxSize, height), Cursor = Cursors.SizeWE, SizeRight = true},
                new SizingBox {Rect = new Rect(right, bottom, ResizingBoxSize, ResizingBoxSize), Cursor = Cursors.SizeNWSE, SizeRight = true, SizeBottom = true},
                new SizingBox {Rect = new Rect(ResizingBoxSize, bottom, width, ResizingBoxSize), Cursor = Cursors.SizeNS, SizeBottom = true},
                new SizingBox {Rect = new Rect(0, bottom, ResizingBoxSize, ResizingBoxSize), Cursor = Cursors.SizeNESW, SizeLeft = true, SizeBottom = true},
                new SizingBox {Rect = new Rect(0, ResizingBoxSize, ResizingBoxSize, height), Cursor = Cursors.SizeWE, SizeLeft = true}
            };
        }

        private void RecalculateSizingBoxes()
        {
            var innerWidth = Width - 2*ResizingBoxSize;
            var innerHeight = Height - 2*ResizingBoxSize;
            var outerWidth = Width - ResizingBoxSize;
            var outerHeight = Height - ResizingBoxSize;

            _sizingBoxes[0].Rect.Width = innerWidth;
            _sizingBoxes[0].Rect.Height = innerHeight;
            _sizingBoxes[2].Rect.Width = innerWidth;
            _sizingBoxes[3].Rect.X = outerWidth;
            _sizingBoxes[4].Rect.X = outerWidth;
            _sizingBoxes[4].Rect.Height = innerHeight;
            _sizingBoxes[5].Rect.X = outerWidth;
            _sizingBoxes[5].Rect.Y = outerHeight;
            _sizingBoxes[6].Rect.Y = outerHeight;
            _sizingBoxes[6].Rect.Width = innerWidth;
            _sizingBoxes[7].Rect.Y = outerHeight;
            _sizingBoxes[8].Rect.Height = innerHeight;
        }

        private void DoResizing(MouseEventArgs e)
        {
            var newPosition = e.GetPosition(this);
            var delta = newPosition - _lastClickPosition;

            var left = Left;
            var top = Top;
            var width = Width;
            var height = Height;

            // TODO: needs a bit of work to improve behaviour when width/height is very close to minimum/maximum
            // when starting resize, get screen position of mouse, and change resizing code to use screen position

            if (_currentResizingBox.SizeLeft)
            {
                width -= delta.X;
                if (width > MinWidth)
                    left += delta.X;
            }

            if (_currentResizingBox.SizeTop)
            {
                height -= delta.Y;
                if (height > MinHeight)
                    top += delta.Y;
            }

            if (_currentResizingBox.SizeRight)
            {
                width += delta.X;
                if (width > MinWidth)
                    _lastClickPosition.X = newPosition.X;
            }

            if (_currentResizingBox.SizeBottom)
            {
                height += delta.Y;
                if (height > MinHeight)
                    _lastClickPosition.Y = newPosition.Y;
            }

            Left = left;
            Top = top;

            if ((width > MinWidth) && (width < MaxWidth))
                Width = width;

            if ((height > MinHeight) && (height < MaxHeight))
                Height = height;
        }

        private void CheckSizingBox(MouseEventArgs e)
        {
            var position = e.GetPosition(this);

            _currentResizingBox = _sizingBoxes.FirstOrDefault(b => b.Rect.Contains(position)) ?? _sizingBoxes[0];

            Cursor = _currentResizingBox.Cursor;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private class SizingBox
        {
            public Cursor Cursor;
            public Rect Rect;
            public bool SizeBottom;
            public bool SizeLeft;
            public bool SizeRight;
            public bool SizeTop;
        }
    }
}