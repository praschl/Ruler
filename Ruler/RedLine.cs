using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MiP.Ruler
{
    public class RedLine : Canvas
    {
        private Line _redLine;
        private TextBlock _pixelText;

        public RedLine()
        {
            Initialize();

            MouseMove += RedLine_MouseMove;
            SizeChanged += RedLine_SizeChanged;
        }

        private void RedLine_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _redLine.Y2 = ActualHeight - 1;
            SetTop(_pixelText, ActualHeight/2-_pixelText.FontSize/2);
        }

        private void Initialize()
        {
            _redLine = new Line
            {
                Stroke = Brushes.Crimson,
                StrokeThickness = 1.0,
                Y1 = 1,
                Y2 = ActualHeight - 1
            };

            Children.Add(_redLine);

            _pixelText = new TextBlock
            {
                Foreground = Brushes.Crimson
            };

            SetLeft(_pixelText, 10);
            SetTop(_pixelText, 10);

            Children.Add(_pixelText);
        }

        private void RedLine_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(this);
            _redLine.X1 = _redLine.X2 = pos.X;

            _pixelText.Text = pos.X.ToString("#");
            _pixelText.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            if (pos.X >= _pixelText.DesiredSize.Width + 4)
                SetLeft(_pixelText, pos.X - _pixelText.DesiredSize.Width - 2);
            else
                SetLeft(_pixelText, pos.X + 2);
        }
    }
}