// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// This contains a list of types that were moved from one assembly to another when DotSpatial.Topology was replaced by NetTopologySuite/GeoAPI.
    /// This is used to make sure that the old dspx files that contain types from DotSpatial.Topology can be loaded using NetTopologySuite.
    /// </summary>
    public class MovedTypes
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MovedTypes"/> class.
        /// </summary>
        public MovedTypes()
        {
            Types = new List<TypeMoveDefintion>
                    {
                        new TypeMoveDefintion("DotSpatial.Topology", "GeoAPI.Geometries", "DotSpatial.Topology", "GeoAPI"),
                        new TypeMoveDefintion("DotSpatial.Topology", "NetTopologySuite.Geometries", "DotSpatial.Topology", "NetTopologySuite"),
                        new TypeMoveDefintion("DotSpatial.Topology", "DotSpatial.NTSExtension", "DotSpatial.Topology", "DotSpatial.NTSExtension")
                    };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a list of all types that were moved when DotSpatial.Topology was replaced by NetTopologySuite/GeoAPI.
        /// </summary>
        public List<TypeMoveDefintion> Types { get; }

        #endregion
    }
}