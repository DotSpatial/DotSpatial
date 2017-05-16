// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.Drawing;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// A default element
    /// </summary>
    public class BlankElement : ModelElement
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BlankElement"/> class.
        /// </summary>
        /// <param name="modelElements">A list of all the elements in the model</param>
        public BlankElement(List<ModelElement> modelElements)
            : base(modelElements)
        {
            Width = 0;
            Height = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="graph">Graphics object used for drawing.</param>
        public override void Paint(Graphics graph)
        {
        }

        #endregion
    }
}