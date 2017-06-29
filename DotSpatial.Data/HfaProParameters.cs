// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2010 3:02:36 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// ProParameters
    /// </summary>
    public class HfaProParameters
    {
        private string _proExeName;
        private string _proName;
        private int _proNumber;
        private double[] _proParams;
        private HfaSpheroid _proSpheroid;
        private HfaProType _proType;
        private int _proZone;

        /// <summary>
        /// Creates a new instance of the ProParameters class
        /// </summary>
        public HfaProParameters()
        {
            _proParams = new double[15];
        }

        /// <summary>
        /// Gets or sets the string exectable name for external projectiosn
        /// </summary>
        public string ProExeName
        {
            get { return _proExeName; }
            set { _proExeName = value; }
        }

        /// <summary>
        /// Gets or sets the string projection name
        /// </summary>
        public string ProName
        {
            get { return _proName; }
            set { _proName = value; }
        }

        /// <summary>
        /// Gets or sets the projection number for internal projections
        /// </summary>
        public int ProNumber
        {
            get { return _proNumber; }
            set { _proNumber = value; }
        }

        /// <summary>
        /// Projection parameters array in the GCTP form
        /// </summary>
        public double[] ProParams
        {
            get { return _proParams; }
            set { _proParams = value; }
        }

        /// <summary>
        /// Gets or sets the projection type
        /// </summary>
        public HfaProType ProType
        {
            get { return _proType; }
            set { _proType = value; }
        }

        /// <summary>
        /// Gets or sets the projection zone (UTM, SP only)
        /// </summary>
        public int ProZone
        {
            get { return _proZone; }
            set { _proZone = value; }
        }

        /// <summary>
        /// The projection spheroid
        /// </summary>
        public HfaSpheroid ProSpheroid
        {
            get { return _proSpheroid; }
            set { _proSpheroid = value; }
        }
    }
}