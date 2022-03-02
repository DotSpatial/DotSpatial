// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel.Composition;
using System.Drawing;
using DotSpatial.Data;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// Defines the way a tool interfaces with the toolbox.
    /// </summary>
    [InheritedExport]
    public interface ITool
    {
        #region Properties

        /// <summary>
        /// Gets a UniqueName Identifying this Tool, if another tool with the same UniqueName exists this tool will not be loaded. The is persisted by the model builder in saved files.
        /// </summary>
        string AssemblyQualifiedName { get; }

        /// <summary>
        /// Gets the Author of the tool's name.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Gets the category of tool that the ITool should be added to.
        /// </summary>
        string Category { get; }

        /// <summary>
        /// Gets the help text to be displayed when no input field is selected.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the image displayed in the help area when no input field is selected.
        /// </summary>
        Bitmap HelpImage { get; }

        /// <summary>
        /// Gets the address of the tools help web page in HTTP://... format. Return a empty string to hide the help hyperlink.
        /// </summary>
        string HelpUrl { get; }

        /// <summary>
        /// Gets a 32x32 Bitmap - The Large icon that will appears in the Tool Dialog Next to the tools name.
        /// </summary>
        Bitmap Icon { get; }

        /// <summary>
        /// Gets the input paramater array.
        /// </summary>
        Parameter[] InputParameters { get; }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the output paramater array.
        /// </summary>
        Parameter[] OutputParameters { get; }

        /// <summary>
        /// Gets a brief description displayed when the user hovers over the tool in the toolbox.
        /// </summary>
        string ToolTip { get; }

        /// <summary>
        /// Gets the Version of the tool.
        /// </summary>
        string Version { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Once the parameters have been configured the Execute command can be called, it returns true if successful.
        /// </summary>
        /// <param name="cancelProgressHandler">A cancel progress handler that used to indicate how much of the tool is done.</param>
        /// <returns>True, if executed succesfully.</returns>
        bool Execute(ICancelProgressHandler cancelProgressHandler);

        /// <summary>
        /// The parameters array should be populated with default values here.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Fires when one of the paramters value has beend changed, usually when a user changes a input or output parameters value, this can be used to populate other parameters default values.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        void ParameterChanged(Parameter sender);

        #endregion
    }
}