// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2009 5:19:07 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using DotSpatial.Data;
using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    public class FeatureSelection : Changeable, IFeatureSelection
    {
        #region Events

        #endregion

        #region Private Variables

        private readonly IFeatureSet _featureSet;
        private readonly IDrawingFilter _filter;
        private FilterType _activeType;
        IFeatureCategory _category;
        int _chunk;
        private IEnvelope _envelope;
        bool _isReadOnly;
        private IFeatureCategory _regionCategory;
        bool _selected;
        private SelectionMode _selectionMode;
        private bool _selectionState;
        private bool _useCategory;
        private bool _useChunks;
        private bool _useSelection;
        private bool _useVisibility;
        private bool _visible;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of FilterCollection, where the current state of the filter is
        /// recorded as the kind of "collection" that this item belongs to.  The filter can be
        /// altered later, and this will retain the original state.
        /// </summary>
        public FeatureSelection(IFeatureSet featureSet, IDrawingFilter inFilter, FilterType activeType)
        {
            _filter = inFilter;
            _activeType = activeType;
            _selectionState = true;
            _featureSet = featureSet;
            Configure();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="featureSet"></param>
        /// <param name="inFilter"></param>
        /// <param name="activeType"></param>
        /// <param name="isReadOnly"></param>
        public FeatureSelection(IFeatureSet featureSet, IDrawingFilter inFilter, FilterType activeType, bool isReadOnly)
        {
            _filter = inFilter;
            _activeType = activeType;
            _isReadOnly = isReadOnly;
            _featureSet = featureSet;
            Configure();
        }

        private void Configure()
        {
            // Cache the active state of the filter.
            _selected = _filter.Selected;
            _selectionState = _filter.Selected;
            _visible = _filter.Visible;
            _chunk = _filter.Chunk;
            _category = _filter.Category;
            _useSelection = _filter.UseSelection;
            _useChunks = _filter.UseChunks;
            _useVisibility = _filter.UseVisibility;
            _useCategory = _filter.UseCategory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adding a feature state sets the drawing state of the item to be
        /// </summary>
        /// <param name="item">The item to add to this category</param>
        /// <exception cref="Serialization.ReadOnlyException">Occurs if this list is set to read-only in the constructor</exception>
        public void Add(IFeature item)
        {
            if (_isReadOnly) throw new ReadOnlyException();
            IDrawnState previousState = _filter[item];
            IFeatureCategory cat = previousState.SchemeCategory;
            if (_useCategory) cat = _category;
            bool sel = previousState.IsSelected;
            if (_useSelection) sel = _selected;
            int chunk = previousState.Chunk;
            if (_useChunks) chunk = _chunk;
            bool vis = previousState.IsVisible;
            if (_useVisibility) vis = _visible;

            _filter[item] = new DrawnState(cat, sel, chunk, vis);
            OnChanged();
        }

        /// <summary>
        /// This uses extent checking (rather than full polygon intersection checking).  It will add
        /// any members that are either contained by or intersect with the specified region
        /// depending on the SelectionMode property.  The order of operation is the region
        /// acting on the feature, so Contains, for instance, would work with points.
        /// </summary>
        /// <param name="region"></param>
        /// <param name="affectedArea">The affected area of this addition</param>
        /// <returns>True if any item was actually added to the collection</returns>
        public bool AddRegion(IEnvelope region, out IEnvelope affectedArea)
        {
            bool added = false;
            SuspendChanges();
            affectedArea = new Envelope();
            Stopwatch sw = new Stopwatch();
            Stopwatch total = new Stopwatch();
            total.Start();
            foreach (IFeature f in FeatureList)
            {
                bool doAdd = false;
                if (_selectionMode == SelectionMode.IntersectsExtent)
                {
                    if (region.Intersects(f.Envelope))
                    {
                        Add(f);
                        affectedArea.ExpandToInclude(f.Envelope);
                        added = true;
                    }
                }
                else if (_selectionMode == SelectionMode.ContainsExtent)
                {
                    if (region.Contains(f.Envelope))
                    {
                        Add(f);
                        affectedArea.ExpandToInclude(f.Envelope);
                        added = true;
                    }
                }

                IGeometry reg;
                if (region.Width == 0 && region.Height == 0)
                {
                    reg = new Point(region.X, region.Y);
                }
                else if (region.Height == 0 || region.Width == 0)
                {
                    Coordinate[] coords = new Coordinate[2];
                    coords[0] = new Coordinate(region.X, region.Y);
                    coords[1] = new Coordinate(region.Bottom(), region.Right());
                    reg = new LineString(coords);
                }
                else
                {
                    reg = region.ToPolygon();
                }
                IGeometry geom = Geometry.FromBasicGeometry(f.BasicGeometry);
                switch (_selectionMode)
                {
                    case SelectionMode.Contains:
                        if (region.Contains(f.Envelope))
                        {
                            doAdd = true;
                        }
                        else if (region.Intersects(f.Envelope))
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
                    case SelectionMode.Disjoint:
                        if (reg.Disjoint(geom)) doAdd = true;
                        break;
                    case SelectionMode.Intersects:

                        if (region.Contains(f.Envelope))
                        {
                            doAdd = true;
                        }
                        else if (region.Intersects(f.Envelope))
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

                if (doAdd)
                {
                    Add(f);
                    affectedArea.ExpandToInclude(f.Envelope);
                    added = true;
                }
            }
            sw.Start();
            ResumeChanges();
            sw.Stop();
            total.Stop();
            Debug.WriteLine("Geometry Intersection Time: " + sw.ElapsedMilliseconds);
            Debug.WriteLine("Total Intersection Time: " + total.ElapsedMilliseconds);
            return added;
        }

        /// <summary>
        /// Inverts the selection based on the current SelectionMode
        /// </summary>
        /// <param name="region">The geographic region to reverse the selected state</param>
        /// <param name="affectedArea">The affected area to invert</param>
        public bool InvertSelection(IEnvelope region, out IEnvelope affectedArea)
        {
            SuspendChanges();
            bool flipped = false;
            affectedArea = new Envelope();

            IDictionary<IFeature, IDrawnState> states = Filter.DrawnStates;
            foreach (KeyValuePair<IFeature, IDrawnState> kvp in states)
            {
                bool doFlip = false;
                IFeature f = kvp.Key;
                if (SelectionMode == SelectionMode.IntersectsExtent)
                {
                    if (region.Intersects(f.Envelope))
                    {
                        kvp.Value.IsSelected = !kvp.Value.IsSelected;
                        affectedArea.ExpandToInclude(f.Envelope);
                    }
                }
                else if (SelectionMode == SelectionMode.ContainsExtent)
                {
                    if (region.Contains(f.Envelope))
                    {
                        kvp.Value.IsSelected = !kvp.Value.IsSelected;
                        affectedArea.ExpandToInclude(f.Envelope);
                    }
                }
                IPolygon reg = region.ToPolygon();
                IGeometry geom = Geometry.FromBasicGeometry(f.BasicGeometry);
                switch (SelectionMode)
                {
                    case SelectionMode.Contains:
                        if (region.Intersects(f.Envelope))
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
                    case SelectionMode.Disjoint:
                        if (reg.Disjoint(geom)) doFlip = true;
                        break;
                    case SelectionMode.Intersects:
                        if (region.Intersects(f.Envelope))
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
                if (doFlip)
                {
                    flipped = true;
                    kvp.Value.IsSelected = !kvp.Value.IsSelected;
                    affectedArea.ExpandToInclude(f.Envelope);
                }
            }
            ResumeChanges();
            return flipped;
        }

        /// <summary>
        /// This cycles through all the members in the current grouping and re-sets the current category
        /// back to the default setting.  In other words, if the active type is selection, then this
        /// will unselect all the features, but won't adjust any of the other categories.
        /// </summary>
        /// <exception cref="ReadOnlyException">Occurs if this list is set to read-only in the constructor</exception>
        public void Clear()
        {
            SetFilter(); // ensure the filter state matches this collection
            List<IFeature> featureList = new List<IFeature>();
            foreach (IFeature f in _filter)
            {
                // modifying the same list we are enumerating causes an exception, even if we aren't deleting
                featureList.Add(f);
            }
            SuspendChanges();
            foreach (IFeature f in featureList)
            {
                Remove(f); // use the same code as remove, ignoring "false" returns
            }

            // reset the envelope (the envelope will be calculated the next time the property is accessed) since all selections are cleared.
            _envelope = null;

            ResumeChanges();
        }

        /// <summary>
        /// Tests to see if the item is contained in this collection.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(IFeature item)
        {
            IDrawnState previousState = _filter[item];
            if (_useSelection)
            {
                if (previousState.IsSelected != _selected) return false;
            }
            if (_useVisibility)
            {
                if (previousState.IsVisible != _visible) return false;
            }
            if (_useChunks)
            {
                if (previousState.Chunk != _chunk) return false;
            }
            if (_useCategory)
            {
                if (previousState.SchemeCategory != _category) return false;
            }
            // Contains is defined as "matching" all the criteria that we care about.
            return true;
        }

        /// <summary>
        /// Copies each of the members of this group to the specified array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(IFeature[] array, int arrayIndex)
        {
            SetFilter();
            int index = arrayIndex;
            foreach (IFeature feature in _filter)
            {
                if (index > array.GetUpperBound(0)) break;
                array[index] = feature;
            }
        }

        /// <summary>
        /// Gets the enumerator for this collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IFeature> GetEnumerator()
        {
            SetFilter();
            return _filter.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Removes the specified item from the subset of this classification if that category is used.
        /// Selected -> !Selected
        /// Category[>0] -> Category[0]
        /// Category[0] -> Null
        /// Visible -> !Visible
        /// Chunk -> -1  or basically a chunk index that is never drawn.
        /// </summary>
        /// <param name="item">The item to change the drawing state of</param>
        /// <returns>Boolean, false if the item does not match the current grouping</returns>
        /// <exception cref="ReadOnlyException">Occurs if this list is set to read-only in the constructor</exception>
        public bool Remove(IFeature item)
        {
            if (_isReadOnly) throw new ReadOnlyException();
            if (Contains(item) == false) return false;

            IDrawnState previousState = _filter[item];
            IFeatureCategory cat = previousState.SchemeCategory;
            int chunk = previousState.Chunk;
            bool sel = previousState.IsSelected;
            bool vis = previousState.IsVisible;
            if (_activeType == FilterType.Category)
            {
                cat = _category != _filter.DefaultCategory ? _filter.DefaultCategory : null;
            }
            if (_activeType == FilterType.Chunk)
            {
                // removing from a chunk effectively means setting to -1 so that it will not get drawn
                // until it is added to a chunk again.
                chunk = -1;
            }
            if (_activeType == FilterType.Selection)
            {
                sel = !_selected;
            }
            if (_activeType == FilterType.Visible)
            {
                vis = !_visible;
            }
            _filter[item] = new DrawnState(cat, sel, chunk, vis);
            OnChanged();
            return true;
        }

        /// <summary>
        /// Tests each member currently in the selected features based on
        /// the SelectionMode.  If it passes, it will remove the feature from
        /// the selection.
        /// </summary>
        /// <param name="region">The geographic region to remove</param>
        /// <param name="affectedArea">A geographic area that was affected by this change.</param>
        /// <returns>Boolean, true if the collection was changed</returns>
        public bool RemoveRegion(IEnvelope region, out IEnvelope affectedArea)
        {
            SuspendChanges();
            bool removed = false;
            affectedArea = new Envelope();

            var query = from pair in _filter.DrawnStates
                        where pair.Value.IsSelected
                        select pair.Key;
            List<IFeature> selectedFeatures = query.ToList();
            foreach (IFeature f in selectedFeatures)
            {
                bool doRemove = false;
                if (_selectionMode == SelectionMode.IntersectsExtent)
                {
                    if (region.Intersects(f.Envelope))
                    {
                        if (Remove(f))
                        {
                            removed = true;
                            affectedArea.ExpandToInclude(f.Envelope);
                        }
                    }
                }
                else if (_selectionMode == SelectionMode.ContainsExtent)
                {
                    if (region.Contains(f.Envelope))
                    {
                        if (Remove(f))
                        {
                            removed = true;
                            affectedArea.ExpandToInclude(f.Envelope);
                        }
                    }
                }
                IPolygon reg = region.ToPolygon();
                IGeometry geom = Geometry.FromBasicGeometry(f.BasicGeometry);
                switch (_selectionMode)
                {
                    case SelectionMode.Contains:
                        if (region.Intersects(f.Envelope))
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
                    case SelectionMode.Disjoint:
                        if (reg.Disjoint(geom)) doRemove = true;
                        break;
                    case SelectionMode.Intersects:
                        if (region.Intersects(f.Envelope))
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
                if (doRemove)
                {
                    if (Remove(f))
                    {
                        affectedArea.ExpandToInclude(f.Envelope);
                        removed = true;
                    }
                }
            }
            ResumeChanges();
            return removed;
        }

        /// <summary>
        /// As an example, choosing myFeatureLayer.SelectedFeatures.ToFeatureSet creates a new set.
        /// </summary>
        /// <returns>An in memory featureset that has not yet been saved to a file in any way.</returns>
        public FeatureSet ToFeatureSet()
        {
            FeatureSet fs = new FeatureSet(ToFeatureList()); // the output features will be copied.
            if (fs.Features.Count == 0)
            {
                if (_filter.FeatureList.Count > 0)
                {
                    fs.CopyTableSchema(_filter.FeatureList[0].ParentFeatureSet);
                }
            }
            return fs;
        }

        /// <summary>
        /// Exports the members of this collection as a list of IFeature.
        /// </summary>
        /// <returns>A List of IFeature</returns>
        public List<IFeature> ToFeatureList()
        {
            List<IFeature> features = new List<IFeature>();
            foreach (IFeature feature in this)
            {
                features.Add(feature);
            }
            return features;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Collections can be nested, but for instance if you have gone into category[2].SelectedFeatures and want to
        /// remove an item, then we would expect it to show up in Category[2].UnSelectedFeatures, not in
        /// Category[0].UnselectedFeatures.  So while add necessarilly acts on every filter constraint, remove or clear only
        /// operates on the most intimate.  The ActiveType records which criteria that is.
        /// </summary>
        public FilterType ActiveType
        {
            get { return _activeType; }
            protected set { _activeType = value; }
        }

        /// <summary>
        /// Gets the feature list of underlying features that the filter is using
        /// </summary>
        protected IFeatureList FeatureList
        {
            get
            {
                return _filter.FeatureList;
            }
        }

        /// <summary>
        /// This cycles through the entire enumeration, so don't access this in every loop.
        /// </summary>
        public int Count
        {
            get
            {
                SetFilter(); // not to worry, if the filter state doesn't change, then the count won't reset.

                return _filter.Count;
            }
        }

        /// <summary>
        /// Gets the envelope that represents the features in this collection.
        /// If the collection changes, this will be invalidated automatically,
        /// and the next envelope request will re-calcualte the envelope.
        /// </summary>
        public IEnvelope Envelope
        {
            get
            {
                if (_envelope == null)
                {
                    _envelope = GetEnvelope();
                }
                return _envelope;
            }
            protected set
            {
                _envelope = value;
            }
        }

        /// <summary>
        /// Gets the drawing filter used by this collection.
        /// </summary>
        public IDrawingFilter Filter
        {
            get { return _filter; }
        }

        /// <summary>
        /// Gets whether or not this collection can be edited with add, remove, or clear.
        /// </summary>
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            protected set { _isReadOnly = value; }
        }

        /// <summary>
        /// Gets or sets the handler to use for progress messages during selection.
        /// </summary>
        public IProgressHandler ProgressHandler { get; set; }

        /// <summary>
        /// Setting this to a specific category will only allow selection by
        /// region to affect the features that are within the specified category.
        /// </summary>
        public IFeatureCategory RegionCategory
        {
            get { return _regionCategory; }
            set { _regionCategory = value; }
        }

        #endregion

        #region Read Only Properties

        /// <summary>
        /// Gets or sets the scheme category to use
        /// </summary>
        public IFeatureCategory Category
        {
            get { return _category; }
            protected set
            {
                _category = value;
            }
        }

        /// <summary>
        /// Gets the integer chunk that the filter should use
        /// </summary>
        public int Chunk
        {
            get { return _chunk; }
            protected set
            {
                _chunk = value;
            }
        }

        /// <summary>
        /// If UseSelection is true, this will get or set the boolean selection state
        /// that will be used to select values.
        /// </summary>
        public bool Selected
        {
            get { return _selected; }
            protected set
            {
                _selected = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether the filter should subdivide based on category.
        /// </summary>
        public bool UseCategory
        {
            get { return _useCategory; }
            protected set
            {
                _useCategory = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether we should use the chunk
        /// </summary>
        public bool UseChunks
        {
            get { return _useChunks; }
            protected set
            {
                _useChunks = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether this filter should use the Selected
        /// </summary>
        public bool UseSelection
        {
            get { return _useSelection; }
            protected set
            {
                _useSelection = value;
            }
        }

        /// <summary>
        /// Gets or sets the boolean indicating whether or not this feature should be drawn.
        /// </summary>
        public bool UseVisibility
        {
            get { return _useVisibility; }
            protected set
            {
                _useVisibility = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean that specifies whether to return visible, or hidden features if UseVisibility is true.
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            protected set
            {
                _visible = value;
            }
        }

        /// <summary>
        /// True if members of this collection are considered selected, false otherwise.
        /// </summary>
        public bool SelectionState
        {
            get { return _selectionState; }
            set { _selectionState = value; }
        }

        /// <summary>
        /// Gets or sets the selection mode to use when Adding or Removing features
        /// from a specified envelope region.
        /// </summary>
        public SelectionMode SelectionMode
        {
            get { return _selectionMode; }
            set { _selectionMode = value; }
        }

        #endregion

        #region Protected Methods

        #endregion

        // The idea is that once a collection is established, its parameters don't change.
        // However sub-classes need to be able to tweak these to their choosing.

        #region IFeatureSelection Members

        /// <inheritdoc />
        public DataTable GetAttributes(int startIndex, int numRows)
        {
            int count = 0;
            DataTable dt = new DataTable();
            dt.Columns.AddRange(_featureSet.GetColumns());
            foreach (IFeature feature in Filter)
            {
                if (count >= startIndex && count < startIndex + numRows)
                {
                    dt.Rows.Add(feature.DataRow.ItemArray);
                }
                if (count > numRows + startIndex) break;
                count++;
            }
            return dt;
        }

        /// <inheritdoc/>
        public DataTable GetAttributes(int startIndex, int numRows, IEnumerable<string> fieldNames)
        {
            int count = 0;
            DataTable dt = new DataTable();
            List<DataColumn> dc = new List<DataColumn>();
            DataColumn[] original = _featureSet.GetColumns();
            foreach (DataColumn c in original)
            {
                if (fieldNames.Contains(c.ColumnName))
                {
                    dc.Add(c);
                }
            }
            dt.Columns.AddRange(dc.ToArray());
            foreach (IFeature feature in Filter)
            {
                if (count >= startIndex && count < startIndex + numRows)
                {
                    foreach (string name in fieldNames)
                    {
                        DataRow dr = dt.NewRow();
                        dr[name] = feature.DataRow[name];
                        dt.Rows.Add(dr);
                    }
                }
                if (count > numRows + startIndex) break;
                count++;
            }
            return dt;
        }

        /// <inheritdoc />
        public void SetAttributes(int startIndex, DataTable values)
        {
            int index = startIndex;
            List<IFeature> features = Filter.ToList();
            foreach (DataRow row in values.Rows)
            {
                features[index].DataRow.ItemArray = row.ItemArray;
                index++;
            }
        }

        /// <inheritdoc />
        public int NumRows()
        {
            return _filter.Count;
        }

        /// <inheritdoc />
        public DataColumn GetColumn(string name)
        {
            return _featureSet.GetColumn(name);
        }

        /// <inheritdoc />
        public DataColumn[] GetColumns()
        {
            return _featureSet.GetColumns();
        }

        /// <summary>
        /// The new row will be added to the underlying featureset, and this selection will not be updated
        /// in any way, as that row is not associated with a featureset here.
        /// </summary>
        /// <param name="values"></param>
        public void AddRow(Dictionary<string, object> values)
        {
            _featureSet.AddRow(values);
        }

        /// <summary>
        /// The new row will be added to the underlying featureset, and this selection will not be updated
        /// in any way, as that row is not associated with a featureset here.
        /// </summary>
        /// <param name="values"></param>
        public void AddRow(DataRow values)
        {
            _featureSet.AddRow(values);
        }

        /// <inheritdoc />
        public void Edit(int index, Dictionary<string, object> values)
        {
            int count = 0;
            foreach (IFeature feature in Filter)
            {
                if (count == index)
                {
                    DataColumnCollection dc = _featureSet.DataTable.Columns;
                    foreach (DataColumn column in dc)
                    {
                        feature.DataRow[column] = values[column.ColumnName];
                        return;
                    }
                }
                count++;
            }
        }

        /// <inheritdoc />
        public void Edit(int index, DataRow values)
        {
            int count = 0;
            foreach (IFeature feature in Filter)
            {
                if (count == index)
                {
                    DataColumnCollection dc = _featureSet.DataTable.Columns;
                    foreach (DataColumn column in dc)
                    {
                        feature.DataRow[column] = values[column.ColumnName];
                        return;
                    }
                }
                count++;
            }
        }

        /// <inheritdoc />
        public int[] GetCounts(string[] expressions, ICancelProgressHandler progressHandler, int maxSampleSize)
        {
            int[] result = new int[expressions.Length];
            for (int i = 0; i < expressions.Length; i++)
            {
                string s = expressions[i];
                int count = 0;
                DataRow[] rows = _featureSet.DataTable.Select(s);
                foreach (DataRow row in rows)
                {
                    if (_filter.DrawnStates[_featureSet.FeatureLookup[row]].IsSelected == Selected) count++;
                }
                result[i] = count;
            }
            return result;
        }

        #endregion

        private IEnvelope GetEnvelope()
        {
            IEnvelope env = new Envelope();
            foreach (IFeature f in this)
            {
                env.ExpandToInclude(f.Envelope);
            }
            return env;
        }

        private void SetFilter()
        {
            _filter.Chunk = _chunk;
            _filter.Category = _category;
            _filter.Selected = _selected;
            _filter.Visible = _visible;
            _filter.UseCategory = _useCategory;
            _filter.UseChunks = _useChunks;
            _filter.UseSelection = _useSelection;
            _filter.UseVisibility = _useVisibility;
        }
    }
}