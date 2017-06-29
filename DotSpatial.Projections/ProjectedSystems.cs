// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 1:51:39 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************
#pragma warning disable 1591

using System.Linq;
using System.Reflection;
using DotSpatial.Projections.ProjectedCategories;

namespace DotSpatial.Projections
{
    /// <summary>
    /// Projected
    /// </summary>
    public class ProjectedSystems : ICoordinateSystemCategoryHolder
    {
        #region Fields

        private string[] _names;
        private Africa _africa;
        private Asia _asia;
        private Europe _europe;
        private GaussKrugerBeijing1954 _gausKrugerBeijing1954;
        private GaussKrugerOther _gausKrugerOther;
        private GaussKrugerPulkovo1942 _gaussKrugerPulkovo1942;
        private KrugerXian1980 _krugerXian1980;
        private Minnesota _minnesotaCounties;
        private Nad1983IntlFeet _nad1983IntlFeet;
        private NationalGrids _nationalGrids;
        private NationalGridsAustralia _nationalGridsAustralia;
        private NationalGridsCanada _nationalGridsCanada;
        private NationalGridsIndia _nationalGridsIndia;
        private NationalGridsJapan _nationalGridsJapan;
        private NationalGridsNewZealand _nationalGridsNewZealand;
        private NationalGridsNorway _nationalGridsNorway;
        private NationalGridsSweden _nationalGridsSweden;
        private NorthAmerica _northAmerica;
        private Polar _polar;
        private SouthAmerica _southAmerica;
        private SpheroidBased _spheroidBased;
        private StatePlaneNad1927 _statePlaneNad1927;
        private StatePlaneNad1983 _statePlaneNad1983;
        private StatePlaneNad1983Feet _statePlaneNad1983Feet;
        private StatePlaneNad1983Harn _statePlaneNad1983Harn;
        private StatePlaneNad1983HarnFeet _statePlaneNad1983HarnFeet;
        private StatePlaneOther _statePlaneOther;
        private StateSystems _stateSystems;
        private TransverseMercatorSystems _transverseMercator;
        private UtmNad1927 _utmNad1927;
        private UtmNad1983 _utmNad1983;
        private UtmOther _utmOther;
        private UtmWgs1972 _utmWgs1972;
        private UtmWgs1984 _utmWgs1984;
        private Wisconsin _wisconsin;
        private World _world;
        private WorldSpheroid _worldSpheroid;

        #endregion

        #region Constructors

        public Africa Africa
        {
            get { return _africa ?? (_africa = new Africa()); }
        }

        public Asia Asia
        {
            get { return _asia ?? (_asia = new Asia()); }
        }

        public Europe Europe
        {
            get { return _europe ?? (_europe = new Europe()); }
        }

        public GaussKrugerBeijing1954 GausKrugerBeijing1954
        {
            get { return _gausKrugerBeijing1954 ?? (_gausKrugerBeijing1954 = new GaussKrugerBeijing1954()); }
        }

        public GaussKrugerOther GausKrugerOther
        {
            get { return _gausKrugerOther ?? (_gausKrugerOther = new GaussKrugerOther()); }
        }

        public GaussKrugerPulkovo1942 GaussKrugerPulkovo1942
        {
            get { return _gaussKrugerPulkovo1942 ?? (_gaussKrugerPulkovo1942 = new GaussKrugerPulkovo1942()); }
        }

        public KrugerXian1980 KrugerXian1980
        {
            get { return _krugerXian1980 ?? (_krugerXian1980 = new KrugerXian1980()); }
        }

        public Minnesota MinnesotaCounties
        {
            get { return _minnesotaCounties ?? (_minnesotaCounties = new Minnesota()); }
        }

        public Nad1983IntlFeet Nad1983IntlFeet
        {
            get { return _nad1983IntlFeet ?? (_nad1983IntlFeet = new Nad1983IntlFeet()); }
        }

        public NationalGrids NationalGrids
        {
            get { return _nationalGrids ?? (_nationalGrids = new NationalGrids()); }
        }

        public NationalGridsAustralia NationalGridsAustralia
        {
            get { return _nationalGridsAustralia ?? (_nationalGridsAustralia = new NationalGridsAustralia()); }
        }

        public NationalGridsCanada NationalGridsCanada
        {
            get { return _nationalGridsCanada ?? (_nationalGridsCanada = new NationalGridsCanada()); }
        }

        public NationalGridsIndia NationalGridsIndia
        {
            get { return _nationalGridsIndia ?? (_nationalGridsIndia = new NationalGridsIndia()); }
        }

        public NationalGridsJapan NationalGridsJapan
        {
            get { return _nationalGridsJapan ?? (_nationalGridsJapan = new NationalGridsJapan()); }
        }

        public NationalGridsNewZealand NationalGridsNewZealand
        {
            get { return _nationalGridsNewZealand ?? (_nationalGridsNewZealand = new NationalGridsNewZealand()); }
        }

        public NationalGridsNorway NationalGridsNorway
        {
            get { return _nationalGridsNorway ?? (_nationalGridsNorway = new NationalGridsNorway()); }
        }

        public NationalGridsSweden NationalGridsSweden
        {
            get { return _nationalGridsSweden ?? (_nationalGridsSweden = new NationalGridsSweden()); }
        }

        public NorthAmerica NorthAmerica
        {
            get { return _northAmerica ?? (_northAmerica = new NorthAmerica()); }
        }

        public Polar Polar
        {
            get { return _polar ?? (_polar = new Polar()); }
        }

        public SouthAmerica SouthAmerica
        {
            get { return _southAmerica ?? (_southAmerica = new SouthAmerica()); }
        }

        public SpheroidBased SpheroidBased
        {
            get { return _spheroidBased ?? (_spheroidBased = new SpheroidBased()); }
        }

        public StatePlaneNad1927 StatePlaneNad1927
        {
            get { return _statePlaneNad1927 ?? (_statePlaneNad1927 = new StatePlaneNad1927()); }
        }

        public StatePlaneNad1983 StatePlaneNad1983
        {
            get { return _statePlaneNad1983 ?? (_statePlaneNad1983 = new StatePlaneNad1983()); }
        }

        public StatePlaneNad1983Feet StatePlaneNad1983Feet
        {
            get { return _statePlaneNad1983Feet ?? (_statePlaneNad1983Feet = new StatePlaneNad1983Feet()); }
        }

        public StatePlaneNad1983Harn StatePlaneNad1983Harn
        {
            get { return _statePlaneNad1983Harn ?? (_statePlaneNad1983Harn = new StatePlaneNad1983Harn()); }
        }

        public StatePlaneNad1983HarnFeet StatePlaneNad1983HarnFeet
        {
            get { return _statePlaneNad1983HarnFeet ?? (_statePlaneNad1983HarnFeet = new StatePlaneNad1983HarnFeet()); }
        }

        public StatePlaneOther StatePlaneOther
        {
            get { return _statePlaneOther ?? (_statePlaneOther = new StatePlaneOther()); }
        }

        public StateSystems StateSystems
        {
            get { return _stateSystems ?? (_stateSystems = new StateSystems()); }
        }

        public TransverseMercatorSystems TransverseMercator
        {
            get { return _transverseMercator ?? (_transverseMercator = new TransverseMercatorSystems()); }
        }

        public UtmNad1927 UtmNad1927
        {
            get { return _utmNad1927 ?? (_utmNad1927 = new UtmNad1927()); }
        }

        public UtmNad1983 UtmNad1983
        {
            get { return _utmNad1983 ?? (_utmNad1983 = new UtmNad1983()); }
        }

        public UtmOther UtmOther
        {
            get { return _utmOther ?? (_utmOther = new UtmOther()); }
        }

        public UtmWgs1972 UtmWgs1972
        {
            get { return _utmWgs1972 ?? (_utmWgs1972 = new UtmWgs1972()); }
        }

        public UtmWgs1984 UtmWgs1984
        {
            get { return _utmWgs1984 ?? (_utmWgs1984 = new UtmWgs1984()); }
        }

        public Wisconsin Wisconsin
        {
            get { return _wisconsin ?? (_wisconsin = new Wisconsin()); }
        }

        public World World
        {
            get { return _world ?? (_world = new World()); }
        }

        public WorldSpheroid WorldSpheroid
        {
            get { return _worldSpheroid ?? (_worldSpheroid = new WorldSpheroid()); }
        }

        #endregion

        private void AddNames()
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            _names = (from property in properties where property.Name != "Names" select property.Name).ToArray();
        }

        /// <summary>
        /// Given the string name, this will return the specified coordinate category
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CoordinateSystemCategory GetCategory(string name)
        {
            var property = GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            if (null == property)
                return null;
            return property.GetValue(this, null) as CoordinateSystemCategory;
        }

        /// <summary>
        /// Gets an array of all the names of the coordinate system categories
        /// in this collection of systems.
        /// </summary>
        public string[] Names
        {
            get
            {
                if (_names == null)
                {
                    AddNames();
                }
                return _names;
            }
        }
    }
}

#pragma warning restore 1591