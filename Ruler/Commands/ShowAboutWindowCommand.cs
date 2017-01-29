using System;
using System.Windows;
using System.Windows.Input;

namespace MiP.Ruler.Commands
{
    public class ShowAboutWindowCommand : ICommand
    {
        private readonly Window _parent;

        public ShowAboutWindowCommand(Window parent)
        {
            _parent = parent;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            AboutWindow.ToggleShow(_parent);
        }

        public event EventHandler CanExecuteChanged;
    }
}