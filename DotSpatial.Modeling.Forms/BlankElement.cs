// ********************************************************************************************************
// Product Name: DotSpatial.Tools.DataElement
// Description:  This class represents data in the model
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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