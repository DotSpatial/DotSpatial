// ********************************************************************************************************
// Product Name: DotSpatial.Tools.ModelElement
// Description:  An abstract class that handles drawing boxes for elements in the modeler window
//
// ********************************************************************************************************
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Nov, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// the basic element for tools
    /// </summary>
    public class ToolElement : ModelElement
    {
        #region Variables

        private readonly ITool _tool;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the tool element
        /// </summary>
        /// <param name="tool">The tool that the tool element represents</param>
        /// <param name="modelElements">A list of all the elements in the model</param>
        public ToolElement(ITool tool, List<ModelElement> modelElements)
            : base(modelElements)
        {
            _tool = tool;
            UpdateStatus();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the status of the tools execution, used when the modeler runs the model.
        /// </summary>
        public ToolExecuteStatus ExecutionStatus { get; set; }

        /// <summary>
        /// Gets the ITool this element represents
        /// </summary>
        public ITool Tool
        {
            get { return _tool; }
        }

        /// <summary>
        /// Gets or sets the list of Elements presently available in the Modeler
        /// </summary>
        public List<ModelElement> ElementsInModel
        {
            get { return ElementsInModel; }
            set { ElementsInModel = value; }
        }

        /// <summary>
        /// Gets the current status of the tool
        /// </summary>
        public ToolStatus ToolStatus { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the status indicator
        /// </summary>
        public void UpdateStatus()
        {
            using (ToolDialog td = new ToolDialog(_tool, ModelElements))
            { ToolStatus = td.ToolStatus; }
        }

        /// <summary>
        /// Draws the status light on the background of the DataElement
        /// </summary>
        /// <param name="graph"></param>
        protected override void DrawStatusLight(Graphics graph)
        {
            Bitmap statusImage = Images.valid;
            if (ToolStatus == ToolStatus.Empty)
                statusImage = Images.Caution;
            else if (ToolStatus == ToolStatus.Error)
                statusImage = Images.Error;
            graph.DrawImage(statusImage, 5, 5, statusImage.Width, statusImage.Height);
        }

        /// <summary>
        /// When the user double clicks on a tool call this method.
        /// </summary>
        public override bool DoubleClick()
        {
            using (ToolDialog td = new ToolDialog(_tool, ModelElements))
            {
                return td.ShowDialog() == DialogResult.OK;
            }
        }

        #endregion
    }
}