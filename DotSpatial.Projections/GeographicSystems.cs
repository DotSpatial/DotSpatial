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

namespace DotSpatial.Projections
{
    /// <summary>
    /// GeographicSystems
    /// </summary>
    public class GeographicSystems : ICoordinateSystemCategoryHolder
    {
        #region Private Variables

        private Africa _africa;
        private Antarctica _antarctica;
        private Asia _asia;
        private Australia _australia;
        private CountySystems _countySystems;
        private Europe _europe;
        private NorthAmerica _northAmerica;
        private Oceans _oceans;
        private SolarSystem _solarSystem;
        private SouthAmerica _southAmerica;
        private SpheroidBased _spheroidBased;
        private World _world;
        private string[] _names;

        #endregion

        #region Constructors

        public Africa Africa
        {
            get { return _africa ?? (_africa = new Africa()); }
        }
        public Antarctica Antarctica
        {
            get { return _antarctica ?? (_antarctica = new Antarctica()); }
        }

        public Asia Asia
        {
            get { return _asia ?? (_asia = new Asia()); }
        }

        public Australia Australia
        {
            get { return _australia ?? (_australia = new Australia()); }
        }

        public CountySystems CountySystems
        {
            get { return _countySystems ?? (_countySystems = new CountySystems()); }
        }

        public Europe Europe
        {
            get { return _europe ?? (_europe = new Europe()); }
        }

        public NorthAmerica NorthAmerica
        {
            get { return _northAmerica ?? (_northAmerica = new NorthAmerica()); }
        }

        public Oceans Oceans
        {
            get { return _oceans ?? (_oceans = new Oceans()); }
        }

        public SolarSystem SolarSystem
        {
            get { return _solarSystem ?? (_solarSystem = new SolarSystem()); }
        }

        public SouthAmerica SouthAmerica
        {
            get { return _southAmerica ?? (_southAmerica = new SouthAmerica()); }
        }

        public SpheroidBased SpheroidBased
        {
            get { return _spheroidBased ?? (_spheroidBased = new SpheroidBased()); }
        }

        public World World
        {
            get { return _world ?? (_world = new World()); }
        }

        private void AddNames()
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            _names = (from property in properties where property.Name != "Names" select property.Name).ToArray();
        }


        #endregion

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