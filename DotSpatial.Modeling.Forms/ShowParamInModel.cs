// ********************************************************************************************************
// Product Name: DotSpatial.Modeling.Forms.dll
// Description:  Supports the Windows Forms UI for the modeling components.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Initial Developer of this Original Code is Ted Dunsford. Created during refactoring 2010.
// ********************************************************************************************************
namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// Defines whether or not to add the parameter to the model
    /// </summary>
    public enum ShowParamInModel
    {
        /// <summary>
        /// Always add the parameter to the model
        /// </summary>
        Always,
        /// <summary>
        /// Show the parameter in the model
        /// </summary>
        Yes,
        /// <summary>
        /// Don't show the parameter in the model
        /// </summary>
        No
    }
}