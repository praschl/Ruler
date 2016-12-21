using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MiP.Ruler.Commands
{
    public class SwitchOrientationCommand : ICommand
    {
        private readonly MainWindow _window;
        private readonly Orientation _orientation;
        private readonly bool _toggle;

        public SwitchOrientationCommand(MainWindow window, bool toggle)
        {
            _window = window;
            _toggle = toggle;
        }

        public SwitchOrientationCommand(MainWindow window, Orientation orientation)
        {
            _window = window;
            _orientation = orientation;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (!_toggle && (
                    _orientation == Orientation.Horizontal && _window.IsHorizontal
                    ||
                    _orientation == Orientation.Vertical && !_window.IsHorizontal
                ))
                return;

            _window._rulerLineDisplay.ClearRulerLines();
            _window.SwitchDirection(new Point(_window.Width/2, _window.Height/2));
        }

        public event EventHandler CanExecuteChanged;
    }
}