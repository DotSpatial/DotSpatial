namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// Status panel that can show a progress.
    /// </summary>
    public class ProgressStatusPanel : StatusPanel
    {
        #region Fields

        private int _percent;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the progress percent.
        /// </summary>
        public int Percent
        {
            get
            {
                return _percent;
            }

            set
            {
                if (_percent == value) return;
                _percent = value;
                OnPropertyChanged("Percent");
            }
        }

        #endregion
    }
}