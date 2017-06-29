// *******************************************************************************************************
// Product: DotSpatial.Analysis.ClipRaster.cs
// Description:  Class for clipping rasters with polygons
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// Dan Ames           |  3/2013            |  Adding this header.  
// *******************************************************************************************************

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
        /// <summary>
        /// Finds the X-coordinate of the first scanline
        /// </summary>
        /// <param name="polygon">The polygon</param>
        /// <param name="inputRaster">The input raster</param>
        /// <returns>the X-coordinate of the first scanline</returns>
        private static double GetXStart(IFeature polygon, IRaster inputRaster)
        {
            double rasterMinXCenter = inputRaster.Xllcenter;

            // Does the poly sit to the left of the raster or does the raster start before the left edge of the poly
            if (polygon.Envelope.Minimum.X < rasterMinXCenter)
                return rasterMinXCenter;
            else
                return FirstColumnToProcess(polygon.Envelope.Minimum.X, rasterMinXCenter, inputRaster.CellWidth);
        }

        /// <summary>
        /// Finds the first raster column corresponding to the left-most edge of the poly
        /// </summary>
        /// <param name="polygon">the polygon</param>
        /// <param name="inputRaster">the input raster</param>
        /// <returns>The raster column corresponding to the left-most edge of the poly 
        /// (if raster starts before left edge of the poly)
        /// or the first raster column (if raster starts after left edge of the poly)</returns>
        /// <remarks>If the poly sits to the left of the raster then the first column of the raster is returned.</remarks>
        private static int GetStartColumn(IFeature polygon, IRaster inputRaster)
        {
            double rasterMinXCenter = inputRaster.Xllcenter;

            // Does the poly sit to the left of the raster or does the raster start before the left edge of the poly
            if (polygon.Envelope.Minimum.X < rasterMinXCenter)
                return 0;
            else
                return ColumnIndexToProcess(polygon.Envelope.Minimum.X, rasterMinXCenter, inputRaster.CellWidth);
        }

        /// <summary>
        /// Finds the last raster column corresponding to the right-most edge of the poly
        /// </summary>
        /// <param name="polygon">the polygon</param>
        /// <param name="inputRaster">the input raster</param>
        /// <returns>The raster column corresponding to the right-most edge of the poly 
        /// (if raster ends after the right edge of the poly)
        /// or the last raster column (if raster ends before right edge of the poly)</returns>
        private static int GetEndColumn(IFeature polygon, IRaster inputRaster)
        {
            double rasterMaxXCenter = inputRaster.Extent.MaxX - inputRaster.CellWidth / 2;

            // Does the poly sit to the right of the raster or does the raster end after the right edge of the poly
            if (polygon.Envelope.Right() > rasterMaxXCenter)
                return inputRaster.NumColumns - 1;
            else
                return ColumnIndexToProcess(polygon.Envelope.Right(), rasterMaxXCenter, inputRaster.CellWidth);
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
        /// <remarks>We assume there is only one part in the polygon. 
        /// Traverses the raster with a vertical scan line from left to right, bottom to top</remarks>
        /// <returns></returns>
        public static IRaster ClipRasterWithPolygon(IFeature polygon, IRaster input, string outputFileName,
                                                    ICancelProgressHandler cancelProgressHandler = null)
        {
            //if the polygon is completely outside the raster
            if (!input.ContainsFeature(polygon))
                return input;

            if (cancelProgressHandler != null)
                cancelProgressHandler.Progress(null, 16, "Retrieving the borders.");

            List<Border> borders = GetBorders(polygon);

            if (cancelProgressHandler != null)
                cancelProgressHandler.Progress(null, 33, "Copying raster.");

            //create output raster
            IRaster output = Raster.CreateRaster(outputFileName, input.DriverCode, input.NumColumns, input.NumRows, 1,
                                                 input.DataType, new[] { string.Empty });
            output.Bounds = input.Bounds.Copy();
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
            pm.StartValue = 33;

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
        /// <param name="column">The column.</param>
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
                    // should be the bottom-most intersection
                    yStart = intersections[i];
                    nextIntersectionIsEndPoint = true;
                }
                else
                {
                    // should be the intersection just above the bottommost one.
                    yEnd = intersections[i];

                    int rowCurrent = output.NumRows - RowIndexToProcess(yStart, output.Extent.MinY, output.CellHeight);
                    int rowEnd = rowCurrent - (int)(Math.Ceiling((yEnd - yStart) / output.CellHeight));

                    //traverse from bottom to top between the two intersections
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
        /// Finds the x-coordinate of the first raster column to process
        /// </summary>
        /// <param name="xMinPolygon">The lowest left coordinate of the polygon.</param>
        /// <param name="xMinRaster">The lowest left coordinate of the raster.</param>
        /// <param name="cellWidth">Size of the cell.</param>
        private static double FirstColumnToProcess(double xMinPolygon, double xMinRaster, double cellWidth)
        {
            double columnIndex = Math.Ceiling((xMinPolygon - xMinRaster) / cellWidth);
            return xMinRaster + (columnIndex * cellWidth);
        }

        /// <summary>
        /// Finds the index of the first raster column to process
        /// </summary>
        /// <param name="xMinPolygon">The lowest left coordinate of the polygon.</param>
        /// <param name="xMinRaster">The lowest left coordinate of the raster.</param>
        /// <param name="cellWidth">Size of the cell.</param>
        private static int ColumnIndexToProcess(double xMinPolygon, double xMinRaster, double cellWidth)
        {
            return (int)Math.Ceiling((xMinPolygon - xMinRaster) / cellWidth);
        }

        /// <summary>
        /// Finds the index of the first raster row to process
        /// </summary>
        /// <param name="yMinPolygon">The lowest left coordinate of the polygon.</param>
        /// <param name="yMinRaster">The lowest left coordinate of the raster.</param>
        /// <param name="cellHeight">Size of the cell.</param>
        private static int RowIndexToProcess(double yMinPolygon, double yMinRaster, double cellHeight)
        {
            return (int)Math.Ceiling((yMinPolygon - yMinRaster) / cellHeight);
        }

        /// <summary>
        /// Finds the y-coordinate of the first raster row to process
        /// </summary>
        /// <param name="yMinPolygon">The lowest left coordinate of the polygon.</param>
        /// <param name="yMinRaster">The lowest left coordinate of the raster.</param>
        /// <param name="cellHeight">Size of the cell.</param>
        private static double FirstRowToProcess(double yMinPolygon, double yMinRaster, double cellHeight)
        {
            double rowIndex = Math.Ceiling((yMinPolygon - yMinRaster) / cellHeight);
            return yMinRaster + (rowIndex * cellHeight);
        }
    }
}