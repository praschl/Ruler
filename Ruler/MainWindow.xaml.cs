using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Ruler
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitializeSizingBoxes();
        }

        private void InitializeSizingBoxes()
        {
            double w2bs = Width - 2 * BoxSize;
            double h2bs = Height - 2 * BoxSize;
            double wr = Width - BoxSize;
            double hb = Height - BoxSize;

            _sizingBoxes = new[]
            {
                new SizingBox { Rect = new Rect(BoxSize, BoxSize, w2bs, h2bs), Cursor = Cursors.Arrow }, // center
                new SizingBox { Rect = new Rect(0, 0, BoxSize, BoxSize), Cursor = Cursors.SizeNWSE, SizeLeft = true, SizeTop = true },
                new SizingBox { Rect = new Rect(BoxSize, 0, w2bs, BoxSize), Cursor = Cursors.SizeNS, SizeTop = true },
                new SizingBox { Rect = new Rect(wr, 0, BoxSize, BoxSize), Cursor = Cursors.SizeNESW, SizeRight = true, SizeTop = true },
                new SizingBox { Rect = new Rect(wr, BoxSize, BoxSize, h2bs), Cursor = Cursors.SizeWE, SizeRight = true },
                new SizingBox { Rect = new Rect(wr, hb, BoxSize, BoxSize), Cursor = Cursors.SizeNWSE, SizeRight = true, SizeBottom = true },
                new SizingBox { Rect = new Rect(BoxSize, hb, w2bs, BoxSize), Cursor = Cursors.SizeNS, SizeBottom = true },
                new SizingBox { Rect = new Rect(0, hb, BoxSize, BoxSize), Cursor = Cursors.SizeNESW, SizeLeft = true, SizeBottom = true },
                new SizingBox { Rect = new Rect(0, BoxSize, BoxSize, h2bs), Cursor = Cursors.SizeWE, SizeLeft = true },
            };
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            DrawRuler(sender as Canvas);
        }

        private void DrawRuler(Canvas canvas)
        {
            canvas.Children.Clear();

            Rectangle border = new Rectangle();
            border.Stroke = Brushes.Black;
            border.StrokeThickness = 1.0;
            Canvas.SetLeft(border, 0);
            Canvas.SetTop(border, 0);
            border.Width = Width;
            border.Height = Height;
            canvas.Children.Add(border);

            for (int x = 0; x < canvas.ActualWidth; x += 2)
            {
                int length = 4;
                if (x % 10 == 0)
                    length = 8;
                if (x % 50 == 0)
                    length = 12;

                Line gridline = new Line();
                gridline.Stroke = Brushes.Black;
                gridline.StrokeThickness = 1.0;
                gridline.X1 = x;
                gridline.X2 = x;
                gridline.Y1 = 0;
                gridline.Y2 = length;

                canvas.Children.Add(gridline);
            }
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
            {
                CheckSizingBox(e);
            }

            if (_resizing)
            {
                DoResizing(e);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RecalculateSizingBoxes();
            DrawRuler(_ticksCanvas);
        }

        private bool _resizing = false;
        private Point _resizeClickPosition;
        private const int BoxSize = 5;
        private SizingBox[] _sizingBoxes;
        private SizingBox _currentSizingBox;

        private void DoResizing(MouseEventArgs e)
        {
            var newPos = e.GetPosition(this);
            Vector delta = newPos - _resizeClickPosition;

            double left = Left;
            double top = Top;
            double width = Width;
            double height = Height;

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

         
            if (left > 0 && left < 1980)
                Left = left;
            if (top > 0 && top < 1100)
                Top = top;
            if (width > MinWidth && width < MaxWidth)
                Width = width;
            if (height > MinHeight && height < MaxHeight)
                Height = height;
        }

        private void CheckSizingBox(MouseEventArgs e)
        {
            var pos = e.GetPosition(this);

            _currentSizingBox = _sizingBoxes.FirstOrDefault(b => b.Rect.Contains(pos));

            if (_currentSizingBox != null)
                Cursor = _currentSizingBox.Cursor;
        }
        
        private void RecalculateSizingBoxes()
        {
            double w2bs = Width - 2 * BoxSize;
            double h2bs = Height - 2 * BoxSize;
            double wr = Width - BoxSize;
            double hb = Height - BoxSize;

            _sizingBoxes[0].Rect = new Rect(BoxSize, BoxSize, w2bs, h2bs);
            _sizingBoxes[2].Rect = new Rect(BoxSize, 0, w2bs, BoxSize);
            _sizingBoxes[3].Rect = new Rect(wr, 0, BoxSize, BoxSize);
            _sizingBoxes[4].Rect = new Rect(wr, BoxSize, BoxSize, h2bs);
            _sizingBoxes[5].Rect = new Rect(wr, hb, BoxSize, BoxSize);
            _sizingBoxes[6].Rect = new Rect(BoxSize, hb, w2bs, BoxSize);
            _sizingBoxes[7].Rect = new Rect(0, hb, BoxSize, BoxSize);
            _sizingBoxes[8].Rect = new Rect(0, BoxSize, BoxSize, h2bs);
        }

        private class SizingBox
        {
            public Rect Rect;
            public Cursor Cursor;
            public bool SizeLeft;
            public bool SizeRight;
            public bool SizeTop;
            public bool SizeBottom;
        }


    }
}
