using System;
using System.Windows;
using System.Windows.Input;

namespace MiP.Ruler
{
    public class CloseCommand : ICommand
    {
        private readonly Window _window;

        public CloseCommand(Window window)
        {
            _window = window;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _window.Close();
        }

        public event EventHandler CanExecuteChanged;
    }
}