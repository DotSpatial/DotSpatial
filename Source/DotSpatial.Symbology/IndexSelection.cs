// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/5/2009 9:37:56 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DotSpatial.Data;
using DotSpatial.NTSExtension;
using GeoAPI.Geometries;

namespace DotSpatial.Symbology
{
    public class IndexSelection : Changeable, IIndexSelection
    {
        #region IIndexSelection Members

        /// <summary>
        /// Adds all of the specified index values to the selection
        /// </summary>
        /// <param name="indices">The indices to add</param>
        public void AddRange(IEnumerable<int> indices)
        {
            foreach (int index in indices)
            {
                _layer.DrawnStates[index].Selected = SelectionState;
            }
            OnChanged();
        }

        /// <summary>
        /// Add REgion
        /// </summary>
        /// <param name="region"></param>
        /// <param name="affectedArea"></param>
        /// <returns></returns>
        public bool AddRegion(Envelope region, out Envelope affectedArea)
        {
            bool added = false;
            SuspendChanges();
            Extent affected = new Extent();
            IPolygon reg = region.ToPolygon();

            for (int shp = 0; shp < _layer.DrawnStates.Length; shp++)
            {
                if (RegionCategory != null && _layer.DrawnStates[shp].Category != RegionCategory) continue;
                bool doAdd = false;
                ShapeRange shape = _shapes[shp];

                if (SelectionMode == SelectionMode.Intersects)
                {
                    // Prevent geometry creation (which is slow) and use ShapeRange instead
                    ShapeRange env = new ShapeRange(region);
                    if (env.Intersects(shape))
                    {
                        _layer.DrawnStates[shp].Selected = SelectionState;
                        affected.ExpandToInclude(shape.Extent);
                        added = true;
                        OnChanged();
                    }
                }
                else if (SelectionMode == SelectionMode.IntersectsExtent)
                {
                    if (shape.Extent.Intersects(region))
                    {
                        _layer.DrawnStates[shp].Selected = SelectionState;
                        affected.ExpandToInclude(shape.Extent);
                        added = true;
                        OnChanged();
                    }
                }
                else if (SelectionMode == SelectionMode.ContainsExtent)
                {
                    if (shape.Extent.Within(region))
                    {
                        _layer.DrawnStates[shp].Selected = SelectionState;
                        affected.ExpandToInclude(shape.Extent);
                        added = true;
                        OnChanged();
                    }
                }
                else if (SelectionMode == SelectionMode.Disjoint)
                {
                    if (shape.Extent.Intersects(region))
                    {
                        IGeometry g = _layer.DataSet.GetFeature(shp).Geometry;
                        if (reg.Disjoint(g)) doAdd = true;
                    }
                    else
                    {
                        doAdd = true;
                    }
                }
                else
                {
                    if (!shape.Extent.Intersects(region)) continue;
                    IGeometry geom = _layer.DataSet.GetFeature(shp).Geometry;
                    switch (SelectionMode)
                    {
                        case SelectionMode.Contains:
                            if (shape.Extent.Within(region))
                            {
                                doAdd = true;
                            }
                            else if (shape.Extent.Intersects(region))
                            {
                                if (reg.Contains(geom)) doAdd = true;
                            }
                            break;
                        case SelectionMode.CoveredBy:
                            if (reg.CoveredBy(geom)) doAdd = true;
                            break;
                        case SelectionMode.Covers:
                            if (reg.Covers(geom)) doAdd = true;
                            break;
                        case SelectionMode.Intersects:
                            if (shape.Extent.Within(region))
                            {
                                doAdd = true;
                            }
                            else if (shape.Extent.Intersects(region))
                            {
                                if (reg.Intersects(geom)) doAdd = true;
                            }
                            break;
                        case SelectionMode.Overlaps:
                            if (reg.Overlaps(geom)) doAdd = true;
                            break;
                        case SelectionMode.Touches:
                            if (reg.Touches(geom)) doAdd = true;
                            break;
                        case SelectionMode.Within:
                            if (reg.Within(geom)) doAdd = true;
                            break;
                    }
                }
                if (!doAdd) continue;
                OnChanged();
                _layer.DrawnStates[shp].Selected = SelectionState;
                affected.ExpandToInclude(shape.Extent);
                added = true;
            }

            ResumeChanges();
            affectedArea = affected.ToEnvelope();
            return added;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="region"></param>
        /// <param name="affectedArea"></param>
        /// <returns></returns>
        public bool InvertSelection(Envelope region, out Envelope affectedArea)
        {
            SuspendChanges();
            bool flipped = false;
            Extent affected = new Extent();
            IPolygon reg = region.ToPolygon();
            for (int shp = 0; shp < _layer.DrawnStates.Length; shp++)
            {
                if (RegionCategory != null && _layer.DrawnStates[shp].Category != RegionCategory) continue;
                bool doFlip = false;
                ShapeRange shape = _shapes[shp];
                if (SelectionMode == SelectionMode.Intersects)
                {
                    // Prevent geometry creation (which is slow) and use ShapeRange instead
                    ShapeRange env = new ShapeRange(region);
                    if (env.Intersects(shape))
                    {
                        _layer.DrawnStates[shp].Selected = !_layer.DrawnStates[shp].Selected;
                        affected.ExpandToInclude(shape.Extent);
                        flipped = true;
                        OnChanged();
                    }
                }
                else if (SelectionMode == SelectionMode.IntersectsExtent)
                {
                    if (shape.Extent.Intersects(region))
                    {
                        _layer.DrawnStates[shp].Selected = !_layer.DrawnStates[shp].Selected;
                        affected.ExpandToInclude(shape.Extent);
                        flipped = true;
                        OnChanged();
                    }
                }
                else if (SelectionMode == SelectionMode.ContainsExtent)
                {
                    if (shape.Extent.Within(region))
                    {
                        _layer.DrawnStates[shp].Selected = !_layer.DrawnStates[shp].Selected;
                        affected.ExpandToInclude(shape.Extent);
                        flipped = true;
                        OnChanged();
                    }
                }
                else if (SelectionMode == SelectionMode.Disjoint)
                {
                    if (shape.Extent.Intersects(region))
                    {
                        IGeometry g = _layer.DataSet.GetFeature(shp).Geometry;
                        if (reg.Disjoint(g)) doFlip = true;
                    }
                    else
                    {
                        doFlip = true;
                    }
                }
                else
                {
                    if (!shape.Extent.Intersects(region)) continue;
                    IGeometry geom = _layer.DataSet.GetFeature(shp).Geometry;  // only get this if envelopes intersect
                    switch (SelectionMode)
                    {
                        case SelectionMode.Contains:
                            if (region.Intersects(geom.EnvelopeInternal))
                            {
                                if (reg.Contains(geom)) doFlip = true;
                            }
                            break;
                        case SelectionMode.CoveredBy:
                            if (reg.CoveredBy(geom)) doFlip = true;
                            break;
                        case SelectionMode.Covers:
                            if (reg.Covers(geom)) doFlip = true;
                            break;
                        case SelectionMode.Intersects:
                            if (region.Intersects(geom.EnvelopeInternal))
                            {
                                if (reg.Intersects(geom)) doFlip = true;
                            }
                            break;
                        case SelectionMode.Overlaps:
                            if (reg.Overlaps(geom)) doFlip = true;
                            break;
                        case SelectionMode.Touches:
                            if (reg.Touches(geom)) doFlip = true;
                            break;
                        case SelectionMode.Within:
                            if (reg.Within(geom)) doFlip = true;
                            break;
                    }
                }
                if (!doFlip) continue;
                flipped = true;
                OnChanged();
                _layer.DrawnStates[shp].Selected = !_layer.DrawnStates[shp].Selected;
                affected.ExpandToInclude(shape.Extent);
            }
            affectedArea = affected.ToEnvelope();
            ResumeChanges();
            return flipped;
        }

        /// <summary>
        /// Attempts to remove all the members from the collection.  If
        /// one of the specified indices is outside the range of possible
        /// values, this returns false, even if others were successfully removed.
        /// This will also return false if none of the states were changed.
        /// </summary>
        /// <param name="indices">The indices to remove</param>
        /// <returns></returns>
        public bool RemoveRange(IEnumerable<int> indices)
        {
            bool problem = false;
            bool changed = false;
            FastDrawnState[] drawnStates = _layer.DrawnStates;
            foreach (int index in indices)
            {
                if (index < 0 || index > drawnStates.Length)
                {
                    problem = true;
                }
                else
                {
                    if (drawnStates[index].Selected != !SelectionState) changed = true;
                    drawnStates[index].Selected = !SelectionState;
                }
            }
            return !problem && changed;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="region"></param>
        /// <param name="affectedArea"></param>
        /// <returns></returns>
        public bool RemoveRegion(Envelope region, out Envelope affectedArea)
        {
            bool removed = false;
            SuspendChanges();

            Extent affected = new Extent();
            IPolygon reg = region.ToPolygon();

            for (int shp = 0; shp < _layer.DrawnStates.Length; shp++)
            {
                if (RegionCategory != null && _layer.DrawnStates[shp].Category != RegionCategory) continue;
                bool doRemove = false;
                ShapeRange shape = _shapes[shp];
                if (SelectionMode == SelectionMode.Intersects)
                {
                    // Prevent geometry creation (which is slow) and use ShapeRange instead
                    ShapeRange env = new ShapeRange(region);
                    if (env.Intersects(shape))
                    {
                        _layer.DrawnStates[shp].Selected = !SelectionState;
                        affected.ExpandToInclude(shape.Extent);
                        removed = true;
                    }
                }
                else if (SelectionMode == SelectionMode.IntersectsExtent)
                {
                    if (shape.Extent.Intersects(region))
                    {
                        _layer.DrawnStates[shp].Selected = !SelectionState;
                        affected.ExpandToInclude(shape.Extent);
                        removed = true;
                    }
                }
                else if (SelectionMode == SelectionMode.ContainsExtent)
                {
                    if (shape.Extent.Within(region))
                    {
                        _layer.DrawnStates[shp].Selected = !SelectionState;
                        affected.ExpandToInclude(shape.Extent);
                        removed = true;
                    }
                }
                else if (SelectionMode == SelectionMode.Disjoint)
                {
                    if (shape.Extent.Intersects(region))
                    {
                        IGeometry g = _layer.DataSet.Features[shp].Geometry;
                        if (reg.Disjoint(g)) doRemove = true;
                    }
                    else
                    {
                        doRemove = true;
                    }
                }
                else
                {
                    if (!shape.Extent.Intersects(region)) continue;
                    IGeometry geom = _layer.DataSet.Features[shp].Geometry;
                    switch (SelectionMode)
                    {
                        case SelectionMode.Contains:
                            if (shape.Extent.Within(region))
                            {
                                doRemove = true;
                            }
                            else if (shape.Extent.Intersects(region))
                            {
                                if (reg.Contains(geom)) doRemove = true;
                            }
                            break;
                        case SelectionMode.CoveredBy:
                            if (reg.CoveredBy(geom)) doRemove = true;
                            break;
                        case SelectionMode.Covers:
                            if (reg.Covers(geom)) doRemove = true;
                            break;
                        case SelectionMode.Intersects:
                            if (shape.Extent.Within(region))
                            {
                                doRemove = true;
                            }
                            else if (shape.Extent.Intersects(region))
                            {
                                if (reg.Intersects(geom)) doRemove = true;
                            }
                            break;
                        case SelectionMode.Overlaps:
                            if (reg.Overlaps(geom)) doRemove = true;
                            break;
                        case SelectionMode.Touches:
                            if (reg.Touches(geom)) doRemove = true;
                            break;
                        case SelectionMode.Within:
                            if (reg.Within(geom)) doRemove = true;
                            break;
                    }
                }
                if (!doRemove) continue;
                OnChanged();
                _layer.DrawnStates[shp].Selected = !SelectionState;
                affected.ExpandToInclude(shape.Extent);
                removed = true;
            }
            affectedArea = affected.ToEnvelope();
            ResumeChanges();
            return removed;
        }

        /// <summary>
        /// Returns a new featureset based on the features in this collection
        /// </summary>
        /// <returns></returns>
        public FeatureSet ToFeatureSet()
        {
            FeatureSet fs = new FeatureSet(ToFeatureList());
            fs.Projection = _layer.DataSet.Projection;
            return fs;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<IFeature> ToFeatureList()
        {
            List<IFeature> result = new List<IFeature>();
            FastDrawnState[] drawnStates = _layer.DrawnStates;
            for (int shp = 0; shp < drawnStates.Length; shp++)
            {
                if (drawnStates[shp].Selected == SelectionState)
                {
                    result.Add(_layer.DataSet.GetFeature(shp));
                }
            }
            return result;
        }

        /// <summary>
        /// Calculates the envelope of this collection
        /// </summary>
        public Envelope Envelope
        {
            get
            {
                if (!_layer.DrawnStatesNeeded) return new Envelope();
                Extent ext = new Extent();
                FastDrawnState[] drawnStates = _layer.DrawnStates;
                for (int shp = 0; shp < drawnStates.Length; shp++)
                {
                    if (drawnStates[shp].Selected == SelectionState)
                    {
                        ext.ExpandToInclude(_shapes[shp].Extent);
                    }
                }
                return ext.ToEnvelope();
            }
        }

        /// <summary>
        /// Selection Mode controls how envelopes are treated when working with geometries.
        /// </summary>
        public SelectionMode SelectionMode { get; set; }

        /// <summary>
        /// Gets or sets whether this should work as "Selected" indices (true) or
        /// "UnSelected" indices (false).
        /// </summary>
        public bool SelectionState { get; set; }

        /// <inheritdoc />
        public DataTable GetAttributes(int startIndex, int numRows)
        {
            return GetAttributes(startIndex, numRows, _layer.DataSet.GetColumns().Select(d => d.ColumnName));
        }

        /// <inheritdoc />
        public DataTable GetAttributes(int startIndex, int numRows, IEnumerable<string> fieldNames)
        {
            var c = new AttributeCache(_layer.DataSet, numRows);
            var fn = new HashSet<string>(fieldNames);
            var result = new DataTable();
            foreach (DataColumn col in _layer.DataSet.GetColumns())
            {
                if (fn.Contains(col.ColumnName))
                {
                    result.Columns.Add(col);
                }
            }

            int i = 0;
            FastDrawnState[] drawnStates = _layer.DrawnStates;
            for (int fid = 0; fid < drawnStates.Length; fid++)
            {
                if (drawnStates[fid].Selected)
                {
                    i++;
                    if (i < startIndex) continue;
                    DataRow dr = result.NewRow();
                    Dictionary<string, object> vals = c.RetrieveElement(fid);
                    foreach (KeyValuePair<string, object> pair in vals)
                    {
                        if (fn.Contains(pair.Key))
                            dr[pair.Key] = pair.Value;
                    }
                    result.Rows.Add(dr);
                    if (i > startIndex + numRows) break;
                }
            }
            return result;
        }

        /// <inheritdoc />
        public void SetAttributes(int startIndex, DataTable values)
        {
            FastDrawnState[] drawnStates = _layer.DrawnStates;
            int sind = -1;
            for (int fid = 0; fid < drawnStates.Length; fid++)
            {
                if (drawnStates[fid].Selected)
                {
                    sind++;
                    if (sind > startIndex + values.Rows.Count)
                    {
                        break;
                    }
                    if (sind >= startIndex)
                    {
                        _layer.DataSet.Edit(fid, values.Rows[sind]);
                    }
                }
            }
        }

        /// <inheritdoc />
        public int NumRows()
        {
            return Count;
        }

        /// <inheritdoc />
        public DataColumn[] GetColumns()
        {
            { return _layer.DataSet.GetColumns(); }
        }

        /// <inheritdoc />
        public DataColumn GetColumn(string name)
        {
            return _layer.DataSet.GetColumn(name);
        }

        /// <summary>
        /// Gets or sets the handler to use for progress messages during selection.
        /// </summary>
        public IProgressHandler ProgressHandler { get; set; }

        /// <inheritdoc />
        public void Add(int index)
        {
            if (index < 0 || index >= _layer.DrawnStates.Length) return;
            if (_layer.DrawnStates[index].Selected == SelectionState) return;
            _layer.DrawnStates[index].Selected = SelectionState;
            OnChanged();
        }

        /// <inheritdoc />
        public void Clear()
        {
            for (int shp = 0; shp < _layer.DrawnStates.Length; shp++)
            {
                _layer.DrawnStates[shp].Selected = !SelectionState;
            }
            OnChanged();
        }

        /// <inheritdoc />
        public bool Contains(int index)
        {
            return _layer.DrawnStates[index].Selected == SelectionState;
        }

        /// <inheritdoc />
        public void CopyTo(int[] array, int arrayIndex)
        {
            int index = arrayIndex;
            foreach (int i in this)
            {
                array[index] = i;
                index++;
            }
        }

        /// <inheritdoc />
        public int Count
        {
            get
            {
                int count = 0;
                if (_layer.DrawnStates == null) return 0;
                for (int i = 0; i < _layer.DrawnStates.Length; i++)
                {
                    if (_layer.DrawnStates[i].Selected == SelectionState) count++;
                }
                return count;
            }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <inheritdoc />
        public bool Remove(int index)
        {
            if (index < 0 || index >= _layer.DrawnStates.Length) return false;
            if (_layer.DrawnStates[index].Selected != SelectionState) return false;
            _layer.DrawnStates[index].Selected = !SelectionState;
            OnChanged();
            return true;
        }

        /// <summary>
        /// Setting this to a specific category will only allow selection by
        /// region to affect the features that are within the specified category.
        /// </summary>
        public IFeatureCategory RegionCategory { get; set; }

        /// <inheritdoc />
        public IEnumerator<int> GetEnumerator()
        {
            return new IndexSelectionEnumerator(_layer.DrawnStates, SelectionState);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void AddRow(Dictionary<string, object> values)
        {
            // Don't worry about the index in this case.
            _layer.DataSet.AddRow(values);
        }

        /// <inheritdoc />
        public void AddRow(DataRow values)
        {
            // Don't worry about the index in this case.
            _layer.DataSet.AddRow(values);
        }

        /// <inheritdoc />
        public void Edit(int index, Dictionary<string, object> values)
        {
            int sourceIndex = GetSourceIndex(index);
            _layer.DataSet.Edit(sourceIndex, values);
        }

        /// <inheritdoc />
        public void Edit(int index, DataRow values)
        {
            int sourceIndex = GetSourceIndex(index);
            _layer.DataSet.Edit(sourceIndex, values);
        }

        /// <inheritdoc />
        public int[] GetCounts(string[] expressions, ICancelProgressHandler progressHandler, int maxSampleSize)
        {
            int numSelected = Count;
            int[] counts = new int[expressions.Length];
            bool requiresRun = false;
            for (int ie = 0; ie < expressions.Length; ie++)
            {
                if (string.IsNullOrEmpty(expressions[ie]))
                {
                    counts[ie] = numSelected;
                }
                else
                {
                    requiresRun = true;
                }
            }
            if (!requiresRun) return counts;
            AttributePager ap = new AttributePager(_layer.DataSet, 100000);
            int numinTable = 0;
            DataTable result = new DataTable();
            result.Columns.AddRange(_layer.DataSet.GetColumns());
            FastDrawnState[] drawnStates = _layer.DrawnStates;
            for (int shp = 0; shp < drawnStates.Length; shp++)
            {
                if (drawnStates[shp].Selected)
                {
                    result.Rows.Add(ap.Row(shp).ItemArray);
                    numinTable++;
                    if (numinTable > 100000)
                    {
                        for (int ie = 0; ie < expressions.Length; ie++)
                        {
                            if (string.IsNullOrEmpty(expressions[ie])) continue;
                            counts[ie] += result.Select(expressions[ie]).Length;
                        }
                        result.Clear();
                    }
                }
            }
            for (int ie = 0; ie < expressions.Length; ie++)
            {
                if (string.IsNullOrEmpty(expressions[ie])) continue;
                counts[ie] += result.Select(expressions[ie]).Length;
            }
            result.Clear();
            return counts;
        }

        #endregion

        private int GetSourceIndex(int selectedIndex)
        {
            // For instance, the 0 index member of the selection might in fact
            // be the 10th member of the featureset.  But we want to edit the 10th member
            // and not the 0 member.
            int count = 0;
            foreach (int i in this)
            {
                if (count == selectedIndex) return i;
                count++;
            }
            throw new IndexOutOfRangeException("Index requested was: " + selectedIndex + " but the selection only has " + count + " members");
        }

        #region Nested type: IndexSelectionEnumerator

        /// <summary>
        /// This class cycles through the members
        /// </summary>
        public class IndexSelectionEnumerator : IEnumerator<int>
        {
            private readonly bool _selectionState;
            private readonly FastDrawnState[] _states;
            private int _current;

            /// <summary>
            /// Creates a new instance of IndexSelectionEnumerator
            /// </summary>
            /// <param name="states"></param>
            /// <param name="selectionState"></param>
            public IndexSelectionEnumerator(FastDrawnState[] states, bool selectionState)
            {
                _states = states;
                _selectionState = selectionState;
                _current = -1;
            }

            #region IEnumerator<int> Members

            /// <inheritdoc />
            public int Current
            {
                get { return _current; }
            }

            /// <inheritdoc />
            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
                get { return _current; }
            }

            /// <inheritdoc />
            public bool MoveNext()
            {
                do
                {
                    _current++;
                } while (_current < _states.Length && _states[_current].Selected != _selectionState);
                return _current != _states.Length;
            }

            /// <inheritdoc />
            public void Reset()
            {
                _current = -1;
            }

            #endregion
        }

        #endregion

        #region Private Variables

        private readonly IFeatureLayer _layer;
        private readonly List<ShapeRange> _shapes;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of IndexSelection
        /// </summary>
        public IndexSelection(IFeatureLayer layer)
        {
            _layer = layer;
            _shapes = layer.DataSet.ShapeIndices;
            SelectionMode = SelectionMode.IntersectsExtent;
            SelectionState = true;
        }

        #endregion
    }
}