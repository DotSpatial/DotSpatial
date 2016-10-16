// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2010 2:57:50 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// Spheroid
    /// </summary>
    public class HfaSpheroid
    {
        #region Private Variables

        private double _a;
        private double _b;
        private double _eSquared;
        private double _radius;
        private string _sphereName;

        #endregion

        #region Constructors

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the semi-major axis of the ellipsoid
        /// </summary>
        public double A
        {
            get { return _a; }
            set { _a = value; }
        }

        /// <summary>
        /// Gets or sets the semi-minor axis of the ellipsoid
        /// </summary>
        public double B
        {
            get { return _b; }
            set { _b = value; }
        }

        /// <summary>
        /// Gets or sets the eccentricity squared
        /// </summary>
        public double ESquared
        {
            get { return _eSquared; }
            set { _eSquared = value; }
        }

        /// <summary>
        /// Gets or sets the radius of the sphere
        /// </summary>
        public double Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        /// <summary>
        /// Gets or sets the string name o the ellipsoid
        /// </summary>
        public string SphereName
        {
            get { return _sphereName; }
            set { _sphereName = value; }
        }

        #endregion
    }
}