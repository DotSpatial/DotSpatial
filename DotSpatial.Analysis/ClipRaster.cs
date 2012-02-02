// -----------------------------------------------------------------------
// <copyright file="ClipRaster.cs" company="DotSpatial Team">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.Topology;

namespace DotSpatial.Analysis
{
    /// <summary>
    /// Clip input raster with polygon.
    /// </summary>
    public static class ClipRaster
    {
        private static double GetXStart(IFeature polygon, IRaster input)
        {
            double rasterMinX = input.Extent.MinX;
            int sign;

            // Does the poly sit to the left of the raster or does the raster start before the left edge of the poly
            if (polygon.Envelope.Minimum.X < rasterMinX + (input.CellHeight / 2))
                sign = -1;
            else
                sign = 1;

            return FirstLineToProcess(polygon.Envelope.Minimum.X, rasterMinX, input.CellHeight, sign);
        }

        /// <summary>
        /// Clips a raster with a polygon feature
        /// </summary>
        /// <param name="inputFileName">The input raster file</param>
        /// <param name="polygon">The clipping polygon feature</param>
        /// <param name="outputFileName">The output raster file</param>
        /// <param name="cancelProgressHandler">Progress handler for reporting progress status and cancelling the operation</param>
        /// <returns>The output clipped raster object</returns>
        public static IRaster ClipRasterWithPolygon(string inputFileName, IFeature polygon, string outputFileName,
                                                    ICancelProgressHandler cancelProgressHandler = null)
        {
            return ClipRasterWithPolygon(polygon, Raster.Open(inputFileName), outputFileName, cancelProgressHandler);
        }

        /// <summary>
        /// Clips a raster with a polygon feature
        /// </summary>
        /// <param name="polygon">The clipping polygon feature</param>
        /// <param name="input">The input raster object</param>
        /// <param name="outputFileName">the output raster file name</param>
        /// <param name="cancelProgressHandler">Progress handler for reporting progress status and cancelling the operation</param>
        /// <remarks>We assume there is only one part in the polygon. Traverses the grid with a vertical scan line from left to right.</remarks>
        /// <returns></returns>
        public static IRaster ClipRasterWithPolygon(IFeature polygon, IRaster input, string outputFileName,
                                                    ICancelProgressHandler cancelProgressHandler = null)
        {
            if (!input.ContainsFeature(polygon))
                return input;

            if (cancelProgressHandler != null)
                cancelProgressHandler.Progress(null, 0, "Retrieving the borders.");

            List<Border> borders = GetBorders(polygon);

            if (cancelProgressHandler != null)
                cancelProgressHandler.Progress(null, 0, "Copying raster.");

            //create output raster
            IRaster output = Raster.CreateRaster(outputFileName, input.DriverCode, input.NumColumns, input.NumRows, 1,
                                                 input.DataType, new[] { string.Empty });
            output.Bounds = input.Bounds;
            if (input.CanReproject)
            {
                output.Projection = input.Projection;
            }

            // set all initial values of Output to NoData
            for (int i = 0; i < output.NumRows; i++)
            {
                for (int j = 0; j < output.NumColumns; j++)
                {
                    output.Value[i, j] = output.NoDataValue;
                }
            }

            // hack: the xCurrent plus one seems to do some magic
            double xCurrent = GetXStart(polygon, input) + 1;

            ProgressMeter pm = new ProgressMeter(cancelProgressHandler, "Clipping Raster", polygon.Envelope.Maximum.X);
            pm.StepPercent = 5;
            pm.StartValue = xCurrent;
            do
            {
                var intersections = GetYIntersections(borders, xCurrent);
                intersections.Sort();
                ParseIntersections(intersections, xCurrent, output, input);
                xCurrent += input.Bounds.CellWidth;

                // update progess meter
                pm.CurrentValue = xCurrent;

                // cancel if requested
                if (cancelProgressHandler != null && cancelProgressHandler.Cancel)
                    return null;
            } while (xCurrent <= polygon.Envelope.Maximum.X);

            output.Save();

            return output;
        }

        /// <summary>
        /// Parses the intersections. Moves from top bottom to mirror the index of rows in a raster.
        /// </summary>
        /// <param name="intersections">The intersections.</param>
        /// <param name="xCurrent">The x current.</param>
        /// <param name="output">The output.</param>
        /// <param name="input">The input.</param>
        private static void ParseIntersections(List<double> intersections, double xCurrent, IRaster output,
                                               IRaster input)
        {
            double yStart = 0;
            double yEnd;
            bool nextIntersectionIsEndPoint = false;
            for (int i = intersections.Count - 1; i >= 0; i--)
            {
                if (!nextIntersectionIsEndPoint)
                {
                    // should be the topmost intersection
                    yStart = intersections[i];
                    nextIntersectionIsEndPoint = true;
                }
                else
                {
                    // should be the intersection just below the topmost one.
                    yEnd = intersections[i];

                    // hack: the yCurrent minus one seems to do some magic
                    double yCurrent = FirstLineToProcess(yStart, output.Extent.MinY, output.Bounds.CellHeight, -1) - 1;
                    do
                    {
                        var pixel = output.ProjToCell(xCurrent, yCurrent);
                        output.Value[pixel.Row, pixel.Column] = input.Value[pixel.Row, pixel.Column];
                        yCurrent -= output.Bounds.CellHeight;
                    } while (yCurrent >= yEnd);
                    nextIntersectionIsEndPoint = false;
                }
            }
        }

        /// <summary>
        /// Gets the Y intersections.
        /// </summary>
        /// <param name="borders">The borders.</param>
        /// <param name="x">The line-scan x-value.</param>
        /// <returns></returns>
        private static List<double> GetYIntersections(IEnumerable<Border> borders, double x)
        {
            var intersections = new List<double>();

            foreach (Border border in borders)
            {
                var x1 = border.X1;
                var x2 = border.X2;

                // determine if the point lies inside of the border
                if (((x >= x1) && (x < x2)) || ((x >= x2) && (x < x1)))
                {
                    intersections.Add(border.M * x + border.Q);
                }
            }
            return intersections;
        }

        /// <summary>
        /// Gets the borders of the specified feature except vertical lines.
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <returns></returns>
        private static List<Border> GetBorders(IFeature feature)
        {
            List<Border> borders = new List<Border>();

            for (int i = 0; i < feature.Coordinates.Count - 1; i++)
            {
                Border border = new Border();
                border.X1 = feature.Coordinates[i].X;
                border.X2 = feature.Coordinates[i + 1].X;

                double y1 = feature.Coordinates[i].Y;
                double y2 = feature.Coordinates[i + 1].Y;
                border.M = (y2 - y1) / (border.X2 - border.X1);
                border.Q = y1 - (border.M * border.X1);

                // if a line is a vertical line, it should not be added to the list of borders.
                if (border.X1 != border.X2)
                    borders.Add(border);
            }
            return borders;
        }

        /// <summary>
        /// Assumes the cell is square.
        /// </summary>
        /// <param name="xyMinPolygon">The lowest left coordinate of the polygon.</param>
        /// <param name="xyMinRaster">The lowest left coordinate of the raster.</param>
        /// <param name="cellSize">Size of the cell.</param>
        /// <param name="sign">The factor.</param>
        /// <returns></returns>
        private static double FirstLineToProcess(double xyMinPolygon, double xyMinRaster, double cellSize, int sign)
        {
            double columnIndex = Math.Ceiling((xyMinRaster - xyMinPolygon) / cellSize);

            // we address the issue where the polygon is to the right of the raster by using the sign parameter.
            // double columnIndexIfStartOfPolyToRightOfRaster = Math.Ceiling((xyMinPolygon - xyMinRaster ) / cellSize);

            return xyMinRaster + (sign * columnIndex * cellSize);
        }
    }
}