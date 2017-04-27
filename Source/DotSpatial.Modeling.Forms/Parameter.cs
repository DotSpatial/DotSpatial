// ********************************************************************************************************
// Product Name: DotSpatial.Tools.Parameters
// Description:  Parameters passed back from a ITool to the toolbox manager
//
// ********************************************************************************************************
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using DotSpatial.Modeling.Forms.Elements;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// EventHandler delegate for the tool parameter when a value is changed.
    /// </summary>
    /// <param name="sender">Sender that raised the event.</param>
    public delegate void EventHandlerValueChanged(Parameter sender);

    /// <summary>
    /// This is the base class for the parameter array to be passed into a ITool
    /// </summary>
    public abstract class Parameter : ICloneable
    {
        #region Fields

        private object _dataValue;

        #endregion

        #region Events

        /// <summary>
        /// Fires when the parameter's value is changed
        /// </summary>
        internal event EventHandlerValueChanged ValueChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the parameter contains a value.
        /// </summary>
        public bool DefaultSpecified { get; set; }

        /// <summary>
        /// Gets or sets the help image that will appear beside the parameter element when it is clicked
        /// </summary>
        public Bitmap HelpImage { get; set; }

        /// <summary>
        /// Gets or sets the help text that will appear beside the parameter element when it is clicked
        /// </summary>
        public string HelpText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the model name. This is used to identify this parameter in the modeler, it will be set automatically by the modeler.
        /// </summary>
        public string ModelName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of that parameter that shows up in auto generated forms.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets the type of data used in this parameter.
        /// </summary>
        public string ParamType { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether a graphic should be added to the model to represent the parameter.
        /// </summary>
        public ShowParamInModel ParamVisible { get; set; }

        /// <summary>
        /// Gets or sets the value of the parameter.
        /// </summary>
        public object Value
        {
            get
            {
                return _dataValue;
            }

            set
            {
                _dataValue = value;
                DefaultSpecified = _dataValue != null;
                OnValueChanged();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This returns a duplicate of this object.
        /// </summary>
        /// <returns>The copy.</returns>
        public object Clone()
        {
            return Copy();
        }

        /// <summary>
        /// Returns a shallow copy of the Parameter class
        /// </summary>
        /// <returns>A new Parameters class that is a shallow copy of the original parameters class</returns>
        public Parameter Copy()
        {
            return MemberwiseClone() as Parameter;
        }

        /// <summary>
        /// Populates the parameter with default values.
        /// </summary>
        /// <param name="path">The path.</param>
        public virtual void GenerateDefaultOutput(string path)
        {
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise INPUT to this parameter
        /// </summary>
        /// <param name="dataSets">A list of DataSetArrays.</param>
        /// <returns>The dialog component that should be used to visualise INPUT to this parameter.</returns>
        public virtual DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            return null;
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise OUTPUT to this parameter
        /// </summary>
        /// <param name="dataSets">A list of DataSetArrays.</param>
        /// <returns>The dialog component that should be used to visualise OUTPUT to this parameter.</returns>
        public virtual DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return null;
        }

        /// <summary>
        /// Call this when the parameter's value is changed
        /// </summary>
        protected void OnValueChanged()
        {
            ValueChanged?.Invoke(this);
        }

        #endregion
    }
}