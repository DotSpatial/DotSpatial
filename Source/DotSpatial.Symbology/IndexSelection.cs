// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
    /// <summary>
    /// A selection that uses indices instead of features.
    /// </summary>
    public class IndexSelection : Changeable, IIndexSelection
    {
        #region Fields

        private readonly IFeatureLayer _layer;
        private readonly List<ShapeRange> _shapes;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexSelection"/> class.
        /// </summary>
        /// <param name="layer">Layer the selected items belong to.</param>
        public IndexSelection(IFeatureLayer layer)
        {
            _layer = layer;
            _shapes = layer.DataSet.ShapeIndices;
            SelectionMode = SelectionMode.IntersectsExtent;
            SelectionState = true;
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public int Count
        {
            get
            {
                return _layer.DrawnStates?.Count(_ => _.Selected == SelectionState) ?? 0;
            }
        }

        /// <summary>
        /// Gets the envelope of this collection.
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

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <summary>
        /// Gets or sets the handler to use for progress messages during selection.
        /// </summary>
        public IProgressHandler ProgressHandler { get; set; }

        /// <summary>
        /// Gets or sets the region category. Setting this to a specific category will only allow selection by
        /// region to affect the features that are within the specified category.
        /// </summary>
        public IFeatureCategory RegionCategory { get; set; }

        /// <summary>
        /// Gets or sets the selection mode. The selection mode controls how envelopes are treated when working with geometries.
        /// </summary>
        public SelectionMode SelectionMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this should work as "Selected" indices (true) or
        /// "UnSelected" indices (false).
        /// </summary>
        public bool SelectionState { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Add(int index)
        {
            if (index < 0 || index >= _layer.DrawnStates.Length) return;
            if (_layer.DrawnStates[index].Selected == SelectionState) return;

            _layer.DrawnStates[index].Selected = SelectionState;
            OnChanged();
        }

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
        /// This uses extent checking (rather than full polygon intersection checking). It will add
        /// any members that are either contained by or intersect with the specified region
        /// depending on the SelectionMode property. The order of operation is the region
        /// acting on the feature, so Contains, for instance, would work with points.
        /// </summary>
        /// <param name="region">The region that contains the features that get added.</param>
        /// <param name="affectedArea">The affected area of this addition</param>
        /// <returns>True if any item was actually added to the collection</returns>
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
        public void Clear()
        {
            foreach (FastDrawnState state in _layer.DrawnStates)
            {
                state.Selected = !SelectionState;
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
                        if (fn.Contains(pair.Key)) dr[pair.Key] = pair.Value;
                    }

                    result.Rows.Add(dr);
                    if (i > startIndex + numRows) break;
                }
            }

            return result;
        }

        /// <inheritdoc />
        public DataColumn GetColumn(string name)
        {
            return _layer.DataSet.GetColumn(name);
        }

        /// <inheritdoc />
        public DataColumn[] GetColumns()
        {
            {
                return _layer.DataSet.GetColumns();
            }
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

        /// <inheritdoc />
        public IEnumerator<int> GetEnumerator()
        {
            return new IndexSelectionEnumerator(_layer.DrawnStates, SelectionState);
        }

        /// <summary>
        /// Inverts the selection based on the current SelectionMode
        /// </summary>
        /// <param name="region">The geographic region to reverse the selected state</param>
        /// <param name="affectedArea">The affected area to invert</param>
        /// <returns>True, if the selection was changed.</returns>
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

                    IGeometry geom = _layer.DataSet.GetFeature(shp).Geometry; // only get this if envelopes intersect
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

        /// <inheritdoc />
        public int NumRows()
        {
            return Count;
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
        /// Attempts to remove all the members from the collection. If
        /// one of the specified indices is outside the range of possible
        /// values, this returns false, even if others were successfully removed.
        /// This will also return false if none of the states were changed.
        /// </summary>
        /// <param name="indices">The indices to remove</param>
        /// <returns>True if the selection was changed.</returns>
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
        /// Tests each member currently in the selected features based on
        /// the SelectionMode. If it passes, it will remove the feature from
        /// the selection.
        /// </summary>
        /// <param name="region">The geographic region to remove</param>
        /// <param name="affectedArea">A geographic area that was affected by this change.</param>
        /// <returns>Boolean, true if the collection was changed</returns>
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

        /// <summary>
        /// Exports the members of this collection as a list of IFeature.
        /// </summary>
        /// <returns>A List of IFeature</returns>
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
        /// Returns a new featureset based on the features in this collection.
        /// </summary>
        /// <returns>An in memory featureset that has not yet been saved to a file in any way.</returns>
        public FeatureSet ToFeatureSet()
        {
            return new FeatureSet(ToFeatureList())
            {
                Projection = _layer.DataSet.Projection
            };
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private int GetSourceIndex(int selectedIndex)
        {
            // For instance, the 0 index member of the selection might in fact
            // be the 10th member of the featureset. But we want to edit the 10th member
            // and not the 0 member.
            int count = 0;
            foreach (int i in this)
            {
                if (count == selectedIndex) return i;

                count++;
            }

            throw new IndexOutOfRangeException("Index requested was: " + selectedIndex + " but the selection only has " + count + " members");
        }

        #endregion

        #region Classes

        /// <summary>
        /// This class cycles through the members
        /// </summary>
        private class IndexSelectionEnumerator : IEnumerator<int>
        {
            #region Fields

            private readonly bool _selectionState;
            private readonly FastDrawnState[] _states;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="IndexSelectionEnumerator"/> class.
            /// </summary>
            /// <param name="states">The states.</param>
            /// <param name="selectionState">The selection state.</param>
            public IndexSelectionEnumerator(FastDrawnState[] states, bool selectionState)
            {
                _states = states;
                _selectionState = selectionState;
                Current = -1;
            }

            #endregion

            #region Properties

            /// <inheritdoc />
            public int Current { get; private set; }

            object IEnumerator.Current => Current;

            #endregion

            #region Methods

            /// <inheritdoc />
            public void Dispose()
            {
            }

            /// <inheritdoc />
            public bool MoveNext()
            {
                do
                {
                    Current++;
                }
                while (Current < _states.Length && _states[Current].Selected != _selectionState);

                return Current != _states.Length;
            }

            /// <inheritdoc />
            public void Reset()
            {
                Current = -1;
            }

            #endregion
        }

        #endregion
    }
}