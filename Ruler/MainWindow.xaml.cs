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

        bool _resizing = false;
        Point _position;

        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {            
            DragMove();

            //CaptureMouse();
            //_resizing = true;
            //_position = e.GetPosition(this);
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //_resizing = false;

            //ReleaseMouseCapture();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_resizing)
            {
                CheckSizingBox(e);
            }

            if (_resizing)
            {
                var newPos = e.GetPosition(this);
                Vector delta = newPos - _position;
                _position = newPos;

                Width += delta.X;
                Height += delta.Y;
            }
        }

        private void CheckSizingBox(MouseEventArgs e)
        {
            var pos = e.GetPosition(this);


        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawRuler(_ticksCanvas);
        }
    }
}
