using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MiP.Ruler.Annotations;

namespace MiP.Ruler
{
    public class RulerLineDisplay : Canvas, INotifyPropertyChanged
    {
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", typeof(Orientation), typeof(RulerLineDisplay), new PropertyMetadata(Orientation.Horizontal, (o, args) => { ((RulerLineDisplay) o)?.DirectionChanged(); }));

        private RulerLine _currentLine;
        private readonly RulerLine[] _currentLineInArray = new RulerLine[1];
        private readonly List<RulerLine> _rulerLines = new List<RulerLine>();

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
            _currentLineInArray[0] = _currentLine = new RulerLine(this, new Point(0, 0));
        }

        private void SizeChangedHandler(object sender, SizeChangedEventArgs e)
        {
            foreach (var line in _rulerLines.Concat(_currentLineInArray))
                line.RefreshSize();
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
            _currentLine.RefreshLine(pos);
            _currentLine.RefreshSize();
        }
        
        public void AddNewRulerLine(Point position)
        {
            _rulerLines.Add(new RulerLine(this, position));
        }

        public void ClearRulerLines()
        {
            foreach (var line in _rulerLines)
                line.RemoveFromDisplay();

            _rulerLines.Clear();
        }

        public void SetCurrentVisible(bool visible)
        {
            _currentLine.SetVisible(visible);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}