// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using DotSpatial.Modeling.Forms.Elements;

namespace DotSpatial.Modeling.Forms.Parameters
{
    /// <summary>
    /// A File access parameter.
    /// </summary>
    public class FileParam : Parameter
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileParam"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="filter">The string dialog filter to use.</param>
        public FileParam(string name, string filter)
        {
            Name = name;
            ParamVisible = ShowParamInModel.Always;
            ParamType = "DotSpatial File Param";
            DialogFilter = filter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileParam"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        public FileParam(string name)
        {
            Name = name;
            ParamVisible = ShowParamInModel.Always;
            ParamType = "DotSpatial File Param";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileParam"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">A TextFile.</param>
        public FileParam(string name, TextFile value)
        {
            Name = name;
            ParamVisible = ShowParamInModel.Always;
            ParamType = "DotSpatial File Param";
            Value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the filter expression to limit extensions that can be used for this FileParam.
        /// </summary>
        public string DialogFilter { get; set; }

        /// <summary>
        /// Gets or sets the value of the parameter (This is also the default value for input).
        /// </summary>
        public new TextFile Value
        {
            get
            {
                return (TextFile)base.Value;
            }

            set
            {
                base.Value = value;
                DefaultSpecified = true;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generates a default instance of the data type so that tools have something to write too.
        /// </summary>
        /// <param name="path">Path of the generated text file.</param>
        public override void GenerateDefaultOutput(string path)
        {
            TextFile addedTextFile = new TextFile
                                         {
                                             Filename = Path.GetDirectoryName(path) + Path.DirectorySeparatorChar + ModelName
                                         };
            Value = addedTextFile;
        }

        /// <inheritdoc />
        public override DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            return new OpenFileElement(this, dataSets);
        }

        /// <inheritdoc />
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return new SaveFileElement(this, dataSets);
        }

        #endregion
    }
}