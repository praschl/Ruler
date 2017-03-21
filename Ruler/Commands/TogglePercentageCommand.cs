using System;
using System.Windows.Input;

namespace MiP.Ruler.Commands
{
    public class TogglePercentageCommand : ICommand
    {
        private readonly MainWindow _window;

        public TogglePercentageCommand(MainWindow window)
        {
            _window = window;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _window.TogglePercentages();
        }

        public event EventHandler CanExecuteChanged;
    }
}