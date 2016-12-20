using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MiP.Ruler.Annotations;

namespace MiP.Ruler
{
    public class RedLine : Canvas, INotifyPropertyChanged
    {
        public static readonly DependencyProperty HorizontalProperty = DependencyProperty.Register(
            "Horizontal", typeof(bool), typeof(RedLine), new PropertyMetadata(true, (o, args) => { ((RedLine)o)?.DirectionChanged((bool)args.NewValue); }));

        private readonly List<Line> _lines = new List<Line>();
        private readonly List<TextBlock> _pixelTexts = new List<TextBlock>();
        private TextBlock _pixelText;
        private Line _redLine;
        private readonly LayoutHandler _horizontalHandler;
        private readonly LayoutHandler _verticalHandler;
        private LayoutHandler _layoutHandler;
        private readonly Size _infiniteSize = new Size(double.PositiveInfinity, double.PositiveInfinity);

        public RedLine()
        {
            Initialize();

            _horizontalHandler = new LayoutHandler
            {
                SizeChanged = SizeChangedHorizontal,
                MouseMoved = MouseMovedHorizontal,
                AddLine = AddLineHorizontal
            };

            _verticalHandler = new LayoutHandler
            {
                SizeChanged = SizeChangedVertical,
                MouseMoved = MouseMovedVertical,
                AddLine = AddLineVertical
            };

            _layoutHandler = _horizontalHandler;

            MouseMove += MouseMovedHandler;
            SizeChanged += SizeChangedHandler;
        }

        private class LayoutHandler
        {
            public Action SizeChanged;
            public Action<MouseEventArgs> MouseMoved;
            public Action<Point> AddLine;
        }


        public bool Horizontal
        {
            get { return (bool)GetValue(HorizontalProperty); }
            set { SetValue(HorizontalProperty, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

            _pixelText.Measure(_infiniteSize);

            Children.Add(_pixelText);
        }

        private void SizeChangedHandler(object sender, SizeChangedEventArgs e)
        {
            _layoutHandler.SizeChanged();
        }

        private void SizeChangedHorizontal()
        {
            _redLine.Y2 = ActualHeight - 1;
            SetTop(_pixelText, ActualHeight / 2 - _pixelText.FontSize / 2);

            foreach (var line in _lines)
                line.Y2 = ActualHeight - 1;
            foreach (var pixelText in _pixelTexts)
                SetTop(pixelText, ActualHeight / 2 - _pixelText.FontSize / 2);
        }

        private void SizeChangedVertical()
        {
            _redLine.X2 = ActualWidth - 1;
            SetLeft(_pixelText, ActualWidth / 2 - _pixelText.DesiredSize.Width / 2);

            foreach (var line in _lines)
                line.X2 = ActualWidth - 1;
            foreach (var pixelText in _pixelTexts)
                SetLeft(pixelText, ActualWidth / 2 - pixelText.DesiredSize.Width / 2);
        }

        private void MouseMovedHandler(object sender, MouseEventArgs e)
        {
            _layoutHandler.MouseMoved(e);
        }

        private void MouseMovedHorizontal(MouseEventArgs e)
        {
            var pos = e.GetPosition(this);

            _redLine.X1 = _redLine.X2 = pos.X;
            _redLine.Y1 = 1;
            _redLine.Y2 = ActualHeight - 2;

            MovePixelTextHorizontal(pos, _pixelText);
        }


        private void MovePixelTextHorizontal(Point pos, TextBlock pixelText)
        {
            pixelText.Text = pos.X.ToString("0");
            pixelText.Measure(_infiniteSize);

            if (pos.X >= pixelText.DesiredSize.Width + 4)
                SetLeft(pixelText, pos.X - pixelText.DesiredSize.Width - 2);
            else
                SetLeft(pixelText, pos.X + 2);
        }
        private void MouseMovedVertical(MouseEventArgs e)
        {
            var pos = e.GetPosition(this);

            _redLine.Y1 = _redLine.Y2 = pos.Y;
            _redLine.X1 = 1;
            _redLine.X2 = ActualWidth - 2;

            MovePixelTextVertical(pos, _pixelText);
        }

        private void MovePixelTextVertical(Point pos, TextBlock pixelText)
        {
            pixelText.Text = pos.Y.ToString("0");
            pixelText.Measure(_infiniteSize);

            if (pos.Y >= pixelText.DesiredSize.Height + 4)
                SetTop(pixelText, pos.Y - pixelText.DesiredSize.Height - 2);
            else
                SetTop(pixelText, pos.Y + 2);
        }

        public void AddLine(Point position)
        {
            _layoutHandler.AddLine(position);
        }

        private void AddLineHorizontal(Point position)
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
            _pixelText.Measure(_infiniteSize);

            MovePixelTextHorizontal(position, newText);
            SetTop(newText, ActualHeight / 2 - newText.FontSize / 2);
        }

        private void AddLineVertical(Point position)
        {
            var newLine = new Line
            {
                Stroke = Brushes.Crimson,
                StrokeThickness = 1.0,
                X1 = 1,
                X2 = ActualWidth - 1,
                Y1 = position.Y,
                Y2 = position.Y
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
            _pixelText.Measure(_infiniteSize);

            MovePixelTextVertical(position, newText);
            SetLeft(newText, ActualWidth / 2 - newText.DesiredSize.Width / 2);
        }

        public void ClearLines()
        {
            foreach (var element in _lines.Cast<UIElement>().Concat(_pixelTexts))
                Children.Remove(element);

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

        private void DirectionChanged(bool isHorizontal)
        {
            _layoutHandler = isHorizontal ? _horizontalHandler : _verticalHandler;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}