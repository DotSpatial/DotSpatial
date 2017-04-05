using DotSpatial.Projections.GeographicCategories.CountySystems;

namespace DotSpatial.Projections.GeographicSystem
{
    public class CountySystems
    {
        private Minnesota _minnesota;

        public Minnesota Minnesota => _minnesota ?? (_minnesota = new Minnesota());
    }
}
