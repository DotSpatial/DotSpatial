// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Windows.Forms;
using DotSpatial.Modeling.Forms.Parameters;
using DotSpatial.Symbology.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    /// <summary>
    /// A modular component that can be inherited to retrieve parameters for functions.
    /// </summary>
    internal partial class IndexElement : DialogElement
    {
        #region Fields

        private bool _click;
        private bool _enableUpdate = true;
        private string _expression;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexElement"/> class.
        /// </summary>
        /// <param name="param">Parameter for this object.</param>
        public IndexElement(IndexParam param)
        {
            InitializeComponent();
            GroupBox.Text = param.Name;

            // We save the parameters passed in
            base.Param = param;
            HandleStatusLight();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the expression.
        /// </summary>
        public string Expression => _expression;

        /// <summary>
        /// Gets or sets the Parameter that the element represents.
        /// </summary>
        public new IndexParam Param
        {
            get
            {
                return (IndexParam)base.Param;
            }

            set
            {
                base.Param = value;
            }
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override void Refresh()
        {
            HandleStatusLight();
        }

        // User Click the Select Button
        private void BtnSelectClick(object sender, EventArgs e)
        {
            if (_enableUpdate)
            {
                if (((IndexParam)base.Param).Fs == null || ((IndexParam)base.Param).Fs.DataTable == null) return;

                if (((IndexParam)base.Param).Fs.DataTable.Rows.Count < 1) return;

                using (SqlExpressionDialog dlgExpression = new SqlExpressionDialog { Table = ((IndexParam)base.Param).Fs.DataTable })
                {
                    if (dlgExpression.ShowDialog() != DialogResult.OK) return;

                    _expression = dlgExpression.Expression;
                    textBox1.Text = _expression;
                    Status = ToolStatus.Ok;
                    _click = true;
                    LightTipText = ModelingMessageStrings.FeaturesSelected;
                }
            }
        }

        private void HandleStatusLight()
        {
            _enableUpdate = false;

            // We load the default parameters
            if (_click)
            {
                textBox1.Text = _expression;
                Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.FeaturesSelected;
            }
            else
            {
                textBox1.Text = @"Expression";
            }

            _enableUpdate = true;
        }

        /// <summary>
        /// When the text box is clicked this event fires.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void TextBox1Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        #endregion
    }
}