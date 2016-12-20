using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using MiP.Ruler.Annotations;

namespace MiP.Ruler
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        private const int BoxSize = 5;
        private Point _clickPosition;
        private SizingBox _currentSizingBox;

        private bool _doubleClicked;

        private Point _oldWindowPosition;
        private Vector _oldWindowSize;

        private bool _resizing;
        private SizingBox[] _sizingBoxes;
        private bool _isHorizontal;
        private string _switchDirectionText;

        public MainWindow()
        {
            InitializeComponent();

            InitializeSizingBoxes();
        }

        public bool IsHorizontal
        {
            get { return _isHorizontal; }
            set
            {
                if (value == _isHorizontal) return;
                _isHorizontal = value;
                OnPropertyChanged();

                SwitchDirectionText = _isHorizontal ? "Switch to vertical" : "Switch to horizontal";
            }
        }

        public ICommand CloseCommand => new CloseCommand(this);

        public ICommand ClearLinesCommand => new ClearLinesCommand(this);

        public ICommand SwitchDirectionCommand => new SwitchDirectionCommand(this);

        public string SwitchDirectionText
        {
            get { return _switchDirectionText; }
            set
            {
                if (value == _switchDirectionText) return;
                _switchDirectionText = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _oldWindowPosition = new Point(Left, Top);
            _oldWindowSize = new Vector(Width, Height);
            _clickPosition = e.GetPosition(this);

            if (Cursor == Cursors.Arrow)
            {
                DragMove();
            }
            else
            {
                CaptureMouse();
                _resizing = true;
                _redLine.HideCurrent();
            }
        }

        private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();

            _resizing = false;
            _redLine.ShowCurrent();

            var newpos = new Point(Left, Top);
            var newSize = new Vector(Width, Height);

            if ((newpos == _oldWindowPosition) && (_oldWindowSize == newSize) && !_doubleClicked)
                _redLine.AddLine(_clickPosition);

            if (_doubleClicked)
            {
                _doubleClicked = false;
                _redLine.ClearLines();
            }
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_resizing)
                CheckSizingBox(e);

            if (_resizing)
                DoResizing(e);
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

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RecalculateSizingBoxes();
            IsHorizontal = Width > Height;
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

        private void MainWindow_OnMouseEnter(object sender, MouseEventArgs e)
        {
            _redLine.ShowCurrent();
        }

        private void MainWindow_OnMouseLeave(object sender, MouseEventArgs e)
        {
            _redLine.HideCurrent();
        }

        private void MainWindow_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _redLine.ClearLines();
            _redLine.HideCurrent();

            var pos = e.GetPosition(this);

            SwitchDirection(pos, true);
        }

        public void SwitchDirection(Point pos, bool isDoubleClick)
        {
            var left = Left + pos.X - pos.Y;
            var top = Top + pos.Y - pos.X;
            var width = Height;
            var height = Width;

            Left = left;
            Top = top;
            Width = width;
            Height = height;

            _doubleClicked = isDoubleClick;
        }

        private void InitializeSizingBoxes()
        {
            // ReSharper disable InconsistentNaming
            var w2bs = Width - 2*BoxSize;
            var h2bs = Height - 2*BoxSize;
            // ReSharper restore InconsistentNaming
            var wr = Width - BoxSize;
            var hb = Height - BoxSize;

            _sizingBoxes = new[]
            {
                new SizingBox {Rect = new Rect(BoxSize, BoxSize, w2bs, h2bs), Cursor = Cursors.Arrow}, // center
                new SizingBox {Rect = new Rect(0, 0, BoxSize, BoxSize), Cursor = Cursors.SizeNWSE, SizeLeft = true, SizeTop = true},
                new SizingBox {Rect = new Rect(BoxSize, 0, w2bs, BoxSize), Cursor = Cursors.SizeNS, SizeTop = true},
                new SizingBox {Rect = new Rect(wr, 0, BoxSize, BoxSize), Cursor = Cursors.SizeNESW, SizeRight = true, SizeTop = true},
                new SizingBox {Rect = new Rect(wr, BoxSize, BoxSize, h2bs), Cursor = Cursors.SizeWE, SizeRight = true},
                new SizingBox {Rect = new Rect(wr, hb, BoxSize, BoxSize), Cursor = Cursors.SizeNWSE, SizeRight = true, SizeBottom = true},
                new SizingBox {Rect = new Rect(BoxSize, hb, w2bs, BoxSize), Cursor = Cursors.SizeNS, SizeBottom = true},
                new SizingBox {Rect = new Rect(0, hb, BoxSize, BoxSize), Cursor = Cursors.SizeNESW, SizeLeft = true, SizeBottom = true},
                new SizingBox {Rect = new Rect(0, BoxSize, BoxSize, h2bs), Cursor = Cursors.SizeWE, SizeLeft = true}
            };
        }

        private void DoResizing(MouseEventArgs e)
        {
            var newPos = e.GetPosition(this);
            var delta = newPos - _clickPosition;

            var left = Left;
            var top = Top;
            var width = Width;
            var height = Height;

            if (_currentSizingBox.SizeLeft)
            {
                width -= delta.X;
                if (width > MinWidth)
                    left += delta.X;
            }

            if (_currentSizingBox.SizeTop)
            {
                height -= delta.Y;
                if (height > MinHeight)
                    top += delta.Y;
            }

            if (_currentSizingBox.SizeRight)
            {
                width += delta.X;
                _clickPosition.X = newPos.X;
            }

            if (_currentSizingBox.SizeBottom)
            {
                height += delta.Y;
                _clickPosition.Y = newPos.Y;
            }

            if ((left + width > 2*BoxSize) && (left < SystemParameters.PrimaryScreenWidth - BoxSize*2))
                Left = left;
            if ((top + height > 2*BoxSize) && (top < SystemParameters.PrimaryScreenHeight - BoxSize*2))
                Top = top;
            if ((width > MinWidth) && (width < MaxWidth))
                Width = width;
            if ((height > MinHeight) && (height < MaxHeight))
                Height = height;
        }

        private void CheckSizingBox(MouseEventArgs e)
        {
            var pos = e.GetPosition(this);

            _currentSizingBox = _sizingBoxes.FirstOrDefault(b => b.Rect.Contains(pos)) ?? _sizingBoxes[0];

            Cursor = _currentSizingBox.Cursor;
        }

        private void RecalculateSizingBoxes()
        {
            // ReSharper disable InconsistentNaming
            var w2bs = Width - 2*BoxSize;
            var h2bs = Height - 2*BoxSize;
            // ReSharper restore InconsistentNaming
            var wr = Width - BoxSize;
            var hb = Height - BoxSize;

            _sizingBoxes[0].Rect.Width = w2bs;
            _sizingBoxes[0].Rect.Height = h2bs;
            _sizingBoxes[2].Rect.Width = w2bs;
            _sizingBoxes[3].Rect.X = wr;
            _sizingBoxes[4].Rect.X = wr;
            _sizingBoxes[4].Rect.Height = h2bs;
            _sizingBoxes[5].Rect.X = wr;
            _sizingBoxes[5].Rect.Y = hb;
            _sizingBoxes[6].Rect.Y = hb;
            _sizingBoxes[6].Rect.Width = w2bs;
            _sizingBoxes[7].Rect.Y = hb;
            _sizingBoxes[8].Rect.Height = h2bs;
        }

        public void ClearLines()
        {
            _redLine.ClearLines();
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