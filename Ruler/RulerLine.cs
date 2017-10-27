using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MiP.Ruler
{
    public class RulerLine
    {
        private readonly RulerLineDisplay _display;
        private readonly bool _isCursorLine;
        private readonly Size _infiniteSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
        private readonly Line _line;
        private readonly TextBlock _textBlock;

        private readonly Brush _colorRed = Brushes.Crimson;
        private readonly Brush _colorBlue = Brushes.MediumBlue;
        private readonly Config _config;

        public RulerLine(RulerLineDisplay display, Point position, bool isCursorLine = false)
        {
            _config = Config.Instance;

            Position = position;
            _isCursorLine = isCursorLine;
            _display = display;

            _line = new Line
            {
                Stroke = _colorRed,
                StrokeThickness = 1.0
            };

            _display.Children.Add(_line);

            _textBlock = new TextBlock
            {
                Foreground = _colorRed
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
                var textY = _display.ActualHeight / 2 - _textBlock.FontSize / 2;
                _line.Y2 = lineY;
                Canvas.SetTop(_textBlock, textY);
            }
            else
            {
                var lineX = _display.ActualWidth - 1;
                var textX = _display.ActualWidth / 2 - _textBlock.DesiredSize.Width / 2;
                _line.X2 = lineX;
                Canvas.SetLeft(_textBlock, textX);
            }
        }

        public bool Visible { get; set; } = true;

        public void SetVisible(bool visible)
        {
            Visible = visible;

            SetColor();
        }

        private void SetColor()
        {
            var color = Visible
                ? (_config.ShowPercentages ? _colorBlue : _colorRed)
                : Brushes.Transparent;

            _line.Stroke = color;
            _textBlock.Foreground = color;
        }

        public void RemoveFromDisplay()
        {
            if (_isCursorLine)
                return;

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
            RefreshTextAndColor(Position);
        }

        public void MoveLineTo(Point position)
        {
            Position = position;
            if (_display.Orientation == Orientation.Horizontal)
            {
                _line.X1 = _line.X2 = position.X;
                _line.Y1 = 1;
                _line.Y2 = _display.ActualHeight - 2;

                RefreshTextAndColor(position);
            }
            else
            {
                _line.Y1 = _line.Y2 = position.Y;
                _line.X1 = 1;
                _line.X2 = _display.ActualWidth - 2;

                RefreshTextAndColor(position);
            }           
        }

        private void RefreshTextAndColor(Point position)
        {
            SetColor();

            if (_display.Orientation == Orientation.Horizontal)
            {
                _textBlock.Text = Format(position.X);

                _textBlock.Measure(_infiniteSize);

                if (position.X >= _textBlock.DesiredSize.Width + 4)
                    Canvas.SetLeft(_textBlock, position.X - _textBlock.DesiredSize.Width - 2);
                else
                    Canvas.SetLeft(_textBlock, position.X + 2);
            }
            else
            {
                _textBlock.Text = Format(position.Y);

                _textBlock.Measure(_infiniteSize);

                if (position.Y >= _textBlock.DesiredSize.Height + 4)
                    Canvas.SetTop(_textBlock, position.Y - _textBlock.DesiredSize.Height - 2);
                else
                    Canvas.SetTop(_textBlock, position.Y + 2);
            }
        }        

        private string Format(double position)
        {
            if (_config.RelativeDisplay)
            {
                double previous = 0;

                foreach (var line in _display.RulerLines)
                {
                    var linePos = _display.Orientation == Orientation.Horizontal ? line.Position.X : line.Position.Y;

                    if (linePos > previous && linePos < position)
                        previous = linePos;
                }                

                position = position - previous;
            }

            position += 1;

            var divisor = _display.Orientation == Orientation.Horizontal ? _display.ActualWidth : _display.ActualHeight;

            if (_config.ShowPercentages)
                return (position / divisor * 100).ToString("0.0") + "%";

            return position.ToString("0");

        }
    }
}