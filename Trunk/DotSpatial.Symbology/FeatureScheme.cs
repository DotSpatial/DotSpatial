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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/20/2009 2:00:23 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
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
        #region IFeatureScheme Members

        /// <summary>
        /// Occurs when a category indicates that its filter expression should be used
        /// to select its members.
        /// </summary>
        public event EventHandler<ExpressionEventArgs> SelectFeatures;

        /// <summary>
        /// Occurs when the deselect features context menu is clicked.
        /// </summary>
        public event EventHandler<ExpressionEventArgs> DeselectFeatures;
        #endregion

        /// <summary>
        /// Occurs when there are more than 1000 unique categories.  If the "Cancel"
        /// is set to true, then only the first 1000 categories are returned.  Otherwise
        /// it may allow the application to lock up, but will return all of them.
        /// If this event is not handled, cancle is assumed to be true.
        /// </summary>
        public event EventHandler<CancelEventArgs> TooManyCategories;

        /// <summary>
        /// Occurs
        /// </summary>
        public event EventHandler NonNumericField;

        #region Private Variables

        private readonly Dictionary<string, ArrayList> _cachedUniqueValues;
        private bool _appearsInLegend;
        private UITypeEditor _propertyEditor;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of FeatureScheme
        /// </summary>
        protected FeatureScheme()
        {
            // This normally is replaced by a shape type specific collection, and is just a precaution.
            _appearsInLegend = false;
            _cachedUniqueValues = new Dictionary<string, ArrayList>();
            EditorSettings = new FeatureEditorSettings();
            Breaks = new List<Break>();
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a boolean that indicates whether or not the legend should draw this item as a categorical
        /// tier in the legend.  If so, it will allow the LegendText to be visible as a kind of group for the
        /// categories.  If not, the categories will appear directly below the layer.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets a boolean that indicates whether or not the legend should draw this item as a grouping")]
        [Serialize("AppearsInLegend")]
        public bool AppearsInLegend
        {
            get { return _appearsInLegend; }
            set { _appearsInLegend = value; }
        }

        /// <summary>
        /// When using this scheme to define the symbology, these individual categories will be referenced in order to
        /// create genuine categories (that will be cached).
        /// </summary>
        public abstract IEnumerable<IFeatureCategory> GetCategories();

        /// <summary>
        /// Gets or sets the dialog settings
        /// </summary>
        public new FeatureEditorSettings EditorSettings
        {
            get { return base.EditorSettings as FeatureEditorSettings; }
            set { base.EditorSettings = value; }
        }

        /// <summary>
        /// Gets or sets the UITypeEditor to use for editing this FeatureScheme
        /// </summary>
        public UITypeEditor PropertyEditor
        {
            get { return _propertyEditor; }
            protected set { _propertyEditor = value; }
        }

        /// <summary>
        /// Gets the number of categories in this scheme
        /// </summary>
        public virtual int NumCategories
        {
            get { return 0; }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// An enumerable of LegendItems allowing the true members to be cycled through.
        /// </summary>
        [ShallowCopy]
        public override IEnumerable<ILegendItem> LegendItems
        {
            get
            {
                if (AppearsInLegend)
                {
                    return GetCategories().Cast<ILegendItem>();
                }

                return base.LegendItems;
            }
        }

        /// <summary>
        /// Queries this layer and the entire parental tree up to the map frame to determine if
        /// this layer is within the selected layers.
        /// </summary>
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
        /// Special handling of not copying the parent during a copy operation
        /// </summary>
        /// <param name="copy"></param>
        protected override void OnCopy(Descriptor copy)
        {
            FeatureScheme scheme = copy as FeatureScheme;
            if (scheme != null && scheme.SelectFeatures != null)
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
        /// <param name="source"></param>
        protected override void OnCopyProperties(object source)
        {
            base.OnCopyProperties(source);
            ILegendItem parent = GetParentItem();
            IFeatureLayer p = parent as IFeatureLayer;
            if (p != null) p.ApplyScheme(this);
        }

        /// <summary>
        /// Ensures that the parentage gets set properly in the event that
        /// this scheme is not appearing in the legend.
        /// </summary>
        /// <param name="value"></param>
        protected override void OnSetParentItem(ILegendItem value)
        {
            base.OnSetParentItem(value);
            if (_appearsInLegend) return;
            IEnumerable<IFeatureCategory> categories = GetCategories();
            foreach (IFeatureCategory category in categories)
            {
                category.SetParentItem(value);
            }
        }

        #endregion

        #region Symbology Methods

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
                    //MessageBox.Show(MessageStrings.FieldNotNumeric);
                    if (NonNumericField != null) NonNumericField(this, EventArgs.Empty);
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
                    //MessageBox.Show(MessageStrings.FieldNotNumeric);
                    if (NonNumericField != null) NonNumericField(this, EventArgs.Empty);
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
        /// <returns></returns>
        public override ICategory CreateRandomCategory()
        {
            return CreateRandomCategory(string.Empty);
        }

        /// <summary>
        /// Gets the values from a file based data source rather than an in memory object.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="progressHandler"></param>
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
                                if (!double.TryParse(row[normField].ToString(), out norm) || double.IsNaN(val))
                                    continue;
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
                                if (!double.TryParse(row[normField].ToString(), out norm) || double.IsNaN(val))
                                    continue;
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

                Random rnd = new Random();
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
                            if (!double.TryParse(dr[normField].ToString(), out norm))
                                failed = true;
                        } while (randomValues.ContainsKey(index) || double.IsNaN(val) || failed);
                        if (normField != null)
                        {
                            Values.Add(val / norm);
                        }
                        else
                        {
                            Values.Add(val);
                        }
                        randomValues.Add(index, val);
                        pm.CurrentValue = i + iPage * countPerPage;
                    }
                    //Application.DoEvents();
                    if (progressHandler != null && progressHandler.Cancel)
                    {
                        break;
                    }
                }
            }
            Values.Sort();
            Statistics.Calculate(Values);
            return;
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
                        Random rnd = new Random();
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
                            } while (randomValues.ContainsKey(index) || double.IsNaN(val) || failed);
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
                    continue;
                }
            }
            else
            {
                // Grab random samples
                Dictionary<int, double> randomValues = new Dictionary<int, double>();
                int count = EditorSettings.MaxSampleCount;
                int max = table.Rows.Count;
                Random rnd = new Random();
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
                    } while (randomValues.ContainsKey(index) || double.IsNaN(val) || failed);
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
        /// Returns
        /// </summary>
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

        /// <summary>
        /// Gets a list of all unique values of the attribute field.
        /// </summary>
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
                        if (TooManyCategories != null) TooManyCategories(this, args);
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

        /// <summary>
        /// Gets a list of all unique values of the attribute field.
        /// </summary>
        private static List<Break> GetUniqueValues(string fieldName, DataTable table)
        {
            ArrayList lst = new ArrayList();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool containsNull = false;
            foreach (DataRow dr in table.Rows)
            {
                object val = dr[fieldName];
                if (val == null || dr[fieldName] is DBNull)
                {
                    val = "[NULL]";
                    containsNull = true;
                }
                if (val.ToString() == string.Empty)
                {
                    containsNull = true;
                    val = "[NULL]";
                }

                if (lst.Contains(val)) continue;
                if (val.ToString() != "[NULL]") lst.Add(val);
            }
            sw.Stop();
            //Debug.WriteLine("GetUniqueValuesTime: " + sw.ElapsedMilliseconds);
            lst.Sort(); // breaks if a value is null.
            List<Break> result = new List<Break>();
            if (containsNull) result.Add(new Break("[NULL]"));
            foreach (object item in lst)
            {
                result.Add(new Break(item.ToString()));
            }
            return result;
        }

        /// <summary>
        /// This checks the type of the specified field whether it's a string field
        /// </summary>
        private static bool CheckFieldType(string fieldName, DataTable table)
        {
            return table.Columns[fieldName].DataType == typeof(string);
        }

        /// <summary>
        /// This checks the type of the specified field whether it's a string field
        /// </summary>
        private static bool CheckFieldType(string fieldName, IAttributeSource source)
        {
            return source.GetColumn(fieldName).DataType == typeof(string);
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
                //get the color for the category
                Color randomColor = colorRamp[colorIndex];
                double size = sizeRamp[colorIndex];
                IFeatureCategory cat = CreateNewCategory(randomColor, size) as IFeatureCategory;
                if (cat != null)
                {
                    //cat.SelectionSymbolizer = _selectionSymbolizer.Copy();
                    cat.LegendText = brk.Name;
                    if (isStringField)
                        cat.FilterExpression = fieldExpression + "= '" + brk.Name.Replace("'", "''") + "'";
                    else
                        cat.FilterExpression = fieldExpression + "=" + brk.Name;

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
                //get the color for the category
                Color randomColor = colorRamp[colorIndex];
                double size = sizeRamp[colorIndex];
                IFeatureCategory cat = CreateNewCategory(randomColor, size) as IFeatureCategory;

                if (cat != null)
                {
                    //cat.SelectionSymbolizer = _selectionSymbolizer.Copy();
                    cat.LegendText = brk.Name;

                    if (isStringField)
                        cat.FilterExpression = fieldExpression + "= '" + brk.Name.Replace("'", "''") + "'";
                    else
                        cat.FilterExpression = fieldExpression + "=" + brk.Name;
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
            if (SelectFeatures != null) SelectFeatures(sender, myE);
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
            if (DeselectFeatures != null) DeselectFeatures(sender, myE);
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
                double dr = (EditorSettings.EndSize - start);
                double dx = dr / count;
                if (!EditorSettings.RampColors)
                {
                    Random rnd = new Random(DateTime.Now.Millisecond);
                    for (int i = 0; i < count; i++)
                    {
                        result.Add(start + rnd.NextDouble() * dr);
                    }
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        result.Add(start + i * dx);
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

        #endregion
    }
}