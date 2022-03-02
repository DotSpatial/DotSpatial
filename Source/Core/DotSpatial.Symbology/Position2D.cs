// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using DotSpatial.Serialization;
using NetTopologySuite.Geometries;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Position2D is much simpler than a typical geometric construct.
    /// </summary>
    [TypeConverter(typeof(Position2DConverter))]
    public class Position2D : Descriptor
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Position2D"/> class.
        /// </summary>
        public Position2D()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Position2D"/> class.
        /// </summary>
        /// <param name="x">The X or horizontal coordinate.</param>
        /// <param name="y">The Y or vertical coordinate.</param>
        public Position2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the position in the X, or horizontal direction.
        /// </summary>
        [Serialize("X")]
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the position in the Y or vertical direction.
        /// </summary>
        [Serialize("Y")]
        public double Y { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a valid 2D coordinate using this position2D class.
        /// </summary>
        /// <returns>A coordinate with the Position2Ds x and y value.</returns>
        public Coordinate ToCoordinate()
        {
            return new Coordinate(X, Y);
        }

        /// <summary>
        /// Randomizes the position by selecting a random position from -1000 to 1000 for X and Y.
        /// </summary>
        /// <param name="generator">The random number generator so that a seed can be chosen.</param>
        protected override void OnRandomize(Random generator)
        {
            X = (generator.NextDouble() * 2000) - 1000;
            Y = (generator.NextDouble() * 2000) - 1000;
        }

        #endregion
    }
}