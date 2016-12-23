using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MiP.Ruler
{
    public class RulerLine
    {
        public Point Position { get; }
        private readonly RulerLineDisplay _display;
        private readonly Line _line;
        private readonly TextBlock _textBlock;
        
        private readonly Size _infiniteSize = new Size(double.PositiveInfinity, double.PositiveInfinity);

        public RulerLine(RulerLineDisplay display, Point position)
        {
            Position = position;
            _display = display;

            _line = new Line
            {
                Stroke = Brushes.Crimson,
                StrokeThickness = 1.0
            };

            _display.Children.Add(_line);

            _textBlock = new TextBlock
            {
                Foreground = Brushes.Crimson
            };

            _display.Children.Add(_textBlock);

            if (_display.Orientation == Orientation.Horizontal)
            {
                _line.Y1 = 1;
                _line.Y2 = _display.ActualHeight - 1;
                _line.X1 = position.X;
                _line.X2 = position.X;
                RefreshRulerText(position);
                Canvas.SetTop(_textBlock, _display.ActualHeight / 2 - _textBlock.FontSize / 2);
            }
            else
            {
                _line.X1 = 1;
                _line.X2 = _display.ActualWidth - 1;
                _line.Y1 = position.Y;
                _line.Y2 = position.Y;
                RefreshRulerText(position);
                Canvas.SetLeft(_textBlock, _display.ActualWidth / 2 - _textBlock.DesiredSize.Width / 2);
            }
        }

        public void RefreshSize()
        {
            if (_display.Orientation == Orientation.Horizontal)
            {
                var lineY = _display.ActualHeight - 1;
                var textY = _display.ActualHeight/2 - _textBlock.FontSize/2;
                _line.Y2 = lineY;
                Canvas.SetTop(_textBlock, textY);
            }
            else
            {
                var lineX = _display.ActualWidth - 1;
                var textX = _display.ActualWidth/2 - _textBlock.DesiredSize.Width/2;
                _line.X2 = lineX;
                Canvas.SetLeft(_textBlock, textX);
            }
        }
        
        public void RefreshLine(Point pos)
        {
            if (_display.Orientation == Orientation.Horizontal)
            {
                _line.X1 = _line.X2 = pos.X;
                _line.Y1 = 1;
                _line.Y2 = _display.ActualHeight - 2;
            }
            else
            {
                _line.Y1 = _line.Y2 = pos.Y;
                _line.X1 = 1;
                _line.X2 = _display.ActualWidth - 2;
            }

            RefreshRulerText(pos);
        }
        
        private void RefreshRulerText(Point pos)
        {
            if (_display.Orientation == Orientation.Horizontal)
            {
                _textBlock.Text = pos.X.ToString("0");
                _textBlock.Measure(_infiniteSize);

                if (pos.X >= _textBlock.DesiredSize.Width + 4)
                    Canvas.SetLeft(_textBlock, pos.X - _textBlock.DesiredSize.Width - 2);
                else
                    Canvas.SetLeft(_textBlock, pos.X + 2);
            }
            else
            {
                _textBlock.Text = pos.Y.ToString("0");
                _textBlock.Measure(_infiniteSize);

                if (pos.Y >= _textBlock.DesiredSize.Height + 4)
                    Canvas.SetTop(_textBlock, pos.Y - _textBlock.DesiredSize.Height - 2);
                else
                    Canvas.SetTop(_textBlock, pos.Y + 2);
            }
        }

        public void SetVisible(bool visible)
        {
            Brush color = visible ? Brushes.Crimson : Brushes.Transparent;
            _line.Stroke = color;
            _textBlock.Foreground = color;
        }

        public void RemoveFromDisplay()
        {
            _display.Children.Remove(_line);
            _display.Children.Remove(_textBlock);
        }
    }
}