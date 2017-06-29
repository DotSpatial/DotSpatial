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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/11/2009 11:14:33 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using DotSpatial.Serialization;
using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Position2D is much simpler than a typical geometric construct, using fields instead of properties to make it work faster.
    /// </summary>
    [TypeConverter(typeof(Position2DConverter))]
    public class Position2D : Descriptor
    {
        /// <summary>
        /// The position in the X, or horizontal direction
        /// </summary>
        [Serialize("X")]
        public double X;

        /// <summary>
        /// The position in the Y or vertical direction
        /// </summary>
        [Serialize("Y")]
        public double Y;

        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Position2D
        /// </summary>
        public Position2D()
        {
        }

        /// <summary>
        /// Creates a new position2D class using the specified coordinates
        /// </summary>
        /// <param name="x">The X or horizontal coordinate</param>
        /// <param name="y">The Y or vertical coordinate</param>
        public Position2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Randomizes the position by selecting a random position from -1000 to 1000 for X and Y
        /// </summary>
        /// <param name="generator">The random number generator so that a seed can be chosen.</param>
        protected override void OnRandomize(Random generator)
        {
            X = generator.NextDouble() * 2000 - 1000;
            Y = generator.NextDouble() * 2000 - 1000;
        }

        /// <summary>
        /// Creates a valid 2D coordinate using this position2D class
        /// </summary>
        /// <returns></returns>
        public Coordinate ToCoordinate()
        {
            return new Coordinate(X, Y);
        }

        #endregion
    }
}