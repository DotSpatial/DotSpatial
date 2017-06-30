// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/17/2008 9:56:23 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ILabel
    /// </summary>
    [Obsolete("Do not use it. This interface is not used in DotSpatial anymore.")] // Marked in 1.7
    public interface ILabel
    {
        #region Events

        /// <summary>
        /// Occurs when the Symbolizer for this label is changed.
        /// </summary>
        event EventHandler<TextSymbolChangedEventArgs> SymbolChanged;

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the geographic position for the anchoring the label.  The relationship
        /// between this and the text depends on the horizontal alignment.
        /// </summary>
        Coordinate AnchorPoint
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the integer index for this label
        /// </summary>
        int Index
        {
            get;
        }

        /// <summary>
        /// Gets the Symbol group that currently contains this label.
        /// </summary>
        ITextSymbolGroup Parent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the text that appears on this layer.
        /// </summary>
        string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the symbolizer for this specific label.  This allows customization for
        /// individual labels, but at the same time can allow many labels to point to one set
        /// of symbolic characteristics.
        /// </summary>
        ILabelSymbolizer Symbolizer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the symbolizer that is being used.  Symbol groups are defined by the
        /// original symbolizer, and there is only one selection symbolizer per group.  This
        /// simply is a shortcut to accessing the symbolgroup's selection symbolizer.
        /// </summary>
        ILabelSymbolizer SelectionSymbolizer
        {
            get;
            set;
        }

        #endregion
    }
}