// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/28/2009 3:38:37 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;

namespace DotSpatial.Projections
{
    /// <summary>
    /// IRandomizable
    /// </summary>
    public interface IProjRandomizable
    {
        /// <summary>
        /// This method will set the values for this class with random values that are
        /// within acceptable parameters for this class.
        /// </summary>
        /// <param name="generator">An existing random number generator so that the random seed can be controlled</param>
        void Randomize(Random generator);
    }
}