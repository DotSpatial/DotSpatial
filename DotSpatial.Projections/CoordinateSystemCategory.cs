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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:43:46 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace DotSpatial.Projections
{
    /// <summary>
    /// CoordinateSystem
    /// </summary>
    public class CoordinateSystemCategory
    {
        #region Private Variables

        private readonly string[] _names;

        #endregion

        #region Constructors
        
        /// <summary>
        /// Creates a new instance of CoordinateSystem
        /// </summary>
        public CoordinateSystemCategory()
        {
            var fields = GetType().GetFields();
            _names = new string[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                _names[i] = fields[i].Name;
            } 
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves the specified projection given the specified name.
        /// </summary>
        /// <param name="name">The string name of the projection to obtain information for</param>
        /// <returns></returns>
        public ProjectionInfo GetProjection(string name)
        {
            foreach (var info in GetType().GetFields())
            {
                if (info.Name == name)
                {
                    return (ProjectionInfo)info.GetValue(this);
                }
            }
            return null;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of names of all the members on this object
        /// </summary>
        public string[] Names
        {
            get { return _names; }
        }

        /// <summary>
        /// Obtains all the members of this category, building a single
        /// array of the projection info classes.  This returns the
        /// original classes, not a copy.
        /// </summary>
        /// <returns>The array of projection info classes</returns>
        public ProjectionInfo[] ToArray()
        {
            return AsEnumerable().ToArray();
        }

        /// <summary>
        /// Obtains all the members of this category. This returns the
        /// original classes, not a copy.
        /// </summary>
        /// <returns>The enumerable of projection info classes with field names</returns>
        public IEnumerable<ProjectionInfo> AsEnumerable()
        {
            return GetType().GetFields()
                .Select(field => (ProjectionInfo) field.GetValue(this));
        }

        #endregion
    }
}