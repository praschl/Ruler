using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MiP.Ruler
{
    public class RightRuler : Canvas
    {
        public RightRuler()
        {
            Loaded += RightRuler_Loaded;
        }

        private void RightRuler_Loaded(object sender, RoutedEventArgs e)
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
                    X1 = Width,
                    X2 = Width - length,
                    Y1 = y,
                    Y2 = y
                };
                Children.Add(gridline);
            }
        }
    }
}