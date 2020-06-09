using System;
using System.Diagnostics;
using System.Windows.Input;

namespace MiP.Ruler.Commands
{
    public class ShowAboutWindowCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Process.Start("https://github.com/praschl/Ruler");
        }

        public event EventHandler CanExecuteChanged;
    }
}