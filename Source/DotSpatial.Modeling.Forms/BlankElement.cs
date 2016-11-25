// ********************************************************************************************************
// Product Name: DotSpatial.Tools.DataElement
// Description:  This class represents data in the model
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

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// A default element
    /// </summary>
    public class BlankElement : ModelElement
    {
        #region --------------- class variables

        #endregion

        #region --------------- Constructors

        /// <summary>
        /// Creates an instance of the Data Element
        /// <param name="modelElements">A list of all the elements in the model</param>
        /// </summary>
        public BlankElement(List<ModelElement> modelElements)
            : base(modelElements)
        {
            Width = 0;
            Height = 0;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="graph"></param>
        public override void Paint(Graphics graph)
        {
        }

        #endregion
    }
}