using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MiP.Ruler
{
    public class RulerBorder : Canvas
    {
        // this is implemented as canvas instead of deriving from Border, to allow clicks go to the window.
        private Rectangle _border;

        public RulerBorder()
        {
            Initialize();

            SizeChanged += BlackBorder_SizeChanged;
        }

        private void Initialize()
        {
            _border = new Rectangle
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1.0,
                Width = ActualWidth,
                Height = ActualHeight
            };
            SetLeft(_border, 0);
            SetTop(_border, 0);
            Children.Add(_border);
        }

        private void BlackBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _border.Width = ActualWidth;
            _border.Height = ActualHeight;
        }
    }
}