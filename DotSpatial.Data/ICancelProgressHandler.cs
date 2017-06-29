// ********************************************************************************************************
// Product Name: DotSpatial.Tools.ICancelProgressHandler
// Description:  Interface for tools for the DotSpatial toolbox
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

namespace DotSpatial.Data
{
    /// <summary>
    /// a IProgressHandler that carries a boolean property allowing the process using the handler to know if you should cancelled
    /// </summary>
    public interface ICancelProgressHandler : IProgressHandler
    {
        /// <summary>
        /// Returns true if the progress handler has been notified that the running process should be cancelled
        /// </summary>
        bool Cancel
        {
            get;
        }
    }
}