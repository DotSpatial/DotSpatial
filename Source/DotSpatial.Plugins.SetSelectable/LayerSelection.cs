using DotSpatial.Symbology;

namespace DotSpatial.Plugins.SetSelectable
{
    /// <summary>
    /// Class to control layers selections through the datagridview.
    /// </summary>
    internal class LayerSelection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerSelection"/> class for the given layer.
        /// </summary>
        /// <param name="layer">Layer, for which the LayerSelection-object gets created.</param>
        public LayerSelection(IFeatureLayer layer)
        {
            Layer = layer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of selected features in the DataGridView. This Property is needed for the DataGridView.
        /// </summary>
        public int DgvcCount => Layer.Selection.Count;

        /// <summary>
        /// Gets the layer name in the DataGridView. This Property is needed for the DataGridView.
        /// </summary>
        public string DgvcLayerName => Layer.LegendText;

        /// <summary>
        /// Gets or sets a value indicating whether the layers features can be selected. This Property is needed for the DataGridView.
        /// </summary>
        public bool DgvcSelectable
        {
            get
            {
                return Layer.SelectionEnabled;
            }

            set
            {
                Layer.SelectionEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the layer, whose selection can be changed.
        /// </summary>
        public IFeatureLayer Layer { get; set; }

        #endregion
    }
}