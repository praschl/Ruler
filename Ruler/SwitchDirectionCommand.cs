using System;
using System.Windows;
using System.Windows.Input;

namespace MiP.Ruler
{
    public class SwitchDirectionCommand : ICommand
    {
        private readonly MainWindow _window;

        public SwitchDirectionCommand(MainWindow window)
        {
            _window = window;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _window._redLine.ClearRulerLines();
            _window.SwitchDirection(new Point(0, 0), false);
        }

        public event EventHandler CanExecuteChanged;
    }
}