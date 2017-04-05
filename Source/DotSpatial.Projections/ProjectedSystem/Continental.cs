using DotSpatial.Projections.ProjectedCategories.Continental;

namespace DotSpatial.Projections.ProjectedSystem
{
    public class Continental
    {
        private Africa _africa;
        private Asia _asia;
        private Europe _europe;
        private NorthAmerica _northAmerica;
        private SouthAmerica _southAmerica;

        public Africa Africa => _africa ?? (_africa = new Africa());
        public Asia Asia => _asia ?? (_asia = new Asia());
        public Europe Europe => _europe ?? (_europe = new Europe());
        public NorthAmerica NorthAmerica => _northAmerica ?? (_northAmerica = new NorthAmerica());
        public SouthAmerica SouthAmerica => _southAmerica ?? (_southAmerica = new SouthAmerica());
    }
}
