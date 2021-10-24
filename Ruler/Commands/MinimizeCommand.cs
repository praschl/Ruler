using System;
using System.Windows;
using System.Windows.Input;

namespace MiP.Ruler.Commands
{
    public class MinimizeCommand : ICommand
    {
        private readonly Window _window;

        public MinimizeCommand(Window window)
        {
            _window = window;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _window.WindowState = WindowState.Minimized;
        }

        public event EventHandler CanExecuteChanged;
    }
}