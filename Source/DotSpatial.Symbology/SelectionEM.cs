// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/24/2009 9:39:52 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using GeoAPI.Geometries;

namespace DotSpatial.Symbology
{
    public static class SelectionEM
    {
        #region Methods

        /// <summary>
        /// Inverts the selection based on the current SelectionMode
        /// </summary>
        /// <param name="self">The ISelection to invert</param>
        /// <param name="region">The geographic region to reverse the selected state</param>
        public static bool InvertSelection(this ISelection self, Envelope region)
        {
            Envelope ignoreMe;
            return self.InvertSelection(region, out ignoreMe);
        }

        #endregion
    }
}