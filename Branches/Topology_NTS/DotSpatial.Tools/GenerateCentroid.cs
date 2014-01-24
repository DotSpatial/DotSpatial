// *******************************************************************************************************
// Product: DotSpatial.Tools.GenerateCentroid.cs
// Description:  Generate a centroid featureset from an input featureset.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name                   |   Date                 |         Comments
//------------------------|------------------------|------------------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some formatting issues using re-sharper
// Ted Dunsford           |  9/17/2009             |  Copy attributes from the original featureset.
// KP                     |  9/2009                |  Used IDW as model for GenerateCentroid
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using System;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Topology;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Generate Centroid tool
    /// </summary>
    public class GenerateCentroid : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Creates a new instance of the centroid tool
        /// </summary>
        public GenerateCentroid()
        {
            this.Name = TextStrings.GenerateCentroid;
            this.Category = TextStrings.VectorOverlay;
            this.Description = TextStrings.GenerateCentroidDescription;
            this.ToolTip = TextStrings.GenerateCentroidfrominputFeatureSet;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or Sets the input paramater array
        /// </summary>
        public override Parameter[] InputParameters
        {
            get
            {
                return _inputParam;
            }
        }

        /// <summary>
        /// Gets or Sets the output paramater array
        /// </summary>
        public override Parameter[] OutputParameters
        {
            get
            {
                return _outputParam;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Once the parameters have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet input1 = _inputParam[0].Value as IFeatureSet;
            if (input1 != null)
            {
                input1.FillAttributes();
            }

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input1, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the generate centroid FeatureSet Opaeration tool programaticaly.
        /// Ping deleted static for external testing 01/2010
        /// </summary>
        /// <param name="input1">The input FeatureSet.</param>
        /// <param name="output">The output FeatureSet.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns></returns>
        public bool Execute(IFeatureSet input1, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (input1 == null || output == null)
            {
                return false;
            }

            bool multiPoint = false;
            foreach (IFeature f1 in input1.Features)
            {
                if (f1.NumGeometries > 1)
                {
                    multiPoint = true;
                }
            }

            output.FeatureType = multiPoint == false ? FeatureType.Point : FeatureType.MultiPoint;

            int previous = 0;
            int i = 0;
            int maxFeature = input1.Features.Count;
            output.CopyTableSchema(input1);
            foreach (IFeature f in input1.Features)
            {
                if (cancelProgressHandler.Cancel)
                {
                    return false;
                }

                IFeature fnew = new Feature(f.Centroid());

                // Add the centroid to output
                output.Features.Add(fnew);

                fnew.CopyAttributes(f);

                int current = Convert.ToInt32(Math.Round(i * 100D / maxFeature));

                // only update when increment in percentage
                if (current > previous)
                {
                    cancelProgressHandler.Progress(string.Empty, current, current + TextStrings.progresscompleted);
                }

                previous = current;
                i++;
            }

            output.SaveAs(output.Filename, true);
            return true;
        }

        /// <summary>
        /// The parameters array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[1];
            _inputParam[0] = new FeatureSetParam(TextStrings.input1FeatureSet)
                                 {
                                     HelpText = TextStrings.InputFeatureSettogenerate
                                 };

            _outputParam = new Parameter[1];
            _outputParam[0] = new FeatureSetParam(TextStrings.OutputFeatureSet)
                                  {
                                      HelpText = TextStrings.SelectResultFeatureSetDirectory
                                  };
        }

        #endregion
    }
}