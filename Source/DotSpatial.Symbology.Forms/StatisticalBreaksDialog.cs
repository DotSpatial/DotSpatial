// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Data;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// StatisticalBreaksDialog.
    /// </summary>
    public partial class StatisticalBreaksDialog : Form
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticalBreaksDialog"/> class.
        /// </summary>
        public StatisticalBreaksDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the string field name to apply statistics to.
        /// </summary>
        public string FieldName
        {
            get
            {
                return breakSliderGraph1.Fieldname;
            }

            set
            {
                breakSliderGraph1.Fieldname = value;
            }
        }

        /// <summary>
        /// Gets or sets the normalization field to apply statistics to.
        /// </summary>
        public string NormalizationField
        {
            get
            {
                return breakSliderGraph1.NormalizationField;
            }

            set
            {
                breakSliderGraph1.NormalizationField = value;
            }
        }

        /// <summary>
        /// Gets or sets the feature scheme to use for coloring and existing break positions.
        /// </summary>
        public IFeatureScheme Scheme
        {
            get
            {
                return breakSliderGraph1.Scheme;
            }

            set
            {
                breakSliderGraph1.Scheme = value;
            }
        }

        /// <summary>
        /// Gets or sets the Table that has the data row values to use for statistics.
        /// </summary>
        public DataTable Table
        {
            get
            {
                return breakSliderGraph1.Table;
            }

            set
            {
                breakSliderGraph1.Table = value;
            }
        }

        /// <summary>
        /// Gets or sets the title for the graph.
        /// </summary>
        public string Title
        {
            get
            {
                return breakSliderGraph1.Title;
            }

            set
            {
                breakSliderGraph1.Title = value;
            }
        }

        #endregion

        #region Methods

        private void ChkLogCheckedChanged(object sender, EventArgs e)
        {
            breakSliderGraph1.LogY = chkLog.Checked;
        }

        private void ChkShowMeanCheckedChanged(object sender, EventArgs e)
        {
            breakSliderGraph1.ShowMean = chkShowMean.Checked;
        }

        private void ChkShowStdCheckedChanged(object sender, EventArgs e)
        {
            breakSliderGraph1.ShowStandardDeviation = chkShowStd.Checked;
        }

        private void CmbIntervalSelectedIndexChanged(object sender, EventArgs e)
        {
            breakSliderGraph1.IntervalMethod = (IntervalMethod)cmbInterval.SelectedItem;
            breakSliderGraph1.ResetBreaks(null);
            breakSliderGraph1.Invalidate();
        }

        private void NudColumnsValueChanged(object sender, EventArgs e)
        {
            breakSliderGraph1.NumColumns = (int)nudColumns.Value;
        }

        private void StatisticalBreaksDialogLoad(object sender, EventArgs e)
        {
            Array methods = Enum.GetValues(typeof(IntervalMethod));
            cmbInterval.Items.Clear();
            foreach (object method in methods)
            {
                cmbInterval.Items.Add(method);
            }

            cmbInterval.SelectedItem = IntervalMethod.EqualInterval;
        }

        #endregion
    }
}