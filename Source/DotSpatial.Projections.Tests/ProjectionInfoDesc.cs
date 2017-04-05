using System.Collections.Generic;
using System.Linq;

namespace DotSpatial.Projections.Tests
{
    public class ProjectionInfoDesc
    {
        public ProjectionInfoDesc(string name, ProjectionInfo projectionInfo)
        {
            Name = name;
            ProjectionInfo = projectionInfo;
        }

        public string Name { get; }
        public ProjectionInfo ProjectionInfo { get; private set; }

        public static IEnumerable<ProjectionInfoDesc> GetForCoordinateSystemCategory(CoordinateSystemCategory category)
        {
            return category.Names.Select(d => new ProjectionInfoDesc(d, category.GetProjection(d)));   
        }

        public override string ToString()
        {
            return Name;
        }
    }
}