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
    public class RulerLineDisplay : Canvas, INotifyPropertyChanged
    {
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", typeof(Orientation), typeof(RulerLineDisplay), new PropertyMetadata(Orientation.Horizontal, (o, args) => { ((RulerLineDisplay) o)?.DirectionChanged(); }));


        private readonly Size _infiniteSize = new Size(double.PositiveInfinity, double.PositiveInfinity);

        private readonly List<Line> _rulerLines = new List<Line>();
        private readonly List<TextBlock> _rulerTexts = new List<TextBlock>();
        private Line _currentRulerLine;
        private Line[] _currentRulerLineArray;
        private TextBlock _currentRulerText;
        private TextBlock[] _currentRulerTextArray;

        public RulerLineDisplay()
        {
            Initialize();

            MouseMove += MouseMoveHandler;
            SizeChanged += SizeChangedHandler;
        }
        
        public Orientation Orientation
        {
            get { return (Orientation) GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Initialize()
        {
            _currentRulerLine = new Line
            {
                Stroke = Brushes.Transparent,
                StrokeThickness = 1.0,
                Y1 = 1,
                Y2 = ActualHeight - 1
            };
            _currentRulerLineArray = new[] {_currentRulerLine};

            Children.Add(_currentRulerLine);

            _currentRulerText = new TextBlock
            {
                Foreground = Brushes.Transparent
            };
            _currentRulerTextArray = new[] {_currentRulerText};

            SetLeft(_currentRulerText, 10);
            SetTop(_currentRulerText, 10);

            _currentRulerText.Measure(_infiniteSize);

            Children.Add(_currentRulerText);
        }

        private void SizeChangedHandler(object sender, SizeChangedEventArgs e)
        {
            double newLineValue;
            double newTextValue;
            Action<Line, double> lineSetter;
            Action<TextBlock, double> textSetter;
            if (Orientation == Orientation.Horizontal)
            {
                newLineValue = ActualHeight - 1;
                newTextValue = ActualHeight/2 - _currentRulerText.FontSize/2;
                lineSetter = (line, y) => line.Y2 = y;
                textSetter = SetTop;
            }
            else
            {
                newLineValue = ActualWidth - 1;
                newTextValue = ActualWidth/2 - _currentRulerText.DesiredSize.Width/2;
                lineSetter = (line, x) => line.X2 = x;
                textSetter = SetLeft;
            }

            foreach (var line in _rulerLines.Concat(_currentRulerLineArray))
                lineSetter(line, newLineValue);

            foreach (var text in _rulerTexts.Concat(_currentRulerTextArray))
                textSetter(text, newTextValue);
        }

        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(this);

            RefreshCurrentRulerLine(pos);
        }

        private void DirectionChanged()
        {
            ClearRulerLines();
            RefreshCurrentRulerLine(Mouse.GetPosition(this));
        }

        private void RefreshCurrentRulerLine(Point pos)
        {
            if (Orientation == Orientation.Horizontal)
            {
                _currentRulerLine.X1 = _currentRulerLine.X2 = pos.X;
                _currentRulerLine.Y1 = 1;
                _currentRulerLine.Y2 = ActualHeight - 2;
            }
            else
            {
                _currentRulerLine.Y1 = _currentRulerLine.Y2 = pos.Y;
                _currentRulerLine.X1 = 1;
                _currentRulerLine.X2 = ActualWidth - 2;
            }

            RefreshRulerText(pos, _currentRulerText);
        }

        private void RefreshRulerText(Point pos, TextBlock pixelText)
        {
            if (Orientation == Orientation.Horizontal)
            {
                pixelText.Text = pos.X.ToString("0");
                pixelText.Measure(_infiniteSize);

                if (pos.X >= pixelText.DesiredSize.Width + 4)
                    SetLeft(pixelText, pos.X - pixelText.DesiredSize.Width - 2);
                else
                    SetLeft(pixelText, pos.X + 2);
            }
            else
            {
                pixelText.Text = pos.Y.ToString("0");
                pixelText.Measure(_infiniteSize);

                if (pos.Y >= pixelText.DesiredSize.Height + 4)
                    SetTop(pixelText, pos.Y - pixelText.DesiredSize.Height - 2);
                else
                    SetTop(pixelText, pos.Y + 2);
            }
        }

        public void AddNewRulerLine(Point position)
        {
            var newLine = new Line
            {
                Stroke = Brushes.Crimson,
                StrokeThickness = 1.0
            };

            _rulerLines.Add(newLine);
            Children.Add(newLine);

            var newText = new TextBlock
            {
                Foreground = Brushes.Crimson
            };

            _rulerTexts.Add(newText);
            Children.Add(newText);

            if (Orientation == Orientation.Horizontal)
            {
                newLine.Y1 = 1;
                newLine.Y2 = ActualHeight - 1;
                newLine.X1 = position.X;
                newLine.X2 = position.X;
                RefreshRulerText(position, newText);
                SetTop(newText, ActualHeight/2 - newText.FontSize/2);
            }
            else
            {
                newLine.X1 = 1;
                newLine.X2 = ActualWidth - 1;
                newLine.Y1 = position.Y;
                newLine.Y2 = position.Y;
                RefreshRulerText(position, newText);
                SetLeft(newText, ActualWidth/2 - newText.DesiredSize.Width/2);
            }
        }

        public void ClearRulerLines()
        {
            foreach (var element in _rulerLines.Cast<UIElement>().Concat(_rulerTexts))
                Children.Remove(element);

            _rulerLines.Clear();
            _rulerTexts.Clear();
        }

        public void SetCurrentVisible(bool visible)
        {
            Brush color = visible ? Brushes.Crimson : Brushes.Transparent;
            _currentRulerLine.Stroke = color;
            _currentRulerText.Foreground = color;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}