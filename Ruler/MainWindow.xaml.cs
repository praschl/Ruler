using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MiP.Ruler
{
    public partial class MainWindow
    {
        private const int BoxSize = 5;
        private SizingBox _currentSizingBox;
        private Point _resizeClickPosition;

        private bool _resizing;
        private SizingBox[] _sizingBoxes;

        public MainWindow()
        {
            InitializeComponent();

            InitializeSizingBoxes();
        }

        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Cursor == Cursors.Arrow)
                DragMove();
            else
            {
                CaptureMouse();
                _resizing = true;
                _resizeClickPosition = e.GetPosition(this);
            }
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _resizing = false;

            ReleaseMouseCapture();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_resizing)
                CheckSizingBox(e);

            if (_resizing)
                DoResizing(e);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RecalculateSizingBoxes();
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
            var delta = newPos - _resizeClickPosition;

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
                _resizeClickPosition.X = newPos.X;
            }

            if (_currentSizingBox.SizeBottom)
            {
                height += delta.Y;
                _resizeClickPosition.Y = newPos.Y;
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

            _currentSizingBox = _sizingBoxes.FirstOrDefault(b => b.Rect.Contains(pos));

            if (_currentSizingBox == null)
                return;
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

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            var pixel = 1;
            if (Keyboard.IsKeyDown(Key.LeftShift) | Keyboard.IsKeyDown(Key.RightShift))
                pixel = 5;
            if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl))
                pixel = 25;

            switch (e.Key)
            {
                case Key.Left:
                    Left -= pixel;
                    break;
                case Key.Right:
                    Left += pixel;
                    break;
                case Key.Up:
                    Top -= pixel;
                    break;
                case Key.Down:
                    Top += pixel;
                    break;
                default:
                    return;
            }
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