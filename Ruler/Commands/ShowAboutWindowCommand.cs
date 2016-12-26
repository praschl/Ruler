using System;
using System.Windows.Input;

namespace MiP.Ruler.Commands
{
    public class ShowAboutWindowCommand : ICommand
    {
        private readonly MainWindow _window;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            // TODO: Show About Window
        }

        public event EventHandler CanExecuteChanged;
    }
}