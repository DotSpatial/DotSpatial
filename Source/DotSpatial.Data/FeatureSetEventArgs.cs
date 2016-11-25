﻿// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is from DotSpatial.Data.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/29/2010 9:05:01 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// An EventArgs specifically tailored to FeatureSet.
    /// </summary>
    public class FeatureSetEventArgs : EventArgs
    {
        private IFeatureSet _featureSet;

        /// <summary>
        /// Initializes a new instance of the FeatureSetEventArgs class.
        /// </summary>
        /// <param name="featureSet">The IFeatureSet that is involved in this event.</param>
        public FeatureSetEventArgs(IFeatureSet featureSet)
        {
            _featureSet = featureSet;
        }

        /// <summary>
        /// Gets the Raster associated with this event.
        /// </summary>
        public IFeatureSet FeatureSet
        {
            get { return _featureSet; }
            protected set { _featureSet = value; }
        }
    }
}