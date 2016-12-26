using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

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

        public static void ShowSingleInstance(Window parent)
        {
            if (_window == null)
            {
                _window = new AboutWindow();
            }

            _window.Show();
            _window.BringIntoView();
            _window.Activate();

            // TODO: Place about window somewhere nice depending on enough space: 
            // above ruler h-center
            // below ruler h-center
            // h-center v-center
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