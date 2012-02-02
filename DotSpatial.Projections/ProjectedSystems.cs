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

using System.Reflection;
using DotSpatial.Projections.ProjectedCategories;

namespace DotSpatial.Projections
{
    /// <summary>
    /// Projected
    /// </summary>
    public class ProjectedSystems
    {
        private string[] _names;

        #region Fields

        public readonly Africa Africa;
        public readonly Asia Asia;
        public readonly Europe Europe;
        public readonly GaussKrugerBeijing1954 GausKrugerBeijing1954;
        public readonly GaussKrugerOther GausKrugerOther;
        public readonly GaussKrugerPulkovo1942 GaussKrugerPulkovo1942;
        public readonly KrugerXian1980 KrugerXian1980;
        public readonly Minnesota MinnesotaCounties;
        public readonly Nad1983IntlFeet Nad1983IntlFeet;
        public readonly NationalGrids NationalGrids;
        public readonly NationalGridsAustralia NationalGridsAustralia;
        public readonly NationalGridsCanada NationalGridsCanada;
        public readonly NationalGridsIndia NationalGridsIndia;
        public readonly NationalGridsJapan NationalGridsJapan;
        public readonly NationalGridsNewZealand NationalGridsNewZealand;
        public readonly NationalGridsNorway NationalGridsNorway;
        public readonly NationalGridsSweden NationalGridsSweden;
        public readonly NorthAmerica NorthAmerica;
        public readonly Polar Polar;
        public readonly SouthAmerica SouthAmerica;
        public readonly SpheroidBased SpheroidBased;
        public readonly StatePlaneNad1927 StatePlaneNad1927;
        public readonly StatePlaneNad1983 StatePlaneNad1983;
        public readonly StatePlaneNad1983Feet StatePlaneNad1983Feet;
        public readonly StatePlaneNad1983Harn StatePlaneNad1983Harn;
        public readonly StatePlaneNad1983HarnFeet StatePlaneNad1983HarnFeet;
        public readonly StatePlaneOther StatePlaneOther;
        public readonly StateSystems StateSystems;
        public readonly TransverseMercatorSystems TransverseMercator;
        public readonly UtmNad1927 UtmNad1927;
        public readonly UtmNad1983 UtmNad1983;
        public readonly UtmOther UtmOther;
        public readonly UtmWgs1972 UtmWgs1972;
        public readonly UtmWgs1984 UtmWgs1984;
        public readonly Wisconsin Wisconsin;
        public readonly World World;
        public readonly WorldSpheroid WorldSpheroid;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Projected
        /// </summary>
        public ProjectedSystems()
        {
            Africa = new Africa();
            Asia = new Asia();
            Europe = new Europe();
            GausKrugerBeijing1954 = new GaussKrugerBeijing1954();
            GausKrugerOther = new GaussKrugerOther();
            GaussKrugerPulkovo1942 = new GaussKrugerPulkovo1942();
            KrugerXian1980 = new KrugerXian1980();
            MinnesotaCounties = new Minnesota();
            Nad1983IntlFeet = new Nad1983IntlFeet();
            NationalGrids = new NationalGrids();
            NationalGridsAustralia = new NationalGridsAustralia();
            NationalGridsCanada = new NationalGridsCanada();
            NationalGridsIndia = new NationalGridsIndia();
            NationalGridsJapan = new NationalGridsJapan();
            NationalGridsNewZealand = new NationalGridsNewZealand();
            NationalGridsNorway = new NationalGridsNorway();
            NationalGridsSweden = new NationalGridsSweden();
            NorthAmerica = new NorthAmerica();
            Polar = new Polar();
            SouthAmerica = new SouthAmerica();
            SpheroidBased = new SpheroidBased();
            StatePlaneNad1927 = new StatePlaneNad1927();
            StatePlaneNad1983 = new StatePlaneNad1983();
            StatePlaneNad1983Feet = new StatePlaneNad1983Feet();
            StatePlaneNad1983Harn = new StatePlaneNad1983Harn();
            StatePlaneNad1983HarnFeet = new StatePlaneNad1983HarnFeet();
            StatePlaneOther = new StatePlaneOther();
            StateSystems = new StateSystems();
            TransverseMercator = new TransverseMercatorSystems();
            UtmNad1927 = new UtmNad1927();
            UtmNad1983 = new UtmNad1983();
            UtmOther = new UtmOther();
            UtmWgs1972 = new UtmWgs1972();
            UtmWgs1984 = new UtmWgs1984();
            Wisconsin = new Wisconsin();
            World = new World();
            WorldSpheroid = new WorldSpheroid();
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
            FieldInfo[] flds = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            _names = new string[flds.Length];
            for (int i = 0; i < flds.Length; i++)
            {
                _names[i] = flds[i].Name;
            }
        }

        /// <summary>
        /// Given the string name, this will return the specified coordinate category
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CoordinateSystemCategory GetCategory(string name)
        {
            FieldInfo[] flds = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < flds.Length; i++)
            {
                if (flds[i].Name == name)
                {
                    return flds[i].GetValue(this) as CoordinateSystemCategory;
                }
            }
            return null;
        }

        #endregion
    }
}

#pragma warning restore 1591