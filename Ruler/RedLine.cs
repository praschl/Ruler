using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Ruler
{
    public class RedLine : Canvas
    {
        private Line _redLine;

        public RedLine()
        {
            Initialize();

            MouseMove += RedLine_MouseMove;
            SizeChanged += RedLine_SizeChanged;
        }

        private void RedLine_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _redLine.Y2 = ActualHeight - 1;
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
        }

        private void RedLine_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(this);
            _redLine.X1 = _redLine.X2 = pos.X;
        }
    }
}