// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace DotSpatial.Projections.Tests
{
    /// <summary>
    /// Used to get all the projection infos to test.
    /// </summary>
    public class ProjectionInfoDesc
    {
        /// <summary>
        /// Initializes a new ProjectionInfoDesc.
        /// </summary>
        /// <param name="name">Name of the category the projection info belongs to.</param>
        /// <param name="projectionInfo">The projection info to test.</param>
        public ProjectionInfoDesc(string name, ProjectionInfo projectionInfo)
        {
            Name = name;
            ProjectionInfo = projectionInfo;
        }

        /// <summary>
        /// Name of the category the projection info belong to.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The projection info to test.
        /// </summary>
        public ProjectionInfo ProjectionInfo { get; private set; }

        /// <summary>
        /// Gets all the ProjectionInfoDesc of the categry that gets tested.
        /// </summary>
        /// <param name="category">Category to get the ProjectionInfoDesc for.</param>
        /// <returns></returns>
        public static IEnumerable<ProjectionInfoDesc> GetForCoordinateSystemCategory(CoordinateSystemCategory category)
        {
            return category.Names.Select(d => new ProjectionInfoDesc(d, category.GetProjection(d)));
        }

        /// <summary>
        /// Returns the name of the category.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}