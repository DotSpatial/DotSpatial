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
// The Original Code is from DotSpatial.Symbology.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/29/2010 8:43 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Feature Set Apply Edit Args.
    /// </summary>
    [Obsolete("Do not use it. This class is not used in DotSpatial anymore.")] // Marked in 1.7
    public class FeatureSetApplyEditArgs : EventArgs
    {
        #region Delegates

        /// <summary>
        /// Defines the structure of the method that would handle the actual
        /// changes for an apply changes request.
        /// </summary>
        /// <param name="editCopy">The IFeatureSet edit copy being manipulated.</param>
        public delegate void ChangeHandler(IFeatureSet editCopy);

        #endregion

        private readonly ChangeHandler _myChangeHandler;
        private IFeatureSet _editCopy;

        /// <summary>
        /// Initializes a new instance of the FeatureSetApplyEditArgs class.
        /// </summary>
        /// <param name="editCopy">The IFeatureSet edit copy.</param>
        /// <param name="handler">The delegate should point to a method handler to work with the edit copy.</param>
        public FeatureSetApplyEditArgs(IFeatureSet editCopy, ChangeHandler handler)
        {
            _myChangeHandler = handler;
        }

        /// <summary>
        /// Gets or sets a duplicate of the IFeatureSet being referenced so that
        /// changes might be applied.
        /// </summary>
        public IFeatureSet EditCopy
        {
            get { return _editCopy; }
            set { _editCopy = value; }
        }

        /// <summary>
        /// Invokes the method that will handle changes using the EditCopy.
        /// </summary>
        public void ApplyChanges()
        {
            if (_myChangeHandler != null) _myChangeHandler(_editCopy);
        }
    }
}