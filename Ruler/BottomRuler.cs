using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Ruler
{
    public class BottomRuler : Canvas
    {
        public BottomRuler()
        {
            Loaded += BottomRuler_Loaded;
        }
        
        private void BottomRuler_Loaded(object sender, RoutedEventArgs e)
        {
            DrawRuler();
        }

        private void DrawRuler()
        {
            Children.Clear();

            for (var x = 0; x < SystemParameters.PrimaryScreenWidth; x += 2)
            {
                var length = 4;
                if (x%10 == 0)
                    length = 8;
                if (x%50 == 0)
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
    }
}