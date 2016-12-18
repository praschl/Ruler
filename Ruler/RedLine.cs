using System.Collections.Generic;
using System.Linq;
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

        private readonly List<Line> _lines = new List<Line>();
        private readonly List<TextBlock> _pixelTexts = new List<TextBlock>();

        public RedLine()
        {
            Initialize();

            MouseMove += RedLine_MouseMove;
            SizeChanged += RedLine_SizeChanged;
        }

        private void RedLine_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _redLine.Y2 = ActualHeight - 1;
            SetTop(_pixelText, ActualHeight/2 - _pixelText.FontSize/2);

            foreach (var line in _lines)
            {
                line.Y2 = ActualHeight - 1;
            }
            foreach (var pixelText in _pixelTexts)
            {
                SetTop(pixelText, ActualHeight / 2 - pixelText.FontSize / 2);
            }
        }

        private void Initialize()
        {
            _redLine = new Line
            {
                Stroke = Brushes.Transparent,
                StrokeThickness = 1.0,
                Y1 = 1,
                Y2 = ActualHeight - 1
            };

            Children.Add(_redLine);

            _pixelText = new TextBlock
            {
                Foreground = Brushes.Transparent
            };

            SetLeft(_pixelText, 10);
            SetTop(_pixelText, 10);

            Children.Add(_pixelText);
        }

        private void RedLine_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(this);

            _redLine.X1 = _redLine.X2 = pos.X;

            MovePixelText(pos, _pixelText);
        }

        private void MovePixelText(Point pos, TextBlock pixelText)
        {
            pixelText.Text = pos.X.ToString("0");
            pixelText.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            if (pos.X >= pixelText.DesiredSize.Width + 4)
                SetLeft(pixelText, pos.X - pixelText.DesiredSize.Width - 2);
            else
                SetLeft(pixelText, pos.X + 2);
        }

        public void AddLine(Point position)
        {
            var newLine = new Line
            {
                Stroke = Brushes.Crimson,
                StrokeThickness = 1.0,
                Y1 = 1,
                Y2 = ActualHeight - 1,
                X1 = position.X,
                X2 = position.X
            };

            _lines.Add(newLine);
            Children.Add(newLine);

            // 

            var newText = new TextBlock
            {
                Foreground = Brushes.Crimson
            };

            _pixelTexts.Add(newText);
            Children.Add(newText);

            MovePixelText(position, newText);
            SetTop(newText, ActualHeight / 2 - newText.FontSize / 2);
        }

        public void ClearLines()
        {
            foreach (var element in _lines.Cast<UIElement>().Concat(_pixelTexts))
            {
                Children.Remove(element);
            }

            _lines.Clear();
            _pixelTexts.Clear();
        }

        public void ShowCurrent()
        {
            _redLine.Stroke = Brushes.Crimson;
            _pixelText.Foreground = Brushes.Crimson;
        }

        public void HideCurrent()
        {
            _redLine.Stroke = Brushes.Transparent;
            _pixelText.Foreground = Brushes.Transparent;
        }
    }
}