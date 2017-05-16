// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DotSpatial.Analysis;
using DotSpatial.Data;
using DotSpatial.Projections;
using GeoAPI.Geometries;

namespace DotSpatial.Plugins.Taudem
{
    /// <summary>
    /// The Hydrology algorithms are especially designed for working with DEMs in the context
    /// of modeling hydrological processes.
    /// </summary>
    public class Hydrology
    {
        /// <summary>
        /// DsNode
        /// </summary>
        public enum DsNode
        {
            /// <summary>
            /// Indicates an outlet.
            /// </summary>
            Outlet,

            /// <summary>
            /// Indicates an inlet.
            /// </summary>
            Inlet,

            /// <summary>
            /// Indicates a reservoir.
            /// </summary>
            Reservoir,

            /// <summary>
            /// Indicates a point source.
            /// </summary>
            PointSource
        }

        /// <summary>
        /// Enum for elevation units used in DEMs
        /// </summary>
        public enum ElevationUnits
        {
            /// <summary>
            /// Meters = 0
            /// </summary>
            Meters,

            /// <summary>
            /// Centimeters = 1
            /// </summary>
            Centimeters,

            /// <summary>
            /// Feet = 2
            /// </summary>
            Feet
        }

        #region Methods

        /// <summary>
        /// A function to apply attributes to a joined basin shapefile.
        /// </summary>
        /// <param name="joinBasinShapePath">Path of the featureset that gets edited.</param>
        /// <param name="elevUnits">Not used.</param>
        /// <param name="callback">The progress handler.</param>
        /// <returns>True, if run successfully.</returns>
        public static bool ApplyJoinBasinAreaAttributes(string joinBasinShapePath, ElevationUnits elevUnits, IProgressHandler callback)
        {
            callback?.Progress("Status", 0, "Calculating Merge Shed Area Attributes");

            int oldperc = 0;
            var mergeshed = FeatureSet.Open(joinBasinShapePath);

            int areamfieldnum = mergeshed.DataTable.Columns.Count;
            mergeshed.DataTable.Columns.Add("Area_M", typeof(double));

            int areaacfieldnum = mergeshed.DataTable.Columns.Count;
            mergeshed.DataTable.Columns.Add("Area_Acre", typeof(double));

            int areasqmifieldnum = mergeshed.DataTable.Columns.Count;
            mergeshed.DataTable.Columns.Add("Area_SqMi", typeof(double));

            var projection = mergeshed.Projection;
            for (int i = 0; i <= mergeshed.NumRows() - 1; i++)
            {
                if (mergeshed.NumRows() > 1)
                {
                    var newperc = Convert.ToInt32((Convert.ToDouble(i) / Convert.ToDouble(mergeshed.NumRows())) * 100);
                    if (newperc > oldperc)
                    {
                        callback?.Progress("Status", newperc, "Calculating Merge Shed Area Attributes");
                        oldperc = newperc;
                    }
                }

                IFeature currShape = mergeshed.GetShape(i);
                var currarea = Utils.AreaOfPart(currShape, 0);
                double aream = 0;
                if (projection != null)
                {
                    if (projection.Unit.Name == "Meter")
                    {
                        aream = currarea;
                    }
                    else if (projection.Unit.Name == "Foot")
                    {
                        aream = currarea * 0.09290304;
                    }
                }
                else
                {
                    aream = currarea;
                }

                var areaacre = aream * 0.000247105;
                var areasqmi = areaacre * 0.0015625;
                mergeshed.EditCellValue(areamfieldnum, i, aream);
                mergeshed.EditCellValue(areaacfieldnum, i, areaacre);
                mergeshed.EditCellValue(areasqmifieldnum, i, areasqmi);
            }

            mergeshed.Save();
            mergeshed.Close();

            callback?.Progress(string.Empty, 0, string.Empty);
            return true;
        }

        /// <summary>
        /// A function that updates the mean width, mean height, length, and slope in the given shape file.
        /// </summary>
        /// <param name="networkShapePath">The path to the streams network shapefile</param>
        /// <param name="basinShapePath">The path to the unjoined watershed shapefile</param>
        /// <param name="joinBasinShapePath">The path to the Joined Basins shapefile</param>
        /// <param name="callback">The progress handler.</param>
        /// <returns>True, if run successfully.</returns>
        public static bool ApplyJoinBasinStreamAttributes(string networkShapePath, string basinShapePath, string joinBasinShapePath, IProgressHandler callback)
        {
            callback?.Progress("Status", 0, "Calculating Merge Shed Area Attributes");

            var shedShapefile = FeatureSet.Open(basinShapePath);

            var streamLinkFieldNum = -1;
            for (var i = 0; i < shedShapefile.DataTable.Columns.Count; i++)
            {
                if (shedShapefile.DataTable.Columns[i].ColumnName == "StreamLink")
                {
                    streamLinkFieldNum = i;
                    break;
                }
            }

            var netShapefile = FeatureSet.Open(networkShapePath);

            var streamIdfn = -1;
            var streamLengthFn = -1;
            var streamSlopeFn = -1;
            var streamMeanWidthFn = -1;
            var streamMeanDepthFn = -1;
            for (var i = 0; i < netShapefile.DataTable.Columns.Count; i++)
            {
                switch (netShapefile.DataTable.Columns[i].ColumnName)
                {
                    case "LINKNO":
                        streamIdfn = i;
                        break;
                    case "Length":
                        streamLengthFn = i;
                        break;
                    case "Slope":
                        streamSlopeFn = i;
                        break;
                    case "MeanWidth":
                        streamMeanWidthFn = i;
                        break;
                    case "MeanDepth":
                        streamMeanDepthFn = i;
                        break;
                }
            }

            var mergeshedShapefile = FeatureSet.Open(joinBasinShapePath);

            var linkIDsFieldNum = -1;
            for (var i = 0; i < mergeshedShapefile.DataTable.Columns.Count; i++)
            {
                if (mergeshedShapefile.DataTable.Columns[i].ColumnName == "LinkIDs")
                {
                    linkIDsFieldNum = i;
                    break;
                }
            }

            // mergeshedShapefile.StartEditingShapes(true, null);
            var streamwidthfieldnum = AddField(mergeshedShapefile, "CH_W2", typeof(double));
            var streamdepthfieldnum = AddField(mergeshedShapefile, "CH_D", typeof(double));
            var streamlengthfieldnum = AddField(mergeshedShapefile, "CH_L", typeof(double));
            var streamslopefieldnum = AddField(mergeshedShapefile, "CH_S", typeof(double));

            var oldperc = 0;
            for (var i = 0; i < mergeshedShapefile.NumRows(); i++)
            {
                if (mergeshedShapefile.NumRows() > 1)
                {
                    var newperc = Convert.ToInt32((Convert.ToDouble(i) / Convert.ToDouble(mergeshedShapefile.NumRows())) * 100);
                    if (newperc > oldperc)
                    {
                        callback?.Progress("Status", newperc, "Calculating Merge Shed Area Attributes");
                        oldperc = newperc;
                    }
                }

                var currLinkIDs = mergeshedShapefile.GetCellValue(linkIDsFieldNum, i).ToString();
                var links = currLinkIDs.Split(',');
                var shedId = int.Parse(links[0]);
                var shedIndex = GetBasinIndexById(shedShapefile, shedId);
                var streamLink = shedShapefile.GetCellValue(streamLinkFieldNum, shedIndex).ToString();

                for (var j = 0; j < netShapefile.NumRows(); j++)
                {
                    if (netShapefile.GetCellValue(streamIdfn, j).ToString() != streamLink)
                    {
                        continue;
                    }

                    mergeshedShapefile.EditCellValue(streamlengthfieldnum, i, netShapefile.GetCellValue(streamLengthFn, j));
                    mergeshedShapefile.EditCellValue(streamslopefieldnum, i, netShapefile.GetCellValue(streamSlopeFn, j));
                    mergeshedShapefile.EditCellValue(streamdepthfieldnum, i, netShapefile.GetCellValue(streamMeanDepthFn, j));
                    mergeshedShapefile.EditCellValue(streamwidthfieldnum, i, netShapefile.GetCellValue(streamMeanWidthFn, j));
                }
            }

            mergeshedShapefile.Save();
            mergeshedShapefile.Close();

            netShapefile.Close();
            shedShapefile.Close();

            callback?.Progress(string.Empty, 0, string.Empty);

            return true;
        }

        /// <summary>
        /// Hydrology function used to add to the stream shapefile attributes.
        /// </summary>
        /// <param name="streamNetworkShapePath">Path of the stream network file.</param>
        /// <param name="demPath">Path of the dem file.</param>
        /// <param name="subBasinShapePath">Path of the sub basin shapefile.</param>
        /// <param name="elevUnits">The elevation units.</param>
        /// <param name="callback">The progress handler.</param>
        /// <returns>True, if run successfully.</returns>
        public static bool ApplyStreamAttributes(string streamNetworkShapePath, string demPath, string subBasinShapePath, ElevationUnits elevUnits, IProgressHandler callback)
        {
            int sindx;
            int oldperc = 0;
            const int IdField = 0;
            const int DsField = 1;
            const int Us1Field = 2;
            const int Us2Field = 3;
            const int DsAreaField = 8;
            const int SlopeField = 10;
            const int UsAreaField = 12;

            callback?.Progress("Status", 0, "Calculating Stream Parameters");

            if (!File.Exists(streamNetworkShapePath))
            {
                MessageBox.Show(string.Format(MessageStrings.FileDoesNotExists, streamNetworkShapePath));
                return false;
            }

            IFeatureSet streamShape = FeatureSet.Open(streamNetworkShapePath);
            if (streamShape == null)
            {
                throw new ApplicationException(string.Format(MessageStrings.ErrorInOpening, streamNetworkShapePath));
            }

            int streamShapeNumShapes = streamShape.NumRows();

            // Add some fields:
            int lowFieldNum = AddField(streamShape, "ElevLow", typeof(double));
            int highFieldNum = AddField(streamShape, "Elevhigh", typeof(double));
            int mwidthFieldNum = AddField(streamShape, "MeanWidth", typeof(double));
            int mdepthFieldNum = AddField(streamShape, "MeanDepth", typeof(double));
            int dsareaAcreFieldNum = AddField(streamShape, "DSAreaAcre", typeof(double));
            int dsareaSqMiFieldNum = AddField(streamShape, "USAreaAcre", typeof(double));
            int usareaAcreFieldNum = AddField(streamShape, "DSAreaSqMi", typeof(double));
            int usareaSqMiFieldNum = AddField(streamShape, "USAreaSqMi", typeof(double));

            ProjectionInfo projStr = streamShape.Projection;
            IRaster demGrid = Raster.Open(demPath);
            IFeatureSet shedShape = FeatureSet.Open(subBasinShapePath);
            int shedShapeNumShapes = shedShape.NumRows();

            for (sindx = 0; sindx < streamShapeNumShapes; sindx++)
            {
                if (callback != null && streamShapeNumShapes > 1)
                {
                    int newperc = Convert.ToInt32((Convert.ToDouble(sindx) / Convert.ToDouble(shedShapeNumShapes)) * 100);
                    if (newperc > oldperc)
                    {
                        callback.Progress("Status", newperc, "Calculating Stream Parameters");
                        oldperc = newperc;
                    }
                }

                int originalStreamId = Convert.ToInt32(streamShape.GetCellValue(IdField, sindx));

                double elevlow;
                double elevhigh;
                GetStreamElevationPoints(sindx, streamShape, demGrid, out elevlow, out elevhigh);

                switch (elevUnits)
                {
                    case ElevationUnits.Centimeters:
                        elevlow /= 100;
                        elevhigh /= 100;
                        break;
                    case ElevationUnits.Feet:
                        elevlow /= 3.280839895;
                        elevhigh /= 3.280839895;
                        break;
                }

                streamShape.EditCellValue(lowFieldNum, sindx, elevlow);
                streamShape.EditCellValue(highFieldNum, sindx, elevhigh);

                // Assume the first field in the watershed shape file is an ID field that corresponds to the IDs in streamShape IDField
                // GridToShapeManhattan creates this field for watershed shapes
                const int WShedIdField = 0;
                for (int shdindx = 0; shdindx < shedShapeNumShapes; shdindx++)
                {
                    if (Convert.ToInt32(shedShape.GetCellValue(WShedIdField, shdindx)) == originalStreamId)
                    {
                        var currShp = shedShape.GetShape(shdindx);
                        double currArea = Utils.AreaOfPart(currShp, 0);
                        double meanWidth = 1.29 * Math.Pow(currArea / 1000000, 0.6);
                        double meanDepth = 0.13 * Math.Pow(currArea / 1000000, 0.4);
                        streamShape.EditCellValue(mwidthFieldNum, sindx, meanWidth);
                        streamShape.EditCellValue(mdepthFieldNum, sindx, meanDepth);
                        break;
                    }
                }

                // Change shape ID (and down/upstream IDs) from zero-based to one-based
                streamShape.EditCellValue(IdField, sindx, originalStreamId + 1);

                int tmpId = Convert.ToInt32(streamShape.GetCellValue(DsField, sindx));
                if (tmpId > -1)
                {
                    tmpId++;
                }

                streamShape.EditCellValue(DsField, sindx, tmpId);

                tmpId = Convert.ToInt32(streamShape.GetCellValue(Us1Field, sindx));
                if (tmpId > 0)
                {
                    tmpId++;
                }
                else
                {
                    tmpId = -1;
                }

                streamShape.EditCellValue(Us1Field, sindx, tmpId);

                tmpId = Convert.ToInt32(streamShape.GetCellValue(Us2Field, sindx));
                if (tmpId > 0)
                {
                    tmpId++;
                }
                else
                {
                    tmpId = -1;
                }

                streamShape.EditCellValue(Us2Field, sindx, tmpId);

                double tmpSlope = Convert.ToDouble(streamShape.GetCellValue(SlopeField, sindx));
                double tmpDsArea = Convert.ToDouble(streamShape.GetCellValue(DsAreaField, sindx));
                double tmpUsArea = Convert.ToDouble(streamShape.GetCellValue(UsAreaField, sindx));

                if (projStr != null)
                {
                    if (projStr.Unit.Name == "Meter")
                    {
                        double dsAreaAcre = tmpDsArea * 0.000247105;
                        double dsAreaSqMi = dsAreaAcre * 0.0015625;
                        double usAreaAcre = tmpUsArea * 0.000247105;
                        double usAreaSqMi = usAreaAcre * 0.0015625;
                        streamShape.EditCellValue(dsareaAcreFieldNum, sindx, dsAreaAcre);
                        streamShape.EditCellValue(dsareaSqMiFieldNum, sindx, dsAreaSqMi);
                        streamShape.EditCellValue(usareaAcreFieldNum, sindx, usAreaAcre);
                        streamShape.EditCellValue(usareaSqMiFieldNum, sindx, usAreaSqMi);
                        switch (elevUnits)
                        {
                            case ElevationUnits.Meters:
                                tmpSlope = tmpSlope * 100;
                                break;
                            case ElevationUnits.Centimeters:
                                break;
                            case ElevationUnits.Feet:
                                tmpSlope = (tmpSlope / 3.280839895) * 100;
                                break;
                        }

                        streamShape.EditCellValue(SlopeField, sindx, tmpSlope);
                    }
                    else if (projStr.Unit.Name == "Foot")
                    {
                        double dsAreaAcre = tmpDsArea * 2.2957E-05;
                        double dsAreaSqMi = dsAreaAcre * 0.0015625;
                        double usAreaAcre = tmpUsArea * 2.2957E-05;
                        double usAreaSqMi = usAreaAcre * 0.0015625;
                        streamShape.EditCellValue(dsareaAcreFieldNum, sindx, dsAreaAcre);
                        streamShape.EditCellValue(dsareaSqMiFieldNum, sindx, dsAreaSqMi);
                        streamShape.EditCellValue(usareaAcreFieldNum, sindx, usAreaAcre);
                        streamShape.EditCellValue(usareaSqMiFieldNum, sindx, usAreaSqMi);
                        switch (elevUnits)
                        {
                            case ElevationUnits.Meters:
                                tmpSlope = (tmpSlope * 3.280839895) * 100;
                                break;
                            case ElevationUnits.Centimeters:
                                tmpSlope = (tmpSlope / 30.48) * 100;
                                break;
                            case ElevationUnits.Feet:
                                tmpSlope = tmpSlope * 100;
                                break;
                        }

                        streamShape.EditCellValue(SlopeField, sindx, tmpSlope);
                    }
                }
                else
                {
                    double dSAreaAcre = tmpDsArea * 0.000247105;
                    double dsAreaSqMi = dSAreaAcre * 0.0015625;
                    double uSAreaAcre = tmpUsArea * 0.000247105;
                    double uSAreaSqMi = uSAreaAcre * 0.0015625;
                    streamShape.EditCellValue(dsareaAcreFieldNum, sindx, dSAreaAcre);
                    streamShape.EditCellValue(dsareaSqMiFieldNum, sindx, dsAreaSqMi);
                    streamShape.EditCellValue(usareaAcreFieldNum, sindx, uSAreaAcre);
                    streamShape.EditCellValue(usareaSqMiFieldNum, sindx, uSAreaSqMi);
                    switch (elevUnits)
                    {
                        case ElevationUnits.Meters:
                            tmpSlope = tmpSlope * 100;
                            break;
                        case ElevationUnits.Centimeters:
                            break;
                        case ElevationUnits.Feet:
                            tmpSlope = (tmpSlope / 3.280839895) * 100;
                            break;
                    }

                    streamShape.EditCellValue(SlopeField, sindx, tmpSlope);
                }
            }

            shedShape.Close();
            demGrid.Close();

            streamShape.Save();
            streamShape.Close();

            callback?.Progress("Status", 0, string.Empty);

            return true;
        }

        /// <summary>
        /// A function to apply area attributes to a watershed polygon shapefile
        /// </summary>
        /// <param name="subBasinShapePath">Subbasin shapefile</param>
        /// <param name="callback">Callback object</param>
        /// <returns>True on success</returns>
        public static bool ApplyWatershedAreaAttributes(string subBasinShapePath, IProgressHandler callback)
        {
            callback?.Progress("Status", 0, "Calculating WS Area Parameters");

            var shedShape = FeatureSet.Open(subBasinShapePath);

            var areaMFieldNum = AddField(shedShape, "Area_M", typeof(double));
            var areaAcreFieldNum = AddField(shedShape, "Area_Acre", typeof(double));
            var areaMileFieldNum = AddField(shedShape, "Area_SqMi", typeof(double));

            double areaM = 0;
            var oldperc = 0;

            for (var sindx = 0; sindx < shedShape.NumRows(); sindx++)
            {
                if (shedShape.NumRows() > 1)
                {
                    var newperc = Convert.ToInt32((Convert.ToDouble(sindx) / Convert.ToDouble(shedShape.NumRows())) * 100);
                    if (newperc > oldperc)
                    {
                        callback?.Progress("Status", newperc, "Calculating WS Area Parameters");

                        oldperc = newperc;
                    }
                }

                var tmpShp = shedShape.GetShape(sindx);

                var tmpArea = Utils.AreaOfPart(tmpShp, 0);
                if (!string.IsNullOrWhiteSpace(shedShape.Projection?.ToString()))
                {
                    if (shedShape.Projection.Unit.Name == "Meter")
                    {
                        areaM = tmpArea;
                    }
                    else if (shedShape.Projection.Unit.Name == "Foot")
                    {
                        areaM = tmpArea * 0.09290304;
                    }
                }
                else
                {
                    areaM = tmpArea;
                }

                var areaAcre = areaM * 0.000247105;
                var areaSqMi = areaAcre * 0.0015625;
                shedShape.EditCellValue(areaMFieldNum, sindx, areaM);
                shedShape.EditCellValue(areaAcreFieldNum, sindx, areaAcre);
                shedShape.EditCellValue(areaMileFieldNum, sindx, areaSqMi);
            }

            shedShape.Save();
            shedShape.Close();

            callback?.Progress(string.Empty, 0, string.Empty);

            return true;
        }

        /// <summary>
        /// Hydrology function used to add to the subbasin shapefile average elevation attribute
        /// </summary>
        /// <param name="subBasinShapePath">Subbasin shapefile</param>
        /// <param name="elevGridPath">Path of the raster.</param>
        /// <param name="callback">The progress handler.</param>
        /// <returns>True, if run successfully.</returns>
        public static bool ApplyWatershedElevationAttribute(string subBasinShapePath, string elevGridPath, IProgressHandler callback)
        {
            int oldperc = 0;

            // CWG 23/1/2011 changed to GeoTiff for Taudem V5
            string tmpClipPath = Path.GetDirectoryName(elevGridPath) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(elevGridPath) + "_clip.tif";

            callback?.Progress("Status", 0, "Calculating WS Elevation Parameters");
            var shedShape = FeatureSet.Open(subBasinShapePath);
            var elevGrid = Raster.Open(elevGridPath);

            var countElev = new int[shedShape.NumRows()];
            var sumElev = new double[shedShape.NumRows()];
            var avgElev = new double[shedShape.NumRows()];

            DataManagement.DeleteGrid(tmpClipPath);
            for (int sindx = 0; sindx <= shedShape.NumRows() - 1; sindx++)
            {
                if (shedShape.NumRows() > 1)
                {
                    int newperc = Convert.ToInt32((Convert.ToDouble(sindx) / Convert.ToDouble(shedShape.NumRows() - 1)) * 100);
                    if (newperc > oldperc)
                    {
                        callback?.Progress("Status", newperc, "Calculating WS Elevation Parameters");
                        oldperc = newperc;
                    }
                }

                var tmpPoly = shedShape.GetShape(sindx);
                IRaster tmpClipGrid = ClipRaster.ClipRasterWithPolygon(elevGridPath, tmpPoly, tmpClipPath);
                if (tmpClipGrid != null)
                {
                    var nr = tmpClipGrid.NumRows;
                    var nc = tmpClipGrid.NumColumns;
                    var nodataVal = tmpClipGrid.NoDataValue;
                    countElev[sindx] = 0;
                    sumElev[sindx] = 0;
                    avgElev[sindx] = 0;
                    int row;
                    for (row = 0; row <= nr - 1; row += 2)
                    {
                        int col;
                        for (col = 0; col <= nc - 1; col += 2)
                        {
                            var currVal = tmpClipGrid.Value[row, col];
                            if (currVal != nodataVal)
                            {
                                countElev[sindx] = countElev[sindx] + 1;
                                sumElev[sindx] = sumElev[sindx] + currVal;
                            }
                        }
                    }

                    tmpClipGrid.Close();
                    DataManagement.DeleteGrid(tmpClipPath);
                }
            }

            callback?.Progress("Status", 0, "Calculating WS Elevation Parameters");

            int slopeFieldNum = AddField(shedShape, "AveElev", typeof(double));
            string slopeProj = elevGrid.Projection.ToString();
            oldperc = 0;

            for (int sindx = 0; sindx <= shedShape.NumRows() - 1; sindx++)
            {
                if (shedShape.NumRows() > 1)
                {
                    int newperc = Convert.ToInt32((Convert.ToDouble(sindx) / Convert.ToDouble(shedShape.NumRows())) * 100);
                    if (newperc > oldperc)
                    {
                        callback?.Progress("Status", newperc, "Calculating WS Elevation Parameters");
                        oldperc = newperc;
                    }
                }

                if (countElev[sindx] > 0)
                {
                    avgElev[sindx] = sumElev[sindx] / countElev[sindx];

                    shedShape.EditCellValue(slopeFieldNum, sindx, avgElev[sindx]);
                }
            }

            shedShape.Save();
            shedShape.Close();
            elevGrid.Close();

            callback?.Progress(string.Empty, 0, string.Empty);
            return true;
        }

        /// <summary>
        /// Hydrology function used to add to the subbasin shapefile average elevation attribute.
        /// </summary>
        /// <param name="subBasinGridPath">Path of the sub basin grid file.</param>
        /// <param name="subBasinShapePath">Subbasin shapefile</param>
        /// <param name="elevGridPath">Path of the elevation grid file.</param>
        /// <param name="callback">The progress handler.</param>
        /// <returns>True on success</returns>
        public static bool ApplyWatershedElevationAttribute(string subBasinGridPath, string subBasinShapePath, string elevGridPath, IProgressHandler callback)
        {
            callback?.Progress("Status", 0, "Calculating WS Elevation Parameters");

            var shedShape = FeatureSet.Open(subBasinShapePath);
            var countElev = new int[shedShape.NumRows()];
            var sumElev = new double[shedShape.NumRows()];
            var avgElev = new double[shedShape.NumRows()];

            var currPolyIdIdx = -1;
            var currLinkIdIdx = -1;
            for (var sindx = 0; sindx < shedShape.DataTable.Columns.Count; sindx++)
            {
                if (shedShape.DataTable.Columns[sindx].ColumnName == "PolygonID" || shedShape.DataTable.Columns[sindx].ColumnName == "MWShapeID")
                {
                    currPolyIdIdx = sindx;
                }

                if (shedShape.DataTable.Columns[sindx].ColumnName == "LinkIDs")
                {
                    currLinkIdIdx = sindx;
                }

                // Paul Meems, 24-Aug-2011: Added:
                if (currPolyIdIdx != -1 && currLinkIdIdx != -1)
                {
                    // Found the values so stop searching:
                    break;
                }
            }

            var linkIdVals = new List<int>();
            var linkIdMerged = new List<int>();
            if (currLinkIdIdx != -1 && currPolyIdIdx != -1)
            {
                for (var sindx = 0; sindx < shedShape.NumRows(); sindx++)
                {
                    var tmpLinkIDs = shedShape.GetCellValue(currLinkIdIdx, sindx).ToString();
                    var tmpLinks = tmpLinkIDs.Split(',');
                    foreach (var tmpLink in tmpLinks)
                    {
                        linkIdMerged.Add(sindx);
                        linkIdVals.Add(int.Parse(tmpLink.Trim()));
                    }
                }
            }
            else
            {
                for (var sindx = 0; sindx < shedShape.NumRows(); sindx++)
                {
                    linkIdMerged.Add(sindx);
                    linkIdVals.Add(int.Parse(shedShape.GetCellValue(currPolyIdIdx, sindx).ToString()));
                }
            }

            var subBasinGrid = Raster.Open(subBasinGridPath);

            var elevGrid = Raster.Open(elevGridPath);
            var nodataVal = elevGrid.NoDataValue;
            var numberRows = elevGrid.NumRows;
            var numberCols = elevGrid.NumColumns;

            var oldperc = -1;
            for (var row = 0; row < numberRows; row++)
            {
                var newperc = Convert.ToInt32((Convert.ToDouble(row) / Convert.ToDouble(numberRows - 1)) * 100);
                if (newperc > oldperc)
                {
                    callback?.Progress("Status", newperc, "Calculating WS Elevation Parameters");

                    oldperc = newperc;
                }

                for (var col = 0; col < numberCols; col++)
                {
                    var currVal = double.Parse(elevGrid.Rows[row][col].ToString());

                    if (currVal == nodataVal)
                    {
                        continue;
                    }

                    var currBasinId = int.Parse(subBasinGrid.Rows[row][col].ToString());

                    // Paul Meems, 24-Aug-2011: Changed:
                    // if (currBasinID != -1)
                    // TODO: Check if the result is still the same
                    if (currBasinId > -1)
                    {
                        var tmp = linkIdVals.IndexOf(currBasinId);
                        if (tmp > -1)
                        {
                            var currId = linkIdMerged[tmp];
                            countElev[currId] = countElev[currId] + 1;
                            sumElev[currId] = sumElev[currId] + currVal;
                        }
                    }
                }
            }

            elevGrid.Close();
            subBasinGrid.Close();
            callback?.Progress("Status", 0, "Calculating WS Elevation Parameters");

            var slopeFieldNum = AddField(shedShape, "AveElev", typeof(double));
            oldperc = 0;

            for (var sindx = 0; sindx < shedShape.NumRows(); sindx++)
            {
                if (shedShape.NumRows() > 1)
                {
                    var newperc = Convert.ToInt32((Convert.ToDouble(sindx) / Convert.ToDouble(shedShape.NumRows())) * 100);
                    if (newperc > oldperc)
                    {
                        callback?.Progress("Status", newperc, "Calculating WS Elevation Parameters");
                        oldperc = newperc;
                    }
                }

                if (countElev[sindx] <= 0)
                {
                    continue;
                }

                avgElev[sindx] = sumElev[sindx] / countElev[sindx];
                shedShape.EditCellValue(slopeFieldNum, sindx, avgElev[sindx]);
            }

            shedShape.Save();
            shedShape.Close();
            callback?.Progress(string.Empty, 0, string.Empty);
            return true;
        }

        /// <summary>
        /// Hydrology function to apply the watershed link attributes copied or interpreted from the stream network.
        /// </summary>
        /// <param name="subBasinShapePath">Subbasin shapefile</param>
        /// <param name="streamNetworkShapePath">Path of the stream network shapefile.</param>
        /// <param name="callback">The progress handler</param>
        /// <returns>0 on success</returns>
        public static int ApplyWatershedLinkAttributes(string subBasinShapePath, string streamNetworkShapePath, IProgressHandler callback)
        {
            int shedIndex;

            callback?.Progress("Status", 0, "Assigning WS Link");

            // Stream fields
            const int IdField = 0;
            const int DsidField = 1;
            const int Us1IdField = 2;
            const int Us2IdField = 3;
            const int DsNodeIdField = 4;
            const int StreamLenId = 6;

            IFeatureSet streamShape = FeatureSet.Open(streamNetworkShapePath);
            if (streamShape == null)
            {
                throw new ApplicationException(string.Format(MessageStrings.ErrorInOpening, streamNetworkShapePath));
            }

            int streamShapeNumShapes = streamShape.NumRows();

            IFeatureSet shedShape = FeatureSet.Open(subBasinShapePath);
            if (shedShape == null)
            {
                throw new ApplicationException(string.Format(MessageStrings.ErrorInOpening, subBasinShapePath));
            }

            int shedShapeNumShapes = shedShape.NumRows();

            const int WShedIdField = 0;
            int streamLinkFieldNum = AddField(shedShape, "StreamLinkNo", typeof(int));
            int streamLenFieldNum = AddField(shedShape, "StreamLen", typeof(int));
            int dsnodeidFieldNum = AddField(shedShape, "DSNodeID", typeof(int));
            int dslinkFieldNum = AddField(shedShape, "DSWSID", typeof(int));
            int us1LinkFieldNum = AddField(shedShape, "US1WSID", typeof(int));
            int us2LinkFieldNum = AddField(shedShape, "US2WSID", typeof(int));

            int oldperc = 0;

            for (shedIndex = 0; shedIndex < shedShapeNumShapes; shedIndex++)
            {
                if (callback != null && shedShapeNumShapes > 1)
                {
                    var newperc = Convert.ToInt32((Convert.ToDouble(shedIndex) / Convert.ToDouble(shedShapeNumShapes)) * 100);
                    if (newperc > oldperc)
                    {
                        callback.Progress("Status", newperc, "Assigning WS Link");
                        oldperc = newperc;
                    }
                }

                // Change watershed ID from zero-based to one-based to match change to stream ID in ApplyStreamAttributes
                int watershedId = Convert.ToInt32(shedShape.GetCellValue(WShedIdField, shedIndex)) + 1;
                shedShape.EditCellValue(WShedIdField, shedIndex, watershedId);
                for (int streamIndex = 0; streamIndex < streamShapeNumShapes; streamIndex++)
                {
                    // Stream IDs have already been incremented to start at 1 in ApplyStreamAttributes, so match with incremented Watershed ID
                    if (Convert.ToInt32(streamShape.GetCellValue(IdField, streamIndex)) == watershedId)
                    {
                        shedShape.EditCellValue(dsnodeidFieldNum, shedIndex, streamShape.GetCellValue(DsNodeIdField, streamIndex));
                        shedShape.EditCellValue(streamLinkFieldNum, shedIndex, streamShape.GetCellValue(IdField, streamIndex));
                        shedShape.EditCellValue(streamLenFieldNum, shedIndex, streamShape.GetCellValue(StreamLenId, streamIndex));
                        int currDsStreamLink = Convert.ToInt32(streamShape.GetCellValue(DsidField, streamIndex));
                        int currUs1StreamLink = Convert.ToInt32(streamShape.GetCellValue(Us1IdField, streamIndex));
                        int currUs2StreamLink = Convert.ToInt32(streamShape.GetCellValue(Us2IdField, streamIndex));

                        int dSwsid = currDsStreamLink == -1 ? -1 : currDsStreamLink;
                        int uS1Wsid = currUs1StreamLink <= 0 ? -1 : currUs1StreamLink;
                        int us2Wsid = currUs2StreamLink <= 0 ? -1 : currUs2StreamLink;

                        shedShape.EditCellValue(dslinkFieldNum, shedIndex, dSwsid);
                        shedShape.EditCellValue(us1LinkFieldNum, shedIndex, uS1Wsid);
                        shedShape.EditCellValue(us2LinkFieldNum, shedIndex, us2Wsid);
                        break;
                    }
                }
            }

            shedShape.Save();
            shedShape.Close();
            streamShape.Close();

            callback?.Progress(string.Empty, 0, string.Empty);

            return 0;
        }

        /// <summary>
        /// Hydrology function used to add to the subbasin shapefile average slope attribute.
        /// </summary>
        /// <param name="subBasinShapePath">Path of the sub basin shapefile.</param>
        /// <param name="slopeGridPath">Path of the slope grid.</param>
        /// <param name="elevUnits">The elevation units.</param>
        /// <param name="callback">The progress handler</param>
        /// <returns>True, if run successfully.</returns>
        public static bool ApplyWatershedSlopeAttribute(string subBasinShapePath, string slopeGridPath, ElevationUnits elevUnits, IProgressHandler callback)
        {
            var path = Path.GetDirectoryName(slopeGridPath);
            if (path == null) return false;

            // CWG 23/1/2011 changed to GeoTiff for Taudem V5
            var tmpClipPath = Path.Combine(path, Path.GetFileNameWithoutExtension(slopeGridPath) + "_clip.tif");

            // DataManagement.DeleteGrid(tmpClipPath);
            callback?.Progress("Status", 0, "Calculating WS Slope Parameters");

            var shedShape = FeatureSet.Open(subBasinShapePath);

            var slopeGrid = Raster.Open(slopeGridPath);
            var slopeProj = slopeGrid.Projection;
            slopeGrid.Close();

            var countSlope = new int[shedShape.NumRows()];
            var sumSlope = new double[shedShape.NumRows()];
            var avgSlope = new double[shedShape.NumRows()];

            var oldperc = 0;
            for (var sindx = 0; sindx < shedShape.NumRows(); sindx++)
            {
                if (shedShape.NumRows() > 1)
                {
                    var newperc = Convert.ToInt32((Convert.ToDouble(sindx) / Convert.ToDouble(shedShape.NumRows() - 1)) * 100);
                    if (newperc > oldperc)
                    {
                        callback?.Progress("Status", newperc, "Calculating WS Slope Parameters");

                        oldperc = newperc;
                    }
                }

                var tmpPoly = shedShape.GetShape(sindx);

                if (ClipRaster.ClipRasterWithPolygon(slopeGridPath, tmpPoly, tmpClipPath) != null)
                {
                    continue;
                }

                var tmpClipGrid = Raster.Open(tmpClipPath);

                var numberRows = tmpClipGrid.NumRows;
                var numberCols = tmpClipGrid.NumColumns;
                var nodataVal = tmpClipGrid.NoDataValue;
                countSlope[sindx] = 0;
                sumSlope[sindx] = 0;
                avgSlope[sindx] = 0;
                int row;
                for (row = 0; row < numberRows; row += 2)
                {
                    int col;
                    for (col = 0; col < numberCols; col += 2)
                    {
                        var currVal = double.Parse(tmpClipGrid.Rows[col][row].ToString());
                        if (currVal == nodataVal)
                        {
                            continue;
                        }

                        countSlope[sindx] = countSlope[sindx] + 1;
                        sumSlope[sindx] = sumSlope[sindx] + currVal;
                    }
                }

                tmpClipGrid.Close();
            }

            callback?.Progress("Status", 0, "Calculating WS Slope Parameters");

            var slopeFieldNum = AddField(shedShape, "AveSlope", typeof(double));
            oldperc = 0;

            for (var sindx = 0; sindx < shedShape.NumRows(); sindx++)
            {
                if (shedShape.NumRows() > 1)
                {
                    var newperc = Convert.ToInt32((Convert.ToDouble(sindx) / Convert.ToDouble(shedShape.NumRows())) * 100);
                    if (newperc > oldperc)
                    {
                        callback?.Progress("Status", newperc, "Calculating WS Slope Parameters");
                        oldperc = newperc;
                    }
                }

                if (countSlope[sindx] <= 0)
                {
                    continue;
                }

                if (slopeProj.Unit.Name == "Meter")
                {
                    switch (elevUnits)
                    {
                        case ElevationUnits.Meters:
                            avgSlope[sindx] = (sumSlope[sindx] / countSlope[sindx]) * 100;
                            break;
                        case ElevationUnits.Centimeters:
                            avgSlope[sindx] = sumSlope[sindx] / countSlope[sindx];
                            break;
                        case ElevationUnits.Feet:
                            avgSlope[sindx] = ((sumSlope[sindx] / countSlope[sindx]) / 3.280839895) * 100;
                            break;
                    }
                }
                else if (slopeProj.Unit.Name == "Foot")
                {
                    switch (elevUnits)
                    {
                        case ElevationUnits.Meters:
                            avgSlope[sindx] = ((sumSlope[sindx] / countSlope[sindx]) * 3.280839895) * 100;
                            break;
                        case ElevationUnits.Centimeters:
                            avgSlope[sindx] = ((sumSlope[sindx] / countSlope[sindx]) / 30.48) * 100;
                            break;
                        case ElevationUnits.Feet:
                            avgSlope[sindx] = (sumSlope[sindx] / countSlope[sindx]) * 100;
                            break;
                    }
                }

                shedShape.EditCellValue(slopeFieldNum, sindx, avgSlope[sindx]);
            }

            shedShape.Save();
            shedShape.Close();

            callback?.Progress(string.Empty, 0, string.Empty);

            return true;
        }

        /// <summary>
        /// Hydrology function used to add to the subbasin shapefile average slope attribute.
        /// </summary>
        /// <param name="subBasinGridPath">Path of the sub basin grid.</param>
        /// <param name="subBasinShapePath">Path of the sub basin shapefile.</param>
        /// <param name="slopeGridPath">Path of the slope grid.</param>
        /// <param name="elevUnits">The elevation units.</param>
        /// <param name="callback">The progress handler.</param>
        /// <returns>True, if run successfully.</returns>
        public static bool ApplyWatershedSlopeAttribute(string subBasinGridPath, string subBasinShapePath, string slopeGridPath, ElevationUnits elevUnits, IProgressHandler callback)
        {
            callback?.Progress("Status", 0, "Calculating WS Slope Parameters");

            var shedShape = FeatureSet.Open(subBasinShapePath);
            var subBasinGrid = Raster.Open(subBasinGridPath);
            var slopeGrid = Raster.Open(slopeGridPath);

            var numberRows = slopeGrid.NumRows;
            var numberCols = slopeGrid.NumColumns;
            var nodataVal = slopeGrid.NoDataValue;
            var slopeProj = slopeGrid.Projection;

            var countSlope = new int[shedShape.NumRows()];
            var sumSlope = new double[shedShape.NumRows()];
            var avgSlope = new double[shedShape.NumRows()];

            var currPolyIdIdx = -1;
            var currLinkIdIdx = -1;
            for (int sindx = 0; sindx < shedShape.DataTable.Columns.Count; sindx++)
            {
                if (shedShape.DataTable.Columns[sindx].ColumnName == "PolygonID" || shedShape.DataTable.Columns[sindx].ColumnName == "MWShapeID")
                {
                    currPolyIdIdx = sindx;
                }

                if (shedShape.DataTable.Columns[sindx].ColumnName == "LinkIDs")
                {
                    currLinkIdIdx = sindx;
                }

                // Paul Meems, 24-Aug-2011: Added:
                if (currPolyIdIdx != -1 && currLinkIdIdx != -1)
                {
                    // Found the values so stop searching:
                    break;
                }
            }

            var linkIdVals = new List<int>();
            var linkIdMerged = new List<int>();

            if (currLinkIdIdx != -1 && currPolyIdIdx != -1)
            {
                for (int sindx = 0; sindx < shedShape.NumRows(); sindx++)
                {
                    var tmpLinkIDs = shedShape.GetCellValue(currLinkIdIdx, sindx).ToString();
                    var tmpLinks = tmpLinkIDs.Split(',');
                    foreach (var tmpLink in tmpLinks)
                    {
                        linkIdMerged.Add(sindx);
                        linkIdVals.Add(int.Parse(tmpLink.Trim()));
                    }
                }
            }
            else
            {
                for (int sindx = 0; sindx < shedShape.NumRows(); sindx++)
                {
                    linkIdMerged.Add(sindx);
                    linkIdVals.Add(int.Parse(shedShape.GetCellValue(currPolyIdIdx, sindx).ToString()));
                }
            }

            var oldperc = 0;
            for (int row = 0; row < numberRows; row++)
            {
                var newperc = Convert.ToInt32((Convert.ToDouble(row) / Convert.ToDouble(numberRows - 1)) * 100);
                if (newperc > oldperc)
                {
                    callback?.Progress("Status", newperc, "Calculating WS Slope Parameters");

                    oldperc = newperc;
                }

                int col;
                for (col = 0; col < numberCols; col++)
                {
                    var currVal = double.Parse(slopeGrid.Rows[col][row].ToString());

                    if (currVal == nodataVal)
                    {
                        continue;
                    }

                    var currBasinId = int.Parse(subBasinGrid.Rows[col][row].ToString());

                    // Paul Meems, 24-Aug-2011: Changed:
                    // if (currBasinID != -1)
                    // TODO: Check if the result is still the same
                    if (currBasinId > -1)
                    {
                        // Paul Meems, 24-Aug-2011: Added extra check:
                        var tmp = linkIdVals.IndexOf(currBasinId);
                        if (tmp != -1)
                        {
                            var currId = linkIdMerged[tmp];

                            countSlope[currId] = countSlope[currId] + 1;
                            sumSlope[currId] = sumSlope[currId] + currVal;
                        }
                    }
                }
            }

            slopeGrid.Close();

            callback?.Progress("Status", 0, "Calculating WS Slope Parameters");

            var slopeFieldNum = AddField(shedShape, "AveSlope", typeof(double));

            oldperc = 0;
            for (int sindx = 0; sindx < shedShape.NumRows(); sindx++)
            {
                // TODO: Why > 1 instead of > 0?
                if (shedShape.NumRows() > 1)
                {
                    var newperc = Convert.ToInt32((Convert.ToDouble(sindx) / Convert.ToDouble(shedShape.NumRows())) * 100);
                    if (newperc > oldperc)
                    {
                        callback?.Progress("Status", newperc, "Calculating WS Slope Parameters");

                        oldperc = newperc;
                    }
                }

                if (countSlope[sindx] <= 0)
                {
                    continue;
                }

                if (slopeProj.Unit.Name == "Meter")
                {
                    switch (elevUnits)
                    {
                        case ElevationUnits.Meters:
                            avgSlope[sindx] = (sumSlope[sindx] / countSlope[sindx]) * 100;
                            break;
                        case ElevationUnits.Centimeters:
                            avgSlope[sindx] = sumSlope[sindx] / countSlope[sindx];
                            break;
                        case ElevationUnits.Feet:
                            avgSlope[sindx] = ((sumSlope[sindx] / countSlope[sindx]) / 3.280839895) * 100;
                            break;
                    }
                }
                else if (slopeProj.Unit.Name == "Foot")
                {
                    switch (elevUnits)
                    {
                        case ElevationUnits.Meters:
                            avgSlope[sindx] = ((sumSlope[sindx] / countSlope[sindx]) * 3.280839895) * 100;
                            break;
                        case ElevationUnits.Centimeters:
                            avgSlope[sindx] = ((sumSlope[sindx] / countSlope[sindx]) / 30.48) * 100;
                            break;
                        case ElevationUnits.Feet:
                            avgSlope[sindx] = (sumSlope[sindx] / countSlope[sindx]) * 100;
                            break;
                    }
                }

                shedShape.EditCellValue(slopeFieldNum, sindx, avgSlope[sindx]);
            }

            shedShape.Save();
            shedShape.Close();

            subBasinGrid.Close();

            callback?.Progress(string.Empty, 0, string.Empty);

            return true;
        }

        /// <summary>
        /// Uses TauDEM V5
        /// Generates an area D8 grid which shows the paths of highest flow and can be used to delineate stream networks.
        /// </summary>
        /// <param name="d8Path">Path to a D8 grid to be converted into an area D8 grid.</param>
        /// <param name="outletsPath">Optional path to a point shape file which is used to designate outlet points on a grid. If this path is given, the resulting area D8 grid will only include values for those areas of the grid which flow into the outlet points given. All other portions of the grid will be set to 0.</param>
        /// <param name="areaD8ResultPath">Path to an area D8 output grid, </param>
        /// <param name="useOutlets">Boolean true for using outlets in delineation d8 areas</param>
        /// <param name="useEdgeContamCheck">Boolean true to ignore off-grid contributing area</param>
        /// <param name="numProcesses">Number of threads to be used by Taudem</param>
        /// <param name="showTaudemOutput">Show Taudem output if true</param>
        /// <param name="callback"> A callback object for internal status messages</param>
        /// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
        public static int AreaD8(string d8Path, string outletsPath, string areaD8ResultPath, bool useOutlets, bool useEdgeContamCheck, int numProcesses, bool showTaudemOutput, IProgressHandler callback)
        {
            Trace.WriteLine("AreaD8(d8Path: " + d8Path + ",\n" + "       outletsPath: " + outletsPath + ",\n" + "       AreaD8ResultPath: " + areaD8ResultPath + ",\n" + "       useOutlets: " + useOutlets + ",\n" + "       useEdgeContamCheck: " + useEdgeContamCheck + "\n" + "       NumProcesses: " + numProcesses + "\n" + "       ShowTaudemOutput: " + showTaudemOutput + "\n" + "       callback)");

            callback?.Progress("Status", 0, "D8 Area");

            var pars = "-p " + Quote(d8Path) + " -ad8 " + Quote(areaD8ResultPath);
            if (useOutlets)
            {
                pars += " -o " + Quote(outletsPath);
            }

            if (!useEdgeContamCheck)
            {
                pars += " -nc";
            }

            var result = RunTaudem("AreaD8.exe", pars, numProcesses, showTaudemOutput);
            if (result != 0)
            {
                var errMsg = string.Format(MessageStrings.TaudemError, result);
                MessageBox.Show(errMsg, errMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            callback?.Progress("Status", 0, string.Empty);

            Trace.WriteLine("Finished AreaD8");
            return result;
        }

        /// <summary>
        /// A function to create the joined basins from a watershed shapefile that has had the basic apply attributes set on it.
        /// </summary>
        /// <param name="subBasinShapePath">Path of the sub basin shapefile.</param>
        /// <param name="joinBasinShapeResultPath">Path of the resulting shapefile.</param>
        /// <param name="callback">The progress handler</param>
        /// <returns>True, if run successfully.</returns>
        public static bool BuildJoinedBasins(string subBasinShapePath, string joinBasinShapeResultPath, IProgressHandler callback)
        {
            return BuildJoinedBasins(subBasinShapePath, string.Empty, joinBasinShapeResultPath, callback);
        }

        /// <summary>
        /// Overload of BuildJoinedBasins that takes an outlets shape path used for Inlets resolution. If no outlets/inlets path given, it will treat all points as outlets.
        /// </summary>
        /// <param name="subBasinShapePath">Path of the sub basin shapefile.</param>
        /// <param name="outletsShapePath">Path of the outlets shapefile.</param>
        /// <param name="joinBasinShapeResultPath">Path of the resulting shapefile.</param>
        /// <param name="callback">The progress handler.</param>
        /// <returns>True, if run successfully.</returns>
        // 2/11/09 rewritten by Chris George to dramatically improve speed:
        //  (a) use utils.ClipPolygon instead of SpatialOperations.MergeShapes
        //  (b) create a binary tree modeling the drainage pattern of the subbasins
        //  (b) merge "upstream-first" using the drainage tree so that each merge combines abutting polygons
        public static bool BuildJoinedBasins(string subBasinShapePath, string outletsShapePath, string joinBasinShapeResultPath, IProgressHandler callback)
        {
            var shapeIdxList = new ArrayList();

            IFeatureSet outlets = null;
            if (outletsShapePath != string.Empty)
            {
                outlets = FeatureSet.Open(outletsShapePath);
            }

            var shed = FeatureSet.Open(subBasinShapePath);

            var dsNodeFieldNum = -1;
            var dsShedFieldNum = -1;
            var us1FieldNum = -1;
            var us2FieldNum = -1;

            for (var i = 0; i < shed.DataTable.Columns.Count; i++)
            {
                switch (shed.DataTable.Columns[i].ColumnName.ToUpper())
                {
                    case "DSNODEID":
                        dsNodeFieldNum = i;
                        break;
                    case "DSWSID":
                        dsShedFieldNum = i;
                        break;
                    case "US1WSID":
                        us1FieldNum = i;
                        break;
                    case "US2WSID":
                        us2FieldNum = i;
                        break;
                }
            }

            var newShed = new FeatureSet
            {
                FeatureType = FeatureType.Polygon,
                Filename = joinBasinShapeResultPath
            };

            var idfieldnum = AddField(newShed, "MWShapeID", typeof(int));
            var linkfieldnum = AddField(newShed, "LinkIDs", typeof(string));
            var outletfieldnum = AddField(newShed, "OutletID", typeof(int));
            var dswsfieldnum = AddField(newShed, "DSWSID", typeof(int));
            var uswsfieldnum1 = AddField(newShed, "USWSID1", typeof(int));
            var uswsfieldnum2 = AddField(newShed, "USWSID2", typeof(int));
            var reservoirfieldnum = AddField(newShed, "Reservoir", typeof(int));

            var oldperc = 0;
            for (var sindx = 0; sindx < shed.NumRows(); sindx++)
            {
                if (shed.NumRows() > 1)
                {
                    var newperc = Convert.ToInt32((Convert.ToDouble(sindx) / Convert.ToDouble(shed.NumRows())) * 100);
                    if (newperc > oldperc)
                    {
                        callback?.Progress("Status", newperc, "Merging Watersheds to Outlets/Inlets");
                        oldperc = newperc;
                    }
                }

                DsNode dsNodeType = DsNode.Outlet;
                var dsNodeVal = int.Parse(shed.GetCellValue(dsNodeFieldNum, sindx).ToString());

                if (dsNodeVal > -1)
                {
                    // an outlet, inlet, reservoir or point source
                    BinTree drainage;
                    if (outletsShapePath == string.Empty)
                    {
                        // assume this is an outlet
                        drainage = GetDrainageFromSubbasin(shed, outlets, false, true, sindx, dsNodeFieldNum, us1FieldNum, us2FieldNum);
                    }
                    else
                    {
                        dsNodeType = GetDsNodeType(outlets, dsNodeVal);
                        if ((dsNodeType == DsNode.Outlet) || (dsNodeType == DsNode.Reservoir))
                        {
                            if (IsUpstreamOfInlet(shed, outlets, sindx))
                            {
                                drainage = null;
                            }
                            else
                            {
                                drainage = GetDrainageFromSubbasin(shed, outlets, true, true, sindx, dsNodeFieldNum, us1FieldNum, us2FieldNum);
                            }
                        }
                        else
                        {
                            // ignore inlets and point sources
                            drainage = null;
                        }
                    }

                    shapeIdxList.Clear();
                    if (drainage != null)
                    {
                        char[] sep = { ',' };
                        var idxs = drainage.ToString().Split(sep);
                        for (var i = 0; i < idxs.Length; i++)
                        {
                            shapeIdxList.Add(idxs[i]);
                        }
                    }

                    if (shapeIdxList.Count != 0)
                    {
                        IFeature mergeShape;
                        string strLinks;
                        if (shapeIdxList.Count == 1)
                        {
                            mergeShape = shed.GetShape(int.Parse(shapeIdxList[0].ToString()));
                            strLinks = shed.GetCellValue(0, int.Parse(shapeIdxList[0].ToString())).ToString();
                        }
                        else
                        {
                            strLinks = shed.GetCellValue(0, int.Parse(shapeIdxList[0].ToString())).ToString();
                            for (int i = 1; i <= shapeIdxList.Count - 1; i++)
                            {
                                strLinks = strLinks + ", " + shed.GetCellValue(0, int.Parse(shapeIdxList[i].ToString()));
                            }

                            DateTime time = DateTime.Now;
                            mergeShape = MergeBasinsByDrainage(shed, drainage);
                            var elapsed = DateTime.Now.Subtract(time);
                            Trace.WriteLine("Made merged watershed of " + shapeIdxList.Count + " subbasins in " + elapsed.TotalSeconds.ToString("F1") + " seconds");
                        }

                        // Check merged shape for single part and clockwise
                        if (mergeShape.Geometry.NumGeometries > 1)
                        {
                            Trace.WriteLine("Merged polygon has " + mergeShape.Geometry.NumGeometries + " parts");
                        }
                        else
                        {
                            var area = SignedArea(mergeShape);
                            if (area < 0)
                            {
                                Trace.WriteLine("Needed to reverse merged polygon");
                                mergeShape = ReverseShape(mergeShape);
                            }
                        }

                        var currshpidx = newShed.NumRows();

                        newShed.AddFeature(mergeShape.Geometry);
                        newShed.EditCellValue(idfieldnum, currshpidx, currshpidx);
                        newShed.EditCellValue(linkfieldnum, currshpidx, strLinks);
                        newShed.EditCellValue(outletfieldnum, currshpidx, dsNodeVal);
                        if (int.Parse(shed.GetCellValue(dsShedFieldNum, sindx).ToString()) != -1)
                        {
                            newShed.EditCellValue(dswsfieldnum, currshpidx, shed.GetCellValue(dsShedFieldNum, sindx));
                        }
                        else
                        {
                            newShed.EditCellValue(dswsfieldnum, currshpidx, -1);
                        }

                        newShed.EditCellValue(uswsfieldnum1, currshpidx, -1);
                        newShed.EditCellValue(uswsfieldnum2, currshpidx, -1);
                        newShed.EditCellValue(reservoirfieldnum, currshpidx, dsNodeType == DsNode.Reservoir ? 1 : 0);
                    }
                }
            }

            BuildMergeDownstreamUpStream(newShed, idfieldnum, linkfieldnum, dswsfieldnum, uswsfieldnum1, uswsfieldnum2);

            newShed.Projection = shed.Projection;

            shed.Close();

            newShed.Save();
            newShed.Close();

            if (outletsShapePath != string.Empty)
            {
                outlets?.Close();
            }

            callback?.Progress(string.Empty, 0, string.Empty);

            return true;
        }

        /// <summary>
        /// Uses TauDEM V5
        /// Generates a D8 directional grid by assigning a number from 1 to 8 (0 to 7 in some algorithms) based on a direction to the lowest elevation cell surrounding that cell.
        /// </summary>
        /// <param name="pitFillPath">Path of a pit-filled DEM.</param>
        /// <param name="d8ResultPath">Output result file of a D8 directional grid.</param>
        /// <param name="d8SlopeResultPath">Path to an output grid containing the slope from the cell to the lowest elevation surrounding cell.</param>
        /// <param name="numProcesses">Number of threads to be used by Taudem</param>
        /// <param name="showTaudemOutput">Show Taudem output if true</param>
        /// <param name="callback"> A callback object for internal status messages</param>
        /// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
        public static bool D8(string pitFillPath, string d8ResultPath, string d8SlopeResultPath, int numProcesses, bool showTaudemOutput, IProgressHandler callback)
        {
            Trace.WriteLine("D8(pitFillPath: " + pitFillPath + "\n" + "   D8ResultPath: " + d8ResultPath + "\n" + "   D8SlopeResultPath: " + d8SlopeResultPath + "\n" + "   NumProcesses: " + numProcesses.ToString() + "\n" + "   ShowTaudemOutput: " + showTaudemOutput.ToString() + "\n" + "   callback)");
            callback?.Progress("Status", 0, "D8 Flow Directions");

            var pars = "-p " + Quote(d8ResultPath) + " -sd8 " + Quote(d8SlopeResultPath) + " -fel " + Quote(pitFillPath);
            var result = RunTaudem("D8FlowDir.exe", pars, numProcesses, showTaudemOutput);

            if (result != 0)
            {
                var errMsg = string.Format(MessageStrings.TaudemError, result);
                MessageBox.Show(errMsg, errMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            callback?.Progress("Status", 0, string.Empty);

            Trace.WriteLine("Finished D8");
            return result == 0;
        }

        /// <summary>
        /// A function which will make appropriate calls to Taudem in order to form the network grids used in delineation.
        /// </summary>
        /// <param name="demGridPath">Path of the dem grid file.</param>
        /// <param name="pitFillPath">Path of the pit fill file.</param>
        /// <param name="d8Path">Path of the D8 file.</param>
        /// <param name="d8SlopePath">Path of the D8 slope file.</param>
        /// <param name="areaD8Path">Path of the D8 area file.</param>
        /// <param name="outletsPath">Path of the outlets file.</param>
        /// <param name="strahlOrdResultPath">Path of the resulting strahl order file.</param>
        /// <param name="longestUpslopeResultPath">Path of the resulting longest upslope file.</param>
        /// <param name="totalUpslopeResultPath">Path of the resulting total upslope file.</param>
        /// <param name="streamGridResultPath">Path of the resulting stream grid file.</param>
        /// <param name="streamOrdResultPath">Path of the resulting stream order file.</param>
        /// <param name="treeDatResultPath">Path of the resultingtree data file.</param>
        /// <param name="coordDatResultPath">Path of the resulting coordinate data file.</param>
        /// <param name="streamShapeResultPath">Path of the resulting stream shape file.</param>
        /// <param name="watershedGridResultPath">Path of the resulting watershed file.</param>
        /// <param name="threshold">The threshold.</param>
        /// <param name="useOutlets">Use outlets</param>
        /// <param name="useEdgeContamCheck">Use edge contam check.</param>
        /// <param name="numProcesses">Number of threads to be used by Taudem</param>
        /// <param name="showTaudemOutput">Show Taudem output if true</param>
        /// <param name="callback">The progress handler.</param>
        /// <returns>Return code from Taudem executable</returns>
        public static int DelinStreamGrids(string demGridPath, string pitFillPath, string d8Path, string d8SlopePath, string areaD8Path, string outletsPath, string strahlOrdResultPath, string longestUpslopeResultPath, string totalUpslopeResultPath, string streamGridResultPath, string streamOrdResultPath, string treeDatResultPath, string coordDatResultPath, string streamShapeResultPath, string watershedGridResultPath, int threshold, bool useOutlets, bool useEdgeContamCheck, int numProcesses, bool showTaudemOutput, IProgressHandler callback)
        {
            return RunDelinStreamGrids(demGridPath, pitFillPath, d8Path, d8SlopePath, areaD8Path, string.Empty, outletsPath, strahlOrdResultPath, longestUpslopeResultPath, totalUpslopeResultPath, streamGridResultPath, streamOrdResultPath, treeDatResultPath, coordDatResultPath, streamShapeResultPath, watershedGridResultPath, threshold, useOutlets, useEdgeContamCheck, false, numProcesses, showTaudemOutput, callback);
        }

        /// <summary>
        /// An overload of delinStreamGrids which allows the use of Dinf in the delineation.
        /// </summary>
        /// <param name="demGridPath">Path of the dem grid file.</param>
        /// <param name="pitFillPath">Path of the pit fill file.</param>
        /// <param name="d8Path">Path of the D8 file.</param>
        /// <param name="d8SlopePath">Path of the D8 slope file.</param>
        /// <param name="areaD8Path">Path of the D8 area file.</param>
        /// <param name="areaDInfPath">Path of the area DInf file.</param>
        /// <param name="outletsPath">Path of the outlets file.</param>
        /// <param name="strahlOrdResultPath">Path of the resulting strahl order file.</param>
        /// <param name="longestUpslopeResultPath">Path of the resulting longest upslope file.</param>
        /// <param name="totalUpslopeResultPath">Path of the resulting total upslope file.</param>
        /// <param name="streamGridResultPath">Path of the resulting stream grid file.</param>
        /// <param name="streamOrdResultPath">Path of the resulting stream order file.</param>
        /// <param name="treeDatResultPath">Path of the resultingtree data file.</param>
        /// <param name="coordDatResultPath">Path of the resulting coordinate data file.</param>
        /// <param name="streamShapeResultPath">Path of the resulting stream shape file.</param>
        /// <param name="watershedGridResultPath">Path of the resulting watershed file.</param>
        /// <param name="threshold">The threshold.</param>
        /// <param name="useOutlets">Use outlets</param>
        /// <param name="useEdgeContamCheck">Use edge contam check.</param>
        /// <param name="useDinf">Use DInf.</param>
        /// <param name="numProcesses">Number of threads to be used by Taudem</param>
        /// <param name="showTaudemOutput">Show Taudem output if true</param>
        /// <param name="callback">The progress handler.</param>
        /// <returns>Return code from Taudem executable</returns>
        public static int DelinStreamGrids(string demGridPath, string pitFillPath, string d8Path, string d8SlopePath, string areaD8Path, string areaDInfPath, string outletsPath, string strahlOrdResultPath, string longestUpslopeResultPath, string totalUpslopeResultPath, string streamGridResultPath, string streamOrdResultPath, string treeDatResultPath, string coordDatResultPath, string streamShapeResultPath, string watershedGridResultPath, int threshold, bool useOutlets, bool useEdgeContamCheck, bool useDinf, int numProcesses, bool showTaudemOutput, IProgressHandler callback)
        {
            return RunDelinStreamGrids(demGridPath, pitFillPath, d8Path, d8SlopePath, areaD8Path, areaDInfPath, outletsPath, strahlOrdResultPath, longestUpslopeResultPath, totalUpslopeResultPath, streamGridResultPath, streamOrdResultPath, treeDatResultPath, coordDatResultPath, streamShapeResultPath, watershedGridResultPath, threshold, useOutlets, useEdgeContamCheck, useDinf, numProcesses, showTaudemOutput, callback);
        }

        /// <summary>A function to call the Taudem d-infinity calculations</summary>
        /// <param name="pitFillPath">Path of the pit fill file.</param>
        /// <param name="dInfResultPath">Path of the resulting dInf File.</param>
        /// <param name="dInfSlopeResultPath">Path of the resulting dInf slope file.</param>
        /// <param name="numProcesses">Number of threads to be used by Taudem</param>
        /// <param name="showTaudemOutput">Show Taudem output if true</param>
        /// <param name="callback">The progress handler.</param>
        /// <returns>Return code from Taudem executable</returns>
        public static int DInf(string pitFillPath, string dInfResultPath, string dInfSlopeResultPath, int numProcesses, bool showTaudemOutput, IProgressHandler callback)
        {
            Trace.WriteLine("Dinf(pitFillPath: " + pitFillPath + ",\n" + "       DInfResultPath: " + dInfResultPath + ",\n" + "       DInfSlopeResultPath: " + dInfSlopeResultPath + ",\n" + "       NumProcesses: " + numProcesses.ToString() + "\n" + "       ShowTaudemOutput: " + showTaudemOutput.ToString() + "\n" + "       callback)");

            callback?.Progress("Status", 0, "D-inf Flow Directions");

            var pars = "-ang " + Quote(dInfResultPath) + " -slp " + Quote(dInfSlopeResultPath) + " -fel " + Quote(pitFillPath);

            var result = RunTaudem("DinfFlowDir.exe", pars, numProcesses, showTaudemOutput);
            if (result != 0)
            {
                var errMsg = string.Format(MessageStrings.TaudemError, result);
                MessageBox.Show(errMsg, errMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            callback?.Progress("Status", 0, string.Empty);

            Trace.WriteLine("Finished DInf");
            return result;
        }

        /// <summary>
        /// Fills depressions in an image
        /// - Files specified by parameters
        /// - Progress and status messages will be sent back via IProgressHandler
        /// - Frames will be sized to default values
        /// </summary>
        /// <param name="sourceFile">String filename of unfilled DEM</param>
        /// <param name="destFile">String filename of output file</param>
        /// <param name="progress">The progress.</param>
        /// <remarks>
        /// Images too large to process all at once are broken down into a framework.
        /// A frame represents what will be loaded into memory at any given time.
        /// </remarks>
        public static void Fill(string sourceFile, string destFile, IProgressHandler progress)
        {
            // 2000 width
            // 1000 height
            FileFill(sourceFile, destFile, true, false, 10000, 2000, progress);
        }

        /// <summary>Runs threshold.exe</summary>
        /// <param name="areaD8Path">Input file</param>
        /// <param name="streamGridResultPath">Output file</param>
        /// <param name="threshold">The threshold</param>
        /// <param name="numProcesses">Number of processes</param>
        /// <param name="showTaudemOutput">Show taudem output</param>
        /// <param name="callback">Callback objet</param>
        /// <returns>Taudem return number, 0 means OK</returns>
        /// <remarks>Created by Paul Meems, 23 Aug 2011</remarks>
        public static int RunAllStreamDelineation(string areaD8Path, string streamGridResultPath, int threshold, int numProcesses, bool showTaudemOutput, IProgressHandler callback)
        {
            Trace.WriteLine("All Stream Delineation");
            callback?.Progress("Status", 0, "All Stream Delineation");

            var areaGridPath = areaD8Path; // TODO CWG inf does not seems to work (useDinf)?areaDInfPath:areaD8Path;
            var pars = "-ssa " + Quote(areaGridPath) + " -src " + Quote(streamGridResultPath) + " -thresh " + threshold.ToString(CultureInfo.InvariantCulture);
            var result = RunTaudem("threshold.exe", pars, numProcesses, showTaudemOutput);
            if (result != 0)
            {
                var errMsg = string.Format(MessageStrings.TaudemError, result);
                MessageBox.Show(errMsg, errMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }

        /// <summary>
        /// Runs GridNet.exe
        /// </summary>
        /// <param name="demGridPath">Input file</param>
        /// <param name="d8Path">Path of the D8 file.</param>
        /// <param name="longestUpslopeResultPath">Output file</param>
        /// <param name="totalUpslopeResultPath">Path of the resulting total upslope file.</param>
        /// <param name="strahlOrdResultPath">Path of the resulting strahl order file.</param>
        /// <param name="outletsPath">Path of the outlets file.</param>
        /// <param name="useOutlets">Use outlets</param>
        /// <param name="numProcesses">Number of processes</param>
        /// <param name="showTaudemOutput">Show taudem output</param>
        /// <param name="callback">Callback objet</param>
        /// <returns>
        /// Taudem return number, 0 means OK
        /// </returns>
        public static int RunGridNetwork(string demGridPath, string d8Path, string longestUpslopeResultPath, string totalUpslopeResultPath, string strahlOrdResultPath, string outletsPath, bool useOutlets, int numProcesses, bool showTaudemOutput, IProgressHandler callback)
        {
            Trace.WriteLine("Grid Network");
            callback?.Progress("Status", 0, "Grid Network");

            var pars = "-p " + Quote(d8Path) + " -plen " + Quote(longestUpslopeResultPath) + " -tlen " + Quote(totalUpslopeResultPath) + " -gord " + Quote(strahlOrdResultPath);
            if (useOutlets)
            {
                pars += " -o " + Quote(outletsPath);
            }

            var result = RunTaudem("GridNet.exe", pars, numProcesses, showTaudemOutput);
            if (result != 0)
            {
                var errMsg = string.Format(MessageStrings.TaudemError, result);
                MessageBox.Show(errMsg, errMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            callback?.Progress("Status", 0, string.Empty);

            return result;
        }

        /// <summary>
        /// Call StreamNet.exe
        /// </summary>
        /// <param name="demGridPath">Input file</param>
        /// <param name="pitFillPath">Path of the pit fill file.</param>
        /// <param name="d8Path">Path of the D8 file.</param>
        /// <param name="areaD8Path">Path of the area D8 file.</param>
        /// <param name="outletsPath">Path of the outlets file.</param>
        /// <param name="streamGridResultPath">Output file</param>
        /// <param name="streamOrdResultPath">Path of the resulting stream ord file.</param>
        /// <param name="streamShapeResultPath">Path of the resulting stream shape file.</param>
        /// <param name="watershedGridResultPath">Path of the resulting watershed grid file.</param>
        /// <param name="coordDatResultPath">Path of the resulting coordinate data file.</param>
        /// <param name="treeDatResultPath">Path of the resulting tree data file.</param>
        /// <param name="useOutlets">Use outlets</param>
        /// <param name="numProcesses">Number of processes</param>
        /// <param name="showTaudemOutput">Show taudem output</param>
        /// <param name="callback">Callback objet</param>
        /// <returns>
        /// Taudem return number, 0 means OK
        /// </returns>
        public static int RunStreamOrderGridRaster(string demGridPath, string pitFillPath, string d8Path, string areaD8Path, string outletsPath, string streamGridResultPath, string streamOrdResultPath, string streamShapeResultPath, string watershedGridResultPath, string coordDatResultPath, string treeDatResultPath, bool useOutlets, int numProcesses, bool showTaudemOutput, IProgressHandler callback)
        {
            Trace.WriteLine("Stream Order Grid and Raster");
            callback?.Progress("Status", 0, "Stream Order Grid and Raster");

            File.Delete(coordDatResultPath);
            File.Delete(treeDatResultPath);

            var pars = "-fel " + Quote(pitFillPath) + " -p " + Quote(d8Path) + " -ad8 " + Quote(areaD8Path) + " -src " + Quote(streamGridResultPath) + " -ord " + Quote(streamOrdResultPath) + " -tree " + Quote(treeDatResultPath) + " -coord " + Quote(coordDatResultPath) + " -net " + Quote(streamShapeResultPath) + " -w " + Quote(watershedGridResultPath);
            if (useOutlets)
            {
                pars += " -o " + Quote(outletsPath);
            }

            var result = RunTaudem("StreamNet.exe", pars, numProcesses, showTaudemOutput);
            if (result == -1)
            {
                if (!File.Exists(streamOrdResultPath))
                {
                    MessageBox.Show(MessageStrings.AutomaticWatershedDilineationEncounteredError, MessageStrings.ApplicationError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (result != 0)
                {
                    var errMsg = string.Format(MessageStrings.TaudemError, result);
                    MessageBox.Show(errMsg, errMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            DataManagement.TryCopy(Path.ChangeExtension(demGridPath, ".prj"), Path.ChangeExtension(streamShapeResultPath, ".prj"));

            callback?.Progress("Status", 0, string.Empty);

            return result;
        }

        /// <summary>
        /// Run a Taudem V5 executable
        /// </summary>
        /// <param name="command">Taudem executable file name</param>
        /// <param name="parameters">Parameters for Taudem executable.  Parameters containing spaces must be quoted.</param>
        /// <param name="numProc">Maximum number of threads (ignored if MPICH2 not running)</param>
        /// <param name="showOutput">Standard output and standard error from Taudem executable are shown iff this flag is true or there is an error in the Taudem run.</param>
        /// <returns>
        /// Return code from Taudem executable
        /// </returns>
        public static int RunTaudem(string command, string parameters, int numProc, bool showOutput)
        {
            var tfc = new TempFileCollection();
            var tempFile = tfc.AddExtension("txt");
            var taudemProcess = new Process
            {
                StartInfo =
                {
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Minimized,
                    WorkingDirectory = TaudemExeDir(),
                    FileName = "runtaudem.bat",
                    Arguments = Quote(tempFile) + " mpiexec -n " + numProc.ToString(CultureInfo.InvariantCulture) + " " + command + " " + parameters
                }
            };
            taudemProcess.Start();
            taudemProcess.WaitForExit();

            var needOutput = showOutput || ((taudemProcess.ExitCode != 0) && (MessageBox.Show(@"Taudem error.  Do you want to see the details?", string.Empty, MessageBoxButtons.YesNo) == DialogResult.Yes));
            if (needOutput)
            {
                var proc = new Process
                {
                    StartInfo =
                    {
                        FileName = Path.GetFileName(tempFile),
                        WorkingDirectory = Path.GetDirectoryName(tempFile)
                    }
                };
                proc.Start();

                // Paul Meems, 23-aug-2011: Don't wait for exit but continue the proces:
                // cwg 13/9/2011 do wait - else get a confusing stack of outputs
                proc.WaitForExit();
            }

            // Paul Meems, 23-aug-2011
            // tfc.Delete();
            return taudemProcess.ExitCode;
        }

        /// <summary>
        /// Returns the signed area for a polygon shape.
        /// The area is positive if the polygon runs clockwise.
        /// </summary>
        /// <param name="shape">Shape to get the area from.</param>
        /// <returns>The area of the shape.</returns>
        public static double SignedArea(IFeature shape)
        {
            if (shape.Geometry.NumPoints < 4)
                return 0.0;

            double sum = 0.0;
            Coordinate ptc = shape.Geometry.Coordinates[0];

            // If this is a close polygon, the starting point will
            // be the ssame as the end point, so we don't look at the last point.
            for (int i = 0; i < shape.Geometry.NumPoints - 1; i++)
            {
                var ptb = ptc;
                ptc = shape.Geometry.Coordinates[i + 1];
                double bx = ptb.X;
                double by = ptb.Y;
                double cx = ptc.X;
                double cy = ptc.Y;
                sum += (bx + cx) * (cy - by);
            }

            return -sum / 2.0;
        }

        /// <summary>
        /// Adds a field to the shapefile
        /// </summary>
        /// <param name="sf">The shapefile</param>
        /// <param name="fieldname">The fieldname</param>
        /// <param name="fieldType">The field type</param>
        /// <param name="width">The width.</param>
        /// <param name="precision">The precision</param>
        /// <returns>The index of the inserted field</returns>
        private static int AddField(IFeatureSet sf, string fieldname, Type fieldType, int width, int precision)
        {
            sf.DataTable.Columns.Add(fieldname, fieldType);
            return sf.DataTable.Columns.Count - 1;
        }

        /// <summary>
        /// Overloaded method to add a field to the shapefile
        /// </summary>
        /// <param name="sf">The shapefile</param>
        /// <param name="fieldname">The fieldname.</param>
        /// <param name="fieldType">The field type.</param>
        /// <returns>The index of the inserted field.</returns>
        private static int AddField(IFeatureSet sf, string fieldname, Type fieldType)
        {
            return AddField(sf, fieldname, fieldType, -1, -1);
        }

        private static void BuildMergeDownstreamUpStream(IFeatureSet newshed, int idFieldNum, int linksFieldNum, int dsFieldNum, int usFieldNum1, int usFieldNum2)
        {
            for (int i = 0; i <= newshed.NumRows() - 1; i++)
            {
                string currDsField = newshed.GetCellValue(dsFieldNum, i).ToString();
                if (currDsField != "-1")
                {
                    for (int j = 0; j <= newshed.NumRows() - 1; j++)
                    {
                        string links = newshed.GetCellValue(linksFieldNum, j).ToString();
                        string[] split = links.Split(',');
                        for (int k = 0; k <= split.Length - 1; k++)
                        {
                            if (split[k].Trim() == currDsField)
                            {
                                newshed.EditCellValue(dsFieldNum, i, newshed.GetCellValue(idFieldNum, j));
                                string upstream1 = newshed.GetCellValue(usFieldNum1, j).ToString();
                                if (upstream1 == "-1")
                                {
                                    newshed.EditCellValue(usFieldNum1, j, newshed.GetCellValue(idFieldNum, i));
                                }
                                else
                                {
                                    newshed.EditCellValue(usFieldNum2, j, newshed.GetCellValue(idFieldNum, i));
                                }

                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Performs a Pitfill. Returns true unless the operation was canceled by the dialog
        /// </summary>
        /// <param name="sourceGrid">The source Grid.</param>
        /// <param name="destGrid">The dest Grid.</param>
        /// <param name="showProgressDialog">The show Progress Dialog.</param>
        /// <param name="frameWidth">The frame Width.</param>
        /// <param name="frameHeight">The frame Height.</param>
        /// <param name="progress">The progress.</param>
        private static void DoFill(IRaster sourceGrid, IRaster destGrid, bool showProgressDialog, int frameWidth, int frameHeight, IProgressHandler progress)
        {
            Trace.WriteLine("DoFill(sourceGrid: '" + sourceGrid.Filename + "\n" + "', mwDestGrid: '" + destGrid.Filename + "\n" + "', ShowProgressDialog: " + showProgressDialog + "\n" + ", FrameWidth: " + frameWidth + "\n" + ", FrameHeight: " + frameHeight + ", callback);\n");
        }

        /// <summary>
        /// Internal File handling
        /// </summary>
        /// <param name="sourceFile">The Source File. </param>
        /// <param name="destFile">The Dest File. </param>
        /// <param name="overwrite">The Overwrite. </param>
        /// <param name="showProgressDialog">The Show Progress Dialog. </param>
        /// <param name="frameWidth">The Frame Width. </param>
        /// <param name="frameHeight">The Frame Height. </param>
        /// <param name="callBack">The CallBack. </param>
        private static void FileFill(string sourceFile, string destFile, bool overwrite, bool showProgressDialog, int frameWidth, int frameHeight, IProgressHandler callBack)
        {
            Trace.WriteLine("Fill(sourceGrid: " + sourceFile + ",\n" + "     mwDestFile: " + destFile + ",\n" + "     Overwrite: " + overwrite + ",\n" + "     ShowProgressDialog: " + showProgressDialog + ",\n" + "     FrameWidth: " + frameWidth + ", \n" + "     FrameHeight: " + frameHeight + ", \n" + "     IProgressHandler");

            callBack?.Progress("Status", 0, "Opening Files");

            var pars = "-z " + Quote(sourceFile) + " -fel " + Quote(destFile);
            var result = RunTaudem("PitRemove.exe", pars, 1, false);

            // todo use tdbChoiceList.numProcesses, tdbChoiceList.ShowTaudemOutput,
            if (result != 0)
            {
                var errMsg = string.Format(MessageStrings.TaudemError, result);
                MessageBox.Show(errMsg, errMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static int GetBasinIndexById(IFeatureSet shed, int id)
        {
            for (var i = 0; i < shed.NumRows(); i++)
            {
                if (int.Parse(shed.DataTable.Rows[i].ItemArray[0].ToString()) == id)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Build drainage tree recursively upstream from outlet or reservoir, stopping at inlet, outlet, or reservoir (ignoring point sources)
        /// </summary>
        /// <param name="shed">Watershed featureset</param>
        /// <param name="outlets">Featureset with the outlets.</param>
        /// <param name="haveOutletsShapefile">if this is false any dsnode not -1 stops the tree </param>
        /// <param name="isRoot">Flag to avoid stopping on root node, which is an outlet or reservoir</param>
        /// <param name="sindx">Row index.</param>
        /// <param name="dsNodeFieldNum">Column of the node.</param>
        /// <param name="us1FieldNum">Column of the left.</param>
        /// <param name="us2FieldNum">Column of the right.</param>
        /// <returns>The created binary tree.</returns>
        private static BinTree GetDrainageFromSubbasin(IFeatureSet shed, IFeatureSet outlets, bool haveOutletsShapefile, bool isRoot, int sindx, int dsNodeFieldNum, int us1FieldNum, int us2FieldNum)
        {
            BinTree left = null;
            BinTree right = null;

            var nodeId = int.Parse(shed.GetCellValue(dsNodeFieldNum, sindx).ToString());
            if ((!isRoot) && (nodeId != -1))
            {
                if (!haveOutletsShapefile)
                {
                    return null;
                }

                if (GetDsNodeType(outlets, nodeId) != DsNode.PointSource)
                {
                    return null;
                }
            }

            var leftId = int.Parse(shed.GetCellValue(us1FieldNum, sindx).ToString());
            var rightId = int.Parse(shed.GetCellValue(us2FieldNum, sindx).ToString());
            if (leftId != -1)
            {
                int idx = GetBasinIndexById(shed, leftId);
                if (idx != -1)
                {
                    left = GetDrainageFromSubbasin(shed, outlets, haveOutletsShapefile, false, idx, dsNodeFieldNum, us1FieldNum, us2FieldNum);
                }
            }

            if (rightId != -1)
            {
                int idx = GetBasinIndexById(shed, rightId);
                if (idx != -1)
                {
                    right = GetDrainageFromSubbasin(shed, outlets, haveOutletsShapefile, false, idx, dsNodeFieldNum, us1FieldNum, us2FieldNum);
                }
            }

            return new BinTree(sindx, left, right);
        }

        /// <summary>
        /// Get type of inlet/outlet for MWSHAPEID equal to ID in outlets shapefile.
        /// </summary>
        /// <param name="outlets">Featureset with the outlets.</param>
        /// <param name="id">Id of the outlet whose type should be returned.</param>
        /// <returns>0 for outlet, 1 for inlet, 2 for reservoir, 3 for point source</returns>
        private static DsNode GetDsNodeType(IFeatureSet outlets, int id)
        {
            string idFieldColumnName = null;

            int inletFieldNum = -1;
            int resFieldNum = -1;
            int srcFieldNum = -1;

            // search for 4 particular columns
            for (int i = 0; i < outlets.DataTable.Columns.Count; i++)
            {
                string currname = outlets.DataTable.Columns[i].ColumnName.ToUpper();
                if (currname == "MWSHAPEID" || currname == "ID")
                {
                    idFieldColumnName = currname;
                }
                else if (currname == "INLET")
                {
                    inletFieldNum = i;
                }
                else if (currname == "RES")
                {
                    resFieldNum = i;
                }
                else if (currname == "PTSOURCE")
                {
                    srcFieldNum = i;
                }
            }

            // The id may represent the row number
            DataRow theRow = outlets.DataTable.Rows[id - 1];

            if (idFieldColumnName != null)
            {
                // The id represents a primary key an may not correspond directly to the row number.
                var query = from row in outlets.DataTable.AsEnumerable() where row.Field<int>(idFieldColumnName) == id select row;

                theRow = query.First();
            }

            var currInlet = (inletFieldNum == -1) ? 0 : (int)theRow[inletFieldNum];
            var currRes = (resFieldNum == -1) ? 0 : (int)theRow[resFieldNum];
            var currSrc = (srcFieldNum == -1) ? 0 : (int)theRow[srcFieldNum];

            if (currInlet == 1)
            {
                if (currSrc == 1)
                    return DsNode.PointSource;

                return DsNode.Inlet;
            }

            if (currRes == 1)
            {
                return DsNode.Reservoir;
            }

            return DsNode.Outlet;
        }

        private static void GetJoinShapesFromSubBasin(Shapefile shed, int sindx, ArrayList shapeIdxList)
        {
            int dsNodeFieldNum = 3;
            int uSlink1FieldNum = 5;
            int uSlink2FieldNum = 6;
            Stack currStack = new Stack();
            shapeIdxList.Clear();

            shapeIdxList.Add(sindx);
            var currLink1Id = int.Parse(shed.GetCellValue(uSlink1FieldNum, sindx).ToString());
            var currLink2Id = int.Parse(shed.GetCellValue(uSlink2FieldNum, sindx).ToString());
            if (currLink1Id != -1)
            {
                int currLinkIdx = GetBasinIndexById(shed, currLink1Id);
                if (currLinkIdx != -1)
                {
                    currStack.Push(currLinkIdx);
                }
            }

            if (currLink2Id != -1)
            {
                int currLinkIdx = GetBasinIndexById(shed, currLink2Id);
                if (currLinkIdx != -1)
                {
                    currStack.Push(currLinkIdx);
                }
            }

            while (currStack.Count > 0)
            {
                var currIdx = int.Parse(currStack.Pop().ToString());
                if (int.Parse(shed.GetCellValue(dsNodeFieldNum, currIdx).ToString()) == -1)
                {
                    shapeIdxList.Add(currIdx);
                    currLink1Id = int.Parse(shed.GetCellValue(uSlink1FieldNum, currIdx).ToString());
                    currLink2Id = int.Parse(shed.GetCellValue(uSlink2FieldNum, currIdx).ToString());
                    if (currLink1Id != -1)
                    {
                        int currLinkIdx = GetBasinIndexById(shed, currLink1Id);
                        if (currLinkIdx != -1)
                        {
                            currStack.Push(currLinkIdx);
                        }
                    }

                    if (currLink2Id != -1)
                    {
                        int currLinkIdx = GetBasinIndexById(shed, currLink2Id);
                        if (currLinkIdx != -1)
                        {
                            currStack.Push(currLinkIdx);
                        }
                    }
                }
            }
        }

        private static void GetStreamElevationPoints(int sindx, IFeatureSet streamShape, IRaster demGrid, out double elevLow, out double elevHigh)
        {
            var shapePoints = streamShape.GetShape(sindx).Geometry.NumPoints;
            elevLow = 10000000;
            elevHigh = -1000000;
            for (var i = 0; i < shapePoints; i += 2)
            {
                var pt = streamShape.GetShape(sindx).Geometry.Coordinates[i];

                RcIndex position = demGrid.ProjToCell(pt.X, pt.Y);
                if (position.IsEmpty()) continue;

                double currVal = demGrid.Value[position.Row, position.Column];
                if (currVal < elevLow)
                {
                    elevLow = currVal;
                }

                if (currVal > elevHigh)
                {
                    elevHigh = currVal;
                }
            }
        }

        private static bool IsUpstreamOfInlet(IFeatureSet shed, IFeatureSet outlets, int sindx)
        {
            const int DsNodeFieldNum = 3;
            const int DsShedFieldNum = 4;
            int currDsid = int.Parse(shed.DataTable.Rows[sindx].ItemArray[DsShedFieldNum].ToString());

            while (currDsid != -1)
            {
                for (int i = 0; i <= shed.DataTable.Rows.Count - 1; i++)
                {
                    if (int.Parse(shed.DataTable.Rows[i].ItemArray[0].ToString()) == currDsid)
                    {
                        currDsid = int.Parse(shed.DataTable.Rows[i].ItemArray[DsShedFieldNum].ToString());

                        int dsNodeVal = int.Parse(shed.DataTable.Rows[i].ItemArray[DsNodeFieldNum].ToString());
                        if (dsNodeVal != -1)
                        {
                            if (GetDsNodeType(outlets, dsNodeVal) == DsNode.Inlet)
                            {
                                return true;
                            }
                        }

                        break;
                    }
                }
            }

            return false;
        }

        private static IFeature MergeAbuttingPolygons(IFeature shape1, IFeature shape2)
        {
            if (shape1 == null) return shape2;
            if (shape2 == null) return shape1;
            IFeature mergeShape = null;

            // check both shapes are single parts
            bool areSingleGeometryFeatures = (shape1.Geometry.NumGeometries == 1) && (shape2.Geometry.NumGeometries == 1);
            if (areSingleGeometryFeatures)
            {
                mergeShape = new Feature(shape1.Geometry.Union(shape2.Geometry));
            }
            else
            {
                Trace.WriteLine("Error - there were multiple parts.");
            }

            return mergeShape;
        }

        /// <summary>
        /// Merge basins.
        /// </summary>
        /// <param name="shed">Featureset whose elements should be merged.</param>
        /// <param name="drainage">Tree used for merging.</param>
        /// <returns>The merge result.</returns>
        private static IFeature MergeBasinsByDrainage(IFeatureSet shed, BinTree drainage)
        {
            if (drainage == null) return null;
            IFeature left = MergeBasinsByDrainage(shed, drainage.Left);
            IFeature right = MergeBasinsByDrainage(shed, drainage.Right);
            IFeature lr = MergeAbuttingPolygons(left, right);
            IFeature outlet = shed.GetShape(drainage.Val);

            // check for multipart shape
            if (outlet.Geometry.NumGeometries > 1)
            {
                Trace.WriteLine("Subbasin " + drainage.Val + " has " + outlet.Geometry.NumGeometries + " parts");
            }
            else
            {
                // check for anticlockwise polygon
                double area = SignedArea(outlet);
                if (area < 0)
                {
                    Trace.WriteLine("Needed to reverse subbasin " + drainage.Val);
                    outlet = ReverseShape(outlet);
                }
            }

            return MergeAbuttingPolygons(lr, outlet);
        }

        /// <summary>
        /// Merge basins using IGeometry, and only converting each shape once.
        /// </summary>
        /// <param name="shed">Shapefile whose elements should be merged.</param>
        /// <param name="drainage">Tree used for merging.</param>
        /// <returns>The resulting geometry.</returns>
        private static IGeometry MergeBasinsByDrainageI(Shapefile shed, BinTree drainage)
        {
            if (drainage == null) return null;
            IGeometry left = MergeBasinsByDrainageI(shed, drainage.Left);
            IGeometry right = MergeBasinsByDrainageI(shed, drainage.Right);
            IFeature outlet = shed.GetShape(drainage.Val);

            // will this work?
            IGeometry outg = outlet.Geometry;
            if (left == null)
            {
                if (right == null)
                    return outg;

                return right.Union(outg);
            }

            if (right == null)
                return left.Union(outg);

            return left.Union(right.Union(outg));
        }

        private static string Quote(string s)
        {
            return "\"" + s + "\"";
        }

        /// <summary>
        /// Reverses a single part shape. Shape must be already checked to have only one part.
        /// </summary>
        /// <param name="shape">Shape that gets reversed.</param>
        /// <returns>The reversed shape.</returns>
        private static IFeature ReverseShape(IFeature shape)
        {
            return new Feature(FeatureType.Polygon, shape.Geometry.Coordinates.Reverse());
        }

        private static int RunDelinStreamGrids(string demGridPath, string pitFillPath, string d8Path, string d8SlopePath, string areaD8Path, string areaDInfPath, string outletsPath, string strahlOrdResultPath, string longestUpslopeResultPath, string totalUpslopeResultPath, string streamGridResultPath, string streamOrdResultPath, string treeDatResultPath, string coordDatResultPath, string streamShapeResultPath, string watershedGridResultPath, int threshold, bool useOutlets, bool useEdgeContamCheck, bool useDinf, int numProcesses, bool showTaudemOutput, IProgressHandler callback)
        {
            Trace.WriteLine("RunDelinStreamGrids(demGridPath: " + demGridPath + "\n " + "pitFillPath: " + pitFillPath + "\n" + "d8Path: " + d8Path + "\n" + "d8SlopePath: " + d8SlopePath + "\n" + "areaD8Path: " + areaD8Path + "\n" + "areaDInfPath: " + areaDInfPath + "\n" + "outletsPath: " + outletsPath + "\n" + "strahlOrdResultPath: " + strahlOrdResultPath + "\n" + "longestUpslopeResultPath: " + longestUpslopeResultPath + "\n" + "totalUpslopeResultPath: " + totalUpslopeResultPath + "\n" + "streamGridResultPath: " + streamGridResultPath + "\n" + "streamOrdResultPath: " + streamOrdResultPath + "\n" + "treeDatResultPath: " + treeDatResultPath + "\n" + "coordDatResultPath: " + coordDatResultPath + "\n" + "streamShapeResultPath: " + streamShapeResultPath + "\n" + "watershedGridResultPath: " + watershedGridResultPath + "\n" + "threshold: " + threshold.ToString() + "\n" + "useOutlets: " + useOutlets + "\n" + "useEdgeContamCheck: " + useEdgeContamCheck + "\n" + "useDinf: " + useDinf + "\n" + "NumProcesses: " + numProcesses.ToString() + "\n" + "ShowTaudemOutput: " + showTaudemOutput.ToString() + "\n" + "callback)");

            // Paul Meems, 23-Aug-2011, Moved to a separate method:
            var result = RunGridNetwork(demGridPath, d8Path, longestUpslopeResultPath, totalUpslopeResultPath, strahlOrdResultPath, outletsPath, useOutlets, numProcesses, showTaudemOutput, callback);
            if (result != 0)
            {
                return result;
            }

            // Paul Meems, 23-Aug-2011, Moved to a separate method:
            result = RunAllStreamDelineation(areaD8Path, streamGridResultPath, threshold, numProcesses, showTaudemOutput, callback);
            if (result != 0)
            {
                return result;
            }

            // Paul Meems, 23-Aug-2011, Moved to a separate method:
            result = RunStreamOrderGridRaster(demGridPath, pitFillPath, d8Path, areaD8Path, outletsPath, streamGridResultPath, streamOrdResultPath, streamShapeResultPath, watershedGridResultPath, coordDatResultPath, treeDatResultPath, useOutlets, numProcesses, showTaudemOutput, callback);

            return result;
        }

        private static string TaudemExeDir()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + @"\Taudem5Exe\";
        }

        #endregion

        #region Classes

        /// <summary>
        /// Binary (integer) trees
        /// </summary>
        public class BinTree
        {
            #region  Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="BinTree"/> class.
            /// Constructor
            /// </summary>
            /// <param name="v">node value</param>
            /// <param name="l">left subtree</param>
            /// <param name="r">right subtree</param>
            public BinTree(int v, BinTree l, BinTree r)
            {
                Val = v;
                Left = l;
                Right = r;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets the left subtree.
            /// </summary>
            public BinTree Left { get; set; }

            /// <summary>
            /// Gets or sets the right subtree.
            /// </summary>
            public BinTree Right { get; set; }

            /// <summary>
            /// Gets or sets the node value.
            /// </summary>
            public int Val { get; set; }

            #endregion

            #region Methods

            /// <summary>
            /// Make a string correponding to (reverse) depth-first traverse.
            /// </summary>
            /// <returns>A string correponding to (reverse) depth-first traverse.</returns>
            public override string ToString()
            {
                string l = Left == null ? string.Empty : ", " + Left;
                string r = Right == null ? string.Empty : ", " + Right;
                string v = Val.ToString();
                return v + r + l;
            }

            #endregion
        }

        #endregion
    }
}