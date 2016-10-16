// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2010 2:42:50 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// MapInfo
    /// </summary>
    public class HfaMapInfo
    {
        #region Private Variables

        private HfaCoordinate _lowerRightCenter;
        private HfaSize _pixelSize;
        private string _proName;
        private string _units;
        private HfaCoordinate _upperLeftCenter;

        #endregion

        #region Constructors

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the map coordinats of the center of the lower right pixel
        /// </summary>
        public HfaCoordinate LowerRightCenter
        {
            get { return _lowerRightCenter; }
            set { _lowerRightCenter = value; }
        }

        /// <summary>
        /// Gets or sets the size of a single pixel in map units
        /// </summary>
        public HfaSize PixelSize
        {
            get { return _pixelSize; }
            set { _pixelSize = value; }
        }

        /// <summary>
        /// Gets or sets the string Projection Name
        /// </summary>
        public string ProName
        {
            get { return _proName; }
            set { _proName = value; }
        }

        /// <summary>
        /// Gets or sets the map units
        /// </summary>
        public string Units
        {
            get { return _units; }
            set { _units = value; }
        }

        /// <summary>
        /// Gets or sets the map coordinates of center of upper left pixel
        /// </summary>
        public HfaCoordinate UpperLeftCenter
        {
            get { return _upperLeftCenter; }
            set { _upperLeftCenter = value; }
        }

        #endregion
    }
}