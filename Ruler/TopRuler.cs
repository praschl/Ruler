using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MiP.Ruler
{
    public class TopRuler : Canvas
    {
        public TopRuler()
        {
            Loaded += TopRuler_Loaded;
        }

        private void TopRuler_Loaded(object sender, RoutedEventArgs e)
        {
            DrawRuler();
        }
        
        private void DrawRuler()
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

                if (x % 100 == 0)
                {
                    var markBlock = new TextBlock {Text = x.ToString("0")};
                    Children.Add(markBlock);
                    markBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    SetTop(markBlock, 12);
                    SetLeft(markBlock, x - markBlock.DesiredSize.Width/2);
                }
            }
        }
    }
}