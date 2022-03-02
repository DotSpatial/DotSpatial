// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Data;
using System.Drawing;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Label categories are used to categorize labels with the same settings.
    /// </summary>
    public class LabelCategory : ILabelCategory
    {
        #region Fields

        private readonly Expression _exp;

        [Serialize("Expression")]
        private string _expression;

        [Serialize("FilterExpression")]
        private string _filterExpression;

        [Serialize("Name")]
        private string _name;

        [Serialize("SelectionSymbolizer")]
        private ILabelSymbolizer _selectionSymbolizer;

        [Serialize("Symbolizer")]
        private ILabelSymbolizer _symbolizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelCategory"/> class.
        /// </summary>
        public LabelCategory()
        {
            _symbolizer = new LabelSymbolizer();
            _selectionSymbolizer = new LabelSymbolizer
            {
                FontColor = Color.Cyan
            };
            _exp = new Expression();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the string expression that controls the integration of field values into the label text.
        /// This is the raw text that is used to do calculations and concat fields and strings.
        /// </summary>
        public string Expression
        {
            get
            {
                return _expression;
            }

            set
            {
                _expression = value;
            }
        }

        /// <summary>
        /// Gets or sets the string filter expression that controls which features
        /// that this should apply itself to.
        /// </summary>
        public string FilterExpression
        {
            get
            {
                return _filterExpression;
            }

            set
            {
                _filterExpression = value;
            }
        }

        /// <summary>
        /// Gets or sets the string name.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Gets or sets the text symbolizer to use for this category.
        /// </summary>
        public ILabelSymbolizer SelectionSymbolizer
        {
            get
            {
                return _selectionSymbolizer;
            }

            set
            {
                _selectionSymbolizer = value;
            }
        }

        /// <summary>
        /// Gets or sets the text symbolizer to use for this category.
        /// </summary>
        public ILabelSymbolizer Symbolizer
        {
            get
            {
                return _symbolizer;
            }

            set
            {
                _symbolizer = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the expression for the given row. The Expression.Columns have to be in sync with the Features columns for calculation to work without error.
        /// </summary>
        /// <param name="row">Datarow the expression gets calculated for.</param>
        /// <param name="selected">Indicates whether the feature is selected.</param>
        /// <param name="fid">The FID of the feature, the expression gets calculated for.</param>
        /// <returns>null if there was an error while parsing the expression, else the calculated expression.</returns>
        public string CalculateExpression(DataRow row, bool selected, int fid)
        {
            string ff = (selected ? _selectionSymbolizer : _symbolizer).FloatingFormat;
            _exp.FloatingFormat = ff?.Trim() ?? string.Empty;
            _exp.ParseExpression(_expression);
            return _exp.CalculateRowValue(row, fid);
        }

        /// <summary>
        /// Returns the Copy() method cast as an object.
        /// </summary>
        /// <returns>A copy of this label category.</returns>
        public object Clone()
        {
            return Copy();
        }

        /// <summary>
        /// Returns a shallow copy of this category with the exception of
        /// the TextSymbolizer, which is duplicated. This uses memberwise
        /// clone, so sublcasses using this method will return an appropriate
        /// version.
        /// </summary>
        /// <returns>A shallow copy of this object.</returns>
        public virtual LabelCategory Copy()
        {
            var result = MemberwiseClone() as LabelCategory;
            if (result == null) return null;

            result.Symbolizer = Symbolizer.Copy();
            result.SelectionSymbolizer = SelectionSymbolizer.Copy();
            return result;
        }

        /// <summary>
        /// Returns the categories name or "No Name" if the name is not set.
        /// </summary>
        /// <returns>The categories name or "No Name" if the name is not set.</returns>
        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? "<No Name>" : Name;
        }

        /// <summary>
        /// Updates the Expression-Object with the columns that exist inside the features that belong to this category. They are used for calculating the expression.
        /// </summary>
        /// <param name="columns">Columns that should be updated.</param>
        /// <returns>False if columns were not set.</returns>
        public bool UpdateExpressionColumns(DataColumnCollection columns)
        {
            return _exp.UpdateFields(columns);
        }

        /// <summary>
        /// Gets a copy of this label category.
        /// </summary>
        /// <returns>The copy.</returns>
        ILabelCategory ILabelCategory.Copy()
        {
            return Copy();
        }

        #endregion
    }
}