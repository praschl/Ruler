using System;
using System.Windows.Input;

namespace MiP.Ruler.Commands
{
    public class ClearRulerLinesCommand : ICommand
    {
        private readonly MainWindow _window;

        public ClearRulerLinesCommand(MainWindow window)
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