// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// PolygonSymbolizerEventArgs.
    /// </summary>
    public class PolygonSymbolizerEventArgs : FeatureSymbolizerEventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonSymbolizerEventArgs"/> class.
        /// </summary>
        /// <param name="symbolizer">Symbolizer of the event.</param>
        public PolygonSymbolizerEventArgs(IPolygonSymbolizer symbolizer)
            : base(symbolizer)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Symbolizer, casting it to an IPolygonSymbolizer.
        /// </summary>
        public new IPolygonSymbolizer Symbolizer
        {
            get
            {
                return base.Symbolizer as IPolygonSymbolizer;
            }

            set
            {
                base.Symbolizer = value;
            }
        }

        #endregion
    }
}