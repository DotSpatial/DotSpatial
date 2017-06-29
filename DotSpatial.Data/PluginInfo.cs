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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/15/2009 4:23:30 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// PluginInfo
    /// </summary>
    [Obsolete("Do not use it. This class is not used in DotSpatial anymore.")] // Marked in 1.7
    public class PluginInfo
    {
        #region Private Variables

        private string _key;
        private string _settings;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PluginInfo
        /// </summary>
        public PluginInfo()
        {
        }

        /// <summary>
        /// Creates a new instance of a PluginInfo class with the specified parameters
        /// </summary>
        /// <param name="inSettings"></param>
        /// <param name="inKey"></param>
        public PluginInfo(string inSettings, string inKey)
        {
            _settings = inSettings;
            _key = inKey;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Settings
        /// </summary>
        public string Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        /// <summary>
        /// Key
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        #endregion
    }
}