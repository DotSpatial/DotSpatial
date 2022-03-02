// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;
using NetTopologySuite.Geometries;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Finds the intersections between two polygon featuresets.
    /// </summary>
    public class InterSectionTool : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;
        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InterSectionTool"/> class.
        /// </summary>
        public InterSectionTool()
        {
            Name = "IntersectionTool";
            Category = "Spatial Analysis";
            Description = "Using this tool you can find the intersection between different FeatureClasses.";
            ToolTip = "A tool used for finding the intersection between different FeatureClasses";
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the input paramater array.
        /// </summary>
        public override Parameter[] InputParameters
        {
            get
            {
                return _inputParam;
            }
        }

        /// <summary>
        /// Gets the output paramater array.
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
        /// Once the parameters have been configured the Execute command can be called, it returns true if succesful.
        /// </summary>
        /// <param name="cancelProgressHandler">The progresshandler used for showing the proress.</param>
        /// <returns>False on error, otherwise true if run successfully.</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (!(_inputParam[0].Value is IFeatureSet inputFs1) || !(_inputParam[1].Value is IFeatureSet inputFs2) || !(_outputParam[0].Value is IFeatureSet output) || inputFs1.FeatureType != FeatureType.Polygon || output.FeatureType != FeatureType.Polygon || inputFs2.FeatureType != FeatureType.Polygon)
            {
                return false;
            }

            output.CopyTableSchema(inputFs1.DataTable);
            output.Projection = inputFs1.Projection;

            if (!output.DataTable.Columns.Contains("InterArea"))
            {
                output.DataTable.Columns.Add("InterArea");
            }

            int maxFeature = inputFs1.Features.Count;

            for (int i = 0; i < inputFs1.Features.Count; i++)
            {
                if (cancelProgressHandler.Cancel)
                {
                    return false;
                }

                int current = Convert.ToInt32(Math.Round((i + 1) * 100d / maxFeature));

                IFeature polygonFeat1 = inputFs1.Features[i];
                Extent tolerant = polygonFeat1.Geometry.EnvelopeInternal.ToExtent();
                List<IFeature> intersectingFeatures = inputFs2.Select(tolerant);

                if (intersectingFeatures?.Count > 0)
                {
                    foreach (IFeature polygonFeat2 in intersectingFeatures)
                    {
                        if (polygonFeat2.Geometry.Intersects(polygonFeat1.Geometry))
                        {
                            try
                            {
                                Geometry intersectedF = polygonFeat2.Geometry.Intersection(polygonFeat1.Geometry);
                                var f = output.AddFeature(intersectedF);
                                f.DataRow["InterArea"] = intersectedF.Area;
                                f.CopyAttributes(polygonFeat1);
                            }
                            catch (Exception e)
                            {
                                cancelProgressHandler.Progress(current, e.Message);
                                return false;
                            }
                        }
                    }
                }

                if (current % 5 == 0)
                {
                    cancelProgressHandler.Progress(current, current + TextStrings.progresscompleted);
                }
            }

            output.Save();
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here.
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[]
            {
                new PolygonFeatureSetParam("FirstFeatureSet")
                {
                    HelpText = "The first polygon feature class used for finding intersecting features."
                },
                new PolygonFeatureSetParam("SecondFeatureSet")
                {
                    HelpText = "The secound polygon feature class used for finding intersecting features."
                }
            };

            _outputParam = new Parameter[]
            {
                 new PolygonFeatureSetParam("Output") { HelpText = "Contains the intersecting features." }
            };
        }

        #endregion
    }
}
