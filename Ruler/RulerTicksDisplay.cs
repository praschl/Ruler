using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MiP.Ruler
{
    public class RulerTicksDisplay : Canvas
    {
        public RulerTicksDisplay()
        {
            Loaded += LeftRuler_Loaded;
        }

        public RulerPositionValues RulerPosition { get; set; }

        private void LeftRuler_Loaded(object sender, RoutedEventArgs e)
        {
            if (RulerPosition == RulerPositionValues.Left)
                DrawRulerLeft();

            if (RulerPosition == RulerPositionValues.Right)
                DrawRulerRight();

            if (RulerPosition == RulerPositionValues.Top)
                DrawRulerTop();

            if (RulerPosition == RulerPositionValues.Bottom)
                DrawRulerBottom();
        }

        private void DrawRulerLeft()
        {
            for (var y = 0; y <= SystemParameters.PrimaryScreenHeight + 2; y += 2)
            {
                var length = 4;
                if (y % 10 == 0)
                    length = 8;
                if (y % 50 == 0)
                    length = 12;

                var gridline = new Line
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.0,
                    X1 = 0,
                    X2 = length,
                    Y1 = y,
                    Y2 = y
                };
                Children.Add(gridline);

                if (y > 0 && y % 100 == 0)
                {
                    var markBlock = new TextBlock { Text = y.ToString("0") };
                    Children.Add(markBlock);
                    markBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    SetLeft(markBlock, 12);
                    SetTop(markBlock, y - markBlock.DesiredSize.Height / 2);
                }
            }
        }

        private void DrawRulerRight()
        {
            for (var y = 0; y <= SystemParameters.PrimaryScreenHeight + 2; y += 2)
            {
                var length = 4;
                if (y % 10 == 0)
                    length = 8;
                if (y % 50 == 0)
                    length = 12;

                var gridline = new Line
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.0,
                    X1 = Width,
                    X2 = Width - length,
                    Y1 = y,
                    Y2 = y
                };
                Children.Add(gridline);
            }
        }
        
        private void DrawRulerTop()
        {
            for (var x = 0; x <= SystemParameters.PrimaryScreenWidth + 2; x += 2)
            {
                var length = 4;
                if (x % 10 == 0)
                    length = 8;
                if (x % 50 == 0)
                    length = 12;

                var gridline = new Line
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.0,
                    X1 = x,
                    X2 = x,
                    Y1 = 0,
                    Y2 = length
                };
                Children.Add(gridline);

                if (x > 0 && x % 100 == 0)
                {
                    var markBlock = new TextBlock { Text = x.ToString("0") };
                    Children.Add(markBlock);
                    markBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    SetTop(markBlock, 12);
                    SetLeft(markBlock, x - markBlock.DesiredSize.Width / 2);
                }
            }
        }

        private void DrawRulerBottom()
        {
            Children.Clear();

            for (var x = 0; x < SystemParameters.PrimaryScreenWidth + 2; x += 2)
            {
                var length = 4;
                if (x % 10 == 0)
                    length = 8;
                if (x % 50 == 0)
                    length = 12;

                var gridline = new Line
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.0,
                    X1 = x,
                    X2 = x,
                    Y1 = Height,
                    Y2 = Height - length
                };
                Children.Add(gridline);
            }
        }

        public enum RulerPositionValues
        {
            Left, Top, Right, Bottom
        }
    }

}