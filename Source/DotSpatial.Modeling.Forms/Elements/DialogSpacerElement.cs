// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Modeling.Forms.Elements
{
    /// <summary>
    /// An element for DialogSpacing.
    /// </summary>
    public partial class DialogSpacerElement : DialogElement
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogSpacerElement"/> class.
        /// </summary>
        public DialogSpacerElement()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current status the input.
        /// </summary>
        public override ToolStatus Status
        {
            get
            {
                return ToolStatus.Ok;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the text of the spacer.
        /// </summary>
        public override string Text
        {
            get
            {
                return _label1.Text;
            }

            set
            {
                _label1.Text = value;
            }
        }

        #endregion

        #region Methods

        private void Label1Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        #endregion
    }
}