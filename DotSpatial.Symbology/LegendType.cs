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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/10/2009 12:51:18 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// LegendType
    /// </summary>
    public enum LegendType
    {
        /// <summary>
        /// Schemes can contain symbols and be contained by layers
        /// </summary>
        Scheme,
        /// <summary>
        /// The ability to contain another layer type is controlled by CanReceiveItem instead
        /// of being specified by these pre-defined criteria.
        /// </summary>
        Custom,
        /// <summary>
        /// Groups can be contained by groups, and contain groups or layers, but not categories or symbols
        /// </summary>
        Group,
        /// <summary>
        /// Layers can contain symbols or categories, but not other layers or groups
        /// </summary>
        Layer,
        /// <summary>
        /// Symbols can't contain anything, but can be contained by layers and categories
        /// </summary>
        Symbol
    }
}