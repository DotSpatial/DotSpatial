// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// ShapeFactory.
    /// </summary>
    public class ShapeFactory
    {
        #region Fields

        private static ShapeFactory instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapeFactory"/> class.
        /// </summary>
        private ShapeFactory()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the shared instance of the shape factory.
        /// </summary>
        public static ShapeFactory Instance
        {
            get
            {
                return instance ?? (instance = new ShapeFactory());
            }

            set
            {
                instance = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new multi-part polygon shape. Winding order should control holes.
        /// </summary>
        /// <param name="allParts">The list of all the lists of coordinates.</param>
        /// <returns>A new Multi-Polygon Shape.</returns>
        public Shape CreateMultiPolygonFromCoordinates(IEnumerable<IEnumerable<Coordinate>> allParts)
        {
            Shape shp = new Shape(FeatureType.Polygon);
            List<Coordinate> allCoords = new List<Coordinate>();
            List<int> offsets = new List<int>();
            List<int> counts = new List<int>();
            int count = 0;
            int numParts = 0;
            foreach (var part in allParts)
            {
                int coordinatecount = 0;
                foreach (Coordinate c in part)
                {
                    allCoords.Add(c);
                    count++;
                    coordinatecount++;
                }

                offsets.Add(count);
                counts.Add(coordinatecount);
                numParts++;
            }

            shp.Vertices = new double[allCoords.Count * 2];
            for (int i = 0; i < allCoords.Count; i++)
            {
                shp.Vertices[i * 2] = allCoords[i].X;
                shp.Vertices[(i * 2) + 1] = allCoords[i].Y;
            }

            ShapeRange result = new ShapeRange(FeatureType.Polygon);

            for (int i = 0; i < numParts; i++)
            {
                PartRange prt = new PartRange(shp.Vertices, 0, offsets[i], FeatureType.Polygon)
                {
                    NumVertices = counts[i]
                };
                result.Parts.Add(prt);
            }

            return shp;
        }

        #endregion
    }
}