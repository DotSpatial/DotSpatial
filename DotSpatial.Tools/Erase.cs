// ********************************************************************************************************
// Product Name: MapWindow.Tools.Erase
// Description:  Eliminate Second featureset from first featureset
//
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is Toolbox.dll for the MapWindow 4.6/6 ToolManager project
//
// The Initializeializeial Developer of this Original Code is Kandasamy Prasanna. Created in 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------
// Ted Dunsford       |  8/24/2009         |  Cleaned up some unnecessary references using re-sharper
// KP                 |  9/2009            |  Used IDW as model for Erase
// Ping  Yang         |  12/2009           |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using System;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;

namespace DotSpatial.Tools
{
    /// <summary>
    /// An Erase tool
    /// </summary>
    public class Erase : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the Erase class.
        /// </summary>
        public Erase()
        {
            this.Name = TextStrings.Erase;
            this.Category = TextStrings.Analysis;
            this.Description = TextStrings.RraseDescription;
            this.ToolTip = TextStrings.EliminateSecondFeatureset;
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
        /// Executes the Erase Opaeration tool programaticaly
        /// </summary>
        /// <param name="self">The input feature that is to be erased</param>
        /// <param name="other">The other feature defining the area to remove</param>
        /// <param name="output">The resulting erased content</param>
        /// <param name="cancelProgressHandler">The progress handler</param>
        /// <returns>Boolean, true if the operation was a success</returns>
        public static bool Execute(
            IFeatureSet self, IFeatureSet other, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (self == null || other == null || output == null)
            {
                return false;
            }

            int previous;
            int max = self.Features.Count * other.Features.Count;

            output.CopyTableSchema(self); // Fill the 1st Featureset fields
            IFeatureSet tempSet = self.CombinedFields(other);

            // go through every feature in 1st featureSet
            for (int i = 0; i < self.Features.Count; i++)
            {
                // go through every feature in 2nd featureSet
                for (int j = 0; j < other.Features.Count; j++)
                {
                    self.Features[i].Difference(other.Features[j], tempSet, FieldJoinType.All);
                    previous = Convert.ToInt32(Math.Round(i * j * 50D / max));
                    if (cancelProgressHandler.Cancel)
                    {
                        return false;
                    }

                    cancelProgressHandler.Progress(string.Empty, previous, previous + TextStrings.progresscompleted);
                }
            }

            // Add to the Output Feature Set
            for (int a = 0; a < tempSet.Features.Count; a++)
            {
                output.Features.Add(tempSet.Features[a]);
                previous = Convert.ToInt32(Math.Round((a * 50D / tempSet.Features.Count) + 50D));
                if (cancelProgressHandler.Cancel)
                {
                    return false;
                }

                cancelProgressHandler.Progress(string.Empty, previous, previous + TextStrings.progresscompleted);
            }

            output.SaveAs(output.Filename, true);

            // add to map?
            return true;
        }

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet self = _inputParam[0].Value as IFeatureSet;
            if (self != null)
            {
                self.FillAttributes();
            }

            IFeatureSet other = _inputParam[1].Value as IFeatureSet;
            if (other != null)
            {
                other.FillAttributes();
            }

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(self, other, output, cancelProgressHandler);
        }

        /// <summary>
        /// Ping Yang Overwrite the function for Executes the Erase Opaeration tool for external testing
        /// </summary>
        public bool Execute(IFeatureSet self, IFeatureSet other, IFeatureSet output)
        {
            if (self == null || other == null || output == null)
            {
                return false;
            }

            output.CopyTableSchema(self); // Fill the 1st Featureset fields
            IFeatureSet tempSet = self.CombinedFields(other);

            // go through every feature in 1st featureSet
            foreach (IFeature t in self.Features)
            {
                // go through every feature in 2nd featureSet
                foreach (IFeature t1 in other.Features)
                {
                    t.Difference(t1, tempSet, FieldJoinType.All);
                }
            }

            // Add to the Output Feature Set
            for (int a = 0; a < tempSet.Features.Count; a++)
            {
                output.Features.Add(tempSet.Features[a]);
            }

            output.SaveAs(output.Filename, true);

            // add to map?
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new FeatureSetParam(TextStrings.BaseFeatureSet);
            _inputParam[1] = new FeatureSetParam(TextStrings.RemoveFeatureSet);
            _outputParam = new Parameter[1];
            _outputParam[0] = new FeatureSetParam(TextStrings.ErasedResultFeatureSet);
        }

        #endregion
    }
}