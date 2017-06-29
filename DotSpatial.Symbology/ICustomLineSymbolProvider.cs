// ********************************************************************************************************
// Product Name: DotSpatial.Drawing.PredefinedSymbols.dll Alpha
// Description:  The basic module for DotSpatial.Drawing version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from DotSpatial.Drawing.PredefinedSymbols.dll version 6.0
//
// The Initial Developer of this Original Code is Jiri Kadlec. Created 5/15/2009 9:56:44 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// The interface IPredefinedLineSymbolProvider defines methods to save and load predefined
    /// line symbolizers from a xml file or from another source.
    /// </summary>
    public interface ICustomLineSymbolProvider
    {
        #region Methods

        /// <summary>
        /// Loads a list of custom line symbolizers from a data file
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <returns>The list of custom line symbolizers</returns>
        IEnumerable<CustomLineSymbolizer> Load(string fileName);

        /// <summary>
        /// Saves a list of custom line symbolizers to a data file
        /// </summary>
        /// <param name="fileName"></param>
        void Save(string fileName);

        #endregion

        #region Properties

        #endregion
    }
}