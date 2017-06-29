// ********************************************************************************************************
// Product Name: DotSpatial.Tools.ToolDialog
// Description:  The tool dialog to be used by tools. It get populated by DialogElements once it is created
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initializeializeial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// A generic form that works with the various dialog elements in order to create a fully working process.
    /// </summary>
    public partial class ToolDialog : Form
    {
        #region Constants and Fields

        private List<DataSetArray> _dataSets = new List<DataSetArray>();

        private int _elementHeight = 3;

        private Extent _extent;

        private List<DialogElement> _listOfDialogElements = new List<DialogElement>();

        private ITool _tool;

        private IContainer components = null;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// The constructor for the ToolDialog
        /// </summary>
        /// <param name="tool">The ITool to create the dialog box for</param>
        /// <param name="dataSets">The list of available DataSets available</param>
        /// <param name="mapExtent">Creates a new instance of the tool dialog with map extent.</param>
        public ToolDialog(ITool tool, List<DataSetArray> dataSets, Extent mapExtent)
        {
            // Required by the designer
            InitializeComponent();
            DataSets = dataSets;
            _extent = mapExtent;
            Initialize(tool);
        }

        /// <summary>
        /// The constructor for the ToolDialog
        /// </summary>
        /// <param name="tool">The ITool to create the dialog box for</param>
        /// <param name="modelElements">A list of all model elements</param>
        public ToolDialog(ITool tool, IEnumerable<ModelElement> modelElements)
        {
            // Required by the designer
            InitializeComponent();

            // We store all the element names here and extract the datasets
            foreach (ModelElement me in modelElements)
            {
                if (me as DataElement != null)
                {
                    bool addData = true;
                    foreach (Parameter par in tool.OutputParameters)
                    {
                        if (par.ModelName == (me as DataElement).Parameter.ModelName)
                        {
                            addData = false;
                        }

                        break;
                    }

                    if (addData)
                    {
                        _dataSets.Add(new DataSetArray(me.Name, (me as DataElement).Parameter.Value as IDataSet));
                    }
                }
            }

            Initialize(tool);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns a list of IDataSet that are available in the ToolDialog excluding any of its own outputs.
        /// </summary>
        public List<DataSetArray> DataSets
        {
            get
            {
                return _dataSets;
            }

            set
            {
                _dataSets = value;
            }
        }

        /// <summary>
        /// Gets the status of the tool
        /// </summary>
        public ToolStatus ToolStatus
        {
            get
            {
                foreach (DialogElement de in _listOfDialogElements)
                {
                    if (de.Status != ToolStatus.Ok)
                    {
                        return ToolStatus.Error;
                    }
                }

                return ToolStatus.Ok;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This adds the Elements to the form incrementally lower down
        /// </summary>
        /// <param name="element">The element to add</param>
        private void AddElement(DialogElement element)
        {
            _listOfDialogElements.Add(element);
            panelElementContainer.Controls.Add(element);
            element.Clicked += ElementClicked;
            element.Location = new Point(5, _elementHeight);
            _elementHeight = element.Height + _elementHeight;
        }

        /// <summary>
        /// When one of the DialogElements is clicked this event fires to populate the help
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElementClicked(object sender, EventArgs e)
        {
            DialogElement element = sender as DialogElement;
            if (element == null)
            {
                PopulateHelp(_tool.Name, _tool.Description, _tool.HelpImage);
            }
            else if (element.Param == null)
            {
                PopulateHelp(_tool.Name, _tool.Description, _tool.HelpImage);
            }
            else if (element.Param.HelpText == string.Empty)
            {
                PopulateHelp(_tool.Name, _tool.Description, _tool.HelpImage);
            }
            else
            {
                PopulateHelp(element.Param.Name, element.Param.HelpText, element.Param.HelpImage);
            }
        }

        /// <summary>
        /// The constructor for the ToolDialog
        /// </summary>
        /// <param name="tool">The ITool to create the dialog box for</param>
        private void Initialize(ITool tool)
        {
            SuspendLayout();

            // Generates the form based on what inputs the ITool has
            _tool = tool;
            Text = tool.Name;

            // Sets up the help link
            if (string.IsNullOrEmpty(tool.HelpUrl))
            {
                helpHyperlink.Visible = false;
            }
            else
            {
                helpHyperlink.Links[0].LinkData = tool.HelpUrl;
                helpHyperlink.Links.Add(0, helpHyperlink.Text.Length, tool.HelpUrl);
            }

            // Sets-up the icon for the Dialog
            Icon = Images.HammerSmall;
            panelToolIcon.BackgroundImage = tool.Icon ?? Images.Hammer;

            DialogSpacerElement inputSpacer = new DialogSpacerElement();
            inputSpacer.Text = ModelingMessageStrings.Input;
            AddElement(inputSpacer);

            // Populates the dialog with input elements
            PopulateInputElements();

            DialogSpacerElement outputSpacer = new DialogSpacerElement();
            outputSpacer.Text = ModelingMessageStrings.Output;
            AddElement(outputSpacer);

            // Populates the dialog with output elements
            PopulateOutputElements();

            // Populate the help text
            PopulateHelp(_tool.Name, _tool.Description, _tool.HelpImage);

            ResumeLayout();
        }

        /// <summary>
        /// Fires when a parameter is changed
        /// </summary>
        /// <param name="sender"></param>
        private void ParamValueChanged(Parameter sender)
        {
            _tool.ParameterChanged(sender);
        }

        /// <summary>
        /// This adds a Bitmap to the help section.
        /// </summary>
        /// <param name="title">The text to appear in the help box.</param>
        /// <param name="body">The title to appear in the help box.</param>
        /// <param name="image">The bitmap to appear at the bottom of the help box.</param>
        private void PopulateHelp(String title, String body, Image image)
        {
            rtbHelp.Text = string.Empty;
            rtbHelp.Size = new Size(0, 0);

            // Add the Title
            Font fBold = new Font("Tahoma", 14, FontStyle.Bold);
            rtbHelp.SelectionFont = fBold;
            rtbHelp.SelectionColor = Color.Black;
            rtbHelp.SelectedText = title + "\r\n\r\n";

            // Add the text body
            fBold = new Font("Tahoma", 8, FontStyle.Bold);
            rtbHelp.SelectionFont = fBold;
            rtbHelp.SelectionColor = Color.Black;
            rtbHelp.SelectedText = body;
            rtbHelp.Size = new Size(rtbHelp.Width, rtbHelp.GetPositionFromCharIndex(rtbHelp.Text.Length).Y + 30);

            // Add the image to the bottom
            if (image != null)
            {
                pnlHelpImage.Visible = true;
                if (image.Size.Width > 250)
                {
                    double height = image.Size.Height;
                    double width = image.Size.Width;
                    int newHeight = Convert.ToInt32(250 * (height / width));
                    pnlHelpImage.BackgroundImage = new Bitmap(image, new Size(250, newHeight));
                    pnlHelpImage.Size = new Size(250, newHeight);
                }
                else
                {
                    pnlHelpImage.BackgroundImage = image;
                    pnlHelpImage.Size = image.Size;
                }
            }
            else
            {
                pnlHelpImage.Visible = false;
                pnlHelpImage.BackgroundImage = null;
                pnlHelpImage.Size = new Size(0, 0);
            }
        }

        /// <summary>
        /// Adds Elements to the dialog based on what input Parameter the ITool contains
        /// </summary>
        private void PopulateInputElements()
        {
            // Loops through all the Parameter in the tool and generated their element
            foreach (Parameter param in _tool.InputParameters)
            {
                // We make sure that the input parameter is defined
                if (param == null)
                {
                    continue;
                }

                // We add an event handler that fires if the parameter is changed
                param.ValueChanged += ParamValueChanged;
                ExtentParam p = param as ExtentParam;
                if (p != null && p.DefaultToMapExtent)
                {
                    p.Value = _extent;
                }

                // Retrieve the dialog element from the parameter and add it to the dialog
                AddElement(param.InputDialogElement(DataSets));
            }
        }

        /// <summary>
        /// Adds Elements to the dialog based on what output Parameter the ITool contains
        /// </summary>
        private void PopulateOutputElements()
        {
            if (_tool.OutputParameters == null)
            {
                return;
            }

            // Loops through all the Parameter in the tool and generated their element
            foreach (Parameter param in _tool.OutputParameters)
            {
                // We add an event handler that fires if the parameter is changed
                param.ValueChanged += ParamValueChanged;

                // Retrieve the dialog element from the parameter and add it to the dialog
                AddElement(param.OutputDialogElement(DataSets));
            }
        }

        /// <summary>
        /// When the user clicks Cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            return;
        }

        /// <summary>
        /// When the user clicks OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// When the hyperlink is clicked this event fires.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpHyperlink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Determine which link was clicked within the LinkLabel.
            helpHyperlink.Links[helpHyperlink.Links.IndexOf(e.Link)].Visited = true;

            // Display the appropriate link based on the value of the
            // LinkData property of the Link object.
            string target = e.Link.LinkData as string;

            Process.Start(target);
        }

        /// <summary>
        /// If the user clicks out side of one of the tool elements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otherElement_Click(object sender, EventArgs e)
        {
            PopulateHelp(_tool.Name, _tool.Description, _tool.HelpImage);
        }

        /// <summary>
        /// When the size of the help panel changes this event fires to move stuff around.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelHelp_SizeChanged(object sender, EventArgs e)
        {
            rtbHelp.Size = new Size(rtbHelp.Width, rtbHelp.GetPositionFromCharIndex(rtbHelp.Text.Length).Y + 30);
        }

        #endregion
    }
}