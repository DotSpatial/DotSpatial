using DotSpatial.Projections.ProjectedCategories.StatePlane;

namespace DotSpatial.Projections.ProjectedSystem
{
    public class StatePlane
    {
        private NAD1927USFeet _nad1927UsFeet;
        private NAD1983CORS96IntlFeet _nad1983Cors96IntlFeet;
        private NAD1983CORS96Meters _nad1983Cors96Meters;
        private NAD1983CORS96USFeet _nad1983Cors96UsFeet;
        private NAD1983HARNIntlFeet _nad1983HarnIntlFeet;
        private NAD1983HARNMeters _nad1983HarnMeters;
        private NAD1983HARNUSFeet _nad1983HarnUsFeet;
        private NAD1983IntlFeet _nad1983IntlFeet;
        private NAD1983Meters _nad1983Meters;
        private NAD1983NSRS2007IntlFeet _nad1983Nsrs2007IntlFeet;
        private NAD1983NSRS2007Meters _nad1983Nsrs2007Meters;
        private NAD1983NSRS2007USFeet _nad1983Nsrs2007UsFeet;
        private NAD1983USFeet _nad1983UsFeet;
        private OtherGCS _otherGcs;

        public NAD1927USFeet Nad1927UsFeet => _nad1927UsFeet ?? (_nad1927UsFeet = new NAD1927USFeet());
        public NAD1983CORS96IntlFeet Nad1983Cors96IntlFeet => _nad1983Cors96IntlFeet ?? (_nad1983Cors96IntlFeet = new NAD1983CORS96IntlFeet());
        public NAD1983CORS96Meters Nad1983Cors96Meters => _nad1983Cors96Meters ?? (_nad1983Cors96Meters = new NAD1983CORS96Meters());
        public NAD1983CORS96USFeet Nad1983Cors96UsFeet => _nad1983Cors96UsFeet ?? (_nad1983Cors96UsFeet = new NAD1983CORS96USFeet());
        public NAD1983HARNIntlFeet Nad1983HarnIntlFeet => _nad1983HarnIntlFeet ?? (_nad1983HarnIntlFeet = new NAD1983HARNIntlFeet());
        public NAD1983HARNMeters Nad1983HarnMeters => _nad1983HarnMeters ?? (_nad1983HarnMeters = new NAD1983HARNMeters());
        public NAD1983HARNUSFeet Nad1983HarnUsFeet => _nad1983HarnUsFeet ?? (_nad1983HarnUsFeet = new NAD1983HARNUSFeet());
        public NAD1983IntlFeet Nad1983IntlFeet => _nad1983IntlFeet ?? (_nad1983IntlFeet = new NAD1983IntlFeet());
        public NAD1983Meters Nad1983Meters => _nad1983Meters ?? (_nad1983Meters = new NAD1983Meters());
        public NAD1983NSRS2007IntlFeet Nad1983Nsrs2007IntlFeet => _nad1983Nsrs2007IntlFeet ?? (_nad1983Nsrs2007IntlFeet = new NAD1983NSRS2007IntlFeet());
        public NAD1983NSRS2007Meters Nad1983Nsrs2007Meters => _nad1983Nsrs2007Meters ?? (_nad1983Nsrs2007Meters = new NAD1983NSRS2007Meters());
        public NAD1983NSRS2007USFeet Nad1983Nsrs2007UsFeet => _nad1983Nsrs2007UsFeet ?? (_nad1983Nsrs2007UsFeet = new NAD1983NSRS2007USFeet());
        public NAD1983USFeet Nad1983UsFeet => _nad1983UsFeet ?? (_nad1983UsFeet = new NAD1983USFeet());
        public OtherGCS OtherGcs => _otherGcs ?? (_otherGcs = new OtherGCS());
    }
}
