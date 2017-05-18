// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// PointSymbolizerEventArgs
    /// </summary>
    public class PointSymbolizerEventArgs : FeatureSymbolizerEventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PointSymbolizerEventArgs"/> class.
        /// </summary>
        /// <param name="symbolizer">The symbolizer of the event.</param>
        public PointSymbolizerEventArgs(IPointSymbolizer symbolizer)
            : base(symbolizer)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbolizer cast as an IPointSymbolizer.
        /// </summary>
        public new IPointSymbolizer Symbolizer
        {
            get
            {
                return base.Symbolizer as IPointSymbolizer;
            }

            protected set
            {
                base.Symbolizer = value;
            }
        }

        #endregion
    }
}