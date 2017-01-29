using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MiP.Ruler.Commands;

namespace MiP.Ruler
{
    public partial class AboutWindow
    {
        private static AboutWindow _window;

        public AboutWindow()
        {
            InitializeComponent();
        }

        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public ICommand CloseCommand => new CloseCommand(this);

        public static void ToggleShow(Window parent)
        {
            if (_window != null)
            {
                CloseWindow();
                return;
            }

            // else show window
            if (_window == null)
            {
                _window = new AboutWindow();
            }

            _window.Show();
            _window.BringIntoView();
            _window.Activate();

            if (Config.Instance.Orientation == Orientation.Horizontal)
            {
                _window.Left = parent.Left + (parent.Width/2 - _window.Width/2);

                if (parent.Top > _window.Height)
                    _window.Top = parent.Top - _window.Height;
                else
                    _window.Top = parent.Top + parent.Height;
            }
            else
            {
                _window.Top = parent.Top + (parent.Height / 2 - _window.Height / 2);

                if (parent.Left > _window.Width)
                    _window.Left = parent.Left - _window.Width;
                else
                    _window.Left = parent.Left + parent.Width;
            }
        }

        public static void CloseWindow()
        {
            _window?.Close();
        }

        private void AboutWindow_OnClosed(object sender, EventArgs e)
        {
            _window = null;
        }

        private void AboutWindow_OnInitialized(object sender, EventArgs e)
        {
            Left = SystemParameters.PrimaryScreenWidth/2 - ActualWidth/2;
            Top = SystemParameters.PrimaryScreenHeight/2 - ActualHeight/2;
        }

        private void AboutWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/praschl/Ruler");
        }
    }
}