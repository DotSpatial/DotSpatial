// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DotSpatial.Projections;
using NetTopologySuite.IO;
using OSGeo.OGR;
using OSGeo.OSR;
using OgrFeature = OSGeo.OGR.Feature;

namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    ///  OgrDatareader provide readonly forward only access to OsGeo data sources accessible though the OGR project.
    /// </summary>
    public class OgrDataReader : IDisposable
    {
        #region Fields

        private readonly string _fileName;

        private readonly DataSource _ogrDataSource;

        #endregion

        #region Constructors

        static OgrDataReader()
        {
            GdalConfiguration.ConfigureOgr();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OgrDataReader"/> class.
        /// </summary>
        /// <param name="sDataSource">The path of the data source file.</param>
        public OgrDataReader(string sDataSource)
        {
            _ogrDataSource = Ogr.Open(sDataSource, 0);
            _fileName = sDataSource;
        }

        #endregion

        #region Methods

        /// <summary>
        /// This disposes the underlying data source.
        /// </summary>
        public void Dispose()
        {
            _ogrDataSource.Dispose();
        }

        /// <summary>
        /// Gets the layers of the given file.
        /// </summary>
        /// <returns>List with the layers of the file.</returns>
        public IList<IFeatureSet> GetLayers()
        {
            var drv = _ogrDataSource.GetDriver().GetName().ToLower();

            if (drv == "dxf") return GetDxfLayers();

            return GetOtherLayers();
        }

        private static void AddColumns(IFeatureSet fs, IList<Column> schemaTable)
        {
            foreach (Column column in schemaTable)
            {
                string sFieldName = column.Name;

                int uniqueNumber = 1;
                string uniqueName = sFieldName;
                while (fs.DataTable.Columns.Contains(uniqueName))
                {
                    uniqueName = sFieldName + uniqueNumber;
                    uniqueNumber++;
                }

                fs.DataTable.Columns.Add(new DataColumn(uniqueName, column.Type));
            }

            // add style and label column
            if (!fs.DataTable.Columns.Contains("style"))
            {
                fs.DataTable.Columns.Add(new DataColumn("style", typeof(string)));
            }
        }

        private static void AddFeatures(IFeatureSet fs, Layer layer, FeatureDefn ogrFeatureDefinition, IList<Column> schema)
        {
            var wkbReader = new WKBReader();
            OgrFeature ogrFeature = layer.GetNextFeature();

            var fieldCount = ogrFeatureDefinition.GetFieldCount();

            while (ogrFeature != null)
            {
                var wkbGeometry = GetGeometry(ogrFeature);
                var geometry = wkbReader.Read(wkbGeometry);

                if (geometry != null && geometry.IsValid)
                {
                    IFeature feature = new Feature(geometry);
                    if (fs.Features.Count == 0 || feature.FeatureType == fs.FeatureType)
                    {
                        fs.Features.Add(feature);
                        for (int i = 0; i < fieldCount; i++)
                        {
                            feature.DataRow[i] = GetValue(i, schema, ogrFeature) ?? DBNull.Value;
                        }
                    }
                }

                ogrFeature = layer.GetNextFeature();
            }
        }

        private static void AddFeatures(Dictionary<FeatureType, IFeatureSet> fs, Layer layer, FeatureDefn ogrFeatureDefinition, IList<Column> schema)
        {
            var styles = new Dictionary<Tuple<string, FeatureType>, object>();

            var wkbReader = new WKBReader();
            OgrFeature ogrFeature = layer.GetNextFeature();

            var fieldCount = ogrFeatureDefinition.GetFieldCount();

            while (ogrFeature != null)
            {
                var wkbGeometry = GetGeometry(ogrFeature);
                var geometry = wkbReader.Read(wkbGeometry);

                if (geometry != null && geometry.IsValid)
                {
                    IFeature feature = new Feature(geometry);
                    if (feature.FeatureType != FeatureType.Unspecified)
                    {
                        fs[feature.FeatureType].Features.Add(feature);
                        for (int i = 0; i < fieldCount; i++)
                        {
                            feature.DataRow[i] = GetValue(i, schema, ogrFeature) ?? DBNull.Value;
                        }

                        var str = ogrFeature.GetStyleString();
                        feature.DataRow["style"] = str;
                        var tup = Tuple.Create(str, feature.FeatureType);
                        if (!styles.ContainsKey(tup))
                        {
                            // add the style to the layer
                            styles.Add(tup, string.Empty);
                        }
                    }
                }

                ogrFeature = layer.GetNextFeature();
            }
        }

        private static IList<Column> BuildSchemaTable(FeatureDefn ogrFeatureDefinition)
        {
            IList<Column> schema = new List<Column>();

            for (int i = 0; i < ogrFeatureDefinition.GetFieldCount(); i++)
            {
                FieldDefn f = ogrFeatureDefinition.GetFieldDefn(i);
                schema.Add(new Column(f.GetName(), f.GetFieldType()));
            }

            return schema;
        }

        /// <summary>
        /// Gets the features geometry as Wkb.
        /// </summary>
        /// <param name="feature">Feature whose geometry is returned.</param>
        /// <returns>The features geometry.</returns>
        private static byte[] GetGeometry(OgrFeature feature)
        {
            Geometry ogrGeometry = feature.GetGeometryRef();
            ogrGeometry.FlattenTo2D();
            byte[] wkbGeometry = new byte[ogrGeometry.WkbSize()];
            ogrGeometry.ExportToWkb(wkbGeometry, wkbByteOrder.wkbXDR);

            return wkbGeometry;
        }

        /// <summary>
        /// Adds the projection info of the OGR layer to the FeatureSet.
        /// </summary>
        /// <param name="layer">The layer that is used to create the FeatureSet.</param>
        /// <returns>Null on error otherwise the ProjectionInfo object that contains the data of the layers proj4 string.</returns>
        private static ProjectionInfo GetProjectionInfo(Layer layer)
        {
            try
            {
                SpatialReference osrSpatialref = layer.GetSpatialRef();
                if (osrSpatialref != null)
                {
                    string sProj4String;
                    osrSpatialref.ExportToProj4(out sProj4String);
                    return ProjectionInfo.FromProj4String(sProj4String);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the value of the field.
        /// </summary>
        /// <param name="i">Index of the field.</param>
        /// <param name="schema">Schema that contains the field type.</param>
        /// <param name="feature">The feature whose value gets returned.</param>
        /// <returns>The field value as object.</returns>
        private static object GetValue(int i, IList<Column> schema, OgrFeature feature)
        {
            switch (schema[i].FieldType)
            {
                case FieldType.OFTString: return feature.GetFieldAsString(i);

                case FieldType.OFTInteger: return feature.GetFieldAsInteger(i);

                case FieldType.OFTDateTime:
                    {
                        int year;
                        int month;
                        int day;

                        int h;
                        int m;
                        int s;
                        int flag;

                        feature.GetFieldAsDateTime(i, out year, out month, out day, out h, out m, out s, out flag);
                        try
                        {
                            return new DateTime(year, month, day, h, m, s);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            return DateTime.MinValue;
                        }
                    }

                case FieldType.OFTReal: return feature.GetFieldAsDouble(i);

                default: return null;
            }
        }

        private static Type TranslateOgrType(FieldType ogrType)
        {
            switch (ogrType)
            {
                case FieldType.OFTInteger: return typeof(int);

                case FieldType.OFTReal: return typeof(double);

                case FieldType.OFTWideString:
                case FieldType.OFTString: return typeof(string);

                case FieldType.OFTBinary: return typeof(byte[]);

                case FieldType.OFTDate:
                case FieldType.OFTTime:
                case FieldType.OFTDateTime: return typeof(DateTime);

                case FieldType.OFTIntegerList:
                case FieldType.OFTRealList:
                case FieldType.OFTStringList:
                case FieldType.OFTWideStringList: throw new NotSupportedException("Type not supported: " + ogrType);

                default: throw new NotSupportedException("Type not supported: " + ogrType);
            }
        }

        private IList<IFeatureSet> GetDxfLayers()
        {
            var liste = new Dictionary<FeatureType, IFeatureSet>
                        {
                            { FeatureType.Point, new FeatureSet(FeatureType.Point) },
                            { FeatureType.Polygon, new FeatureSet(FeatureType.Polygon) },
                            { FeatureType.MultiPoint, new FeatureSet(FeatureType.MultiPoint) },
                            { FeatureType.Line, new FeatureSet(FeatureType.Line) }
                        };

            using (var layer = _ogrDataSource.GetLayerByIndex(0)) // dxf files contain only one layer called entities, the layers seen in programs are defined by the Layer column
            using (var ogrFeatureDefinition = layer.GetLayerDefn())
            {
                var schema = BuildSchemaTable(ogrFeatureDefinition);
                var projInfo = GetProjectionInfo(layer);

                foreach (var entry in liste)
                {
                    AddColumns(entry.Value, schema);
                    if (projInfo != null) entry.Value.Projection = projInfo;
                    entry.Value.Name = entry.Value.FeatureType.ToString();
                }

                AddFeatures(liste, layer, ogrFeatureDefinition, schema);
            }

            return liste.Where(_ => _.Value.Features.Count > 0).Select(_ => _.Value).ToList();
        }

        /// <summary>
        /// Gets the layers of the given file.
        /// </summary>
        /// <returns>The list of featuresets contained in this file.</returns>
        private IList<IFeatureSet> GetOtherLayers()
        {
            using (var layer = _ogrDataSource.GetLayerByIndex(0)) // dxf files contain only one layer called entities, the layers seen in programs are defined by the Layer column
            using (var ogrFeatureDefinition = layer.GetLayerDefn())
            {
                var schema = BuildSchemaTable(ogrFeatureDefinition);
                var projInfo = GetProjectionInfo(layer);

                IFeatureSet fs = new FeatureSet();
                AddColumns(fs, schema);
                if (projInfo != null) fs.Projection = projInfo;
                fs.Name = Path.GetFileNameWithoutExtension(_fileName);
                fs.Filename = _fileName;

                AddFeatures(fs, layer, ogrFeatureDefinition, schema);
                return new List<IFeatureSet>
                       {
                           fs
                       };
            }
        }

        #endregion

        #region Classes

        private class Column
        {
            #region Constructors

            public Column(string name, FieldType fieldType)
            {
                Name = name;
                FieldType = fieldType;
                Type = TranslateOgrType(fieldType);
            }

            #endregion

            #region Properties

            public FieldType FieldType { get; }

            public string Name { get; }

            public Type Type { get; }

            #endregion
        }

        #endregion
    }
}