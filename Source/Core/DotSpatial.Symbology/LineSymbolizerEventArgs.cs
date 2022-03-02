// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// LineSymbolizerEventArgs.
    /// </summary>
    public class LineSymbolizerEventArgs : FeatureSymbolizerEventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSymbolizerEventArgs"/> class.
        /// </summary>
        /// <param name="symbolizer">The symbolizer of the event.</param>
        public LineSymbolizerEventArgs(ILineSymbolizer symbolizer)
            : base(symbolizer)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Symbolizer.
        /// </summary>
        public new ILineSymbolizer Symbolizer
        {
            get
            {
                return base.Symbolizer as ILineSymbolizer;
            }

            set
            {
                base.Symbolizer = value;
            }
        }

        #endregion
    }
}