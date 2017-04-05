// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
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
using DotSpatial.Projections.ProjectedCategories.CountySystems.Minnesota;
using DotSpatial.Projections.ProjectedCategories.GaussKruger;
using DotSpatial.Projections.ProjectedCategories.NationalGrids;
using DotSpatial.Projections.ProjectedCategories.StatePlane;
using DotSpatial.Projections.ProjectedCategories.UTM;
using DotSpatial.Projections.Transforms;

using Africa = DotSpatial.Projections.ProjectedCategories.Continental.Africa;
using Asia = DotSpatial.Projections.ProjectedCategories.Continental.Asia;
using Europe = DotSpatial.Projections.ProjectedCategories.Continental.Europe;
using NewZealand = DotSpatial.Projections.ProjectedCategories.NationalGrids.NewZealand;
using NorthAmerica = DotSpatial.Projections.ProjectedCategories.Continental.NorthAmerica;
using SouthAmerica = DotSpatial.Projections.ProjectedCategories.Continental.SouthAmerica;

namespace DotSpatial.Projections
{
    public class ProjectedSystems : ICoordinateSystemCategoryHolder
    {
        #region Fields
        private string[] _names;

        private ARCEqualarcSecond _arcEqualarcSecond;
        private ProjectedSystem.Continental _continental;
        private ProjectedSystem.CountySystems _countySystems;
        private ProjectedSystem.GaussKruger _gaussKruger;
        private ProjectedSystem.NationalGrids _nationalGrids;
        private Polar _polar;
        private ProjectedSystem.StatePlane _statePlane;
        private StateSystems _stateSystems;
        private ProjectedSystem.Utm _utm;
        private World _world;
        private WorldSphereBased _worldSphereBased;
        #endregion

        #region Properties
        public ARCEqualarcSecond ArcEqualarcSecond => _arcEqualarcSecond ?? (_arcEqualarcSecond = new ARCEqualarcSecond());
        public ProjectedSystem.Continental Continental => _continental ?? (_continental = new ProjectedSystem.Continental());
        public ProjectedSystem.CountySystems CountySystems => _countySystems ?? (_countySystems = new ProjectedSystem.CountySystems());
        public ProjectedSystem.GaussKruger GaussKruger => _gaussKruger ?? (_gaussKruger = new ProjectedSystem.GaussKruger());
        public ProjectedSystem.NationalGrids NationalGrids => _nationalGrids ?? (_nationalGrids = new ProjectedSystem.NationalGrids());
        public Polar Polar => _polar ?? (_polar = new Polar());
        public ProjectedSystem.StatePlane StatePlane => _statePlane ?? (_statePlane = new ProjectedSystem.StatePlane());
        public StateSystems StateSystems => _stateSystems ?? (_stateSystems = new StateSystems());
        public ProjectedSystem.Utm Utm => _utm ?? (_utm = new ProjectedSystem.Utm());
        public World World => _world ?? (_world = new World());
        public WorldSphereBased WorldSphereBased => _worldSphereBased ?? (_worldSphereBased = new WorldSphereBased());
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