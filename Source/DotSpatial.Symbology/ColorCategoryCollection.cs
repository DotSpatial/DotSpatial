// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
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
        #region Fields

        private IColorScheme _scheme;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorCategoryCollection"/> class.
        /// </summary>
        public ColorCategoryCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorCategoryCollection"/> class.
        /// </summary>
        /// <param name="scheme">The scheme to use ofr this collection.</param>
        public ColorCategoryCollection(IColorScheme scheme)
            : this()
        {
            _scheme = scheme;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the parent scheme for this collection
        /// </summary>
        [Serialize("Scheme", ConstructorArgumentIndex = 0)]
        [ShallowCopy]
        public IColorScheme Scheme
        {
            get
            {
                return _scheme;
            }

            set
            {
                _scheme = value;
                UpdateItemParentPointers();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates all of the categories so that they have a parent item that matches the
        /// schemes parent item.
        /// </summary>
        public void UpdateItemParentPointers()
        {
            foreach (var item in InnerList)
            {
                item.SetParentItem(_scheme?.GetParentItem());
            }
        }

        /// <summary>
        /// Changes the parent item of the specified category.
        /// </summary>
        /// <param name="item">The item to exclude.</param>
        protected override void OnExclude(IColorCategory item)
        {
            if (item == null) return;

            item.SetParentItem(null);
            base.OnExclude(item);
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

        #endregion
    }
}