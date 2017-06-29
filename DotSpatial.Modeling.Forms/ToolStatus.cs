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

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// Indicates the allowed values for the status of the element, illustrated by the light
    /// </summary>
    public enum ToolStatus
    {
        /// <summary>
        /// Indicates that no value has been set for this yet.
        /// </summary>
        Empty,
        /// <summary>
        /// Indicates that the element parameter is ok and won't halt.
        /// </summary>
        Ok,
        /// <summary>
        /// Indicates that the element value will cause an error.
        /// </summary>
        Error,
    }
}