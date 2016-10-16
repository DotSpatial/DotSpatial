// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/28/2009 3:38:37 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IRandomizable
    /// </summary>
    public interface IRandomizable
    {
        /// <summary>
        /// This method will set the values for this class with random values that are
        /// within acceptable parameters for this class.
        /// </summary>
        /// <param name="generator">An existing random number generator so that the random seed can be controlled</param>
        void Randomize(Random generator);
    }
}