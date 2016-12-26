using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using MiP.Ruler.Annotations;

namespace MiP.Ruler
{
    public class Config : INotifyPropertyChanged
    {
        private bool _clearLinesOnOrientationChange;
        private bool _lockOrientationOnResize = true;
        private Orientation _vertical = Orientation.Horizontal;

        public static Config Instance { get; } = GetInstance();

        public bool ClearLinesOnOrientationChange
        {
            get { return _clearLinesOnOrientationChange; }
            set
            {
                if (value == _clearLinesOnOrientationChange) return;
                _clearLinesOnOrientationChange = value;
                OnPropertyChanged();
            }
        }

        public bool LockOrientationOnResize
        {
            get { return _lockOrientationOnResize; }
            set
            {
                if (value == _lockOrientationOnResize) return;
                _lockOrientationOnResize = value;
                OnPropertyChanged();
            }
        }

        public Orientation Orientation
        {
            get { return _vertical; }
            set
            {
                if (value == _vertical) return;
                _vertical = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private static Config GetInstance()
        {
            // TODO: Load config
            return new Config();
        }

        public void Save()
        {
            // TODO: Save config
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}