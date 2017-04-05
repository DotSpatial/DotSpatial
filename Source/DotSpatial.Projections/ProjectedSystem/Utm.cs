using DotSpatial.Projections.ProjectedCategories.UTM;

namespace DotSpatial.Projections.ProjectedSystem
{
    public class Utm
    {
        private Africa _africa;
        private Asia _asia;
        private Europe _europe;
        private Indonesia _indonesia;
        private Malaysia _malaysia;
        private NAD1927 _nad1927;
        private NAD1983 _nad1983;
        private NewZealand _newZealand;
        private NorthAmerica _northAmerica;
        private Oceans _oceans;
        private SouthAmerica _southAmerica;
        private Wgs1972 _wgs1972;
        private Wgs1984 _wgs1984;

        public Africa Africa => _africa ?? (_africa = new Africa());
        public Asia Asia => _asia ?? (_asia = new Asia());
        public Europe Europe => _europe ?? (_europe = new Europe());
        public Indonesia Indonesia => _indonesia ?? (_indonesia = new Indonesia());
        public Malaysia Malaysia => _malaysia ?? (_malaysia = new Malaysia());
        public NAD1927 Nad1927 => _nad1927 ?? (_nad1927 = new NAD1927());
        public NAD1983 Nad1983 => _nad1983 ?? (_nad1983 = new NAD1983());
        public NewZealand NewZealand => _newZealand ?? (_newZealand = new NewZealand());
        public NorthAmerica NorthAmerica => _northAmerica ?? (_northAmerica = new NorthAmerica());
        public Oceans Oceans => _oceans ?? (_oceans = new Oceans());
        public SouthAmerica SouthAmerica => _southAmerica ?? (_southAmerica = new SouthAmerica());
        public Wgs1972 Wgs1972 => _wgs1972 ?? (_wgs1972 = new Wgs1972());
        public Wgs1984 Wgs1984 => _wgs1984 ?? (_wgs1984 = new Wgs1984());
    }
}
