// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/29/2008 11:15:25 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using GeoAPI.Geometries;

namespace DotSpatial.Symbology
{
    ///<summary>
    /// Represents the argument for events that need to report changed features and the corresponding envelope.
    ///</summary>
    public class FeatureChangeEnvelopeArgs : FeatureChangeArgs
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of FeatureChangeEnvelopeArgs
        /// </summary>
        /// <param name="inChangedFeatures">Features that were changed.</param>
        /// <param name="inEnvelope">Geographic envelope for the most recent selection event.</param>
        public FeatureChangeEnvelopeArgs(List<int> inChangedFeatures, Envelope inEnvelope)
            : base(inChangedFeatures)
        {
            Envelope = inEnvelope;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the geographic envelope for the most recent selection event.
        /// </summary>
        public Envelope Envelope { get; protected set; }

        #endregion
    }
}