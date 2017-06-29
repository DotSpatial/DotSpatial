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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2009 2:36:26 PM
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

namespace DotSpatial.Symbology
{
    /// <summary>
    /// DrawingFilter
    /// </summary>
    public class DrawingFilter : IDrawingFilter
    {
        #region Events

        /// <summary>
        /// Occurs after this filter has built its internal list of items.
        /// </summary>
        public event EventHandler Initialized;

        #endregion

        #region Private Variables

        private IFeatureCategory _category;
        private int _chunk;
        private int _chunkSize;
        private int _count;
        private bool _countIsValid;
        private IDictionary<IFeature, IDrawnState> _drawnStates;
        private IFeatureList _featureList;
        private bool _isInitialized;
        private IFeatureScheme _scheme;
        private bool _selected;
        private bool _useCategory;
        private bool _useChunks;
        private bool _useSelection;
        private bool _useVisibility;
        private bool _visible;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DrawingFilter without using any chunks.  The use chunks
        /// value will be false, and sub-categories will not be selected based on the chunk.
        /// </summary>
        public DrawingFilter(IFeatureList features, IFeatureScheme scheme)
        {
            _useChunks = false;
            _chunkSize = -1;
            Configure(features, scheme);
        }

        /// <summary>
        /// Creates a new instance of DrawingFilter, sub-dividing the features into chunks.
        /// regardless of selection or category, chunks simply subdivide the filter
        /// into chunks of equal size.
        /// </summary>
        public DrawingFilter(IFeatureList features, IFeatureScheme scheme, int chunkSize)
        {
            _useChunks = true;
            _chunkSize = chunkSize;
            Configure(features, scheme);
        }

        private void Configure(IFeatureList features, IFeatureScheme scheme)
        {
            _featureList = features;
            _scheme = scheme;
            //features.FeatureAdded += new EventHandler<FeatureEventArgs>(features_FeatureAdded);
            //features.FeatureRemoved += new EventHandler<FeatureEventArgs>(features_FeatureRemoved);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a shallow copy
        /// </summary>
        /// <returns>Returns a shallow copy of this object.</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// This will use the filter expressions on the categories to change the categories for those members.
        /// This means that an item will be classified as the last filter that it qualifies for.
        /// </summary>
        /// <param name="scheme">The scheme of categories to apply to the drawing states</param>
        public void ApplyScheme(IFeatureScheme scheme)
        {
            _scheme = scheme;
            if (_isInitialized == false) DoInitialize();
            List<IFeatureCategory> fc = _scheme.GetCategories().ToList();
            // Short cut the rest of this (and prevent loading features) in the case where we know everything is in the default category
            if (fc.Count == 1 && string.IsNullOrEmpty(fc[0].FilterExpression)) return;
            List<DataTable> tables = new List<DataTable>(); // just in case there is more than one Table somehow
            List<DataRow> allRows = new List<DataRow>();
            List<DataTable> tempList = new List<DataTable>();
            bool containsFID = false;
            IEnumerable<IFeatureCategory> categories = _scheme.GetCategories();

            foreach (var category in categories)
            {
                if (category.FilterExpression != null && category.FilterExpression.Contains("[FID]"))
                {
                    containsFID = true;
                }
            }
            foreach (IFeature f in _featureList)
            {
                if (f.DataRow == null)
                {
                    f.ParentFeatureSet.FillAttributes();
                }

                if (f.DataRow != null)
                {
                    DataTable t = f.DataRow.Table;
                    if (tables.Contains(t) == false)
                    {
                        tables.Add(t);
                        if (containsFID && t.Columns.Contains("FID") == false)
                        {
                            f.ParentFeatureSet.AddFid();
                            tempList.Add(t);
                        }
                    }
                    allRows.Add(f.DataRow);
                }
                if (_drawnStates.ContainsKey(f)) _drawnStates[f].SchemeCategory = null;
            }
            foreach (IFeatureCategory cat in categories)
            {
                foreach (DataTable dt in tables)
                {
                    DataRow[] rows = dt.Select(cat.FilterExpression);

                    foreach (DataRow dr in rows)
                    {
                        int index = allRows.IndexOf(dr);
                        if (index != -1)
                        {
                            _drawnStates[_featureList[index]].SchemeCategory = cat;
                        }
                    }
                }
            }
            foreach (DataTable table in tempList)
            {
                table.Columns.Remove("FID");
            }
        }

        /// <summary>
        /// If UseChunks is true, this uses the index value combined with the chunk size
        /// to calculate the chunk, and also sets the category to the [0] category and the
        /// selection state to unselected.  This can be overridden in sub-classes to come up
        /// with a different default state.
        /// </summary>
        /// <param name="index">The integer index to get the default state of</param>
        /// <returns>An IDrawnState</returns>
        public virtual IDrawnState GetDefaultState(int index)
        {
            if (_useChunks)
            {
                return new DrawnState(_scheme.GetCategories().First(), false, index / _chunkSize, true);
            }
            return new DrawnState(_scheme.GetCategories().First(), false, 0, true);
        }

        /// <summary>
        /// Gets an enumator for cycling through exclusively the features that satisfy all the listed criteria,
        /// including chunk index, selected state, and scheme category.
        /// </summary>
        /// <returns>An Enumerator for cycling through the values</returns>
        public IEnumerator<IFeature> GetEnumerator()
        {
            if (_isInitialized == false) DoInitialize();

            Func<IDrawnState, bool> alwaysTrue = drawnState => true;
            Func<IDrawnState, bool> visConstraint = alwaysTrue; // by default, don't test visibility
            Func<IDrawnState, bool> selConstraint = alwaysTrue; // by default, don't test selection
            Func<IDrawnState, bool> catConstraint = alwaysTrue; // by default, don't test category
            Func<IDrawnState, bool> chunkConstraint = alwaysTrue; // by default, don't test chunk
            if (_useVisibility)
            {
                visConstraint = drawnState => drawnState.IsVisible == _visible;
            }
            if (_useChunks)
            {
                chunkConstraint = drawnState => drawnState.Chunk == _chunk;
            }
            if (_useSelection)
            {
                selConstraint = drawnState => drawnState.IsSelected == _selected;
            }
            if (_useCategory)
            {
                catConstraint = drawnState => drawnState.SchemeCategory == _category;
            }

            Func<IDrawnState, bool> constraint = drawnState => selConstraint(drawnState)
                                                               && chunkConstraint(drawnState)
                                                               && catConstraint(drawnState)
                                                               && visConstraint(drawnState);

            var query =
                from kvp in _drawnStates
                where
                    constraint(kvp.Value)
                select kvp.Key;
            query.Count();
            return query.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Invalidates this drawing filter, effectively eliminating all the original
        /// categories, selection statuses, and only keeps the basic chunk size.
        /// </summary>
        public void Invalidate()
        {
            _isInitialized = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the scheme category to use
        /// </summary>
        public IFeatureCategory Category
        {
            get { return _category; }
            set
            {
                if (_category != value) _countIsValid = false;
                _category = value;
            }
        }

        /// <summary>
        /// Gets the integer chunk that the filter should use
        /// </summary>
        public int Chunk
        {
            get { return _chunk; }
            set
            {
                if (_chunk != value) _countIsValid = false;
                _chunk = value;
            }
        }

        /// <summary>
        /// Gets or sets the integer size of each chunk.  Setting this to
        /// a new value will cycle through and update the chunk on all
        /// the features.
        /// </summary>
        public int ChunkSize
        {
            get { return _chunkSize; }
            set
            {
                if (_chunkSize != value)
                {
                    _chunkSize = value;
                    //UpdateChunks(); // chunks are obsolete
                }
            }
        }

        /// <summary>
        /// If the drawing state for any features has changed, or else if
        /// the state of any members has changed, this will cycle through
        /// the filter members and cache a new count.  If nothing has
        /// changed, then this will simply return the cached value.
        /// </summary>
        public int Count
        {
            get
            {
                if (_countIsValid) return _count;
                IEnumerator<IFeature> en = GetEnumerator();
                _count = 0;
                while (en.MoveNext())
                {
                    _count++;
                }
                _countIsValid = true;
                return _count;
            }
        }

        /// <summary>
        /// Gets the default category for the scheme.
        /// </summary>
        public IFeatureCategory DefaultCategory
        {
            get { return _scheme.GetCategories().First(); }
        }

        /// <summary>
        /// Gets the dictionary of drawn states that this drawing filter uses.
        /// </summary>
        public IDictionary<IFeature, IDrawnState> DrawnStates
        {
            get { return _drawnStates; }
        }

        /// <summary>
        /// Gets the underlying list of features that this drawing filter
        /// is ultimately based upon.
        /// </summary>
        public IFeatureList FeatureList
        {
            get { return _featureList; }
        }

        /// <summary>
        /// If chunks are being used, then this indicates the total count of chunks.
        /// Otherwise, this returns 1 as everything is effectively in one chunk.
        /// </summary>
        public int NumChunks
        {
            get
            {
                if (_useChunks) return Convert.ToInt32(Math.Ceiling(_featureList.Count / (double)_chunkSize));
                return 1;
            }
        }

        /// <summary>
        /// If UseSelection is true, this will get or set the boolean selection state
        /// that will be used to select values.
        /// </summary>
        public bool Selected
        {
            get { return _selected; }
            set
            {
                if (_selected != value)
                {
                    _countIsValid = false;
                    _selected = value;
                }
            }
        }

        /// <summary>
        /// This uses the feature as the key and attempts to find the specified drawn state
        /// that describes selection, chunk and category.
        /// </summary>
        /// <param name="key">The feature</param>
        /// <remarks>The strength is that if someone inserts a new member or re-orders
        /// the features in the featureset, we don't forget which ones are selected.
        /// The disadvantage is that duplicate features in the same featureset
        /// will cause an exception.</remarks>
        /// <returns></returns>
        public IDrawnState this[IFeature key]
        {
            get
            {
                if (_isInitialized == false) DoInitialize();
                if (_drawnStates == null) return null;
                return _drawnStates[key];
            }
            set
            {
                if (_isInitialized == false) DoInitialize();
                // this will cause an exception if _drawnStates is null, but that might be what is needed
                if (_drawnStates[key] != value) _countIsValid = false;
                _drawnStates[key] = value;
            }
        }

        /// <summary>
        /// This is less direct as it requires searching two indices rather than one, but
        /// allows access to the drawn state based on the feature ID.
        /// </summary>
        /// <param name="index">The integer index in the underlying featureSet.</param>
        /// <returns>The current IDrawnState for the current feature.</returns>
        public IDrawnState this[int index]
        {
            get
            {
                if (_isInitialized == false) DoInitialize();
                return _drawnStates[_featureList[index]];
            }
            set
            {
                if (_isInitialized == false) DoInitialize();
                if (_drawnStates[_featureList[index]] != value) _countIsValid = false;
                _drawnStates[_featureList[index]] = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether the filter should subdivide based on category.
        /// </summary>
        public bool UseCategory
        {
            get { return _useCategory; }
            set
            {
                if (_useCategory != value)
                {
                    _countIsValid = false;
                }
                _useCategory = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether we should use the chunk
        /// </summary>
        public bool UseChunks
        {
            get { return _useChunks; }
            set
            {
                if (_useChunks != value) _countIsValid = false;
                _useChunks = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether this filter should use the Selected
        /// </summary>
        public bool UseSelection
        {
            get { return _useSelection; }
            set
            {
                if (_useSelection != value) _countIsValid = false;
                _useSelection = value;
            }
        }

        /// <summary>
        /// Gets or sets the boolean indicating whether or not this feature should be drawn.
        /// </summary>
        public bool UseVisibility
        {
            get { return _useVisibility; }
            set
            {
                if (_useVisibility != value) _countIsValid = false;
                _useVisibility = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean that specifies whether to return visible, or hidden features if UseVisibility is true.
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set
            {
                if (_visible != value) _countIsValid = false;
                _visible = value;
            }
        }

        #endregion

        #region Protected Method

        /// <summary>
        /// Fires the Initialized Event
        /// </summary>
        protected virtual void OnInitialize()
        {
            _isInitialized = true;
            if (Initialized != null) Initialized(this, new EventArgs());
        }

        #endregion

        #region Event Handlers

        //void features_FeatureRemoved(object sender, FeatureEventArgs e)
        //{
        //    _drawnStates.Remove(e.Feature);
        //}

        //void features_FeatureAdded(object sender, FeatureEventArgs e)
        //{
        //    IDrawnState defaultState = GetDefaultState(_featureList.Count);
        //    _drawnStates.Add(new KeyValuePair<IFeature, IDrawnState>(e.Feature, defaultState));
        //}

        #endregion

        #region Private Methods

        /// <summary>
        /// This block of code actually cycles through the source features, and assigns a default
        /// drawing state to each feature.  I thought duplicate features would be less of a problem
        /// then people re-ordering an indexed list at some point, so for now we are using
        /// features to index the values.
        /// </summary>
        private void DoInitialize()
        {
            _drawnStates = new Dictionary<IFeature, IDrawnState>();
            for (int i = 0; i < _featureList.Count; i++)
            {
                _drawnStates.Add(new KeyValuePair<IFeature, IDrawnState>(_featureList[i], GetDefaultState(i)));
            }
            OnInitialize();
        }

        #endregion
    }
}