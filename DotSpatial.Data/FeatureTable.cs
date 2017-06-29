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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/5/2010 6:59:48 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Data;
using System.Runtime.Serialization;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    ///Represents the strongly named DataTable class.
    ///</summary>
    public class FeatureTable : TypedTableBase<FeatureRow>
    {
        #region Events

        /// <summary>
        /// Occurs while the row is changing
        /// </summary>
        public event EventHandler<FeatureRowChangeEvent> FeatureRowChanging;

        /// <summary>
        /// Occurs after the row has been changed.
        /// </summary>
        public event EventHandler<FeatureRowChangeEvent> FeatureRowChanged;

        /// <summary>
        /// Occurs while the row is being deleted.
        /// </summary>
        public event EventHandler<FeatureRowChangeEvent> FeatureRowDeleting;

        /// <summary>
        /// Occurs after the row has been deleted.
        /// </summary>
        public event EventHandler<FeatureRowChangeEvent> FeatureRowDeleted;

        #endregion

        #region Private Variables

        private DataColumn _columnFID;

        private DataColumn _columnGeometry;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of a FeatureTable class.
        /// </summary>
        public FeatureTable()
            : this("MyFeatureTable")
        {
        }

        /// <summary>
        /// Initializes a new instance of a FeatureTable class.
        /// </summary>
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
            FeatureRow rowFeatureRow = ((FeatureRow)(NewRow()));
            object[] columnValuesArray = new object[] {
                                                          null,
                                                          wellKnownBinary};
            rowFeatureRow.ItemArray = columnValuesArray;
            Rows.Add(rowFeatureRow);
            return rowFeatureRow;
        }

        /// <summary>
        /// Retrieves a newly generated row from this table cast as a FeatureRow.
        /// </summary>
        /// <returns>The FeatureRow class.</returns>
        public FeatureRow NewFeatureRow()
        {
            return ((FeatureRow)(NewRow()));
        }

        /// <inheritdocs/>
        public override DataTable Clone()
        {
            FeatureTable cln = ((FeatureTable)(base.Clone()));
            cln.InitVars();
            return cln;
        }

        /// <summary>
        /// Finds a FeatureRow by the FID field.
        /// </summary>
        /// <param name="fid">The long fid to find</param>
        /// <returns>A FeatureRow</returns>
        public FeatureRow FindByFID(long fid)
        {
            return ((FeatureRow)(Rows.Find(new object[] { fid })));
        }

        /// <summary>
        /// Removes the specified row from the table.
        /// </summary>
        /// <param name="row">The FeatureRow to remove.</param>
        public void RemoveFeatureRow(FeatureRow row)
        {
            Rows.Remove(row);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The integer number of rows in this table.
        /// </summary>
        [Browsable(false)]
        public int Count
        {
            get
            {
                return Rows.Count;
            }
        }

        /// <summary>
        /// The Column containing the FID value.
        /// </summary>
        public DataColumn FidColumn
        {
            get
            {
                return _columnFID;
            }
        }

        /// <summary>
        /// The Column containing the Geometry column.
        /// </summary>
        public DataColumn GeometryColumn
        {
            get
            {
                return _columnGeometry;
            }
        }

        /// <summary>
        /// Optionally gets or sets a geometry factory to use when instantiating geometries
        /// from WKB for the rows of this table.
        /// </summary>
        public IGeometryFactory GeometryFactory { get; set; }

        /// <summary>
        /// Accesses the FeatureRow of this table based on the specified index.
        /// </summary>
        /// <param name="index">The integer index of the feature row to access.</param>
        /// <returns>This FeatureRow</returns>
        public FeatureRow this[int index]
        {
            get
            {
                return ((FeatureRow)(Rows[index]));
            }
        }

        /// <summary>
        /// Generates a new feature row using only the goemetry.
        /// </summary>
        /// <param name="geometry">The byte form of the well known text to use in creating a new, otherwise empty row.</param>
        /// <returns>The newly created FeatureRow with the specified well known text.</returns>
        public FeatureRow AddFeatureRow(IGeometry geometry)
        {
            FeatureRow rowFeatureRow = ((FeatureRow)(NewRow()));
            object[] columnValuesArray = new object[] {
                                                          null, geometry.ToBinary()};
            rowFeatureRow.Geometry = geometry;
            rowFeatureRow.ItemArray = columnValuesArray;
            Rows.Add(rowFeatureRow);
            return rowFeatureRow;
        }

        #endregion

        #region protected or private Methods

        /// <inheritdocs/>
        protected override DataTable CreateInstance()
        {
            return new FeatureTable();
        }

        private void InitClass()
        {
            _columnFID = new DataColumn("FID", typeof(long), null, MappingType.Element);
            Columns.Add(_columnFID);
            _columnGeometry = new DataColumn("GEOMETRY", typeof(byte[]), null, MappingType.Element);
            Columns.Add(_columnGeometry);
            Constraints.Add(new UniqueConstraint("Constraint1", new[] {
                                                                          _columnFID}, true));
            _columnFID.AutoIncrement = true;
            _columnFID.AutoIncrementSeed = -1;
            _columnFID.AutoIncrementStep = -1;
            _columnFID.AllowDBNull = false;
            _columnFID.Unique = true;
        }

        /// <inheritdocs/>
        protected override Type GetRowType()
        {
            return typeof(FeatureRow);
        }

        /// <inheritdocs/>
        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new FeatureRow(builder);
        }

        /// <inheritdocs/>
        protected override void OnRowChanged(DataRowChangeEventArgs e)
        {
            base.OnRowChanged(e);
            if ((FeatureRowChanged != null))
            {
                FeatureRowChanged(this, new FeatureRowChangeEvent(((FeatureRow)(e.Row)), e.Action));
            }
        }

        /// <inheritdocs/>
        protected override void OnRowChanging(DataRowChangeEventArgs e)
        {
            base.OnRowChanging(e);
            if ((FeatureRowChanging != null))
            {
                FeatureRowChanging(this, new FeatureRowChangeEvent(((FeatureRow)(e.Row)), e.Action));
            }
        }

        /// <inheritdocs/>
        protected override void OnRowDeleted(DataRowChangeEventArgs e)
        {
            base.OnRowDeleted(e);
            if ((FeatureRowDeleted != null))
            {
                FeatureRowDeleted(this, new FeatureRowChangeEvent(((FeatureRow)(e.Row)), e.Action));
            }
        }

        /// <inheritdocs/>
        protected override void OnRowDeleting(DataRowChangeEventArgs e)
        {
            base.OnRowDeleting(e);
            if ((FeatureRowDeleting != null))
            {
                FeatureRowDeleting(this, new FeatureRowChangeEvent(((FeatureRow)(e.Row)), e.Action));
            }
        }

        #endregion

        #region Internal

        /// <summary>
        /// This may or may not be required for proper functioning, but is not part of the public API.
        /// </summary>
        /// <param name="table">A DataTable</param>
        internal FeatureTable(DataTable table)
        {
            TableName = table.TableName;
            if ((table.CaseSensitive != table.DataSet.CaseSensitive))
            {
                CaseSensitive = table.CaseSensitive;
            }
            if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
            {
                Locale = table.Locale;
            }
            if ((table.Namespace != table.DataSet.Namespace))
            {
                Namespace = table.Namespace;
            }
            Prefix = table.Prefix;
            MinimumCapacity = table.MinimumCapacity;
        }

        /// <summary>
        /// This may not work since I stripped off the xml related auto text that seems to rely on an existing dataset
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected FeatureTable(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
            InitVars();
        }

        internal void InitVars()
        {
            _columnFID = Columns["FID"];
            _columnGeometry = Columns["GEOMETRY"];
        }

        #endregion
    }
}