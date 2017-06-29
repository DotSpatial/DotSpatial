namespace DotSpatial.Controls.Header
{
    public class ProgressStatusPanel : StatusPanel
    {
        private int _percent;

        /// <summary>
        /// Gets or sets the progress percent.
        /// </summary>
        public int Percent
        {
            get { return _percent; }
            set
            {
                if (_percent == value) return;
                _percent = value;
                OnPropertyChanged("Percent");
            }
        }
    }
}