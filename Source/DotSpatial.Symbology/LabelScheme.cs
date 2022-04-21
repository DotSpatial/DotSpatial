// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A scheme for labels.
    /// </summary>
    public class LabelScheme : ILabelScheme
    {
        #region Fields

        [Serialize("Categories")]
        private IList<ILabelCategory> _categories;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelScheme"/> class.
        /// </summary>
        public LabelScheme()
        {
            _categories = new BaseList<ILabelCategory>();
            AddCategory();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Categories.
        /// </summary>
        public IList<ILabelCategory> Categories
        {
            get
            {
                return _categories;
            }

            set
            {
                _categories = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a category to the scheme, and also names the category with an integer that has not yet been used.
        /// </summary>
        /// <returns>A new category with a name that has not yet been used.</returns>
        public ILabelCategory AddCategory()
        {
            string name = "Category 0";
            bool unused = false;
            int i = 0;
            while (!unused)
            {
                unused = true;
                foreach (ILabelCategory cat in Categories)
                {
                    if (cat.Name == name)
                    {
                        unused = false;
                        i++;
                        name = "Category " + i;
                        break;
                    }
                }
            }

            ILabelCategory catnew = new LabelCategory();
            catnew.Name = name;
            Categories.Add(catnew);
            return catnew;
        }

        /// <summary>
        /// Returns the Copy, but as an object.
        /// </summary>
        /// <returns>A copy of this.</returns>
        public object Clone()
        {
            return Copy();
        }

        /// <summary>
        /// The individual categories are copied, meaning that the text symbolizers
        /// will be new, and disconnected from the original text symbolizers of this
        /// scheme. MemberwiseClone is used so that any subclass members appended
        /// to this will be shallow copies unless this method is overridden.
        /// </summary>
        /// <returns>A Duplicate LabelScheme, but with new, duplicated categories and symbolizers.</returns>
        public virtual LabelScheme Copy()
        {
            LabelScheme result = MemberwiseClone() as LabelScheme;
            if (result == null) return null;

            result.Categories = new BaseList<ILabelCategory>();
            foreach (ILabelCategory cat in Categories)
            {
                result.Categories.Add(cat.Copy());
            }

            return result;
        }

        /// <summary>
        /// Attempts to reduce the integer index representing this categories rank in the
        /// list. By doing this, it will be drawn sooner, and therefore subsequent
        /// layers will be drawn on top of this layer, and so it reduces the categories
        /// priority. If this collection does not contain the category or it is already
        /// at index 0, this will return false.
        /// </summary>
        /// <param name="category">The ILabelCategory to demote.</param>
        /// <returns>Boolean, true if the demotion was successful.</returns>
        public bool Demote(ILabelCategory category)
        {
            if (category == null || Categories == null || Categories.Count == 0 || !Categories.Contains(category)) return false;

            int index = Categories.IndexOf(category);
            if (index == 0) return false;

            index -= 1;
            Categories.Remove(category);
            Categories.Insert(index, category);
            return true;
        }

        /// <summary>
        /// This attempts to increase the numeric index, which will cause it to be drawn later,
        /// or higher up on the cue, which means it will be drawn AFTER the previous layers,
        /// and therefore is a higher priority. If the category does not exist in the collection
        /// or the category is already at the highest value, this returns false.
        /// </summary>
        /// <param name="category">The category to promote if possible.</param>
        /// <returns>Boolean, true if the promotion was successful.</returns>
        public bool Promote(ILabelCategory category)
        {
            if (category == null || Categories == null || Categories.Count == 0 || !Categories.Contains(category)) return false;

            int index = Categories.IndexOf(category);
            if (index == Categories.Count - 1) return false;

            index += 1;
            Categories.Remove(category);
            Categories.Insert(index, category);
            return true;
        }

        /// <summary>
        /// Gets a copy of this label scheme.
        /// </summary>
        /// <returns>The copy of this.</returns>
        ILabelScheme ILabelScheme.Copy()
        {
            return Copy();
        }

        #endregion
    }
}