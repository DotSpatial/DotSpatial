using DotSpatial.Projections.ProjectedCategories.CountySystems.Minnesota;

namespace DotSpatial.Projections.ProjectedSystem
{
    public class Minnesota
    {
        private Meters _meters;
        private USFeet _usFeet;

        public Meters Meters => _meters ?? (_meters = new Meters());
        public USFeet UsFeet => _usFeet ?? (_usFeet = new USFeet());
    }
}
