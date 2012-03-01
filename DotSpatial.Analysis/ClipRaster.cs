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
            //double rasterMinX = input.Extent.MinX;
            double rasterMinXCenter = input.Xllcenter;

            // Does the poly sit to the left of the raster or does the raster start before the left edge of the poly
            if (polygon.Envelope.Minimum.X < rasterMinXCenter)
                return rasterMinXCenter;
            else
                return FirstColumnToProcess(polygon.Envelope.Minimum.X, rasterMinXCenter, input.CellHeight, 1);
        }

        private static int GetStartColumn(IFeature polygon, IRaster input)
        {
            //double rasterMinX = input.Extent.MinX;
            double rasterMinXCenter = input.Xllcenter;

            // Does the poly sit to the left of the raster or does the raster start before the left edge of the poly
            if (polygon.Envelope.Minimum.X < rasterMinXCenter)
                return 0;
            else
                return FirstColumnIndexToProcess(polygon.Envelope.Minimum.X, rasterMinXCenter, input.CellHeight, 1);
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
        /// <remarks>We assume there is only one part in the polygon. Traverses the raster with a vertical scan line from left to right.</remarks>
        /// <remarks>This method doesn't complete when the polygon is not completely within the raster.</remarks>
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
            output.NoDataValue = input.NoDataValue;
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

            double xStart = GetXStart(polygon, output);
            int columnStart = GetStartColumn(polygon, output); //get the index of first column
            double xCurrent = xStart;

            ProgressMeter pm = new ProgressMeter(cancelProgressHandler, "Clipping Raster", output.NumColumns);
            pm.StepPercent = 5;
            pm.StartValue = columnStart;

            int col = 0;
            for (int columnCurrent = columnStart; columnCurrent < output.NumColumns; columnCurrent++)
            {
                xCurrent = xStart + col * output.CellWidth;
                
                var intersections = GetYIntersections(borders, xCurrent);
                intersections.Sort();
                ParseIntersections(intersections, xCurrent, columnCurrent, output, input);              

                // update progess meter
                pm.CurrentValue = xCurrent;

                //update counter
                col++;

                // cancel if requested
                if (cancelProgressHandler != null && cancelProgressHandler.Cancel)
                    return null;
            } 

            output.Save();

            return output;
        }

        /// <summary>
        /// Parses the intersections. Moves bottom to top.
        /// </summary>
        /// <param name="intersections">The intersections.</param>
        /// <param name="xCurrent">The x current.</param>
        /// <param name="output">The output.</param>
        /// <param name="input">The input.</param>
        private static void ParseIntersections(List<double> intersections, double xCurrent, int column, IRaster output,
                                               IRaster input)
        {
            double yStart = 0;
            double yEnd;
            bool nextIntersectionIsEndPoint = false;
            for (int i = 0; i < intersections.Count; i++)
            {
                if (!nextIntersectionIsEndPoint)
                {
                    // should be the bottommost intersection
                    yStart = intersections[i];
                    nextIntersectionIsEndPoint = true;
                }
                else
                {
                    // should be the intersection just above the bottommost one.
                    yEnd = intersections[i];

                    //double yCurrent = FirstRowToProcess(yStart, output.Extent.MinY, output.Bounds.CellHeight, 1);
                    //int rowCurrent = output.ProjToCell(xCurrent, yStart).Row; ; //bottom-most row        

                    //double yEnd2 = FirstRowToProcess(yEnd, output.Extent.MinY, output.Bounds.CellHeight, 1);
                    //int rowEnd = output.ProjToCell(xCurrent, yEnd2).Row; //find row corresponding to end intersection

                    int rowCurrent = output.NumRows - RowIndexToProcess(yStart, output.Extent.MinY, output.CellHeight, 1);
                    //int rowEnd = RowIndexToProcess(yEnd, output.Extent.MinY, output.CellHeight, 1);
                    int rowEnd = rowCurrent - (int)(Math.Ceiling((yEnd - yStart) / output.CellHeight));

                    while (rowCurrent > rowEnd)
                    {
                        if (rowCurrent < 0 && rowEnd < 0) break;

                        if (rowCurrent >= 0 && rowCurrent < output.NumRows)
                        {
                            output.Value[rowCurrent, column] = input.Value[rowCurrent, column]; 
                        }
                        rowCurrent--;
                    }
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
        private static double FirstColumnToProcess(double xMinPolygon, double xMinRaster, double cellWidth, int sign)
        {
            double columnIndex = Math.Ceiling((xMinPolygon - xMinRaster) / cellWidth);

            // we address the issue where the polygon is to the right of the raster by using the sign parameter.
            // double columnIndexIfStartOfPolyToRightOfRaster = Math.Ceiling((xyMinPolygon - xyMinRaster ) / cellSize);

            return xMinRaster + (sign * columnIndex * cellWidth);
        }

        private static int FirstColumnIndexToProcess(double xMinPolygon, double xMinRaster, double cellWidth, int sign)
        {
            return (int)Math.Ceiling((xMinPolygon - xMinRaster) / cellWidth);
        }

        private static int RowIndexToProcess(double yMinPolygon, double yMinRaster, double cellHeight, int sign)
        {
            return (int)Math.Ceiling((yMinPolygon - yMinRaster) / cellHeight);
        }

        private static double FirstRowToProcess(double yMinPolygon, double yMinRaster, double cellHeight, int sign)
        {
            double rowIndex = Math.Ceiling((yMinPolygon - yMinRaster) / cellHeight);

            // we address the issue where the polygon is to the right of the raster by using the sign parameter.
            // double columnIndexIfStartOfPolyToRightOfRaster = Math.Ceiling((xyMinPolygon - xyMinRaster ) / cellSize);

            return yMinRaster + (sign * rowIndex * cellHeight);
        }
    }
}