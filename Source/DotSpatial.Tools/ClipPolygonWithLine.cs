// *******************************************************************************************************
// Product: DotSpatial.Tools.ClipPolygonWithLine.cs
// Description:  A tool for clipping a polygon with a line.

// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders.
//--------------------------------------------------------------------------------------------------------
// Name                   |   Date                 |         Comments
//------------------------|------------------------|------------------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some formatting issues using re-sharper
// KP                     |  9/2009                |  Used IDW as model for ClipPolygonWithLine
// Ping                   |  12/2009               |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;
using GeoAPI.Geometries;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Geometries;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Clip Polygon with line tool
    /// </summary>
    public class ClipPolygonWithLine : Tool
    {
        #region Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipPolygonWithLine"/> class.
        /// </summary>
        public ClipPolygonWithLine()
        {
            Name = TextStrings.ClipPolygonwithLine;
            Category = TextStrings.VectorOverlay;
            Description = TextStrings.ClipPolygonwithLine;
            ToolTip = TextStrings.ClipPolygonwithLine;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the intersection tolerance for comparing to near 0.0.
        /// </summary>
        public static double FindIntersectionTolerance { get; set; } = 1E-10;

        /// <summary>
        /// Gets the input paramater array.
        /// </summary>
        public override Parameter[] InputParameters => _inputParam;

        /// <summary>
        /// Gets the output paramater array
        /// </summary>
        public override Parameter[] OutputParameters => _outputParam;

        #endregion

        #region Methods

        /// <summary>
        /// Accurate Clip Polygon with Line
        /// </summary>
        /// <param name="polygon">The polygon to clip.</param>
        /// <param name="line">The line used for clipping.</param>
        /// <param name="resultFeatureSet">The resulting feature set.</param>
        /// <returns>False if errors where encountered.</returns>
        public static bool AccurateClipPolygonWithLine(ref IFeature polygon, ref IFeature line, ref IFeatureSet resultFeatureSet)
        {
            if (polygon == null || line == null) return false;
            if (!line.Geometry.Crosses(polygon.Geometry)) return false;

            // find if all of the line is inside, outside, or part in and out of polygon
            // line might intersect polygon mutliple times
            int numPoints = line.Geometry.NumPoints;
            bool[] ptsInside = new bool[numPoints];
            int numInside = 0;
            int numOutside = 0;
            int numParts = polygon.Geometry.NumPoints;
            if (numParts == 0) numParts = 1;

            Coordinate[][] polyVertArray;
            ConvertPolyToVertexArray(ref polygon, out polyVertArray);

            // check each point in the line to see if the entire line is either
            // inside of the polygon or outside of it (we know it's inside polygon bounding box).
            for (int i = 0; i <= numPoints - 1; i++)
            {
                Point currPt = new Point(line.Geometry.Coordinates[i]);
                if (polygon.Geometry.Covers(currPt))
                {
                    ptsInside[i] = true;
                    numInside += 1;
                }
                else
                {
                    ptsInside[i] = false;
                    numOutside += 1;
                }
            }

            // case: all points are inside polygon - check for possible intersections
            if (numInside == numPoints)
                return ProcessAllInside(ref line, ref polygon, ref resultFeatureSet);

            // case: all points are outside of the polygon - check for possible intersections
            if (numOutside == numPoints)
                return ProcessAllOutside(ref line, ref polygon, ref resultFeatureSet);

            // case: part of line is inside and part is outside - find inside segments.
            return ProcessPartInAndOut(ref ptsInside, ref line, ref polygon, ref resultFeatureSet);
        }

        /// <summary>
        /// This will clip MultiPart Polygon with line.
        /// </summary>
        /// <param name="polygon">Input Polygon.</param>
        /// <param name="line">Input Line.</param>
        /// <param name="resultFile">Output Featureset.</param>
        /// <param name="speedOptimized">The speed optimizer.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool ClipMultiPartPolyWithLine(ref IFeature polygon, ref IFeature line, ref IFeatureSet resultFile, bool speedOptimized)
        {
            int numParts = polygon.Geometry.NumGeometries;
            if (numParts == 0) numParts = 1;

            if (numParts > 1)
            {
                // multiple parts
                FixMultiPartPoly(ref polygon);
                IFeature[] polyParts;
                SeparateParts(ref polygon, out polyParts);
                IFeatureSet holeSf = new FeatureSet(FeatureType.Polygon);
                IFeatureSet tempResult = new FeatureSet(FeatureType.Polygon);
                IFeatureSet modPolySf = new FeatureSet(FeatureType.Polygon);
                IFeatureSet resultSf = new FeatureSet(FeatureType.Polygon);

                for (int i = 0; i <= numParts - 1; i++)
                {
                    IFeature currPart = polyParts[i];
                    if (!CGAlgorithms.IsCCW(currPart.Geometry.Coordinates))
                    {
                        if (speedOptimized)
                        {
                            FastClipPolygonWithLine(ref currPart, ref line, out tempResult);
                        }
                        else
                        {
                            AccurateClipPolygonWithLine(ref currPart, ref line, ref tempResult);
                        }

                        int numResults = tempResult.Features.Count;
                        if (numResults > 0)
                        {
                            for (int j = 0; j <= numResults - 1; j++)
                            {
                                modPolySf.Features.Add(tempResult.Features[j]);
                            }
                        }
                    }
                    else
                    {
                        holeSf.Features.Add(currPart);
                    }
                }

                if (holeSf.Features.Count > 0)
                {
                    ErasePolygonShapefileWithPolygonShapefile(modPolySf, holeSf, resultSf);
                }

                resultFile = resultSf;
                return resultSf.Features.Count > 0;
            }

            if (speedOptimized)
            {
                return FastClipPolygonWithLine(ref polygon, ref line, out resultFile);
            }

            return AccurateClipPolygonWithLine(ref polygon, ref line, ref resultFile);
        }

        /// <summary>
        /// Returns the portions of the polygons in polySF that lie within polygon as a
        /// new shapefile of polygons: resultPolySF.
        /// </summary>
        /// <param name="polygonFeatureSet">The shapefile of polygons that are to be clipped.</param>
        /// <param name="polygon">The polygon used for clipping.</param>
        /// <param name="resultFeatureSet">The result shapefile for the resulting polygons to be saved (in-memory).</param>
        /// <param name="copyAttributes">True if copying attrs</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool ClipPolygonFeatureSetWithPolygon(IFeatureSet polygonFeatureSet, IFeature polygon, IFeatureSet resultFeatureSet, bool copyAttributes)
        {
            if (copyAttributes)
            {
                polygonFeatureSet.FillAttributes();
                resultFeatureSet.CopyTableSchema(polygonFeatureSet);
            }

            if (polygonFeatureSet.Features.Count != 0 && polygon.Geometry.NumPoints != 0 && polygonFeatureSet.FeatureType == FeatureType.Polygon)
            {
                int numShapes = polygonFeatureSet.Features.Count;
                for (int i = 0; i <= numShapes - 1; i++)
                {
                    polygonFeatureSet.Features[i].Intersection(polygon, resultFeatureSet, FieldJoinType.LocalOnly);
                }
            }

            return true;
        }

        /// <summary>
        /// Takes an array of simple polygons and combines them into one multi-part shape.
        /// </summary>
        /// <param name="parts">The array of polygons.</param>
        /// <param name="resultShp">The resulting multi-part shape.</param>
        public static void CombineParts(ref IFeature[] parts, out IFeature resultShp)
        {
            int numParts = parts.Length;
            Polygon po = new Polygon(new LinearRing(parts[0].Geometry.Coordinates));
            for (int i = 0; i <= numParts - 1; i++)
            {
                var poly = new Polygon(new LinearRing(parts[i].Geometry.Coordinates));
                po = poly.Union(po) as Polygon;
            }

            resultShp = new Feature(po);
        }

        /// <summary>
        /// Takes a MapWinGIS polygon shape and stores all x/y coordinates into a vertex array.
        /// </summary>
        /// <param name="poly">The polygon to be converted.</param>
        /// <param name="polyVertArray">The array[numParts][] that will contain the polygon vertices.</param>
        public static void ConvertPolyToVertexArray(ref IFeature poly, out Coordinate[][] polyVertArray)
        {
            int numParts = poly.Geometry.NumGeometries;
            if (numParts == 0) numParts = 1;

            int numPoints = poly.Geometry.NumPoints;
            Coordinate[][] vertArray = new Coordinate[numParts][];
            if (numParts > 1)
            {
                // separate parts of polygon
                for (int i = 0; i <= numParts - 1; i++)
                {
                    int numPtsInPart = poly.Geometry.GetGeometryN(i).Coordinates.Length;
                    vertArray[i] = new Coordinate[numPtsInPart];
                    for (int j = 0; j <= numPtsInPart - 2; j++)
                    {
                        vertArray[i][j] = poly.Geometry.GetGeometryN(i).Coordinates[j];
                    }

                    // be sure to 'close' the polygon in the vertex array!
                    vertArray[i][numPtsInPart - 1] = vertArray[i][0];
                }
            }
            else
            {
                // all points in polygon go into same vertex array
                vertArray[0] = new Coordinate[numPoints];
                for (int i = 0; i <= numPoints - 1; i++)
                {
                    vertArray[0][i] = poly.Geometry.Coordinates[i];
                }
            }

            polyVertArray = vertArray;
        }

        /// <summary>
        /// Divides a polygon into multiple sections depending on where a line crosses it. Saves the resulting
        /// polygon sections to a new polygon shapefile.
        /// </summary>
        /// <param name="polygon">The polygon to be divided.</param>
        /// <param name="line">The line that will be used to divide the polgyon.</param>
        /// <param name="resultSf">The in-memory shapefile where resulting polygons should be saved.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool DoClipPolygonWithLine(ref IFeature polygon, ref IFeature line, ref IFeatureSet resultSf)
        {
            if (polygon.Geometry.NumGeometries > 1)
            {
                return ClipMultiPartPolyWithLine(ref polygon, ref line, ref resultSf, false);
            }

            return AccurateClipPolygonWithLine(ref polygon, ref line, ref resultSf);
        }

        /// <summary>
        /// Removes portions of the input polygon shapefile that are within the erase polygons.
        /// </summary>
        /// <param name="inputShapefile">The input polygon shapefile.</param>
        /// <param name="eraseShapefile">The erase polygon shapefile.</param>
        /// <param name="resultShapefile">The resulting shapefile, with portions removed.</param>
        public static void ErasePolygonShapefileWithPolygonShapefile(IFeatureSet inputShapefile, IFeatureSet eraseShapefile, IFeatureSet resultShapefile)
        {
            // Validates the input and resultSF data
            if (inputShapefile == null || eraseShapefile == null || resultShapefile == null) return;

            resultShapefile.CopyTableSchema(inputShapefile); // Fill the 1st Featureset fields
            IFeatureSet tempSet = inputShapefile.CombinedFields(eraseShapefile);

            // go through every feature in 1st featureSet
            foreach (IFeature t in inputShapefile.Features)
            {
                // go through every feature in 2nd featureSet
                foreach (IFeature t1 in eraseShapefile.Features)
                {
                    t.Difference(t1, tempSet, FieldJoinType.All);
                }
            }

            // Add to the resultSF Feature Set
            for (int a = 0; a < tempSet.Features.Count; a++)
            {
                resultShapefile.Features.Add(tempSet.Features[a]);
            }

            resultShapefile.Save();
        }

        /// <summary>
        /// For faster clipping of polygons with lines. Limits the finding of intersections to
        /// outside->inside or inside->outside 2pt segments. Assumes only one intersections exists
        /// per segment, that a segment of two inside points or two outside points will not intersect
        /// the polygon.
        /// </summary>
        /// <param name="polygon">The polygon that will be sectioned by the line.</param>
        /// <param name="line">The line that will clip the polygon into multiple parts.</param>
        /// <param name="resultFeatureSet">The in-memory shapefile where the polygon sections will be saved.</param>
        /// <returns>False if errors are encountered, true otherwise.</returns>
        public static bool FastClipPolygonWithLine(ref IFeature polygon, ref IFeature line, out IFeatureSet resultFeatureSet)
        {
            IFeatureSet resultFile = new FeatureSet(FeatureType.Polygon);
            if (polygon == null || line == null || polygon.FeatureType != FeatureType.Polygon || !polygon.Geometry.Overlaps(line.Geometry))
            {
                // make sure polygon and line aren't null, we're dealing with a valid shapefile type and line and polygon overlap
                resultFeatureSet = resultFile;
                return false;
            }

            // find if all of the line is inside, outside, or part in and out of polygon
            // line might intersect polygon mutliple times
            int numPoints = line.Geometry.NumPoints;
            bool[] ptsInside = new bool[numPoints];

            int numInside = 0;
            int numOutside = 0;

            int numParts = polygon.Geometry.NumGeometries;
            if (numParts == 0) numParts = 1;

            Coordinate[][] polyVertArray;
            ConvertPolyToVertexArray(ref polygon, out polyVertArray);

            // check each point in the line to see if the entire line is either
            // inside of the polygon or outside of it (we know it's inside polygon bounding box).
            for (int i = 0; i <= numPoints - 1; i++)
            {
                Point currPt = new Point(line.Geometry.Coordinates[i]);

                if (polygon.Geometry.Covers(currPt))
                {
                    ptsInside[i] = true;
                    numInside += 1;
                }
                else
                {
                    ptsInside[i] = false;
                    numOutside += 1;
                }
            }

            if (numInside != numPoints && numOutside != numPoints)
            {
                // case: part of line is inside and part is outside - find inside segments.
                if (!FastProcessPartInAndOut(ptsInside, line, polygon, resultFile))
                {
                    resultFeatureSet = resultFile;
                    return false;
                }
            }

            // resultSF result file, do not save to disk.
            resultFeatureSet = resultFile;
            return true;
        }

        /// <summary>
        /// Sorts all valid intersects in the array intersectPts.
        /// </summary>
        /// <param name="numIntersects">Expected number of valid intersects.</param>
        /// <param name="intersectPts">Array of all possible intersect points.</param>
        /// <param name="validIntersects">Array that will contain only the valid intersect points in sorted order.</param>
        /// <param name="startPt">The reference point to sort the valid intersect points by.</param>
        public static void FindAndSortValidIntersects(int numIntersects, ref IPoint[] intersectPts, ref IPoint[] validIntersects, ref IPoint startPt)
        {
            for (int i = 0; i <= numIntersects - 1; i++)
            {
                validIntersects[i] = intersectPts[i];
            }

            SortPointsArray(ref startPt, ref validIntersects);
        }

        /// <summary>
        /// Find the intersection points of lines that cross a polygon
        /// </summary>
        /// <param name="lineFeatureSet">The line features</param>
        /// <param name="polygon">The polygon feature</param>
        /// <param name="intersectsPerLineSeg">Output array of intersection counts</param>
        /// <param name="intersectionPts">Output array of intersection points</param>
        /// <param name="polyIntersectLocs">Output array of intersection locations in reference to polygon vertex indices</param>
        /// <returns>Number of found intersections.</returns>
        public static int FindIntersections(IFeatureSet lineFeatureSet, IFeature polygon, out int[] intersectsPerLineSeg, out IPoint[][] intersectionPts, out int[][] polyIntersectLocs)
        {
            int numSignChanges = 0; // tracks number of determinant sign changes
            int numLines = lineFeatureSet.Features.Count;
            int numVerticies = polygon.Geometry.NumPoints;
            int[][] detSigns = new int[numLines][];
            bool[][] signChanges = new bool[numLines][]; // keeps track of where sign changes occur
            int[][] changeLocations = new int[numLines][];
            int[] intersectsPerLine = new int[numLines];
            IPoint[][] intersectPts = new IPoint[numLines][];

            IList<Coordinate> coorPoly = polygon.Geometry.Coordinates;

            // ICoordinate[] secCoor = new ICoordinate[2];
            for (int lineNo = 0; lineNo <= numLines - 1; lineNo++)
            {
                IFeature line = lineFeatureSet.Features[lineNo];
                IList<Coordinate> coorLine = line.Geometry.Coordinates;
                int numChangesPerLine = 0;
                detSigns[lineNo] = new int[numVerticies];
                signChanges[lineNo] = new bool[numVerticies];
                intersectPts[lineNo] = new IPoint[numVerticies];
                changeLocations[lineNo] = new int[numVerticies];

                for (int vertNo = 0; vertNo <= numVerticies - 1; vertNo++)
                {
                    intersectPts[lineNo][vertNo] = Point.Empty;
                    IPoint intersectPt = Point.Empty;

                    // Calculate the determinant (3x3 square matrix)
                    double si = TurboDeterm(coorPoly[vertNo].X, coorLine[0].X, coorLine[1].X, coorPoly[vertNo].Y, coorLine[0].Y, coorLine[1].Y);

                    // Check the determinant result
                    switch (vertNo)
                    {
                        case 0:
                            if (Math.Abs(si) <= FindIntersectionTolerance)
                            {
                                detSigns[lineNo][vertNo] = 0; // we have hit a vertex
                            }
                            else if (si > 0)
                            {
                                detSigns[lineNo][vertNo] = 1; // +'ve
                            }
                            else if (si < 0)
                            {
                                detSigns[lineNo][vertNo] = -1; // -'ve
                            }

                            signChanges[lineNo][0] = false; // First element will NEVER be a sign change
                            break;
                        default:
                            if (Math.Abs(si) <= FindIntersectionTolerance)
                            {
                                detSigns[lineNo][vertNo] = 0;
                            }
                            else if (si > 0)
                            {
                                detSigns[lineNo][vertNo] = 1;
                            }
                            else if (si < 0)
                            {
                                detSigns[lineNo][vertNo] = -1;
                            }

                            // Check for sign change
                            if (detSigns[lineNo][vertNo - 1] != detSigns[lineNo][vertNo])
                            {
                                IList<Coordinate> secCoor = new List<Coordinate>
                                                                {
                                                                    coorPoly[vertNo - 1],
                                                                    coorPoly[vertNo]
                                                                };

                                // calculate the actual intercept point
                                LineString polyTestLine1 = new LineString(secCoor.ToArray());
                                secCoor = new List<Coordinate>
                                              {
                                                  coorLine[0],
                                                  coorLine[1]
                                              };
                                LineString polyTestLine2 = new LineString(secCoor.ToArray());
                                bool validIntersect = polyTestLine1.Intersects(polyTestLine2);
                                IGeometry inPt = polyTestLine1.Intersection(polyTestLine2);
                                if (inPt.Coordinates.Length == 1)
                                {
                                    intersectPt = new Point(inPt.Coordinate);
                                }

                                if (validIntersect)
                                {
                                    signChanges[lineNo][vertNo] = true;
                                    numSignChanges += 1;
                                    numChangesPerLine += 1;
                                    intersectsPerLine[lineNo] = numChangesPerLine;

                                    // we want to store the valid intersect pts at the
                                    // beginning of the array so we don't have to search for them
                                    intersectPts[lineNo][numChangesPerLine - 1] = intersectPt;

                                    // keep track of where the intersect occurs in reference to polygon
                                    changeLocations[lineNo][numChangesPerLine - 1] = vertNo;

                                    // intersect pt occurs between vertNo-1 and vertNo
                                }
                                else
                                {
                                    signChanges[lineNo][vertNo] = false;
                                }
                            }
                            else
                            {
                                signChanges[lineNo][vertNo] = false;
                            }

                            break;
                    }

                    // end of switch
                }
            }

            polyIntersectLocs = changeLocations;
            intersectionPts = intersectPts;
            intersectsPerLineSeg = intersectsPerLine;
            return numSignChanges;
        }

        /// <summary>
        /// Determines which shapes are holes and which shapes are islands in
        /// a multi-part polygon and fixes their orientations accordingly.
        /// </summary>
        /// <param name="polygon">The multi-part polygon whose parts need to be checked.</param>
        public static void FixMultiPartPoly(ref IFeature polygon)
        {
            int numParts = polygon.Geometry.NumGeometries;
            if (numParts < 1) return;

            // Multiple parts exist
            IFeature[] parts;
            SeparateParts(ref polygon, out parts);
            for (int i = 0; i <= numParts - 1; i++)
            {
                bool currIsClockwise = !CGAlgorithms.IsCCW(parts[i].Geometry.Coordinates);
                bool partIsHole = false;

                // Decide if the current part is an island or a hole.
                // Properties of Holes:
                // 1) Extents are inside the extents of another part.
                // 2) All points are inside the above part.
                for (int j = 0; j <= numParts - 1; j++)
                {
                    if (j == i || !parts[j].Geometry.Envelope.Contains(parts[i].Geometry.Envelope)) continue;

                    // found a potential hole, do further checking
                    Point pt = new Point(parts[i].Geometry.Coordinates[0]);

                    if (!parts[j].Geometry.Covers(pt)) continue;
                    partIsHole = true;
                    break;
                }

                // done checking current part against all other parts
                if (partIsHole && currIsClockwise)
                {
                    // Hole, make sure it's in counter-clockwise order
                }
                else if (!partIsHole && !currIsClockwise)
                {
                    // Island, make sure it's in clockwise order
                }

                /* if (reverse == true)
                 {
                     ReverseSimplePoly(ref parts[i]);
                 }
                 */
            }

            // done looping through parts and correcting orientation (if necessary)
            IFeature resultShp;

            // resultShp.Create(polygon.ShapeType);
            CombineParts(ref parts, out resultShp);
            polygon = resultShp;

            // done with multiple parts
        }

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        /// <param name="pt0">The first point.</param>
        /// <param name="pt1">The second point.</param>
        /// <returns>The distance between pt0 and pt1.</returns>
        public static double PtDistance(ref IPoint pt0, ref IPoint pt1)
        {
            double xDiff = pt1.X - pt0.X;
            double yDiff = pt1.Y - pt0.Y;
            double distance = Math.Sqrt((xDiff * xDiff) + (yDiff * yDiff));
            return distance;
        }

        /// <summary>
        /// Separate Parts.
        /// </summary>
        /// <param name="poly">Polygon that gets separated.</param>
        /// <param name="polyParts">Resulting parts.</param>
        public static void SeparateParts(ref IFeature poly, out IFeature[] polyParts)
        {
            int numParts = poly.Geometry.NumGeometries;
            if (numParts == 0) numParts = 1;

            IFeature[] parts = new IFeature[numParts];

            if (numParts > 1)
            {
                for (int i = 0; i <= numParts - 1; i++)
                {
                    int countPoints = poly.Geometry.GetGeometryN(i).Coordinates.Length;
                    List<Coordinate> partsList = new List<Coordinate>();
                    for (int j = 0; j <= countPoints - 1; j++)
                    {
                        partsList.Insert(j, poly.Geometry.Coordinates[j]);
                    }

                    parts[i] = new Feature(FeatureType.Polygon, partsList);
                }

                polyParts = parts;
            }
            else
            {
                parts[0] = new Feature();
                parts[0] = poly;
                polyParts = parts;
            }
        }

        /// <summary>
        /// Once the parameters have been configured the Execute command can be called, it returns true if successful
        /// </summary>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True, if executed successfully.</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet input1 = _inputParam[0].Value as IFeatureSet;
            input1?.FillAttributes();

            var input2 = _inputParam[1].Value as IFeatureSet;
            input2?.FillAttributes();

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input1, input2, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the ClipPolygonWithLine Operation tool programmatically.
        /// </summary>
        /// <param name="input1">The input Polygon FeatureSet.</param>
        /// <param name="input2">The input Polyline FeatureSet.</param>
        /// <param name="output">The output Polygon FeatureSet.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True, if executed successfully.</returns>
        public bool Execute(IFeatureSet input1, IFeatureSet input2, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (input1 == null || input2 == null || output == null || cancelProgressHandler.Cancel) return false;

            string filename = output.Filename; // changed by jany_ (2016-05-23) remember filename here because output gets replaced a few times and we don't want to reset the filename again and again just because we need it to save the shapefile here

            IFeature polygon = input1.Features[0];
            IFeature line = input2.Features[0];
            IFeatureSet resultFs = new FeatureSet(FeatureType.Polygon);
            int previous = 0;
            if (!DoClipPolygonWithLine(ref polygon, ref line, ref output))
            {
                throw new SystemException(TextStrings.Exceptioninclipin);
            }

            int intFeature = output.Features.Count;
            for (int i = 0; i < intFeature; i++)
            {
                Polygon poly = new Polygon(new LinearRing(output.Features[i].Geometry.Coordinates));
                resultFs.AddFeature(poly);

                int current = Convert.ToInt32(Math.Round(i * 100D / intFeature));

                // only update when increment in percentage
                if (current > previous)
                {
                    cancelProgressHandler.Progress(string.Empty, current, current + TextStrings.progresscompleted);
                }

                previous = current;
            }

            cancelProgressHandler.Progress(string.Empty, 100, 100 + TextStrings.progresscompleted);
            resultFs.SaveAs(filename, true);
            return true;
        }

        /// <summary>
        /// The parameters array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new PolygonFeatureSetParam(TextStrings.input1PolygonShapefile)
            {
                HelpText = TextStrings.InputPolygonforCliping
            };
            _inputParam[1] = new LineFeatureSetParam(TextStrings.input2LineforCliping)
            {
                HelpText = TextStrings.Inputlineforcliping
            };

            _outputParam = new Parameter[2];
            _outputParam[0] = new FeatureSetParam(TextStrings.ResultShapefile)
            {
                HelpText = TextStrings.SelectResultShapefileDirectory
            };
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        /// <summary>
        /// Given a line that contains portion both inside and outside of the polygon, this
        /// function will split the polygon based only on the segments that completely bisect
        /// the polygon. It assumes: out->out, and in->in 2pt segments do not intersect the
        /// polygon, and out->in, in->out 2pt segments have only one point of intersection.
        /// </summary>
        /// <param name="insidePts">A boolean array indicating if a point is inside the polygon or not.</param>
        /// <param name="line">The line that intersects the polygon.</param>
        /// <param name="polygon">The polygon that will be split by the intersecting line.</param>
        /// <param name="resultFeatureSet">The shapefile that the polygon sections will be saved to.</param>
        /// <returns>False if errors were encountered or an assumption violated, true otherwise.</returns>
        private static bool FastProcessPartInAndOut(bool[] insidePts, IFeature line, IFeature polygon, IFeatureSet resultFeatureSet)
        {
            int numLinePts = line.Geometry.NumPoints;
            int numLineSegs = numLinePts - 1;
            int numPolyPts = polygon.Geometry.NumPoints;
            int[] intersectsPerSeg;
            IPoint[][] intersectPts = new IPoint[numLineSegs][];
            int[][] polyIntLocs = new int[numLineSegs][];

            // intersection occurs between polygon point indexed by polyIntLoc[][] and the previous point.

            // cut line into 2pt segments and put in new shapefile.
            IFeatureSet lineSegments = new FeatureSet();

            IFeature lineSegment;
            IList<Coordinate> coordi = line.Geometry.Coordinates;

            for (int i = 0; i <= numLineSegs - 1; i++)
            {
                Coordinate[] secCoordinate = new Coordinate[2];
                secCoordinate[0] = coordi[i];
                secCoordinate[1] = coordi[i + 1];
                lineSegment = new Feature(FeatureType.Line, secCoordinate);
                lineSegments.Features.Add(lineSegment);

                intersectPts[i] = new IPoint[numPolyPts];
                polyIntLocs[i] = new int[numPolyPts];
            }

            // find number of intersections, intersection pts, and locations for each 2pt segment
            int numIntersects = FindIntersections(lineSegments, polygon, out intersectsPerSeg, out intersectPts, out polyIntLocs);

            if (numIntersects == 0)
            {
                return false;
            }

            IFeature insideLine = new Feature();
            List<Coordinate> insideLineList = new List<Coordinate>();

            IPoint startIntersect = Point.Empty;
            bool startIntExists = false;
            bool validInsideLine = false;
            int insideStart = 0;
            int startIntPolyLoc = 0;

            // loop through each 2pt segment
            for (int i = 0; i <= numLinePts - 2; i++)
            {
                lineSegment = lineSegments.Features[i];
                int numSegIntersects = intersectsPerSeg[i];

                // ****************** case: inside->inside **************************************//
                int ptIndex;
                if (insidePts[i] && insidePts[i + 1])
                {
                    if (numSegIntersects == 0 && i != numLinePts - 2 && i != 0)
                    {
                        // add points to an inside line segment
                        if (startIntExists)
                        {
                            ptIndex = 0;
                            insideLineList.Insert(ptIndex, startIntersect.Coordinate);
                            startIntExists = false;
                            validInsideLine = true;
                            insideStart = startIntPolyLoc;
                        }

                        if (validInsideLine)
                        {
                            ptIndex = insideLine.Geometry.NumPoints;
                            insideLineList.Insert(ptIndex, lineSegment.Geometry.Coordinates[0]);
                        }
                    }
                    else
                    {
                        // we do not handle multiple intersections in the fast version
                        return false;
                    }

                    // end of inside->inside
                }
                else if (insidePts[i] && insidePts[i + 1] == false)
                {
                    // ********************** case: inside->outside ****************************************
                    if (numSegIntersects == 0)
                    {
                        return false;
                    }

                    if (numSegIntersects == 1)
                    {
                        if (startIntExists)
                        {
                            var intersectSegList = new List<Coordinate>();
                            ptIndex = 0;
                            intersectSegList.Insert(ptIndex, startIntersect.Coordinate);
                            ptIndex = 1;
                            intersectSegList.Insert(ptIndex, lineSegment.Geometry.Coordinates[0]);
                            ptIndex = 2;
                            intersectSegList.Insert(ptIndex, intersectPts[i][0].Coordinate);
                            IFeature intersectSeg = new Feature(FeatureType.Line, intersectSegList);

                            int firstPolyLoc = startIntPolyLoc;
                            int lastPolyLoc = polyIntLocs[i][0] - 1;
                            if (SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultFeatureSet) == false)
                            {
                                return false;
                            }

                            startIntExists = false; // we just used it up!
                        }
                        else if (insideLine.Geometry.NumPoints != 0 && validInsideLine)
                        {
                            ptIndex = insideLineList.Count;
                            insideLineList.Insert(ptIndex, lineSegment.Geometry.Coordinates[0]);
                            ptIndex++;
                            insideLineList.Insert(ptIndex, intersectPts[i][0].Coordinate);
                            insideLine = new Feature(FeatureType.Line, insideLineList);

                            int firstPolyLoc = insideStart;
                            int lastPolyLoc = polyIntLocs[i][0] - 1;
                            if (!SectionPolygonWithLine(ref insideLine, ref polygon, firstPolyLoc, lastPolyLoc, ref resultFeatureSet)) return false;

                            validInsideLine = false;

                            // TODO jany_ do we need to clear the Coordinates?
                            // insideLine.Geometry.Coordinates.Clear();
                        }
                    }
                    else
                    {
                        // we do not handle multiple intersections in the fast version
                        return false;
                    }

                    // end of inside->outside
                }
                else if (insidePts[i] == false && insidePts[i + 1])
                {
                    // ********************** case: outside->inside ***************************************
                    validInsideLine = false;

                    if (numSegIntersects == 0)
                    {
                        return false;
                    }

                    if (numSegIntersects == 1)
                    {
                        startIntExists = true;
                        startIntersect = intersectPts[i][0];
                        startIntPolyLoc = polyIntLocs[i][0] - 1;
                    }
                    else
                    {
                        // we do not handle multiple intersections in the fast version
                        return false;
                    }
                }
                else if (insidePts[i] == false && insidePts[i + 1] == false)
                {
                    // ************************ case: outside->outside ***********************************
                    startIntExists = false;
                    validInsideLine = false;

                    if (numSegIntersects == 0)
                    {
                        // do nothing
                    }
                    else
                    {
                        // we do not handle multiple intersections in the fast version
                        return false;
                    }
                }

                // end of outside->outside
            }

            // end of looping through 2pt segments
            return true;
        }

        /// <summary>
        /// Sorts all valid intersects in the array intersectPts, along with corresponding polygon locations in array polyLoc.
        /// </summary>
        /// <param name="numIntersects">Expected number of valid intersects.</param>
        /// <param name="intersectPts">Array of all possible intersect points.</param>
        /// <param name="validIntersects">Array that will contain only the valid intersect points in sorted order.</param>
        /// <param name="startPt">The reference point to sort the valid intersect points by.</param>
        /// <param name="polyLoc">Array with corresponding indicies to where an intersect pt occurs in polygon.</param>
        private static void FindAndSortValidIntersects(int numIntersects, ref IPoint[] intersectPts, ref IPoint[] validIntersects, ref IPoint startPt, ref int[] polyLoc)
        {
            for (int i = 0; i <= numIntersects - 1; i++)
            {
                validIntersects[i] = intersectPts[i];
            }

            SortIntersectAndLocationArrays(ref startPt, ref validIntersects, ref polyLoc);
        }

        /// <summary>
        /// For lines where every point lies within the polygon, this function will
        /// find if any 2pt segment crosses through the polygon. If so, it will split
        /// the polygon into mutliple parts using the intersecting line segments.
        /// </summary>
        /// <param name="line">The line whose points are all inside the polygon.</param>
        /// <param name="polygon">The polygon being checked for intersection.</param>
        /// <param name="resultFeatureSet">The file where new polygon sections should be saved to.</param>
        /// <returns>False if errors were encountered, true otherwise.</returns>
        private static bool ProcessAllInside(ref IFeature line, ref IFeature polygon, ref IFeatureSet resultFeatureSet)
        {
            int numLinePts = line.Geometry.NumPoints;
            int numLineSegs = numLinePts - 1;
            int numPolyPts = polygon.Geometry.NumPoints;
            int[] intersectsPerSeg;
            IPoint[][] intersectPts = new IPoint[numLineSegs][];
            int[][] polyIntLocs = new int[numLineSegs][];

            // intersection occurs between polygon point indexed by polyIntLoc[][] and the previous point.

            // cut line into 2pt segments and put in new shapefile.
            IFeatureSet lineSegments = new FeatureSet(FeatureType.Line);
            IList<Coordinate> coordi = line.Geometry.Coordinates;
            for (int i = 0; i <= numLineSegs - 1; i++)
            {
                Coordinate[] secCoordinate = new Coordinate[2];
                secCoordinate[0] = coordi[i];
                secCoordinate[1] = coordi[i + 1];
                IFeature lineSegment = new Feature(FeatureType.Line, secCoordinate);
                lineSegments.Features.Add(lineSegment);

                intersectPts[i] = new IPoint[numPolyPts];
                polyIntLocs[i] = new int[numPolyPts];
            }

            if (FindIntersections(lineSegments, polygon, out intersectsPerSeg, out intersectPts, out polyIntLocs) == 0) return true; // entire line is inside the polygon, no splitting occurs

            // intersections exist! Find out where.
            List<Coordinate> intersectSegList = new List<Coordinate>();
            for (int i = 0; i <= numLineSegs - 1; i++)
            {
                int numSegIntersects = intersectsPerSeg[i];

                // if there are less than 4 intersects, the line will not cross the
                // polygon in such a way that a new polygon section can be created.
                if (numSegIntersects <= 2)
                {
                    // inside lines should be ignored, we only want a portion that crosses the polygon.
                    int c = i + 1;
                    while (intersectsPerSeg[c] <= 2 && c <= numLineSegs - 1)
                    {
                        c++;
                        if (c == numLineSegs) break;
                    }

                    i = c - 1;
                }
                else
                {
                    // there should always be an even # of intersects for a line of all inside pts
                    // find intersecting segments that will split the polygon
                    IPoint[] intPts = new IPoint[numSegIntersects];
                    IPoint startPt = new Point(lineSegments.Features[i].Geometry.Coordinates[0]);
                    FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);

                    for (int j = 0; j <= numSegIntersects - 1; j++)
                    {
                        if (j == 0 || j == numSegIntersects - 1) continue; // Any segment formed from inside pt -> intersect pt or intersect pt -> inside pt will NOT cross the polygon.

                        intersectSegList.Insert(0, intPts[j].Coordinate);
                        intersectSegList.Insert(1, intPts[j + 1].Coordinate);
                        IFeature intersectSeg = new Feature(FeatureType.Line, intersectSegList);
                        int polyStartIndex = polyIntLocs[i][j] - 1;
                        int polyEndIndex = polyIntLocs[i][j + 1] - 1;
                        if (!SectionPolygonWithLine(ref intersectSeg, ref polygon, polyStartIndex, polyEndIndex, ref resultFeatureSet)) return false;

                        // TODO jany_ do we need to clear the Coordinates?
                        // intersectSeg.Geometry.Coordinates.Clear();
                        j++;
                    }

                    // end of looping through intersect pts
                }

                // end of more than 2 intersects exist
            }

            // end of looping through 2pt line segments
            return true;
        }

        /// <summary>
        /// For lines where every point lies outside the polygon, this function will
        /// find if any 2pt segment crosses through the polygon. If so, it will split
        /// the polygon into mutliple parts using the intersecting line segments.
        /// </summary>
        /// <param name="line">The line whose points are all inside the polygon.</param>
        /// <param name="polygon">The polygon being checked for intersection.</param>
        /// <param name="resultFeatureSet">The file where new polygon sections should be saved to.</param>
        /// <returns>False if errors were encountered, true otherwise.</returns>
        private static bool ProcessAllOutside(ref IFeature line, ref IFeature polygon, ref IFeatureSet resultFeatureSet)
        {
            int numLinePts = line.Geometry.NumPoints;
            int numLineSegs = numLinePts - 1;
            int numPolyPts = polygon.Geometry.NumPoints;
            int[] intersectsPerSeg;
            IPoint[][] intersectPts = new IPoint[numLineSegs][];
            int[][] polyIntLocs = new int[numLineSegs][];

            // intersection occurs between polygon point indexed by polyIntLoc[][] and the previous point..

            // cut line into 2pt segments and put in new shapefile.
            // string tempPath = System.IO.Path.GetTempPath() + "tempLineSF.shp";
            IFeatureSet lineSegments = new FeatureSet(FeatureType.Line);

            // Globals.PrepareResultSF(ref tempPath, ref lineSegSF, line.ShapeType);
            IList<Coordinate> coordi = line.Geometry.Coordinates;

            for (int i = 0; i <= numLineSegs - 1; i++)
            {
                Coordinate[] secCoordinate = new Coordinate[2];
                secCoordinate[0] = coordi[i];
                secCoordinate[1] = coordi[i + 1];
                IFeature lineSegment = new Feature(FeatureType.Line, secCoordinate);
                lineSegments.Features.Add(lineSegment);

                intersectPts[i] = new IPoint[numPolyPts];
                polyIntLocs[i] = new int[numPolyPts];
            }

            // Add polygon to result feature set for loop
            resultFeatureSet.Features.Add(polygon);

            int numOriginalIntersects = FindIntersections(lineSegments, polygon, out intersectsPerSeg, out intersectPts, out polyIntLocs);
            if (numOriginalIntersects < 1) return true;

            for (int nCurrentCut = 1; nCurrentCut <= (numOriginalIntersects / 2); nCurrentCut++)
            {
                IFeatureSet ifsNewResultFs = new FeatureSet();

                foreach (IFeature feature in resultFeatureSet.Features)
                {
                    // update with point from feature
                    numPolyPts = feature.Geometry.NumPoints;
                    for (int i = 0; i <= numLineSegs - 1; i++)
                    {
                        intersectPts[i] = new IPoint[numPolyPts];
                        polyIntLocs[i] = new int[numPolyPts];
                    }

                    if (FindIntersections(lineSegments, feature, out intersectsPerSeg, out intersectPts, out polyIntLocs) == 0)
                    {
                        // entire line is outside the polygon, no splitting occurs

                        // add this feature to the result
                        ifsNewResultFs.Features.Add(feature);
                    }
                    else
                    {
                        // intersections exist! Find out where.
                        List<Coordinate> intersectSegList = new List<Coordinate>();
                        for (int i = 0; i <= numLineSegs - 1; i++)
                        {
                            int numSegIntersects = intersectsPerSeg[i];

                            // if there are less than 2 intersects, the line will not cross the
                            // polygon in such a way that a new polygon section can be created.
                            if (numSegIntersects < 2)
                            {
                                // loop to find next segment with enough intersections
                                int c = i + 1;
                                while (c <= numLineSegs - 1 && intersectsPerSeg[c] < 2)
                                {
                                    c++;
                                    if (c == numLineSegs) break;
                                }

                                i = c - 1; // setup the next segment to use
                            }
                            else
                            {
                                // there should always be an even # of intersects for a line of all outside pts
                                // find the valid intersect points from our array
                                IPoint[] intPts = new IPoint[numSegIntersects];
                                IPoint startPt = new Point(lineSegments.Features[i].Geometry.Coordinates[0]);

                                FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);

                                intersectSegList.Insert(0, intPts[0].Coordinate); // make only one cut at a time
                                intersectSegList.Insert(1, intPts[1].Coordinate);
                                IFeature intersectSeg = new Feature(FeatureType.Line, intersectSegList);

                                IFeatureSet ifsNewFs = new FeatureSet();
                                IFeature currentFeature = feature;
                                int polyStartIndex = polyIntLocs[i][0] - 1;
                                int polyEndIndex = polyIntLocs[i][1] - 1;
                                if (!SectionPolygonWithLine(ref intersectSeg, ref currentFeature, polyStartIndex, polyEndIndex, ref ifsNewFs)) return false;

                                // Now add these to the new Feature Set
                                foreach (IFeature feature1 in ifsNewFs.Features)
                                {
                                    ifsNewResultFs.Features.Add(feature1);
                                }

                                // TODO jany_ do we need to clear the Coordinates?
                                // intersectSeg.Geometry.Coordinates.Clear();
                            }

                            // end of else intersects exist for 2pt segment
                        }

                        // end of looping through 2pt line segments
                    }

                    // end of else intersects exist
                }

                // for each feature in FeatureSet

                // update the result set
                resultFeatureSet = ifsNewResultFs;
            }

            // for number of cuts to make

            // Remove any duplicate features
            RemoveDuplicateFeaturesFromFeatureSet(ref resultFeatureSet);

            return true;
        }

        /// <summary>
        /// Given a line that contains portions both inside and outside of the polygon, this
        /// function will split the polygon based only on the segments that completely bisect
        /// the polygon. The possibility of mutliple intersections for any 2pt segment is taken
        /// into account.
        /// </summary>
        /// <param name="insidePts">A boolean array indicating if a point is inside the polygon or not.</param>
        /// <param name="line">The line that intersects the polygon.</param>
        /// <param name="polygon">The polygon that will be split by the intersecting line.</param>
        /// <param name="resultFeatureSet">The shapefile that the polygon sections will be saved to.</param>
        /// <returns>False if errors were encountered or an assumption violated, true otherwise.</returns>
        private static bool ProcessPartInAndOut(ref bool[] insidePts, ref IFeature line, ref IFeature polygon, ref IFeatureSet resultFeatureSet)
        {
            int numLinePts = line.Geometry.NumPoints;
            int numLineSegs = numLinePts - 1;
            int numPolyPts = polygon.Geometry.NumPoints;
            int[] intersectsPerSeg;
            IPoint[][] intersectPts = new IPoint[numLineSegs][];
            int[][] polyIntLocs = new int[numLineSegs][];

            // intersection occurs between polygon point indexed by polyIntLoc[][] and the previous point.

            // cut line into 2pt segments and put in new shapefile.
            IFeatureSet lineSegments = new FeatureSet(FeatureType.Line);
            IList<Coordinate> coordi = line.Geometry.Coordinates;

            for (int i = 0; i <= numLineSegs - 1; i++)
            {
                Coordinate[] secCoordinate = new Coordinate[2];
                secCoordinate[0] = coordi[i];
                secCoordinate[1] = coordi[i + 1];
                IFeature lineSegment = new Feature(FeatureType.Line, secCoordinate);
                lineSegments.Features.Add(lineSegment);

                intersectPts[i] = new IPoint[numPolyPts];
                polyIntLocs[i] = new int[numPolyPts];
            }

            // find number of intersections, intersection pts, and locations for each 2pt segment
            int numIntersects = FindIntersections(lineSegments, polygon, out intersectsPerSeg, out intersectPts, out polyIntLocs);
            if (numIntersects == 0) return false;

            // CASE: If no split was made, try using the whole line.
            if (resultFeatureSet.Features.Count != 0) return true;

            resultFeatureSet.Features.Add(polygon); // start with original polygon
            List<IFeature> colLineParts = new List<IFeature>();
            List<Coordinate> colPolyIntLocPoints = new List<Coordinate>();

            ProcessPartInAndOutWithWholeLine(ref insidePts, ref line, ref polygon, ref resultFeatureSet, numIntersects, numLineSegs, intersectPts, polyIntLocs, numPolyPts, colLineParts, colPolyIntLocPoints);

            RemoveDuplicateFeaturesFromFeatureSet(ref resultFeatureSet);

            return true;
        }

        private static bool ProcessPartInAndOutWithWholeLine(ref bool[] insidePts, ref IFeature line, ref IFeature polygon, ref IFeatureSet resultFeatureSet, int numIntersects, int numLineSegs, IPoint[][] intersectPts, int[][] polyIntLocs, int numPolyPts, List<IFeature> colLineParts, List<Coordinate> colPolyIntLocPoints)
        {
            IPoint[] intPts = new IPoint[numIntersects];
            IPoint startPt = new Point(line.Geometry.Coordinates[0]);
            IPoint firstIntPt = null;
            IPoint lastIntPt = null;
            List<int> listIntLocs = new List<int>();
            List<int> listLinePartIndexValuesForOrigLine = new List<int>();
            List<List<int>> colListForEachLinePart = new List<List<int>>();
            List<int> listLinePartStartEndIndexValues = new List<int>();
            int nNewLineStartIndex = 0;
            int nNewLineEndIndex = 0;
            int nLastIndexUsed = -1;
            List<Coordinate> listNewLine = new List<Coordinate>();
            int nLineParts = 0;

            // First time only: collect the information needed for all intersections
            if (colLineParts.Count == 0)
            {
                for (int i = 0; i <= numLineSegs - 1; i++)
                {
                    FindAndSortValidIntersects(numIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);

                    // Find the intersect points & start/end line points
                    for (int j = 0; j < numPolyPts; j++)
                    {
                        if (polyIntLocs[i][j] != 0)
                        {
                            // Save this index in the polygon points
                            listIntLocs.Add(polyIntLocs[i][j] - 1);
                        }

                        if (!intersectPts[i][j].X.Equals(double.NaN))
                        {
                            if (firstIntPt == null)
                            {
                                firstIntPt = intersectPts[i][j]; // first intersection for this cut
                                nNewLineStartIndex = i;
                                listNewLine.Add(new Coordinate(intersectPts[i][j].X, intersectPts[i][j].Y, 0.0));
                                listLinePartStartEndIndexValues.Add(listNewLine.Count - 1);
                            }
                            else
                            {
                                lastIntPt = intersectPts[i][j]; // last intersection for this cut
                                nNewLineEndIndex = i;
                            }
                        }
                    }

                    if (firstIntPt != null && lastIntPt == null)
                    {
                        // Collect the point from the line that are between the two poly intersetion points
                        listNewLine.Add(line.Geometry.Coordinates[i]);
                        if (i > nLastIndexUsed)
                        {
                            listLinePartIndexValuesForOrigLine.Add(i);
                            nLastIndexUsed = i; // we want one of each index
                        }
                    }
                    else if (firstIntPt != null && lastIntPt != null)
                    {
                        listNewLine.Add(new Coordinate(lastIntPt.X, lastIntPt.Y, 0.0));
                        listLinePartStartEndIndexValues.Add(listNewLine.Count - 1);

                        colListForEachLinePart.Add(new List<int>(listLinePartIndexValuesForOrigLine));

                        listLinePartIndexValuesForOrigLine.Clear();
                        firstIntPt = null;
                        lastIntPt = null;
                    }
                }

                for (int i = 0; i < (listIntLocs.Count / 2); i++)
                {
                    // Save these points so we can find the "new" index values later
                    colPolyIntLocPoints.Add(polygon.Geometry.Coordinates[listIntLocs[2 * i]]);
                    colPolyIntLocPoints.Add(polygon.Geometry.Coordinates[listIntLocs[2 * i + 1]]);

                    // Build the line parts that we need for each cut
                    int nStart = listLinePartStartEndIndexValues[2 * nLineParts];
                    int nEnd = listLinePartStartEndIndexValues[2 * nLineParts + 1];
                    List<int> currentListOfIndexValues = colListForEachLinePart[nLineParts];

                    List<Coordinate> listLinePart = new List<Coordinate> { listNewLine[nStart] };

                    // Add first intersection
                    foreach (int j in currentListOfIndexValues)
                    {
                        listLinePart.Add(line.Geometry.Coordinates[j + 1]); // get values from original line
                    }

                    listLinePart.Add(listNewLine[nEnd]); // Add second intersection

                    // Now, we have the part of the line we need make this cut
                    IFeature newLinePart = new Feature(FeatureType.Line, listLinePart);
                    colLineParts.Add(newLinePart);

                    nLineParts++; // get ready for next part
                }
            }

            bool bSwapIntLocs = false;
            nLineParts = 0;

            // For each intersecting line part, see if a cut needs to be made.
            for (int i = 0; i < colLineParts.Count; i++)
            {
                IFeature newLinePart = colLineParts[i];
                IFeatureSet tempFeatureSet = new FeatureSet(FeatureType.Polygon);

                foreach (IFeature feature in resultFeatureSet.Features)
                {
                    IFeature currentFeature = feature;

                    // Does this part of the line really intersect?
                    if (newLinePart.Intersects(currentFeature.Geometry.EnvelopeInternal))
                    {
                        int firstIntLoc = -1;
                        int lastIntLoc = -1;
                        Coordinate firstCoord = colPolyIntLocPoints[2 * nLineParts];
                        Coordinate lastCoord = colPolyIntLocPoints[2 * nLineParts + 1];

                        // Find the index of both points
                        for (int j = 0; j < currentFeature.Geometry.Coordinates.Length; j++)
                        {
                            Coordinate coord = currentFeature.Geometry.Coordinates[j];
                            if (coord.Equals(firstCoord) && firstIntLoc == -1)
                                firstIntLoc = j;
                            if (coord.Equals(lastCoord) && lastIntLoc == -1)
                                lastIntLoc = j;
                        }

                        if (bSwapIntLocs)
                        {
                            int nTemp = firstIntLoc;
                            firstIntLoc = lastIntLoc;
                            lastIntLoc = nTemp;
                        }

                        IFeatureSet tempSplitFeatureSet = new FeatureSet(FeatureType.Polygon);
                        if (!SectionPolygonWithLine(ref newLinePart, ref currentFeature, firstIntLoc, lastIntLoc, ref tempSplitFeatureSet)) return false;

                        if (tempSplitFeatureSet.Features.Count == 0)
                        {
                            tempFeatureSet.Features.Add(currentFeature); // no cut was made
                        }
                        else
                        {
                            foreach (IFeature feature2 in tempSplitFeatureSet.Features)
                            {
                                tempFeatureSet.Features.Add(feature2);
                            }
                        }
                    }
                    else
                    {
                        tempFeatureSet.Features.Add(currentFeature); // no cut needed for this polygon
                    }
                }

                // Next time we will need to do the opposite
                nLineParts++; // get ready for next part

                // Replace original features with new ones
                resultFeatureSet = new FeatureSet(tempFeatureSet.Features);
            }

            return true;
        }

        /// <summary>
        /// Removes duplicate features from the IFeatureSet.
        /// </summary>
        /// <param name="resultFeatureSet">IFeatureSet to remove features from.</param>
        private static void RemoveDuplicateFeaturesFromFeatureSet(ref IFeatureSet resultFeatureSet)
        {
            IFeatureSet ifsNewResultFs = new FeatureSet();
            int nIndexOfCurrent = 0;
            List<int> listIndexesToNotAdd = new List<int>();
            List<int> listIndexesAddedToNewFs = new List<int>();

            if (resultFeatureSet.Features.Count < 2) return; // no need to check

            while (nIndexOfCurrent < resultFeatureSet.Features.Count)
            {
                IFeature currentFeature = resultFeatureSet.Features[nIndexOfCurrent];
                if (!listIndexesToNotAdd.Contains(nIndexOfCurrent))
                {
                    for (int j = 0; j < resultFeatureSet.Features.Count; j++)
                    {
                        if (nIndexOfCurrent != j && !listIndexesAddedToNewFs.Contains(j))
                        {
                            IFeature feature = resultFeatureSet.Features[j];
                            IFeature diffFeature = currentFeature.SymmetricDifference(feature.Geometry);
                            if (diffFeature == null)
                            {
                                // Exact duplicate
                                if (!listIndexesToNotAdd.Contains(j)) listIndexesToNotAdd.Add(j);
                            }
                            else if (diffFeature.Geometry.Area <= FindIntersectionTolerance && !listIndexesToNotAdd.Contains(j))
                            {
                                // Only a "sliver" remained; treat as a duplicate
                                listIndexesToNotAdd.Add(j);
                            }
                        }
                    }
                }

                if (!listIndexesToNotAdd.Contains(nIndexOfCurrent))
                {
                    ifsNewResultFs.Features.Add(currentFeature);
                    listIndexesAddedToNewFs.Add(nIndexOfCurrent);
                }

                nIndexOfCurrent++;
            }

            // Done
            resultFeatureSet = ifsNewResultFs;
        }

        /// <summary>
        /// Sections a polygon into multiple parts depending on where line crosses it and if previous sectioning has occured.
        /// </summary>
        /// <param name="line">The line that splits the polygon. First and last points are intersect points.</param>
        /// <param name="polygon">The polygon that is to be split by the line.</param>
        /// <param name="polyStart">Index to polygon segment where the first intersect point is found.</param>
        /// <param name="polyEnd">Index to polygon segment where last intersect point is found.</param>
        /// <param name="resultFeatureSet">Reference to result shapefile where new polygon sections will be saved.</param>
        /// <returns>False if an error occurs, true otherwise.</returns>
        private static bool SectionPolygonWithLine(ref IFeature line, ref IFeature polygon, int polyStart, int polyEnd, ref IFeatureSet resultFeatureSet)
        {
            int numResults = resultFeatureSet.Features.Count;

            // we can now make two new polygons by splitting the original one with the line segment
            IFeature poly1;
            IFeature poly2;

            SplitPolyInTwo(ref line, ref polygon, polyStart, polyEnd, out poly1, out poly2);
            if (numResults == 0)
            {
                // if this split creates a sliver, we do not want to add either poly
                if (poly1.Geometry.Area > FindIntersectionTolerance && poly2.Geometry.Area > FindIntersectionTolerance)
                {
                    resultFeatureSet.Features.Insert(0, poly1);
                    resultFeatureSet.Features.Insert(1, poly2);
                }
            }
            else
            {
                // this polygon underwent previous splittings, check
                // if the new results overlay the old ones before adding to resultSF
                IFeatureSet testFeatureSet = new FeatureSet(FeatureType.Polygon);
                IFeatureSet testFeatureSet2 = new FeatureSet(FeatureType.Polygon);

                if (!ClipPolygonFeatureSetWithPolygon(resultFeatureSet, poly1, testFeatureSet, false)) return false;

                if (!ClipPolygonFeatureSetWithPolygon(resultFeatureSet, poly2, testFeatureSet2, false)) return false;

                if (testFeatureSet.Features.Count > 0 || testFeatureSet2.Features.Count > 0)
                {
                    int numTestShapes = testFeatureSet.Features.Count;
                    const int InsertIndex = 0;
                    IFeature insertShape;
                    for (int j = 0; j <= numTestShapes - 1; j++)
                    {
                        insertShape = testFeatureSet.Features[j];
                        resultFeatureSet.Features.Insert(InsertIndex, insertShape);
                    }

                    numTestShapes = testFeatureSet2.Features.Count;
                    for (int j = 0; j <= numTestShapes - 1; j++)
                    {
                        insertShape = testFeatureSet2.Features[j];
                        resultFeatureSet.Features.Insert(InsertIndex, insertShape);
                    }
                }
            }

            // end of checking against previous splits
            return true;
        }

        /// <summary>
        /// Given a reference point to the line, and an array of points that
        /// lie along the line, this method sorts the array of points from the point
        /// closest to the reference pt to the pt farthest away. It also sorts the corresponding
        /// polygon location array so that the indicies refer to the correct intersection point.
        /// </summary>
        /// <param name="startPt">Point in line segment used as reference.</param>
        /// <param name="intersectPts">Array of points that lie on the same line as startPt.</param>
        /// <param name="polyLoc">Array indexing where in polygon an intersect occurs.</param>
        private static void SortIntersectAndLocationArrays(ref IPoint startPt, ref IPoint[] intersectPts, ref int[] polyLoc)
        {
            double dist1;
            int numIntersectPts = intersectPts.Length;
            if (numIntersectPts == 2)
            {
                // if 0 or 1 the points don't need to be sorted
                // do a brute sort
                // just compare distances of each pt to the start pt.
                dist1 = startPt.Distance(intersectPts[0]);
                double dist2 = startPt.Distance(intersectPts[1]);
                if (dist1 > dist2)
                {
                    // need to swap locations
                    IPoint tempPt = intersectPts[0];
                    intersectPts[0] = intersectPts[1];
                    intersectPts[1] = tempPt;

                    // move poly location so it corresponds to correct intersect point
                    int tempLoc = polyLoc[0];
                    polyLoc[0] = polyLoc[1];
                    polyLoc[1] = tempLoc;
                }
            }
            else if (numIntersectPts > 2 /*&& numintersectPts <= 10*/)
            {
                // use insertion sort for small arrays
                for (int i = 0; i <= numIntersectPts - 1; i++)
                {
                    IPoint compPt1 = intersectPts[i];
                    int tempLoc1 = polyLoc[i];
                    dist1 = startPt.Distance(compPt1);
                    int c = i;
                    IPoint compPt2 = c != 0 ? intersectPts[c - 1] : intersectPts[0];
                    while (c > 0 && PtDistance(ref startPt, ref compPt2) > dist1)
                    {
                        intersectPts[c] = intersectPts[c - 1];
                        polyLoc[c] = polyLoc[c - 1];
                        c--;
                        if (c != 0)
                        {
                            compPt2 = intersectPts[c - 1];
                        }
                    }

                    if (c != i)
                    {
                        intersectPts[c] = compPt1;
                        polyLoc[c] = tempLoc1;
                    }
                }
            }

            // else if (numIntersectPts > 10)
            // {
            // TODO: write a quick-sort function to aid in time
            // haven't done this because it is rare to have
            // a large number of intersect pts for a small line segment
            // quick-sort performs poorly on small lists, that's why insertion
            // sort is used above.
            // }
        }

        /// <summary>
        /// Given a reference point to the line, and an array of points that
        /// lie along the line, this method sorts the array of points from the point
        /// closest to the reference pt to the pt farthest away.
        /// </summary>
        /// <param name="startPt">Point in line segment used as reference.</param>
        /// <param name="intersectPts">Array of points that lie on the same line as startPt.</param>
        private static void SortPointsArray(ref IPoint startPt, ref IPoint[] intersectPts)
        {
            double dist1;
            int numIntersectPts = intersectPts.Length;
            if (numIntersectPts == 2)
            {
                // if 0 or 1 the points don't need to be sorted
                // do a brute sort
                // just compare distances of each pt to the start pt.
                dist1 = PtDistance(ref startPt, ref intersectPts[0]);
                double dist2 = PtDistance(ref startPt, ref intersectPts[1]);
                if (dist1 > dist2)
                {
                    // need to swap locations
                    IPoint tempPt = intersectPts[0];
                    intersectPts[0] = intersectPts[1];
                    intersectPts[1] = tempPt;
                }
            }
            else if (numIntersectPts > 2 /*&& numintersectPts <= 10*/)
            {
                // use insertion sort for small arrays
                for (int i = 0; i <= numIntersectPts - 1; i++)
                {
                    IPoint compPt1 = intersectPts[i];
                    dist1 = PtDistance(ref startPt, ref compPt1);
                    int c = i;
                    IPoint compPt2 = c != 0 ? intersectPts[c - 1] : intersectPts[0];
                    while (c > 0 && PtDistance(ref startPt, ref compPt2) > dist1)
                    {
                        intersectPts[c] = intersectPts[c - 1];
                        c--;
                        if (c != 0)
                        {
                            compPt2 = intersectPts[c - 1];
                        }
                    }

                    if (c != i)
                    {
                        intersectPts[c] = compPt1;
                    }
                }
            }

            // else if (numIntersectPts > 10)
            // {
            // TODO: write a quick-sort function to aid in time
            // haven't done this because it is rare to have
            // a large number of intersect pts for a small line segment
            // quick-sort performs poorly on small lists, that's why insertion
            // sort is used above.
            // }
        }

        /// <summary>
        /// Splits original polygon into two portions depending on where line crosses it.
        /// </summary>
        /// <param name="line">The line the crosses the polygon. First and last points are intersects.</param>
        /// <param name="polygon">The polygon that is split by the line.</param>
        /// <param name="beginPolySeg">The section of the polygon where the first intersect point is found.</param>
        /// <param name="endPolySeg">The section of the polygon where the last intersect point is found.</param>
        /// <param name="poly1">First portion of polygon returned after splitting.</param>
        /// <param name="poly2">Second portion of polygon returned after splitting.</param>
        private static void SplitPolyInTwo(ref IFeature line, ref IFeature polygon, int beginPolySeg, int endPolySeg, out IFeature poly1, out IFeature poly2)
        {
            // function assumes first and last pts in line are the two intersection pts
            List<Coordinate> firstPartList = new List<Coordinate>();
            List<Coordinate> secondPartList = new List<Coordinate>();
            int numPolyPts = polygon.Geometry.NumPoints;
            int numLinePts = line.Geometry.NumPoints;
            int count;

            // now, see if we'll be crossing the zero pt while building the first result poly
            bool crossZeroPt = beginPolySeg < endPolySeg + 1;

            // split the poly into two portions
            // begin by creating the side where the line will be inserted in the forward direction
            // add all line pts in forward direction
            for (int i = 0; i <= numLinePts - 1; i++)
            {
                firstPartList.Add(line.Geometry.Coordinates[i]);
            }

            // add polygon pts that are clockwise of the ending line point
            if (crossZeroPt)
            {
                // we'll be crossing the zero point when creating a clockwise poly
                count = (numPolyPts - 1) - (endPolySeg + 1);

                // add all points before the zero point and clockwise of last point in line
                for (int i = 0; i <= count - 1; i++)
                {
                    int position = (endPolySeg + 1) + i;
                    firstPartList.Add(polygon.Geometry.Coordinates[position]);

                    // firstPart.Coordinates.Add(polygon.Coordinates[position]);
                }

                // add all points after the zero point and up to first line point
                for (int i = 0; i <= beginPolySeg; i++)
                {
                    firstPartList.Add(polygon.Geometry.Coordinates[i]);
                }
            }
            else
            {
                // we don't need to worry about crossing the zero point
                for (int i = endPolySeg + 1; i <= beginPolySeg; i++)
                {
                    firstPartList.Add(polygon.Geometry.Coordinates[i]);
                }
            }

            // add beginning line point to close the new polygon
            firstPartList.Add(line.Geometry.Coordinates[0]);
            IFeature firstPart = new Feature(FeatureType.Polygon, firstPartList);

            // create second portion by removing first from original polygon
            // secondPart = utils.ClipPolygon(MapWinGIS.PolygonOperation.DIFFERENCE_OPERATION, polygon, firstPart);
            // above method (difference) adds unnecessary points to the resulting shape, use below instead.
            // begin by creating the side where the line will be inserted in the forward direction
            // add line pts in reverse order
            for (int i = numLinePts - 1; i >= 0; i--)
            {
                secondPartList.Add(line.Geometry.Coordinates[i]);
            }

            // add polygon pts that are clockwise of the first line point
            // This may be confusing, but if crossZeroPt was true above, then it would
            // mean that the secondPart does not require crossing over the zero pt.
            // However, if crossZeroPt was false before, then secondPart will require
            // crossing the zeroPt while adding the polygon pts to the new shape.
            if (crossZeroPt == false)
            {
                // we'll be crossing the zero point when creating the second poly
                count = (numPolyPts - 1) - (beginPolySeg + 1);

                // add all points before the zero point and clockwise of first point in line
                for (int i = 0; i <= count - 1; i++)
                {
                    int position = (beginPolySeg + 1) + i;
                    secondPartList.Add(polygon.Geometry.Coordinates[position]);
                }

                // add all points after the zero point and up to last line point
                for (int i = 0; i <= endPolySeg; i++)
                {
                    secondPartList.Add(polygon.Geometry.Coordinates[i]);
                }
            }
            else
            {
                // we don't need to worry about crossing the zero point
                for (int i = beginPolySeg + 1; i <= endPolySeg; i++)
                {
                    secondPartList.Add(polygon.Geometry.Coordinates[i]);
                }
            }

            // add ending line point to close the new polygon
            secondPartList.Add(line.Geometry.Coordinates[numLinePts - 1]);
            IFeature secondPart = new Feature(FeatureType.Polygon, secondPartList);
            poly1 = firstPart;
            poly2 = secondPart;
        }

        /// <summary>
        /// Calculates the determinant of a 3X3 matrix, where the first two rows
        /// represent the x, y values of two lines, and the third row is (1 1 1).
        /// </summary>
        /// <param name="elem11">The first element of the first row in the matrix.</param>
        /// <param name="elem12">The second element of the first row in the matrix.</param>
        /// <param name="elem13">The third element of the first row in the matrix.</param>
        /// <param name="elem21">The first element of the second row in the matrix.</param>
        /// <param name="elem22">The second element of the second row in the matrix.</param>
        /// <param name="elem23">The third element of the second row in the matrix.</param>
        /// <returns>The determinant of the matrix.</returns>
        private static double TurboDeterm(double elem11, double elem12, double elem13, double elem21, double elem22, double elem23)
        {
            // The third row of the 3x3 matrix is (1, 1, 1)
            return elem11 * (elem22 - elem23) - elem12 * (elem21 - elem23) + elem13 * (elem21 - elem22);
        }

        #endregion
    }
}