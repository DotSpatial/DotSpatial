// ********************************************************************************************************
// Product Name: DotSpatial.Tools.Enums
// Description:  An Enumeration defining all of the parameter types which can be passed back from a ITool
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
// The Initial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// Defines the data types which can be parameters for a ITool
    /// </summary>
    [Obsolete("Do not use it. This class is not used in DotSpatial anymore.")] // Marked in 1.7
    public enum ParameterType
    {
        /// <summary>
        /// Defines a parameter of type Int which takes a max/min and default value
        /// </summary>
        IntParam,
        /// <summary>
        /// Defines a parameter of type Double which takes a max/min and default value
        /// </summary>
        DoubleParam,
        /// <summary>
        /// Defines a parameter which specifies a raster
        /// </summary>
        RasterParam,
        /// <summary>
        /// Defines a parameter which specifies a Feature Set
        /// </summary>
        FeatureSetParam,
        /// <summary>
        /// Defines a parameter which specifies a Point Feature Set
        /// </summary>
        PointFeatureSetParam,
        /// <summary>
        /// Defines a parameter which specifies a Line Feature Set
        /// </summary>
        LineFeatureSetParam,
        /// <summary>
        /// Defines a parameter which specifies a Polygon Feature Set
        /// </summary>
        PolygonFeatureSetParam,
        /// <summary>
        /// Defines a parameter which specifies a string
        /// </summary>
        StringParam,
        /// <summary>
        /// Defines a parameter which specifies a boolean value
        /// </summary>
        BooleanParam,
        /// <summary>
        /// Defines a parameter that is presented by a comboBox in the form
        /// </summary>
        ListParam
    }
}