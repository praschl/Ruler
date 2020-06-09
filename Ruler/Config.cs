using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace MiP.Ruler
{
    public class Config : INotifyPropertyChanged
    {
        private static string _configPath;
        private static string _configFile;
        private bool _clearLinesOnOrientationChange;
        private bool _lockOrientationOnResize = true;
        private Orientation _orientation = Orientation.Horizontal;
        private bool _showPercentages;
        private double _windowLeft = 100;
        private double _windowTop = 100;
        private double _windowWidth= 600;
        private double _windowHeight = 75;
        private bool _relativeDisplay;

        public static Config Instance { get; } = GetInstance();

        public static string ConfigFile
        {
            get
            {
                _configPath = _configPath ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Ruler");

                if (!Directory.Exists(_configPath))
                    Directory.CreateDirectory(_configPath);

                _configFile = _configFile ?? Path.Combine(_configPath, "Ruler.json");

                return _configFile;
            }
        }

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
            get { return _orientation; }
            set
            {
                if (value == _orientation) return;
                _orientation = value;
                OnPropertyChanged();
            }
        }

        public bool ShowPercentages
        {
            get { return _showPercentages; }
            set
            {
                if (value == _showPercentages) return;
                _showPercentages = value;
                OnPropertyChanged();
            }
        }

        public double WindowLeft
        {
            get { return _windowLeft; }
            set
            {
                if (value.Equals(_windowLeft)) return;
                _windowLeft = value;
                OnPropertyChanged();
            }
        }

        public double WindowTop
        {
            get { return _windowTop; }
            set
            {
                if (value.Equals(_windowTop)) return;
                _windowTop = value;
                OnPropertyChanged();
            }
        }

        public double WindowWidth
        {
            get { return _windowWidth; }
            set
            {
                if (value.Equals(_windowWidth)) return;
                _windowWidth = value;
                OnPropertyChanged();
            }
        }

        public double WindowHeight
        {
            get { return _windowHeight; }
            set
            {
                if (value.Equals(_windowHeight)) return;
                _windowHeight = value;
                OnPropertyChanged();
            }
        }

        public bool RelativeDisplay
        {
            get => _relativeDisplay;
            set
            {
                if (value.Equals(_relativeDisplay)) return;
                _relativeDisplay = value;
                OnPropertyChanged();
            }
        }

        public double Opacity { get; set; } = 1.0;

        public event PropertyChangedEventHandler PropertyChanged;

        private static Config GetInstance()
        {
            var config = LoadJson();

            return config ?? new Config();
        }

        private static Config LoadJson()
        {
            if (!File.Exists(ConfigFile))
                return null;

            var json = File.ReadAllText(ConfigFile);

            return JsonConvert.DeserializeObject<Config>(json);
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);

            File.WriteAllText(ConfigFile, json);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}