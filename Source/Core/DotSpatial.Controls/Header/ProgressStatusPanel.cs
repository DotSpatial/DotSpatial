// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
                OnPropertyChanged(nameof(Percent));
            }
        }

        #endregion
    }
}