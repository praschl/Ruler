using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MiP.Ruler
{
    public class RulerLine
    {
        private readonly RulerLineDisplay _display;

        private readonly Size _infiniteSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
        private readonly Line _line;
        private readonly TextBlock _textBlock;

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

            MoveLineTo(position);
            ParentResized();
        }

        public Point Position { get; private set; }

        public void ParentResized()
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

        public void ChangeDirection()
        {
            Position = new Point(Position.Y, Position.X);
            MoveLineTo(Position);
            ParentResized();
        }

        public void RefreshText()
        {
            RefreshText(Position);
        }

        public void MoveLineTo(Point position)
        {
            if (_display.Orientation == Orientation.Horizontal)
            {
                _line.X1 = _line.X2 = position.X;
                _line.Y1 = 1;
                _line.Y2 = _display.ActualHeight - 2;

                RefreshText(position);
            }
            else
            {
                _line.Y1 = _line.Y2 = position.Y;
                _line.X1 = 1;
                _line.X2 = _display.ActualWidth - 2;

                RefreshText(position);
            }

            Position = position;
        }

        private void RefreshText(Point position)
        {
            if (_display.Orientation == Orientation.Horizontal)
            {
                _textBlock.Text = Format(position.X + 1);

                _textBlock.Measure(_infiniteSize);

                if (position.X >= _textBlock.DesiredSize.Width + 4)
                    Canvas.SetLeft(_textBlock, position.X - _textBlock.DesiredSize.Width - 2);
                else
                    Canvas.SetLeft(_textBlock, position.X + 2);
            }
            else
            {
                _textBlock.Text = Format(position.Y + 1);

                _textBlock.Measure(_infiniteSize);

                if (position.Y >= _textBlock.DesiredSize.Height + 4)
                    Canvas.SetTop(_textBlock, position.Y - _textBlock.DesiredSize.Height - 2);
                else
                    Canvas.SetTop(_textBlock, position.Y + 2);
            }
        }

        private string Format(double position)
        {
            var divisor = _display.Orientation == Orientation.Horizontal ? _display.ActualWidth : _display.ActualHeight;

            if (_display.ShowPercentages)
                return (position/divisor*100).ToString("0.0");

            return position.ToString("0");
        }
    }
}