// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using DotSpatial.Data;
using DotSpatial.NTSExtension;
using NetTopologySuite.Geometries;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// FeatureSelection.
    /// </summary>
    public class FeatureSelection : Changeable, IFeatureSelection
    {
        #region Fields

        private readonly IFeatureSet _featureSet;
        private Envelope _envelope;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSelection"/> class.
        /// Creates a new instance of FilterCollection, where the current state of the filter is
        /// recorded as the kind of "collection" that this item belongs to. The filter can be
        /// altered later, and this will retain the original state.
        /// </summary>
        /// <param name="featureSet">The feature set.</param>
        /// <param name="inFilter">The drawing filter.</param>
        /// <param name="activeType">The filter type.</param>
        public FeatureSelection(IFeatureSet featureSet, IDrawingFilter inFilter, FilterType activeType)
        {
            Filter = inFilter;
            ActiveType = activeType;
            SelectionState = true;
            _featureSet = featureSet;
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSelection"/> class.
        /// </summary>
        /// <param name="featureSet">The feature set.</param>
        /// <param name="inFilter">The drawing filter.</param>
        /// <param name="activeType">The filter type.</param>
        /// <param name="isReadOnly">Indicates whether the selection is read only.</param>
        public FeatureSelection(IFeatureSet featureSet, IDrawingFilter inFilter, FilterType activeType, bool isReadOnly)
        {
            Filter = inFilter;
            ActiveType = activeType;
            IsReadOnly = isReadOnly;
            _featureSet = featureSet;
            Configure();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the filter type. Collections can be nested, but for instance if you have gone into category[2].SelectedFeatures and want to
        /// remove an item, then we would expect it to show up in Category[2].UnSelectedFeatures, not in
        /// Category[0].UnselectedFeatures. So while add necessarilly acts on every filter constraint, remove or clear only
        /// operates on the most intimate. The ActiveType records which criteria that is.
        /// </summary>
        public FilterType ActiveType { get; protected set; }

        /// <summary>
        /// Gets or sets the scheme category to use.
        /// </summary>
        public IFeatureCategory Category { get; protected set; }

        /// <summary>
        /// Gets or sets the integer chunk that the filter should use.
        /// </summary>
        public int Chunk { get; protected set; }

        /// <summary>
        /// Gets the count. This cycles through the entire enumeration, so don't access this in every loop.
        /// </summary>
        public int Count
        {
            get
            {
                SetFilter(); // not to worry, if the filter state doesn't change, then the count won't reset.

                return Filter.Count;
            }
        }

        /// <summary>
        /// Gets or sets the envelope that represents the features in this collection.
        /// If the collection changes, this will be invalidated automatically,
        /// and the next envelope request will re-calcualte the envelope.
        /// </summary>
        public Envelope Envelope
        {
            get
            {
                return _envelope ??= GetEnvelope();
            }

            protected set
            {
                _envelope = value;
            }
        }

        /// <summary>
        /// Gets the drawing filter used by this collection.
        /// </summary>
        public IDrawingFilter Filter { get; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this collection can be edited with add, remove, or clear.
        /// </summary>
        public bool IsReadOnly { get; protected set; }

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
        /// Gets or sets a value indicating whether elements in this list are selected. If UseSelection is true, this will get or set the boolean selection state
        /// that will be used to select values.
        /// </summary>
        public bool Selected { get; protected set; }

        /// <summary>
        /// Gets or sets the selection mode to use when Adding or Removing features
        /// from a specified envelope region.
        /// </summary>
        public SelectionMode SelectionMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether members of this collection are considered selected.
        /// </summary>
        public bool SelectionState { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the filter should subdivide based on category.
        /// </summary>
        public bool UseCategory { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should use the chunks.
        /// </summary>
        public bool UseChunks { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this filter should use the Selected.
        /// </summary>
        public bool UseSelection { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this feature should be drawn.
        /// </summary>
        public bool UseVisibility { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether to return visible, or hidden features if UseVisibility is true.
        /// </summary>
        public bool Visible { get; protected set; }

        /// <summary>
        /// Gets the feature list of underlying features that the filter is using.
        /// </summary>
        protected IFeatureList FeatureList => Filter.FeatureList;

        #endregion

        #region Methods

        /// <summary>
        /// Adding a feature state sets the drawing state of the item to be.
        /// </summary>
        /// <param name="item">The item to add to this category.</param>
        /// <exception cref="Serialization.ReadOnlyException">Occurs if this list is set to read-only in the constructor.</exception>
        public void Add(IFeature item)
        {
            if (IsReadOnly) throw new ReadOnlyException();

            IDrawnState previousState = Filter[item];
            IFeatureCategory cat = previousState.SchemeCategory;
            if (UseCategory) cat = Category;
            bool sel = previousState.IsSelected;
            if (UseSelection) sel = Selected;
            int chunk = previousState.Chunk;
            if (UseChunks) chunk = Chunk;
            bool vis = previousState.IsVisible;
            if (UseVisibility) vis = Visible;

            Filter[item] = new DrawnState(cat, sel, chunk, vis);
            _envelope = null; //reset the envelope so it will be calculated from the selected features the next time the property is accessed
            OnChanged();
        }

        /// <summary>
        /// This uses extent checking (rather than full polygon intersection checking). It will add
        /// any members that are either contained by or intersect with the specified region
        /// depending on the SelectionMode property. The order of operation is the region
        /// acting on the feature, so Contains, for instance, would work with points.
        /// </summary>
        /// <param name="region">The region that contains the features that get added.</param>
        /// <param name="affectedArea">The affected area of this addition.</param>
        /// <returns>True if any item was actually added to the collection.</returns>
        public bool AddRegion(Envelope region, out Envelope affectedArea)
        {
            bool added = false;
            SuspendChanges();
            affectedArea = new Envelope();

#if DEBUG
            var total = new Stopwatch();
            total.Start();
#endif
            foreach (IFeature f in FeatureList)
            {
                bool doAdd = false;
                if (SelectionMode == SelectionMode.IntersectsExtent)
                {
                    if (region.Intersects(f.Geometry.EnvelopeInternal))
                    {
                        Add(f);
                        affectedArea.ExpandToInclude(f.Geometry.EnvelopeInternal);
                        added = true;
                    }
                }
                else if (SelectionMode == SelectionMode.ContainsExtent)
                {
                    if (region.Contains(f.Geometry.EnvelopeInternal))
                    {
                        Add(f);
                        affectedArea.ExpandToInclude(f.Geometry.EnvelopeInternal);
                        added = true;
                    }
                }

                Geometry reg;
                if (region.Width == 0 && region.Height == 0)
                {
                    reg = new NetTopologySuite.Geometries.Point(region.MinX, region.MaxY);
                }
                else if (region.Height == 0 || region.Width == 0)
                {
                    Coordinate[] coords = new Coordinate[2];
                    coords[0] = new Coordinate(region.MinX, region.MaxY);
                    coords[1] = new Coordinate(region.MinY, region.MaxX);
                    reg = new LineString(coords);
                }
                else
                {
                    reg = region.ToPolygon();
                }

                Geometry geom = f.Geometry;
                switch (SelectionMode)
                {
                    case SelectionMode.Contains:
                        if (region.Contains(f.Geometry.EnvelopeInternal))
                        {
                            doAdd = true;
                        }
                        else if (region.Intersects(f.Geometry.EnvelopeInternal))
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

                        if (region.Contains(f.Geometry.EnvelopeInternal))
                        {
                            doAdd = true;
                        }
                        else if (region.Intersects(f.Geometry.EnvelopeInternal))
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
                    affectedArea.ExpandToInclude(f.Geometry.EnvelopeInternal);
                    added = true;
                }
            }

#if DEBUG
            var sw = new Stopwatch();
            sw.Start();
#endif

            ResumeChanges();

#if DEBUG
            sw.Stop();
            total.Stop();
            Debug.WriteLine("Geometry Intersection Time: " + sw.ElapsedMilliseconds);
            Debug.WriteLine("Total Intersection Time: " + total.ElapsedMilliseconds);
#endif

            return added;
        }

        /// <summary>
        /// The new row will be added to the underlying featureset, and this selection will not be updated
        /// in any way, as that row is not associated with a featureset here.
        /// </summary>
        /// <param name="values">Values that get added to the row.</param>
        public void AddRow(Dictionary<string, object> values)
        {
            _featureSet.AddRow(values);
        }

        /// <summary>
        /// The new row will be added to the underlying featureset, and this selection will not be updated
        /// in any way, as that row is not associated with a featureset here.
        /// </summary>
        /// <param name="values">Values that get added to the row.</param>
        public void AddRow(DataRow values)
        {
            _featureSet.AddRow(values);
        }

        /// <summary>
        /// This cycles through all the members in the current grouping and re-sets the current category
        /// back to the default setting. In other words, if the active type is selection, then this
        /// will unselect all the features, but won't adjust any of the other categories.
        /// </summary>
        /// <exception cref="ReadOnlyException">Occurs if this list is set to read-only in the constructor.</exception>
        public void Clear()
        {
            SetFilter(); // ensure the filter state matches this collection
            List<IFeature> featureList = new();
            foreach (IFeature f in Filter)
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
        /// <param name="item">Item that should be contained.</param>
        /// <returns>True, if the item is contained.</returns>
        public bool Contains(IFeature item)
        {
            IDrawnState previousState = Filter[item];
            if (UseSelection && previousState.IsSelected != Selected) return false;
            if (UseVisibility && previousState.IsVisible != Visible) return false;
            if (UseChunks && previousState.Chunk != Chunk) return false;
            if (UseCategory && previousState.SchemeCategory != Category) return false;

            // Contains is defined as "matching" all the criteria that we care about.
            return true;
        }

        /// <summary>
        /// Copies each of the members of this group to the specified array.
        /// </summary>
        /// <param name="array">Array to copy the features to.</param>
        /// <param name="arrayIndex">Index of the first feature that gets copied.</param>
        public void CopyTo(IFeature[] array, int arrayIndex)
        {
            SetFilter();
            int index = arrayIndex;
            foreach (IFeature feature in Filter)
            {
                if (index > array.GetUpperBound(0)) break;

                array[index] = feature;
            }
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
        public DataTable GetAttributes(int startIndex, int numRows)
        {
            int count = 0;
            DataTable dt = new();
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
            DataTable dt = new();
            List<DataColumn> dc = new();
            DataColumn[] original = _featureSet.GetColumns();
            var names = fieldNames as IList<string> ?? fieldNames.ToList();
            foreach (DataColumn c in original)
            {
                if (names.Contains(c.ColumnName))
                {
                    dc.Add(c);
                }
            }

            dt.Columns.AddRange(dc.ToArray());
            foreach (IFeature feature in Filter)
            {
                if (count >= startIndex && count < startIndex + numRows)
                {
                    DataRow dr = dt.NewRow();
                    foreach (string name in names)
                    {
                        dr[name] = feature.DataRow[name];
                    }

                    dt.Rows.Add(dr);
                }

                if (count > numRows + startIndex) break;

                count++;
            }

            return dt;
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
                    if (Filter.DrawnStates[_featureSet.FeatureLookup[row]].IsSelected == Selected) count++;
                }

                result[i] = count;
            }

            return result;
        }

        /// <summary>
        /// Gets the enumerator for this collection.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<IFeature> GetEnumerator()
        {
            SetFilter();
            return Filter.GetEnumerator();
        }

        /// <summary>
        /// Inverts the selection based on the current SelectionMode.
        /// </summary>
        /// <param name="region">The geographic region to reverse the selected state.</param>
        /// <param name="affectedArea">The affected area to invert.</param>
        /// <returns>True, if the selection was changed.</returns>
        public bool InvertSelection(Envelope region, out Envelope affectedArea)
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
                    if (region.Intersects(f.Geometry.EnvelopeInternal))
                    {
                        kvp.Value.IsSelected = !kvp.Value.IsSelected;
                        affectedArea.ExpandToInclude(f.Geometry.EnvelopeInternal);
                    }
                }
                else if (SelectionMode == SelectionMode.ContainsExtent)
                {
                    if (region.Contains(f.Geometry.EnvelopeInternal))
                    {
                        kvp.Value.IsSelected = !kvp.Value.IsSelected;
                        affectedArea.ExpandToInclude(f.Geometry.EnvelopeInternal);
                    }
                }

                Polygon reg = region.ToPolygon();
                Geometry geom = f.Geometry;
                switch (SelectionMode)
                {
                    case SelectionMode.Contains:
                        if (region.Intersects(f.Geometry.EnvelopeInternal))
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
                        if (region.Intersects(f.Geometry.EnvelopeInternal))
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
                    affectedArea.ExpandToInclude(f.Geometry.EnvelopeInternal);
                }
            }

            ResumeChanges();
            return flipped;
        }

        /// <inheritdoc />
        public int NumRows()
        {
            return Filter.Count;
        }

        /// <summary>
        /// Removes the specified item from the subset of this classification if that category is used.
        /// Selected -> !Selected
        /// Category[>0] -> Category[0]
        /// Category[0] -> Null
        /// Visible -> !Visible
        /// Chunk -> -1  or basically a chunk index that is never drawn.
        /// </summary>
        /// <param name="item">The item to change the drawing state of.</param>
        /// <returns>Boolean, false if the item does not match the current grouping.</returns>
        /// <exception cref="ReadOnlyException">Occurs if this list is set to read-only in the constructor.</exception>
        public bool Remove(IFeature item)
        {
            if (IsReadOnly) throw new ReadOnlyException();

            if (Contains(item) == false) return false;

            IDrawnState previousState = Filter[item];
            IFeatureCategory cat = previousState.SchemeCategory;
            int chunk = previousState.Chunk;
            bool sel = previousState.IsSelected;
            bool vis = previousState.IsVisible;
            if (ActiveType == FilterType.Category)
            {
                cat = Category != Filter.DefaultCategory ? Filter.DefaultCategory : null;
            }

            if (ActiveType == FilterType.Chunk)
            {
                // removing from a chunk effectively means setting to -1 so that it will not get drawn
                // until it is added to a chunk again.
                chunk = -1;
            }

            if (ActiveType == FilterType.Selection)
            {
                sel = !Selected;
            }

            if (ActiveType == FilterType.Visible)
            {
                vis = !Visible;
            }

            Filter[item] = new DrawnState(cat, sel, chunk, vis);
            _envelope = null; //reset the envelope so it will be calculated from the selected features the next time the property is accessed
            OnChanged();
            return true;
        }

        /// <summary>
        /// Tests each member currently in the selected features based on
        /// the SelectionMode. If it passes, it will remove the feature from
        /// the selection.
        /// </summary>
        /// <param name="region">The geographic region to remove.</param>
        /// <param name="affectedArea">A geographic area that was affected by this change.</param>
        /// <returns>Boolean, true if the collection was changed.</returns>
        public bool RemoveRegion(Envelope region, out Envelope affectedArea)
        {
            SuspendChanges();
            bool removed = false;
            affectedArea = new Envelope();

            var query = from pair in Filter.DrawnStates where pair.Value.IsSelected select pair.Key;
            List<IFeature> selectedFeatures = query.ToList();
            foreach (IFeature f in selectedFeatures)
            {
                bool doRemove = false;
                if (SelectionMode == SelectionMode.IntersectsExtent)
                {
                    if (region.Intersects(f.Geometry.EnvelopeInternal))
                    {
                        if (Remove(f))
                        {
                            removed = true;
                            affectedArea.ExpandToInclude(f.Geometry.EnvelopeInternal);
                        }
                    }
                }
                else if (SelectionMode == SelectionMode.ContainsExtent)
                {
                    if (region.Contains(f.Geometry.EnvelopeInternal))
                    {
                        if (Remove(f))
                        {
                            removed = true;
                            affectedArea.ExpandToInclude(f.Geometry.EnvelopeInternal);
                        }
                    }
                }

                Polygon reg = region.ToPolygon();
                Geometry geom = f.Geometry;
                switch (SelectionMode)
                {
                    case SelectionMode.Contains:
                        if (region.Intersects(f.Geometry.EnvelopeInternal))
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
                        if (region.Intersects(f.Geometry.EnvelopeInternal))
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
                        affectedArea.ExpandToInclude(f.Geometry.EnvelopeInternal);
                        removed = true;
                    }
                }
            }

            ResumeChanges();
            return removed;
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

        /// <summary>
        /// Exports the members of this collection as a list of IFeature.
        /// </summary>
        /// <returns>A List of IFeature.</returns>
        public List<IFeature> ToFeatureList()
        {
            List<IFeature> features = new();
            foreach (IFeature feature in this)
            {
                features.Add(feature);
            }

            return features;
        }

        /// <summary>
        /// As an example, choosing myFeatureLayer.SelectedFeatures.ToFeatureSet creates a new set.
        /// </summary>
        /// <returns>An in memory featureset that has not yet been saved to a file in any way.</returns>
        public FeatureSet ToFeatureSet()
        {
            FeatureSet fs = new(ToFeatureList()) // the output features will be copied.
            {
                Projection = _featureSet.Projection,
                CoordinateType = _featureSet.CoordinateType,
                FeatureGeometryFactory = _featureSet.FeatureGeometryFactory,
                FeatureType = _featureSet.FeatureType
            };

            if (fs.Features.Count == 0)
            {
                if (Filter.FeatureList.Count > 0)
                {
                    fs.CopyTableSchema(Filter.FeatureList[0].ParentFeatureSet);
                }
            }

            return fs;
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void Configure()
        {
            // Cache the active state of the filter.
            Selected = Filter.Selected;
            SelectionState = Filter.Selected;
            Visible = Filter.Visible;
            Chunk = Filter.Chunk;
            Category = Filter.Category;
            UseSelection = Filter.UseSelection;
            UseChunks = Filter.UseChunks;
            UseVisibility = Filter.UseVisibility;
            UseCategory = Filter.UseCategory;
        }

        private Envelope GetEnvelope()
        {
            Envelope env = new();
            foreach (IFeature f in this)
            {
                env.ExpandToInclude(f.Geometry.EnvelopeInternal);
            }

            return env;
        }

        private void SetFilter()
        {
            Filter.Chunk = Chunk;
            Filter.Category = Category;
            Filter.Selected = Selected;
            Filter.Visible = Visible;
            Filter.UseCategory = UseCategory;
            Filter.UseChunks = UseChunks;
            Filter.UseSelection = UseSelection;
            Filter.UseVisibility = UseVisibility;
        }

        #endregion

        // However sub-classes need to be able to tweak these to their choosing.

        // The idea is that once a collection is established, its parameters don't change.
    }
}