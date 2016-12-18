using System;
using System.Windows.Input;

namespace MiP.Ruler
{
    public class ClearLinesCommand : ICommand
    {
        private readonly MainWindow _window;

        public ClearLinesCommand(MainWindow window)
        {
            _window = window;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _window.ClearLines();
        }

        public event EventHandler CanExecuteChanged;
    }
}