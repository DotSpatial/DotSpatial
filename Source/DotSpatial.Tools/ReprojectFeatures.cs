// -----------------------------------------------------------------------
// *******************************************************************************************************
// Product: DotSpatial.Tools.ReprojectFeatures.cs
// Description:  Reprojects a given featureset to a new coordinate system.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name                   |   Date                 |         Comments
//------------------------|------------------------|------------------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some formatting issues using re-sharper
// KP                     |  9/2009                |  Used IDW as model for ReprojectFeatureSet
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Projections;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Reproject Feature Set
    /// </summary>
    public class ReprojectFeatureSet : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the ReprojectFeatureSet class.
        /// </summary>
        public ReprojectFeatureSet()
        {
            this.Name = TextStrings.ReprojectFeatures;
            this.Category = TextStrings.SpatialReference;
            this.Description = TextStrings.ReprojectFeatureSetDescription;
            this.ToolTip = TextStrings.Reprojectsallcoordinates;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or Sets the input parameter array
        /// </summary>
        public override Parameter[] InputParameters
        {
            get
            {
                return _inputParam;
            }
        }

        /// <summary>
        /// Gets or Sets the output parameter array
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
        /// Once the parameters have been configured the Execute command can be called, it returns true if successful
        /// </summary>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet input1 = _inputParam[0].Value as IFeatureSet;
            if (input1 != null)
            {
                input1.FillAttributes();
            }

            ProjectionParam source = _inputParam[1] as ProjectionParam;
            ProjectionParam dest = _inputParam[2] as ProjectionParam;
            ProjectionInfo pSource = null;
            if (source != null)
            {
                pSource = source.Value;
            }

            if (dest == null)
            {
                return false;
            }

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input1, pSource, dest.Value, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the ReprojectFeatureSet Operation tool programaticaly.
        /// </summary>
        /// <param name="featureSet">The input FeatureSet.</param>
        /// <param name="sourceProjection">The input Expression string to select features to Delete.</param>
        /// <param name="destProjection">The target projected coordinate system to reproject the featureset to</param>
        /// <param name="output">The output FeatureSet.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// Ping deleted "static" for external testing
        /// <returns></returns>
        public bool Execute(
            IFeatureSet featureSet,
            ProjectionInfo sourceProjection,
            ProjectionInfo destProjection,
            IFeatureSet output,
            ICancelProgressHandler cancelProgressHandler)
        {
            output.CopyFeatures(featureSet, true);
            output.Projection = featureSet.Projection;
            if (sourceProjection != null)
            {
                output.Projection = sourceProjection;
            }

            output.Reproject(destProjection);
            output.SaveAs(output.Filename, true);
            return true;
        }

        /// <summary>
        /// The parameters array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[3];
            _inputParam[0] = new FeatureSetParam(TextStrings.InputFeatureSet)
                                 {
                                     HelpText = TextStrings.InputFeatureSettoreproject
                                 };

            _inputParam[1] = new ProjectionParam(TextStrings.SourceProjection)
                                 {
                                     HelpText = TextStrings.sourceprojectiondifferent
                                 };

            _inputParam[2] = new ProjectionParam(TextStrings.DesiredOutputProjection)
                                 {
                                     HelpText = TextStrings.Thedestinationprojection
                                 };

            _outputParam = new Parameter[1];
            _outputParam[0] = new FeatureSetParam(TextStrings.OutputFeatureSet)
                                  {
                                      HelpText = TextStrings.SelectResultFeatureSetDirectory
                                  };
        }

        /// <summary>
        /// Fires when one of the parameters value has been changed, usually when a user changes a input or output
        /// parameters value, this can be used to populate input2 parameters default values.
        /// </summary>
        public override void ParameterChanged(Parameter sender)
        {
            // This will give the Featureset values to second parameter
            if (sender != _inputParam[0])
            {
                return;
            }

            FeatureSetParam fsp = _inputParam[0] as FeatureSetParam;
            if (fsp == null || fsp.Value == null)
            {
                return;
            }

            _inputParam[1].Value = fsp.Value.Projection;
        }

        #endregion
    }
}