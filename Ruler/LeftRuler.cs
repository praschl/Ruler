using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MiP.Ruler
{
    public class LeftRuler : Canvas
    {
        public LeftRuler()
        {
            Loaded += LeftRuler_Loaded;
        }
        
        private void LeftRuler_Loaded(object sender, RoutedEventArgs e)
        {
            DrawRuler();
        }

        private void DrawRuler()
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
    }
}