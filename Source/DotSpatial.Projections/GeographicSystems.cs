// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:36:37 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

using System.Linq;
using System.Reflection;
using DotSpatial.Projections.GeographicCategories;
using DotSpatial.Projections.GeographicSystem;

namespace DotSpatial.Projections
{
    public class GeographicSystems : ICoordinateSystemCategoryHolder
    {
        #region Private Variables

        private Africa _africa;
        private Antarctica _antarctica;
        private Asia _asia;
        private AustraliaNewZealand _australia;
        private Caribbean _caribbean;
        private CountySystems _countySystems;
        private Europe _europe;
        private IndianOcean _indianOcean;
        private NorthAmerica _northAmerica;
        private PacificOcean _oceans;
        private SouthAmerica _southAmerica;
        private SpheroidBased _spheroidBased;
        private World _world;
        private string[] _names;

        private SolarSystem _solarSystem;

        #endregion

        #region Properties

        public Africa Africa => _africa ?? (_africa = new Africa());
        public Antarctica Antarctica => _antarctica ?? (_antarctica = new Antarctica());
        public Asia Asia => _asia ?? (_asia = new Asia());
        public AustraliaNewZealand AustraliaNewZealand => _australia ?? (_australia = new AustraliaNewZealand());
        public Caribbean Caribbean => _caribbean ?? (_caribbean = new Caribbean());
        public CountySystems CountySystems => _countySystems ?? (_countySystems = new CountySystems());
        public Europe Europe => _europe ?? (_europe = new Europe());
        public IndianOcean IndianOcean => _indianOcean ?? (_indianOcean = new IndianOcean());
        public NorthAmerica NorthAmerica => _northAmerica ?? (_northAmerica = new NorthAmerica());
        public PacificOcean PacificOcean => _oceans ?? (_oceans = new PacificOcean());
        public SolarSystem SolarSystem => _solarSystem ?? (_solarSystem = new SolarSystem());
        public SouthAmerica SouthAmerica => _southAmerica ?? (_southAmerica = new SouthAmerica());
        public SpheroidBased SpheroidBased => _spheroidBased ?? (_spheroidBased = new SpheroidBased());
        public World World => _world ?? (_world = new World());

        #endregion

        /// <summary>
        /// Given the string name, this will return the specified coordinate category.
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

        private void AddNames()
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            _names = (from property in properties where property.Name != "Names" select property.Name).ToArray();
        }

    }
}

#pragma warning restore 1591