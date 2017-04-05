
namespace DotSpatial.Projections.ProjectedSystem
{
    public class CountySystems
    {
        private Minnesota _minnesota;
        private WisconsinCrs _wisconsinCrs;

        public Minnesota Minnesota => _minnesota ?? (_minnesota = new Minnesota());
        public WisconsinCrs WisconsinCrs => _wisconsinCrs ?? (_wisconsinCrs = new WisconsinCrs());
    }
}
