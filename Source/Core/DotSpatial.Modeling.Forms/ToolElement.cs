// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// the basic element for tools.
    /// </summary>
    public class ToolElement : ModelElement
    {
        #region Fields

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolElement"/> class.
        /// </summary>
        /// <param name="tool">The tool that the tool element represents.</param>
        /// <param name="modelElements">A list of all the elements in the model.</param>
        public ToolElement(ITool tool, List<ModelElement> modelElements)
            : base(modelElements)
        {
            Tool = tool;
            UpdateStatus();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list of Elements presently available in the Modeler.
        /// </summary>
        public List<ModelElement> ElementsInModel { get; set; }

        /// <summary>
        /// Gets or sets the status of the tools execution, used when the modeler runs the model.
        /// </summary>
        public ToolExecuteStatus ExecutionStatus { get; set; }

        /// <summary>
        /// Gets the ITool this element represents.
        /// </summary>
        public ITool Tool { get; }

        /// <summary>
        /// Gets the current status of the tool.
        /// </summary>
        public ToolStatus ToolStatus { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// When the user double clicks on a tool call this method.
        /// </summary>
        /// <returns>Indicates whether the tool dialog was exited with ok.</returns>
        public override bool DoubleClick()
        {
            using (ToolDialog td = new ToolDialog(Tool, ModelElements))
            {
                return td.ShowDialog() == DialogResult.OK;
            }
        }

        /// <summary>
        /// Updates the status indicator.
        /// </summary>
        public void UpdateStatus()
        {
            using (ToolDialog td = new ToolDialog(Tool, ModelElements))
            {
                ToolStatus = td.ToolStatus;
            }
        }

        /// <summary>
        /// Draws the status light on the background of the DataElement.
        /// </summary>
        /// <param name="graph">Graphics object used for drawing.</param>
        protected override void DrawStatusLight(Graphics graph)
        {
            Bitmap statusImage = Images.valid;
            if (ToolStatus == ToolStatus.Empty)
                statusImage = Images.Caution;
            else if (ToolStatus == ToolStatus.Error)
                statusImage = Images.Error;
            graph.DrawImage(statusImage, 5, 5, statusImage.Width, statusImage.Height);
        }

        #endregion
    }
}