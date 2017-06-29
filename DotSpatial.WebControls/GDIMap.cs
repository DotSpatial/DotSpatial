using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Controls;
using DotSpatial.Symbology;
using DotSpatial.Data;
using System.Drawing;
using DotSpatial.Serialization;
using DotSpatial.Topology;
using Point = System.Drawing.Point;
using DotSpatial.Projections;

namespace DotSpatial.WebControls
{
    [Serializable()]
    public class GDIMap
    {
        private GDIMapFrame _gdiMapFrame;

        Size _size;
        public Size Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
                _gdiMapFrame.View = new Rectangle(0, 0, _size.Width, _size.Height);
            }
        }

       
        public Extent ViewExtents
        {
            get
            {
                return MapFrame.ViewExtents;
            }
            set
            {
                MapFrame.ViewExtents = value;
            }
        }

        public Extent MapExtents
        {
            get
            {
                return MapFrame.MapExtent;
            }
            set
            {
                MapFrame.MapExtent = value;
            }
        }

        public IMapLayerCollection Layers
        {
            get
            {
                if (_gdiMapFrame != null) return _gdiMapFrame.Layers;
                return null;
            }
        }


        /// <summary>
        /// Gets or sets the projection.  This should reflect the projection of the first data layer loaded.
        /// Loading subsequent, but non-matching projections should throw an alert, and allow reprojection.
        /// </summary>
        public ProjectionInfo Projection
        {
            get
            {
                return _gdiMapFrame.Projection;
            }
            set
            {
                _gdiMapFrame.Projection = value;
            }
        }

        /// <summary>
        /// Gets or sets the Projection Esri string of the map. This property is used for serializing
        /// the projection string to the project file.
        /// </summary>
        [Serialize("ProjectionEsriString")]
        public string ProjectionEsriString
        {
            get
            {
                return Projection != null ? Projection.ToEsriString() : null;
            }
            set
            {
                if (Projection != null)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        Projection = null;
                    }
                    else
                    {
                        if (Projection.ToEsriString() != value)
                        {
                            Projection = ProjectionInfo.FromEsriString(value);
                        }
                    }
                }
            }
        }
        [Serialize("MapFrame")]
        public GDIMapFrame MapFrame
        {
            get { return _gdiMapFrame; }
            set
            {
                _gdiMapFrame = value;
            }
        }

        public GDIMap()
        {
            
            MapFrame = new GDIMapFrame();

            Size = new Size(800, 600);

            if (Data.DataSet.ProjectionSupported())
            {
                Projection = KnownCoordinateSystems.Geographic.World.WGS1984;
            }
        }

        /// <summary>
        /// Adds the fileName as a new layer to the map, returning the new layer.
        /// </summary>
        /// <param name="fileName">The string fileName of the layer to add</param>
        /// <returns>The IMapLayer added to the file.</returns>
        public IMapLayer AddLayer(string fileName)
        {
            IDataSet dataSet = DataManager.DefaultDataManager.OpenFile(fileName);

            if (dataSet.Projection != Projection)
            {
                dataSet.Reproject(Projection);
            }

            return Layers.Add(dataSet);
        }


        public Bitmap Draw()
        {
            Bitmap b = new Bitmap(Size.Width, Size.Height);
            Graphics g = Graphics.FromImage(b);

            Rectangle r = new Rectangle(0, 0, Size.Width, Size.Height);

            MapFrame.View = r;

            MapFrame.Print(g, r);

            return b;
        }

        public IList<ILayer> GetLayers()
        {
            return Layers.Cast<ILayer>().ToList();
        }

        /// <summary>
        /// The envelope that contains all of the layers for this data frame.  Essentially this would be
        /// the extents to use if you want to zoom to the world view.
        /// </summary>
        public Extent Extent
        {
            get
            {
                Extent ext = null;
                IList<ILayer> layers = GetLayers();

                if (layers != null)
                {
                    foreach (ILayer layer in layers)
                    {
                        if (layer.Extent != null)
                        {
                            if (ext == null)
                            {
                                ext = (Extent)layer.Extent.Clone();
                            }
                            else
                            {
                                ext.ExpandToInclude(layer.Extent);
                            }
                        }
                    }
                }

                return ext;
            }
        }

        /// <summary>
        /// Instructs the map to change the perspective to include the entire drawing content, and
        /// in the case of 3D maps, changes the perspective to look from directly overhead.
        /// </summary>
        public void ZoomToMaxExtent()
        {
            
            if (MapExtents != null)
            {
                ViewExtents = Extent;
            }
            else
            {
                // to prevent exception when zoom to map with one layer with one point
                double eps = 1e-7;
                if (Extent.Width < eps || Extent.Height < eps)
                {
                    Extent newExtent = new Extent(Extent.MinX - eps, Extent.MinY - eps, Extent.MaxX + eps, Extent.MaxY + eps);
                    ViewExtents = newExtent;
                }
                else
                {
                    ViewExtents = Extent;
                }
            }

        }

        /// <summary>
        /// Zooms in one notch, so that the scale becomes larger and the features become larger.
        /// </summary>
        public void ZoomIn()
        {
            _gdiMapFrame.ZoomIn();
        }

        /// <summary>
        /// Zooms out one notch so that the scale becomes smaller and the features become smaller.
        /// </summary>
        public void ZoomOut()
        {
            _gdiMapFrame.ZoomOut();
        }

        /// <summary>
        /// Pans the image for this map frame.  Instead of drawing entirely new content, from all 5 zones,
        /// just the slivers of newly revealed area need to be re-drawn.
        /// </summary>
        /// <param name="shift">A Point showing the amount to shift in pixels</param>
        public void Pan(Point shift)
        {
            _gdiMapFrame.Pan(shift);
        }

        /// <summary>
        /// Converts a single point location into an equivalent geographic coordinate
        /// </summary>
        /// <param name="position">The client coordinate relative to the map control</param>
        /// <returns>The geographic ICoordinate interface</returns>
        public Coordinate PixelToProj(System.Drawing.Point position)
        {
            return _gdiMapFrame.PixelToProj(position);
        }

        /// <summary>
        /// Converts a rectangle in pixel coordinates relative to the map control into
        /// a geographic envelope.
        /// </summary>
        /// <param name="rect">The rectangle to convert</param>
        /// <returns>An IEnvelope interface</returns>
        public Extent PixelToProj(Rectangle rect)
        {
            return _gdiMapFrame.PixelToProj(rect);
        }

        /// <summary>
        /// Converts a single geographic location into the equivalent point on the
        /// screen relative to the top left corner of the map.
        /// </summary>
        /// <param name="location">The geographic position to transform</param>
        /// <returns>A Point with the new location.</returns>
        public System.Drawing.Point ProjToPixel(Coordinate location)
        {
            return _gdiMapFrame.ProjToPixel(location);
        }

        /// <summary>
        /// Converts a single geographic envelope into an equivalent Rectangle
        /// as it would be drawn on the screen.
        /// </summary>
        /// <param name="env">The geographic IEnvelope</param>
        /// <returns>A Rectangle</returns>
        public Rectangle ProjToPixel(Extent env)
        {
            return _gdiMapFrame.ProjToPixel(env);
        }


    }
}