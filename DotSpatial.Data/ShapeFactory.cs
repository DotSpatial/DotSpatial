// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created during DotSpatial refactoring 2010.
// ********************************************************************************************************

using System.Collections.Generic;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// ShapeFactory
    /// </summary>
    public class ShapeFactory
    {
        private static ShapeFactory _instance;

        /// <summary>
        /// for singleton pattern.
        /// </summary>
        private ShapeFactory()
        {
        }

        /// <summary>
        /// Gets the shared instance of the shape factory;
        /// </summary>
        public static ShapeFactory Instance
        {
            get { return _instance ?? (_instance = new ShapeFactory()); }
            set { _instance = value; }
        }

        /// <summary>
        /// Creates a new multi-part polygon shape.  Winding order should control holes.
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
                shp.Vertices[i * 2 + 1] = allCoords[i].Y;
            }
            ShapeRange result = new ShapeRange(FeatureType.Polygon);

            for (int i = 0; i < numParts; i++)
            {
                PartRange prt = new PartRange(shp.Vertices, 0, offsets[i], FeatureType.Polygon);
                prt.NumVertices = counts[i];
                result.Parts.Add(prt);
            }
            return shp;
        }
    }
}