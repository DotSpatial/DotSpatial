
using DotSpatial.Projections.ProjectedCategories.NationalGrids;

namespace DotSpatial.Projections.ProjectedSystem
{
    public class NationalGrids
    {
        private Africa _africa;
        private Argentina _argentina;
        private Asia _asia;
        private Australia _australia;
        private Austria _austria;
        private Canada _canada;
        private Europe _europe;
        private France _france;
        private Germany _germany;
        private Indiansubcontinent _indiansubcontinent;
        private Indonesia _indonesia;
        private Japan _japan;
        private Libya _libya;
        private MalaysiaSingapore _malaysiaSingapore;
        private NewZealand _newZealand;
        private NorthAmerica _northAmerica;
        private Norway _norway;
        private Oceans _oceans;
        private SouthAfrica _southAfrica;
        private SouthAmerica _southAmerica;
        private Sweden _sweden;

        public Africa Africa => _africa ?? (_africa = new Africa());
        public Argentina Argentina => _argentina ?? (_argentina = new Argentina());
        public Asia Asia => _asia ?? (_asia = new Asia());
        public Australia Australia => _australia ?? (_australia = new Australia());
        public Austria Austria => _austria ?? (_austria = new Austria());
        public Canada Canada => _canada ?? (_canada = new Canada());
        public Europe Europe => _europe ?? (_europe = new Europe());
        public France France => _france ?? (_france = new France());
        public Germany Germany => _germany ?? (_germany = new Germany());
        public Indiansubcontinent Indiansubcontinent => _indiansubcontinent ?? (_indiansubcontinent = new Indiansubcontinent());
        public Indonesia Indonesia => _indonesia ?? (_indonesia = new Indonesia());
        public Japan Japan => _japan ?? (_japan = new Japan());
        public Libya Libya => _libya ?? (_libya = new Libya());
        public MalaysiaSingapore MalaysiaSingapore => _malaysiaSingapore ?? (_malaysiaSingapore = new MalaysiaSingapore());
        public NewZealand NewZealand => _newZealand ?? (_newZealand = new NewZealand());
        public NorthAmerica NorthAmerica => _northAmerica ?? (_northAmerica = new NorthAmerica());
        public Norway Norway => _norway ?? (_norway = new Norway());
        public Oceans Oceans => _oceans ?? (_oceans = new Oceans());
        public SouthAfrica SouthAfrica => _southAfrica ?? (_southAfrica = new SouthAfrica());
        public SouthAmerica SouthAmerica => _southAmerica ?? (_southAmerica = new SouthAmerica());
        public Sweden Sweden => _sweden ?? (_sweden = new Sweden());
    }
}
