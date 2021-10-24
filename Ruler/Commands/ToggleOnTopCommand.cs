using System;
using System.Windows.Input;

namespace MiP.Ruler.Commands
{
    public class ToggleOnTopCommand : ICommand
    {
        private readonly MainWindow _window;

        public ToggleOnTopCommand(MainWindow window)
        {
            _window = window;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _window.ToggleOnTop();
        }

        public event EventHandler CanExecuteChanged;
    }
}