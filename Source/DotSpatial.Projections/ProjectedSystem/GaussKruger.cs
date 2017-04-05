using DotSpatial.Projections.ProjectedCategories.GaussKruger;

namespace DotSpatial.Projections.ProjectedSystem
{
    public class GaussKruger
    {
        private Beijing1954 _africa;
        private Asia _asia;
        private Europe _europe;
        private Pulkovo1942 _pulkovo1942;
        private Pulkovo1995 _pulkovo1995;
        private Xian1980 _xian1980;

        public Beijing1954 Beijing1954 => _africa ?? (_africa = new Beijing1954());
        public Asia Asia => _asia ?? (_asia = new Asia());
        public Europe Europe => _europe ?? (_europe = new Europe());
        public Pulkovo1942 Pulkovo1942 => _pulkovo1942 ?? (_pulkovo1942 = new Pulkovo1942());
        public Pulkovo1995 Pulkovo1995 => _pulkovo1995 ?? (_pulkovo1995 = new Pulkovo1995());
        public Xian1980 Xian1980 => _xian1980 ?? (_xian1980 = new Xian1980());
    }
}
