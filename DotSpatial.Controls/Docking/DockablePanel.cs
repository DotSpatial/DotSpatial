// -----------------------------------------------------------------------
// <copyright file="DockablePanel.cs" company="DotSpatial Team">
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Controls.Docking
{
    /// <summary>
    /// Named DockablePanel to avoid the name conflict with DockPanel in WPF and most control libraries.
    /// </summary>
    public class DockablePanel : INotifyPropertyChanged
    {
        #region Constants and Fields

        private string caption;
        private short defaultSortOrder;

        private DockStyle dock;

        private Control innerControl;

        private string key;

        private Image smallImage;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DockablePanel" /> class.
        /// </summary>
        public DockablePanel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DockablePanel"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="innerControl">The inner control.</param>
        /// <param name="dock">The dock.</param>
        public DockablePanel(string key, string caption, Control innerControl, DockStyle dock)
        {
            Dock = dock;
            Key = key;
            InnerControl = innerControl;
            Caption = caption;
        }

        #endregion

        #region Public Events

        /// <summary>
        ///   Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the caption of the panel and any tab button.
        /// </summary>
        /// <value>
        ///   The caption.
        /// </value>
        public string Caption
        {
            get
            {
                return caption;
            }

            set
            {
                if (caption == value)
                {
                    return;
                }

                caption = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Caption"));
            }
        }

        /// <summary>
        ///   Gets or sets The dock location.
        /// </summary>
        /// <value>
        ///   The dock location.
        /// </value>
        public DockStyle Dock
        {
            get
            {
                return dock;
            }

            set
            {
                if (dock == value)
                {
                    return;
                }

                dock = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Dock"));
            }
        }

        /// <summary>
        ///   Gets or sets the InnerControl.
        /// </summary>
        /// <value>
        ///   The InnerControl.
        /// </value>
        public Control InnerControl
        {
            get
            {
                return innerControl;
            }

            set
            {
                if (innerControl == value)
                {
                    return;
                }

                innerControl = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InnerControl"));
            }
        }

        /// <summary>
        ///   Gets or sets the key.
        /// </summary>
        /// <value>The unique identifier.</value>
        public string Key
        {
            get
            {
                return key;
            }

            set
            {
                if (key == value)
                {
                    return;
                }

                key = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Key"));
            }
        }

        /// <summary>
        /// Gets or sets the small image.
        /// </summary>
        /// <value>The small image.</value>
        public Image SmallImage
        {
            get
            {
                return smallImage;
            }

            set
            {
                smallImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SmallImage"));
            }
        }

        /// <summary>
        /// Gets or sets the sort order. Lower values will suggest that an item should appear further left in a LeftToRight environment. Or higher up in a top to bottom environment.
        /// </summary>
        /// <remarks>Use a multiple of 100 or so to allow other developers some 'space' to place their panels.</remarks>
        /// <value>
        /// The sort order.
        /// </value>
        public short DefaultSortOrder
        {
            get
            {
                return defaultSortOrder;
            }

            set
            {
                if (defaultSortOrder == value)
                {
                    return;
                }

                defaultSortOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DefaultSortOrder"));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Triggers the PropertyChanged event.
        /// </summary>
        /// <param name="ea">
        /// The ea.
        /// </param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs ea)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, ea);
            }
        }

        #endregion
    }
}