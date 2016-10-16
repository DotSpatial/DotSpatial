// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/29/2008 11:01:44 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Represents the argument for events that need to report changed features.
    /// </summary>
    public class FeatureChangeArgs : EventArgs
    {

        #region Constructors

        /// <summary>
        /// Creates a new instance of FeatureChangeArgs.
        /// </summary>
        public FeatureChangeArgs(List<int> inChangedFeatures)
        {
            ChangedFeatures = inChangedFeatures;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of features that were changed by this event.
        /// </summary>
        public List<int> ChangedFeatures { get; protected set; }

        #endregion
    }
}