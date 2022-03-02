// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Data;
using System.Runtime.Serialization;
using NetTopologySuite.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// Represents the strongly named DataTable class.
    /// </summary>
    public class FeatureTable : TypedTableBase<FeatureRow>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureTable"/> class.
        /// </summary>
        public FeatureTable()
            : this("MyFeatureTable")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureTable"/> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public FeatureTable(string tableName)
        {
            // Blame microsoft for the virtual call in constructor alert here, not me.
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            TableName = tableName;
            BeginInit();
            InitClass();
            EndInit();

            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureTable"/> class.
        /// This may or may not be required for proper functioning, but is not part of the public API.
        /// </summary>
        /// <param name="table">A DataTable.</param>
        internal FeatureTable(DataTable table)
        {
            TableName = table.TableName;
            if (table.CaseSensitive != table.DataSet.CaseSensitive)
            {
                CaseSensitive = table.CaseSensitive;
            }

            if (table.Locale.ToString() != table.DataSet.Locale.ToString())
            {
                Locale = table.Locale;
            }

            if (table.Namespace != table.DataSet.Namespace)
            {
                Namespace = table.Namespace;
            }

            Prefix = table.Prefix;
            MinimumCapacity = table.MinimumCapacity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureTable"/> class.
        /// This may not work since I stripped off the xml related auto text that seems to rely on an existing dataset.
        /// </summary>
        /// <param name="info">The SerializationInfo.</param>
        /// <param name="context">The StreamingContext.</param>
        protected FeatureTable(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            InitVars();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs after the row has been changed.
        /// </summary>
        public event EventHandler<FeatureRowChangeEvent> FeatureRowChanged;

        /// <summary>
        /// Occurs while the row is changing
        /// </summary>
        public event EventHandler<FeatureRowChangeEvent> FeatureRowChanging;

        /// <summary>
        /// Occurs after the row has been deleted.
        /// </summary>
        public event EventHandler<FeatureRowChangeEvent> FeatureRowDeleted;

        /// <summary>
        /// Occurs while the row is being deleted.
        /// </summary>
        public event EventHandler<FeatureRowChangeEvent> FeatureRowDeleting;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of rows in this table.
        /// </summary>
        [Browsable(false)]
        public int Count => Rows.Count;

        /// <summary>
        /// Gets the Column containing the FID value.
        /// </summary>
        public DataColumn FidColumn { get; private set; }

        /// <summary>
        /// Gets the Column containing the Geometry column.
        /// </summary>
        public DataColumn GeometryColumn { get; private set; }

        /// <summary>
        /// Gets or sets a geometry factory to use when instantiating geometries
        /// from WKB for the rows of this table.
        /// </summary>
        public GeometryFactory GeometryFactory { get; set; }

        #endregion

        #region Indexers

        /// <summary>
        /// Accesses the FeatureRow of this table based on the specified index.
        /// </summary>
        /// <param name="index">The integer index of the feature row to access.</param>
        /// <returns>This FeatureRow.</returns>
        public FeatureRow this[int index] => (FeatureRow)Rows[index];

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified FeatureRow to this FeatureTable.
        /// </summary>
        /// <param name="row">the row to add.</param>
        public void AddFeatureRow(FeatureRow row)
        {
            Rows.Add(row);
        }

        /// <summary>
        /// Generates a new feature row using only the goemetry.
        /// </summary>
        /// <param name="wellKnownBinary">The byte form of the well known text to use in creating a new, otherwise empty row.</param>
        /// <returns>The newly created FeatureRow with the specified well known text.</returns>
        public FeatureRow AddFeatureRow(byte[] wellKnownBinary)
        {
            FeatureRow rowFeatureRow = (FeatureRow)NewRow();
            object[] columnValuesArray = { null, wellKnownBinary };
            rowFeatureRow.ItemArray = columnValuesArray;
            Rows.Add(rowFeatureRow);
            return rowFeatureRow;
        }

        /// <summary>
        /// Generates a new feature row using only the goemetry.
        /// </summary>
        /// <param name="geometry">The byte form of the well known text to use in creating a new, otherwise empty row.</param>
        /// <returns>The newly created FeatureRow with the specified well known text.</returns>
        public FeatureRow AddFeatureRow(Geometry geometry)
        {
            FeatureRow rowFeatureRow = (FeatureRow)NewRow();
            object[] columnValuesArray = { null, geometry.AsBinary() };
            rowFeatureRow.Geometry = geometry;
            rowFeatureRow.ItemArray = columnValuesArray;
            Rows.Add(rowFeatureRow);
            return rowFeatureRow;
        }

        /// <inheritdoc />
        public override DataTable Clone()
        {
            FeatureTable cln = (FeatureTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        /// <summary>
        /// Finds a FeatureRow by the FID field.
        /// </summary>
        /// <param name="fid">The long fid to find.</param>
        /// <returns>A FeatureRow.</returns>
        public FeatureRow FindByFid(long fid)
        {
            return (FeatureRow)Rows.Find(new object[] { fid });
        }

        /// <summary>
        /// Retrieves a newly generated row from this table cast as a FeatureRow.
        /// </summary>
        /// <returns>The FeatureRow class.</returns>
        public FeatureRow NewFeatureRow()
        {
            return (FeatureRow)NewRow();
        }

        /// <summary>
        /// Removes the specified row from the table.
        /// </summary>
        /// <param name="row">The FeatureRow to remove.</param>
        public void RemoveFeatureRow(FeatureRow row)
        {
            Rows.Remove(row);
        }

        /// <summary>
        /// Initializes the FidColumn and GeometryColumn.
        /// </summary>
        internal void InitVars()
        {
            FidColumn = Columns["FID"];
            GeometryColumn = Columns["GEOMETRY"];
        }

        /// <inheritdoc />
        protected override DataTable CreateInstance()
        {
            return new FeatureTable();
        }

        /// <inheritdoc />
        protected override Type GetRowType()
        {
            return typeof(FeatureRow);
        }

        /// <inheritdoc />
        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new FeatureRow(builder);
        }

        /// <inheritdoc />
        protected override void OnRowChanged(DataRowChangeEventArgs e)
        {
            base.OnRowChanged(e);
            FeatureRowChanged?.Invoke(this, new FeatureRowChangeEvent((FeatureRow)e.Row, e.Action));
        }

        /// <inheritdoc />
        protected override void OnRowChanging(DataRowChangeEventArgs e)
        {
            base.OnRowChanging(e);
            FeatureRowChanging?.Invoke(this, new FeatureRowChangeEvent((FeatureRow)e.Row, e.Action));
        }

        /// <inheritdoc />
        protected override void OnRowDeleted(DataRowChangeEventArgs e)
        {
            base.OnRowDeleted(e);
            FeatureRowDeleted?.Invoke(this, new FeatureRowChangeEvent((FeatureRow)e.Row, e.Action));
        }

        /// <inheritdoc />
        protected override void OnRowDeleting(DataRowChangeEventArgs e)
        {
            base.OnRowDeleting(e);
            FeatureRowDeleting?.Invoke(this, new FeatureRowChangeEvent((FeatureRow)e.Row, e.Action));
        }

        private void InitClass()
        {
            FidColumn = new DataColumn("FID", typeof(long), null, MappingType.Element);
            Columns.Add(FidColumn);
            GeometryColumn = new DataColumn("GEOMETRY", typeof(byte[]), null, MappingType.Element);
            Columns.Add(GeometryColumn);
            Constraints.Add(new UniqueConstraint("Constraint1", new[] { FidColumn }, true));
            FidColumn.AutoIncrement = true;
            FidColumn.AutoIncrementSeed = -1;
            FidColumn.AutoIncrementStep = -1;
            FidColumn.AllowDBNull = false;
            FidColumn.Unique = true;
        }

        #endregion
    }
}