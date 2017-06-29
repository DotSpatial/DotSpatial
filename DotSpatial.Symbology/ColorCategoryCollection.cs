// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/11/2009 11:54:56 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using DotSpatial.Data;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ColorCategoryCollection
    /// </summary>
    [Serializable]
    public class ColorCategoryCollection : ChangeEventList<IColorCategory>
    {
        private IColorScheme _scheme;

        /// <summary>
        /// Default constructor for the PolygonCategoryCollection class.
        /// </summary>
        public ColorCategoryCollection()
        {
        }

        /// <summary>
        /// Initializes a new PolygonCategoryCollection instance with the supplied scheme.
        /// </summary>
        /// <param name="scheme">The scheme to use ofr this collection.</param>
        public ColorCategoryCollection(IColorScheme scheme)
            : this()
        {
            _scheme = scheme;
        }

        /// <summary>
        /// Gets or sets the parent scheme for this collection
        /// </summary>
        [Serialize("Scheme", ConstructorArgumentIndex = 0), ShallowCopy]
        public IColorScheme Scheme
        {
            get { return _scheme; }
            set
            {
                _scheme = value;
                UpdateItemParentPointers();
            }
        }

        /// <summary>
        /// Ensures that newly added categories can navigate to higher legend items.
        /// </summary>
        /// <param name="item">The newly added legend item.</param>
        protected override void OnInclude(IColorCategory item)
        {
            if (item != null)
            {
                if (_scheme != null) item.SetParentItem(_scheme.GetParentItem());
            }

            base.OnInclude(item);
        }

        /// <summary>
        /// Changes the parent item of the specified category
        /// </summary>
        /// <param name="item"></param>
        protected override void OnExclude(IColorCategory item)
        {
            if (item == null) return;
            item.SetParentItem(null);
            base.OnExclude(item);
        }

        /// <summary>
        /// Updates all of the categories so that they have a parent item that matches the
        /// schemes parent item.
        /// </summary>
        public void UpdateItemParentPointers()
        {
            foreach (var item in InnerList)
            {
                item.SetParentItem(_scheme == null ? null : _scheme.GetParentItem());
            }
        }
    }
}