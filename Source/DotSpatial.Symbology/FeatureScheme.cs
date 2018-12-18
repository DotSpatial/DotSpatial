// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using DotSpatial.Data;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// FeatureScheme
    /// </summary>
    public abstract class FeatureScheme : Scheme, IFeatureScheme
    {
        #region Fields

        private readonly Dictionary<string, ArrayList> _cachedUniqueValues;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureScheme"/> class.
        /// </summary>
        protected FeatureScheme()
        {
            // This normally is replaced by a shape type specific collection, and is just a precaution.
            AppearsInLegend = false;
            _cachedUniqueValues = new Dictionary<string, ArrayList>();
            EditorSettings = new FeatureEditorSettings();
            Breaks = new List<Break>();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the deselect features context menu is clicked.
        /// </summary>
        public event EventHandler<ExpressionEventArgs> DeselectFeatures;

        /// <summary>
        /// Occurs
        /// </summary>
        public event EventHandler NonNumericField;

        /// <summary>
        /// Occurs when a category indicates that its filter expression should be used
        /// to select its members.
        /// </summary>
        public event EventHandler<ExpressionEventArgs> SelectFeatures;

        /// <summary>
        /// Occurs when there are more than 1000 unique categories. If the "Cancel"
        /// is set to true, then only the first 1000 categories are returned. Otherwise
        /// it may allow the application to lock up, but will return all of them.
        /// If this event is not handled, cancle is assumed to be true.
        /// </summary>
        public event EventHandler<CancelEventArgs> TooManyCategories;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether or not the legend should draw this item as a categorical
        /// tier in the legend. If so, it will allow the LegendText to be visible as a kind of group for the
        /// categories. If not, the categories will appear directly below the layer.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets a boolean that indicates whether or not the legend should draw this item as a grouping")]
        [Serialize("AppearsInLegend")]
        public bool AppearsInLegend { get; set; }

        /// <summary>
        /// Gets or sets the dialog settings
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FeatureEditorSettings EditorSettings
        {
            get
            {
                return base.EditorSettings as FeatureEditorSettings;
            }

            set
            {
                base.EditorSettings = value;
            }
        }

        /// <summary>
        /// Gets an enumerable of LegendItems allowing the true members to be cycled through.
        /// </summary>
        [ShallowCopy]
        public override IEnumerable<ILegendItem> LegendItems
        {
            get
            {
                if (AppearsInLegend)
                {
                    return GetCategories();
                }

                return base.LegendItems;
            }
        }

        /// <summary>
        /// Gets the number of categories in this scheme.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int NumCategories => 0;

        /// <summary>
        /// Gets or sets the UITypeEditor to use for editing this FeatureScheme.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UITypeEditor PropertyEditor { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates categories either based on unique values, or classification method.
        /// If the method is
        /// </summary>
        /// <param name="table">The System.DataTable to that has the data values to use</param>
        public void CreateCategories(DataTable table)
        {
            string fieldName = EditorSettings.FieldName;
            if (EditorSettings.ClassificationType == ClassificationType.Custom) return;

            if (EditorSettings.ClassificationType == ClassificationType.UniqueValues)
            {
                CreateUniqueCategories(fieldName, table);
            }
            else
            {
                if (table.Columns[fieldName].DataType == typeof(string))
                {
                    // MessageBox.Show(MessageStrings.FieldNotNumeric);
                    NonNumericField?.Invoke(this, EventArgs.Empty);
                    return;
                }

                if (GetUniqueValues(fieldName, table).Count <= EditorSettings.NumBreaks)
                {
                    CreateUniqueCategories(fieldName, table);
                }
                else
                {
                    GetValues(table);
                    CreateBreakCategories();
                }
            }

            AppearsInLegend = true;
            LegendText = fieldName;
        }

        /// <inheritdoc />
        public void CreateCategories(IAttributeSource source, ICancelProgressHandler progressHandler)
        {
            string fieldName = EditorSettings.FieldName;
            if (EditorSettings.ClassificationType == ClassificationType.Custom) return;

            if (EditorSettings.ClassificationType == ClassificationType.UniqueValues)
            {
                CreateUniqueCategories(fieldName, source, progressHandler);
            }
            else
            {
                if (source.GetColumn(fieldName).DataType == typeof(string))
                {
                    NonNumericField?.Invoke(this, EventArgs.Empty);
                    return;
                }

                if (!SufficientValues(fieldName, source, EditorSettings.NumBreaks))
                {
                    CreateUniqueCategories(fieldName, source, progressHandler);
                }
                else
                {
                    GetValues(source, progressHandler);
                    CreateBreakCategories();
                }
            }

            AppearsInLegend = true;
            LegendText = fieldName;
        }

        /// <summary>
        /// Uses the settings on this scheme to create a random category.
        /// </summary>
        /// <returns>A new IFeatureCategory</returns>
        /// <param name="filterExpression">The filter expression to use</param>
        public virtual IFeatureCategory CreateRandomCategory(string filterExpression)
        {
            return null;
        }

        /// <summary>
        /// This simply ensures that we are creating the appropriate empty filter expression version
        /// of create random category.
        /// </summary>
        /// <returns>The created category.</returns>
        public override ICategory CreateRandomCategory()
        {
            return CreateRandomCategory(string.Empty);
        }

        /// <summary>
        /// When using this scheme to define the symbology, these individual categories will be referenced in order to
        /// create genuine categories (that will be cached).
        /// </summary>
        /// <returns>The categories of the scheme.</returns>
        public abstract IEnumerable<IFeatureCategory> GetCategories();

        /// <summary>
        /// Gets the values from a file based data source rather than an in memory object.
        /// </summary>
        /// <param name="source">Source to get the values from.</param>
        /// <param name="progressHandler">The progress handler.</param>
        public void GetValues(IAttributeSource source, ICancelProgressHandler progressHandler)
        {
            int pageSize = 100000;

            Values = new List<double>();
            string normField = EditorSettings.NormField;
            string fieldName = EditorSettings.FieldName;
            if (source.NumRows() < EditorSettings.MaxSampleCount)
            {
                int numPages = (int)Math.Ceiling((double)source.NumRows() / pageSize);
                for (int ipage = 0; ipage < numPages; ipage++)
                {
                    int numRows = (ipage == numPages - 1) ? source.NumRows() % pageSize : pageSize;
                    DataTable table = source.GetAttributes(ipage * pageSize, numRows);
                    if (!string.IsNullOrEmpty(EditorSettings.ExcludeExpression))
                    {
                        DataRow[] rows = table.Select("NOT (" + EditorSettings.ExcludeExpression + ")");
                        foreach (DataRow row in rows)
                        {
                            double val;
                            if (!double.TryParse(row[fieldName].ToString(), out val)) continue;
                            if (double.IsNaN(val)) continue;

                            if (normField != null)
                            {
                                double norm;
                                if (!double.TryParse(row[normField].ToString(), out norm) || double.IsNaN(val)) continue;

                                Values.Add(val / norm);
                                continue;
                            }

                            Values.Add(val);
                        }
                    }
                    else
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            double val;
                            if (!double.TryParse(row[fieldName].ToString(), out val)) continue;
                            if (double.IsNaN(val)) continue;

                            if (normField != null)
                            {
                                double norm;
                                if (!double.TryParse(row[normField].ToString(), out norm) || double.IsNaN(val)) continue;

                                Values.Add(val / norm);
                                continue;
                            }

                            Values.Add(val);
                        }
                    }
                }
            }
            else
            {
                Dictionary<int, double> randomValues = new Dictionary<int, double>();
                pageSize = 10000;
                int count = EditorSettings.MaxSampleCount;

                // Specified seed is required for consistently recreating the break values
                Random rnd = new Random(9999);
                AttributePager ap = new AttributePager(source, pageSize);
                int countPerPage = count / ap.NumPages();
                ProgressMeter pm = new ProgressMeter(progressHandler, "Sampling " + count + " random values", count);
                for (int iPage = 0; iPage < ap.NumPages(); iPage++)
                {
                    for (int i = 0; i < countPerPage; i++)
                    {
                        double val;
                        double norm = 1;
                        int index;
                        bool failed = false;
                        do
                        {
                            index = rnd.Next(ap.StartIndex, ap.StartIndex + pageSize);
                            DataRow dr = ap.Row(index);
                            if (!double.TryParse(dr[fieldName].ToString(), out val)) failed = true;
                            if (normField == null) continue;

                            if (!double.TryParse(dr[normField].ToString(), out norm)) failed = true;
                        }
                        while (randomValues.ContainsKey(index) || double.IsNaN(val) || failed);

                        if (normField != null)
                        {
                            Values.Add(val / norm);
                        }
                        else
                        {
                            Values.Add(val);
                        }

                        randomValues.Add(index, val);
                        pm.CurrentValue = i + (iPage * countPerPage);
                    }

                    if (progressHandler != null && progressHandler.Cancel)
                    {
                        break;
                    }
                }
            }

            Values.Sort();
            Statistics.Calculate(Values);
        }

        /// <summary>
        /// Before attempting to create categories using a color ramp, this must be calculated
        /// to updated the cache of values that govern statistics and break placement.
        /// </summary>
        /// <param name="table">The data Table to use.</param>
        public void GetValues(DataTable table)
        {
            Values = new List<double>();
            string normField = EditorSettings.NormField;
            string fieldName = EditorSettings.FieldName;
            if (!string.IsNullOrEmpty(EditorSettings.ExcludeExpression))
            {
                DataRow[] rows = table.Select("NOT (" + EditorSettings.ExcludeExpression + ")");
                foreach (DataRow row in rows)
                {
                    if (rows.Length < EditorSettings.MaxSampleCount)
                    {
                        double val;
                        if (!double.TryParse(row[fieldName].ToString(), out val)) continue;
                        if (double.IsNaN(val)) continue;

                        if (normField != null)
                        {
                            double norm;
                            if (!double.TryParse(row[normField].ToString(), out norm) || double.IsNaN(val)) continue;

                            Values.Add(val / norm);
                            continue;
                        }

                        Values.Add(val);
                    }
                    else
                    {
                        Dictionary<int, double> randomValues = new Dictionary<int, double>();
                        int count = EditorSettings.MaxSampleCount;
                        int max = rows.Length;

                        // Specified seed is required for consistently recreating the break values
                        Random rnd = new Random(9999);

                        for (int i = 0; i < count; i++)
                        {
                            double val;
                            double norm = 1;
                            int index;
                            bool failed = false;
                            do
                            {
                                index = rnd.Next(max);
                                if (!double.TryParse(rows[index][fieldName].ToString(), out val)) failed = true;
                                if (normField == null) continue;

                                if (!double.TryParse(rows[index][normField].ToString(), out norm)) failed = true;
                            }
                            while (randomValues.ContainsKey(index) || double.IsNaN(val) || failed);

                            if (normField != null)
                            {
                                Values.Add(val / norm);
                            }
                            else
                            {
                                Values.Add(val);
                            }

                            randomValues.Add(index, val);
                        }
                    }
                }

                Values.Sort();
                Statistics.Calculate(Values);
                return;
            }

            if (table.Rows.Count < EditorSettings.MaxSampleCount)
            {
                // Simply grab all the values
                foreach (DataRow row in table.Rows)
                {
                    double val;
                    if (!double.TryParse(row[fieldName].ToString(), out val)) continue;
                    if (double.IsNaN(val)) continue;

                    if (normField == null)
                    {
                        Values.Add(val);
                        continue;
                    }

                    double norm;
                    if (!double.TryParse(row[normField].ToString(), out norm) || double.IsNaN(val)) continue;

                    Values.Add(val / norm);
                }
            }
            else
            {
                // Grab random samples
                Dictionary<int, double> randomValues = new Dictionary<int, double>();
                int count = EditorSettings.MaxSampleCount;
                int max = table.Rows.Count;

                // Specified seed is required for consistently recreating the break values
                Random rnd = new Random(9999);
                for (int i = 0; i < count; i++)
                {
                    double val;
                    double norm = 1;
                    int index;
                    bool failed = false;
                    do
                    {
                        index = rnd.Next(max);
                        if (!double.TryParse(table.Rows[index][fieldName].ToString(), out val)) failed = true;
                        if (normField == null) continue;

                        if (!double.TryParse(table.Rows[index][normField].ToString(), out norm)) failed = true;
                    }
                    while (randomValues.ContainsKey(index) || double.IsNaN(val) || failed);

                    if (normField != null)
                    {
                        Values.Add(val / norm);
                    }
                    else
                    {
                        Values.Add(val);
                    }

                    randomValues.Add(index, val);
                }
            }

            Values.Sort();
        }

        /// <summary>
        /// Queries this layer and the entire parental tree up to the map frame to determine if
        /// this layer is within the selected layers.
        /// </summary>
        /// <returns>True, if the layer is selected.</returns>
        public bool IsWithinLegendSelection()
        {
            if (IsSelected) return true;

            ILayer lyr = GetParentItem() as ILayer;
            while (lyr != null)
            {
                if (lyr.IsSelected) return true;

                lyr = lyr.GetParentItem() as ILayer;
            }

            return false;
        }

        /// <summary>
        /// This keeps the existing categories, but uses the current scheme settings to apply
        /// a new color to each of the symbols.
        /// </summary>
        public void RegenerateColors()
        {
            List<IFeatureCategory> cats = GetCategories().ToList();
            List<Color> colors = GetColorSet(cats.Count);
            int i = 0;
            foreach (IFeatureCategory category in cats)
            {
                category.SetColor(colors[i]);
                i++;
            }
        }

        /// <summary>
        /// Uses the current settings to generate a random color between the start and end color.
        /// </summary>
        /// <returns>A Randomly created color that is within the bounds of this scheme.</returns>
        protected Color CreateRandomColor()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            return CreateRandomColor(rnd);
        }

        /// <summary>
        /// Calculates the unique colors as a scheme.
        /// </summary>
        /// <param name="fs">The featureset with the data table definition</param>
        /// <param name="uniqueField">The unique field</param>
        /// <param name="categoryFunc">Func for creating category</param>
        /// <returns>A hastable with the unique colors.</returns>
        protected Hashtable GenerateUniqueColors(IFeatureSet fs, string uniqueField, Func<Color, IFeatureCategory> categoryFunc)
        {
            if (categoryFunc == null) throw new ArgumentNullException(nameof(categoryFunc));

            var result = new Hashtable(); // a hashtable of colors
            var dt = fs.DataTable;
            var vals = new ArrayList();
            var i = 0;
            var rnd = new Random();
            foreach (DataRow row in dt.Rows)
            {
                var ind = -1;
                if (uniqueField != "FID")
                {
                    if (!vals.Contains(row[uniqueField]))
                    {
                        ind = vals.Add(row[uniqueField]);
                    }
                }
                else
                {
                    ind = vals.Add(i);
                    i++;
                }

                if (ind == -1) continue;

                var item = vals[ind];
                var c = rnd.NextColor();
                while (result.ContainsKey(c))
                {
                    c = rnd.NextColor();
                }

                var cat = categoryFunc(c);
                string flt = "[" + uniqueField + "] = ";
                if (uniqueField == "FID")
                {
                    flt += item;
                }
                else
                {
                    if (dt.Columns[uniqueField].DataType == typeof(string))
                    {
                        flt += "'" + item + "'";
                    }
                    else
                    {
                        flt += Convert.ToString(item, CultureInfo.InvariantCulture);
                    }
                }

                cat.FilterExpression = flt;
                AddCategory(cat);
                result.Add(c, item);
            }

            return result;
        }

        /// <summary>
        /// This replaces the constant size calculation with a size
        /// calculation that is appropriate for features.
        /// </summary>
        /// <param name="count">The integer count of the number of sizes to create.</param>
        /// <returns>A list of double valued sizes.</returns>
        protected override List<double> GetSizeSet(int count)
        {
            List<double> result = new List<double>();
            if (EditorSettings.UseSizeRange)
            {
                double start = EditorSettings.StartSize;
                double dr = EditorSettings.EndSize - start;
                double dx = dr / count;
                if (!EditorSettings.RampColors)
                {
                    Random rnd = new Random(DateTime.Now.Millisecond);
                    for (int i = 0; i < count; i++)
                    {
                        result.Add(start + (rnd.NextDouble() * dr));
                    }
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        result.Add(start + (i * dx));
                    }
                }
            }
            else
            {
                Size2D sizes = new Size2D(2, 2);
                IPointSymbolizer ps = EditorSettings.TemplateSymbolizer as IPointSymbolizer;
                if (ps != null) sizes = ps.GetSize();
                double size = Math.Max(sizes.Width, sizes.Height);
                for (int i = 0; i < count; i++)
                {
                    result.Add(size);
                }
            }

            return result;
        }

        /// <summary>
        /// Special handling of not copying the parent during a copy operation.
        /// </summary>
        /// <param name="copy">The copy.</param>
        protected override void OnCopy(Descriptor copy)
        {
            FeatureScheme scheme = copy as FeatureScheme;
            if (scheme?.SelectFeatures != null)
            {
                foreach (var handler in scheme.SelectFeatures.GetInvocationList())
                {
                    scheme.SelectFeatures -= (EventHandler<ExpressionEventArgs>)handler;
                }
            }

            // Disconnecting the parent prevents the immediate application of copied scheme categories to the original layer.
            SuspendEvents();
            base.OnCopy(copy);
            ResumeEvents();
        }

        /// <summary>
        /// Handles the special case of not copying the parent during an on copy properties operation
        /// </summary>
        /// <param name="source">The source to copy the properties from.</param>
        protected override void OnCopyProperties(object source)
        {
            base.OnCopyProperties(source);
            ILegendItem parent = GetParentItem();
            IFeatureLayer p = parent as IFeatureLayer;
            p?.ApplyScheme(this);
        }

        /// <summary>
        /// Instructs the parent layer to select features matching the specified expression.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event args.</param>
        protected virtual void OnDeselectFeatures(object sender, ExpressionEventArgs e)
        {
            ExpressionEventArgs myE = e;
            if (EditorSettings.ExcludeExpression != null)
            {
                myE = new ExpressionEventArgs(myE.Expression + " AND NOT (" + EditorSettings.ExcludeExpression + ")");
            }

            DeselectFeatures?.Invoke(sender, myE);
        }

        /// <summary>
        /// Instructs the parent layer to select features matching the specified expression.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event args.</param>
        protected virtual void OnSelectFeatures(object sender, ExpressionEventArgs e)
        {
            ExpressionEventArgs myE = e;
            if (EditorSettings.ExcludeExpression != null)
            {
                myE = new ExpressionEventArgs(myE.Expression + " AND NOT (" + EditorSettings.ExcludeExpression + ")");
            }

            SelectFeatures?.Invoke(sender, myE);
        }

        /// <summary>
        /// Ensures that the parentage gets set properly in the event that
        /// this scheme is not appearing in the legend.
        /// </summary>
        /// <param name="value">Legend item to set the parent for.</param>
        protected override void OnSetParentItem(ILegendItem value)
        {
            base.OnSetParentItem(value);
            if (AppearsInLegend) return;

            IEnumerable<IFeatureCategory> categories = GetCategories();
            foreach (IFeatureCategory category in categories)
            {
                category.SetParentItem(value);
            }
        }

        /// <summary>
        /// This checks the type of the specified field whether it's a string field.
        /// </summary>
        /// <param name="fieldName">Name of the field that gets checked.</param>
        /// <param name="table">Table that contains the field.</param>
        /// <returns>True, if the field is of type string.</returns>
        private static bool CheckFieldType(string fieldName, DataTable table)
        {
            return table.Columns[fieldName].DataType == typeof(string);
        }

        /// <summary>
        /// This checks the type of the specified field whether it's a string field
        /// </summary>
        /// <param name="fieldName">Name of the field that gets checked.</param>
        /// <param name="source">Attribute source that contains the field.</param>
        /// <returns>True, if the field is of type boolean.</returns>
        private static bool CheckFieldType(string fieldName, IAttributeSource source)
        {
            return source.GetColumn(fieldName).DataType == typeof(string);
        }

        /// <summary>
        /// Gets a list of all unique values of the attribute field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="table">Table that contains the field.</param>
        /// <returns>A list of the unique values of the attribute field.</returns>
        private static List<Break> GetUniqueValues(string fieldName, DataTable table)
        {
            HashSet<object> lst = new HashSet<object>();
            bool containsNull = false;
            foreach (DataRow dr in table.Rows)
            {
                object val = dr[fieldName];
                if (val == null || dr[fieldName] is DBNull || val.ToString() == string.Empty)
                {
                    containsNull = true;
                }
                else if (!lst.Contains(val))
                {
                    lst.Add(val);
                }
            }

            List<Break> result = new List<Break>();
            if (containsNull) result.Add(new Break("[NULL]"));
            foreach (object item in lst.OrderBy(o => o))
            {
                result.Add(new Break(string.Format(CultureInfo.InvariantCulture, "{0}", item))); // Changed by jany_ (2015-07-27) use InvariantCulture because this is used in Datatable.Select in FeatureCategoryControl and causes error when german localization is used
            }

            return result;
        }

        /// <summary>
        /// Checks whether the source contains more than numValues different values in the given field.
        /// </summary>
        /// <param name="fieldName">Name of the field that gets checked.</param>
        /// <param name="source">Attribute source that contains the field.</param>
        /// <param name="numValues">Minimal number of values that should exist.</param>
        /// <returns>True, if more than num values exist.</returns>
        private static bool SufficientValues(string fieldName, IAttributeSource source, int numValues)
        {
            ArrayList lst = new ArrayList();
            AttributeCache ac = new AttributeCache(source, numValues);

            foreach (Dictionary<string, object> dr in ac)
            {
                object val = dr[fieldName] ?? "[NULL]";
                if (val.ToString() == string.Empty) val = "[NULL]";
                if (lst.Contains(val)) continue;

                lst.Add(val);
                if (lst.Count > numValues) return true;
            }

            return false;
        }

        private void CreateUniqueCategories(string fieldName, IAttributeSource source, ICancelProgressHandler progressHandler)
        {
            Breaks = GetUniqueValues(fieldName, source, progressHandler);

            string fieldExpression = "[" + fieldName.ToUpper() + "]";
            ClearCategories();

            bool isStringField = CheckFieldType(fieldName, source);

            ProgressMeter pm = new ProgressMeter(progressHandler, "Building Feature Categories", Breaks.Count);

            List<double> sizeRamp = GetSizeSet(Breaks.Count);
            List<Color> colorRamp = GetColorSet(Breaks.Count);
            for (int colorIndex = 0; colorIndex < Breaks.Count; colorIndex++)
            {
                Break brk = Breaks[colorIndex];

                // get the color for the category
                Color randomColor = colorRamp[colorIndex];
                double size = sizeRamp[colorIndex];
                IFeatureCategory cat = CreateNewCategory(randomColor, size) as IFeatureCategory;
                if (cat != null)
                {
                    cat.LegendText = brk.Name;
                    if (isStringField) cat.FilterExpression = fieldExpression + "= '" + brk.Name.Replace("'", "''") + "'";
                    else cat.FilterExpression = fieldExpression + "=" + brk.Name;

                    AddCategory(cat);
                }

                colorIndex++;
                pm.CurrentValue = colorIndex;
            }

            pm.Reset();
        }

        private void CreateUniqueCategories(string fieldName, DataTable table)
        {
            Breaks = GetUniqueValues(fieldName, table);
            List<double> sizeRamp = GetSizeSet(Breaks.Count);
            List<Color> colorRamp = GetColorSet(Breaks.Count);
            string fieldExpression = "[" + fieldName.ToUpper() + "]";
            ClearCategories();

            bool isStringField = CheckFieldType(fieldName, table);

            int colorIndex = 0;

            foreach (Break brk in Breaks)
            {
                // get the color for the category
                Color randomColor = colorRamp[colorIndex];
                double size = sizeRamp[colorIndex];
                IFeatureCategory cat = CreateNewCategory(randomColor, size) as IFeatureCategory;

                if (cat != null)
                {
                    cat.LegendText = brk.Name;

                    if (isStringField) cat.FilterExpression = fieldExpression + "= '" + brk.Name.Replace("'", "''") + "'";
                    else cat.FilterExpression = fieldExpression + "=" + brk.Name;
                    if (cat.FilterExpression != null)
                    {
                        if (cat.FilterExpression.Contains("=[NULL]"))
                        {
                            cat.FilterExpression = cat.FilterExpression.Replace("=[NULL]", " is NULL");
                        }
                        else if (cat.FilterExpression.Contains("= '[NULL]'"))
                        {
                            cat.FilterExpression = cat.FilterExpression.Replace("= '[NULL]'", " is NULL");
                        }
                    }

                    AddCategory(cat);
                }

                colorIndex++;
            }
        }

        /// <summary>
        /// Gets a list of all unique values of the attribute field.
        /// </summary>
        /// <param name="fieldName">Name of the field that gets checked.</param>
        /// <param name="source">Attribute source that contains the field.</param>
        /// <param name="progressHandler">The progress handler.</param>
        /// <returns>A list with the unique values.</returns>
        private List<Break> GetUniqueValues(string fieldName, IAttributeSource source, ICancelProgressHandler progressHandler)
        {
            ArrayList lst;
            bool hugeCountOk = false;
            if (_cachedUniqueValues.ContainsKey(fieldName))
            {
                lst = _cachedUniqueValues[fieldName];
            }
            else
            {
                lst = new ArrayList();
                AttributePager ap = new AttributePager(source, 5000);
                ProgressMeter pm = new ProgressMeter(progressHandler, "Discovering Unique Values", source.NumRows());
                for (int row = 0; row < source.NumRows(); row++)
                {
                    object val = ap.Row(row)[fieldName] ?? "[NULL]";
                    if (val.ToString() == string.Empty) val = "[NULL]";
                    if (lst.Contains(val)) continue;

                    lst.Add(val);
                    if (lst.Count > 1000 && !hugeCountOk)
                    {
                        CancelEventArgs args = new CancelEventArgs(true);
                        TooManyCategories?.Invoke(this, args);
                        if (args.Cancel) break;

                        hugeCountOk = true;
                    }

                    pm.CurrentValue = row;
                    if (progressHandler.Cancel) break;
                }

                lst.Sort();
                if (lst.Count < EditorSettings.MaxSampleCount)
                {
                    _cachedUniqueValues[fieldName] = lst;
                }
            }

            List<Break> result = new List<Break>();

            if (lst != null)
            {
                foreach (object item in lst)
                {
                    result.Add(new Break(item.ToString()));
                }
            }

            return result;
        }

        #endregion
    }
}