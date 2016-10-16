// ********************************************************************************************************
// Product Name: DotSpatial.Drawing.PredefinedSymbols.dll Alpha
// Description:  The basic module for DotSpatial.Drawing version 6.0
// ********************************************************************************************************
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