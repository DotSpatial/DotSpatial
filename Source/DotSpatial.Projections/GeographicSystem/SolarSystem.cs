using DotSpatial.Projections.GeographicCategories.SolarSystem;

namespace DotSpatial.Projections.GeographicSystem
{
    public class SolarSystem
    {
        private Earth _earth;
        private Jupiter _jupiter;
        private Mars _mars;
        private Mercury _mercury;
        private Neptune _neptune;
        private Pluto _pluto;
        private Saturn _saturn;
        private Uranus _uranus;
        private Venus _venus;

        public Earth Earth => _earth ?? (_earth = new Earth());
        public Jupiter Jupiter => _jupiter ?? (_jupiter = new Jupiter());
        public Mars Mars => _mars ?? (_mars = new Mars());
        public Mercury Mercury => _mercury ?? (_mercury = new Mercury());
        public Neptune Neptune => _neptune ?? (_neptune = new Neptune());
        public Pluto Pluto => _pluto ?? (_pluto = new Pluto());
        public Saturn Saturn => _saturn ?? (_saturn = new Saturn());
        public Uranus Uranus => _uranus ?? (_uranus = new Uranus());
        public Venus Venus => _venus ?? (_venus = new Venus());
    }
}
