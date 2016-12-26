namespace MiP.Ruler
{
    public class Config
    {
        public static Config Instance { get; } = GetInstance();

        private static Config GetInstance()
        {
            // TODO: Load config
            return new Config();
        }

        public void Save()
        {
            // TODO: Save config
        }

        public bool ClearLinesOnOrientationChange { get; set; } = false;

        public bool LockOrientationOnResize { get; set; } = true;
    }
}