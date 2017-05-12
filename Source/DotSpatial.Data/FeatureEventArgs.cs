// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2009 1:27:58 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// FeatureEventArgs
    /// </summary>
    public class FeatureEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureEventArgs"/> class.
        /// </summary>
        /// <param name="inFeature">The feature of the event.</param>
        public FeatureEventArgs(IFeature inFeature)
        {
            Feature = inFeature;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the feature being referenced by this event.
        /// </summary>
        public IFeature Feature { get; protected set; }

        #endregion
    }
}