using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Serialization;
using DotSpatial.Controls;
using System.Drawing;
using DotSpatial.Data;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using DotSpatial.Symbology;
using DotSpatial.Topology;
using Point = System.Drawing.Point;
using DotSpatial.Projections;


namespace DotSpatial.WebControls
{
  
    public class GDIMapFrame 
    {
        private IMapLayerCollection _layers;

        [Serialize("Layers")]
        public IMapLayerCollection Layers
        {
            get 
            { 
                return _layers; 
            }
            set
            {
                _layers = value;
            }
        }

        private Extent _viewextent;
        
        [Serialize("ViewExtents")]
        public Extent ViewExtents
        {
            get
            {
                return _viewextent;
            }
            set
            {
                if (value == null) return;
                Extent ext = value;
                ResetAspectRatio(ext);
                // reset buffer initializes with correct buffer.  Don't allow initialization yet.
                _viewextent = ext;
            }
        }

        [Serialize("MapExtent")]
        public Extent MapExtent
        {
            get;
            set;
        }

        public Rectangle _view;
        /// <summary>
        /// gets or sets the rectangle in pixel coordinates that will be drawn to the entire screen.
        /// </summary>
        public Rectangle View
        {
            get 
            { 
                return _view; 
            }
            set 
            {
                
                _view = value;

                Extent Ext = PixelToProj(_view); 

                ResetAspectRatio(Ext);
                _viewextent = Ext;
            }
        }


        /// <summary>
        /// Gets or sets the projection.  This should reflect the projection of the first data layer loaded.
        /// Loading subsequent, but non-matching projections should throw an alert, and allow reprojection.
        /// </summary>
        public ProjectionInfo Projection
        {
            get;
            set;
        }

      
        /// <summary>
        /// Gets the geographic extents
        /// </summary>
        public Extent GeographicExtents
        {
            get { return ViewExtents; }
        }


        /// <summary>
        /// Converts a single point location into an equivalent geographic coordinate
        /// </summary>
        /// <param name="self">This IProj</param>
        /// <param name="position">The client coordinate relative to the map control</param>
        /// <returns>The geographic ICoordinate interface</returns>
        public Coordinate PixelToProj(Point position)
        {
            double x = Convert.ToDouble(position.X);
            double y = Convert.ToDouble(position.Y);
            
            if (GeographicExtents != null)
            {
                x = x * GeographicExtents.Width / View.Width + GeographicExtents.MinX;
                y = GeographicExtents.MaxY - y * GeographicExtents.Height / View.Height;
            }

            return new Coordinate(x, y, 0.0);
        }


        /// <summary>
        /// Converts a rectangle in pixel coordinates relative to the map control into
        /// a geographic envelope.
        /// </summary>
        /// <param name="self">This IProj</param>
        /// <param name="rect">The rectangle to convert</param>
        /// <returns>An IEnvelope interface</returns>
        public Extent PixelToProj( Rectangle rect)
        {
            Point tl = new Point(rect.X, rect.Y);
            Point br = new Point(rect.Right, rect.Bottom);
            Coordinate topLeft = PixelToProj(tl);
            Coordinate bottomRight = PixelToProj(br);
            return new Extent(topLeft.X, bottomRight.Y, bottomRight.X, topLeft.Y);
        }

        /// <summary>
        /// Projects all of the rectangles int the specified list of rectangles into geographic regions.
        /// </summary>
        /// <param name="self">This IProj</param>
        /// <param name="clipRects">The clip rectangles</param>
        /// <returns>A List of IEnvelope geographic bounds that correspond to the specified clip rectangles.</returns>
        public List<Extent> PixelToProj(List<Rectangle> clipRects)
        {
            List<Extent> result = new List<Extent>();
            foreach (Rectangle r in clipRects)
            {
                result.Add(PixelToProj(r));
            }
            return result;
        }

        /// <summary>
        /// Converts a single geographic location into the equivalent point on the
        /// screen relative to the top left corner of the map.
        /// </summary>
        /// <param name="self">This IProj</param>
        /// <param name="location">The geographic position to transform</param>
        /// <returns>A Point with the new location.</returns>
        public Point ProjToPixel( Coordinate location)
        {
            if (GeographicExtents.Width == 0 || GeographicExtents.Height == 0) return Point.Empty;
            int x = Convert.ToInt32((location.X - GeographicExtents.MinX) *
                (View.Width / GeographicExtents.Width));
            int y = Convert.ToInt32((GeographicExtents.MaxY - location.Y) *
                (View.Height / GeographicExtents.Height));
            return new Point(x, y);
        }

        /// <summary>
        /// Converts a single geographic envelope into an equivalent Rectangle
        /// as it would be drawn on the screen.
        /// </summary>
        /// <param name="self">This IProj</param>
        /// <param name="env">The geographic IEnvelope</param>
        /// <returns>A Rectangle</returns>
        public Rectangle ProjToPixel(Extent env)
        {
            Coordinate tl = new Coordinate(env.MinX, env.MaxY);
            Coordinate br = new Coordinate(env.MaxX, env.MinY);
            Point topLeft = ProjToPixel(tl);
            Point bottomRight = ProjToPixel(br);
            return new Rectangle(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
        }

        /// <summary>
        /// Translates all of the geographic regions, forming an equivalent list of rectangles.
        /// </summary>
        /// <param name="self">This IProj</param>
        /// <param name="regions">The list of geographic regions to project</param>
        /// <returns>A list of pixel rectangles that describe the specified region</returns>
        public List<Rectangle> ProjToPixel(List<Extent> regions)
        {
            List<Rectangle> result = new List<Rectangle>();
            foreach (Extent region in regions)
            {
                if (region == null) continue;
                result.Add(ProjToPixel(region));
            }
            return result;
        }

        /// <summary>
        /// Calculates an integer length distance in pixels that corresponds to the double
        /// length specified in the image.
        /// </summary>
        /// <param name="self">The IProj that this describes</param>
        /// <param name="distance">The double distance to obtain in pixels</param>
        /// <returns>The integer distance in pixels</returns>
        public double ProjToPixel(double distance)
        {
            return (distance * View.Width / View.Width);
        }

        public GDIMapFrame()
        {
            Layers = new MapLayerCollection();
        }

        /// <summary>
        /// Instead of using the usual buffers, this bypasses any buffering and instructs the layers
        /// to draw directly to the specified target rectangle on the graphics object.  This is useful
        /// for doing vector drawing on much larger pages.  The result will be centered in the
        /// specified target rectangle bounds.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="targetRectangle"></param>
        public void Print(Graphics device, Rectangle targetRectangle)
        {
            Print(device, targetRectangle, ViewExtents);
        }

        public void Print(Graphics device, Rectangle targetRectangle, Extent targetEnvelope)
        {
            MapArgs args = new MapArgs(targetRectangle, targetEnvelope, device);
            Matrix oldMatrix = device.Transform;
            try
            {
                device.TranslateTransform(targetRectangle.X, targetRectangle.Y);

                foreach (IMapLayer ml in Layers)
                {
                    PrintLayer(ml, args);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                device.Transform = oldMatrix;
            }
        }
        
        /// <summary>
        /// Zooms in one notch, so that the scale becomes larger and the features become larger.
        /// </summary>
        public void ZoomIn()
        {
            Rectangle r = View;
            r.Inflate(-r.Width / 4, -r.Height / 4);

            ViewExtents = PixelToProj(r);
        }

        /// <summary>
        /// Zooms out one notch so that the scale becomes smaller and the features become smaller.
        /// </summary>
        public void ZoomOut()
        {
            Rectangle r = View;
            r.Inflate(r.Width / 2, r.Height / 2);

            ViewExtents = PixelToProj(r);
        }

        /// <summary>
        /// Pans the image for this map frame.  Instead of drawing entirely new content, from all 5 zones,
        /// just the slivers of newly revealed area need to be re-drawn.
        /// </summary>
        /// <param name="shift">A Point showing the amount to shift in pixels</param>
        public void Pan(Point shift)
        {
            Rectangle r = new Rectangle(_view.X + shift.X, _view.Y + shift.Y, _view.Width, _view.Height);
            ViewExtents = PixelToProj(r);
        }


        private void PrintLayer(IMapLayer layer, MapArgs args)
        {
            //MapLabelLayer.ExistingLabels.Clear();
            IMapGroup group = layer as IMapGroup;
            if (group != null)
            {
                foreach (IMapLayer subLayer in group.Layers)
                {
                    PrintLayer(subLayer, args);
                }
            }

            IMapLayer geoLayer = layer;
            if (geoLayer != null)
            {
                if (geoLayer.UseDynamicVisibility)
                {
                    if (ViewExtents.Width > geoLayer.DynamicVisibilityWidth)
                    {
                        return;  // skip the geoLayer if we are zoomed out too far.
                    }
                }

                if (geoLayer.IsVisible == false) return;

                geoLayer.DrawRegions(args, new List<Extent> { args.GeographicExtents });

                IMapFeatureLayer mfl = geoLayer as IMapFeatureLayer;
                if (mfl != null)
                {
                    if (mfl.UseDynamicVisibility)
                    {
                        if (ViewExtents.Width > mfl.DynamicVisibilityWidth) return;
                    }
                    if (mfl.ShowLabels && mfl.LabelLayer != null)
                    {
                        if (mfl.LabelLayer.UseDynamicVisibility)
                        {
                            if (ViewExtents.Width > mfl.LabelLayer.DynamicVisibilityWidth) return;
                        }
                        mfl.LabelLayer.DrawRegions(args, new List<Extent> { args.GeographicExtents });
                    }
                }
            }
        }


        /// <summary>
        /// If a BackBuffer.Extents exists, this will enlarge those extents to match the aspect ratio
        /// of the pixel view.  If one doesn't exist, the _mapFrame.Extents will be used instead.
        /// </summary>
        /// <param name="newEnv">The envelope to adjust</param>
        protected void ResetAspectRatio(Extent newEnv)
        {

            // ---------- Aspect Ratio Handling
            if (newEnv == null) return;
            double h = View.Height;
            double w = View.Width;
            // It isn't exactly an exception, but rather just an indication not to do anything here.
            if (h == 0 || w == 0) return;

            double controlAspect = w / h;
            double envelopeAspect = newEnv.Width / newEnv.Height;
            Coordinate center = newEnv.ToEnvelope().Center();

            if (controlAspect > envelopeAspect)
            {
                // The Control is proportionally wider than the envelope to display.
                // If the envelope is proportionately wider than the control, "reveal" more width without
                // changing height If the envelope is proportionately taller than the control,
                // "hide" width without changing height
                newEnv.SetCenter(center, newEnv.Height * controlAspect, newEnv.Height);
            }
            else
            {
                // The control is proportionally taller than the content is
                // If the envelope is proportionately wider than the control,
                // "hide" the extra height without changing width
                // If the envelope is proportionately taller than the control, "reveal" more height without changing width
                newEnv.SetCenter(center, newEnv.Width, newEnv.Width / controlAspect);
            }

        }






    }
}
