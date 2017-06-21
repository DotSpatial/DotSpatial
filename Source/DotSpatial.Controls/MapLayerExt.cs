// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/21/2009 4:03:59 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Controls
{
    /// <summary>
    /// GeoLayerEM
    /// </summary>
    public static class MapLayerExt
    {
        #region Methods

        ///// <summary>
        ///// Clears the entire back buffer as transparent.  You can also call StartDrawing
        ///// with preserve set to false to prevent having to draw and then clear contents
        ///// from the front buffer.  This is more useful to erase something being drawn.
        ///// The new color will be transparent.
        ///// </summary>
        ///// <param name="self">The IGeoLayer to clear</param>
        //public static void Clear(this IMapLayer self)
        //{
        //    self.Clear(new List<Rectangle> { self.BufferRectangle }, Color.Transparent);
        //}

        ///// <summary>
        ///// Clears the specified rectangle.  The replacement color will be transparent.
        ///// </summary>
        ///// <param name="self">The IGeoLayer to clear</param>
        ///// <param name="rectangle">The rectangle to clear</param>
        //public static void Clear(this IMapLayer self, Rectangle rectangle)
        //{
        //    self.Clear(new List<Rectangle> { rectangle }, Color.Transparent);
        //}

        ///// <summary>
        ///// Clears the list of rectangles using the transparent color
        ///// </summary>
        ///// <param name="self">The IGeoLayer to clear</param>
        ///// <param name="rectangles">The rectangles to clear</param>
        //public static void Clear(this IMapLayer self, List<Rectangle> rectangles)
        //{
        //    self.Clear(rectangles, Color.Transparent);
        //}

        ///// <summary>
        ///// Clears the specified rectangle.  The replacement color will be transparent.
        ///// </summary>
        ///// <param name="self">The IGeoLayer to clear</param>
        ///// <param name="rectangle">The rectangle to clear</param>
        ///// <param name="color">The color to use when clearing content</param>
        //public static void Clear(this IMapLayer self, Rectangle rectangle, Color color)
        //{
        //    self.Clear(new List<Rectangle> { rectangle }, color);
        //}

        ///// <summary>
        ///// Clears the specified rectangle.  The replacement color will be transparent.
        ///// </summary>
        ///// <param name="self">The IGeoLayer to clear</param>
        ///// <param name="region">The Geographic region to clear</param>
        ///// <param name="color">The color to use when clearing content</param>
        //public static void Clear(this IMapLayer self, IEnvelope region, Color color)
        //{
        //    MapArgs args = self.CreateGeoArgs();
        //    Rectangle rectangle = args.ProjToPixel(region);
        //    if (rectangle.IsEmpty == false)
        //    {
        //        self.Clear(new List<Rectangle> { rectangle }, color);
        //    }
        //}

        ///// <summary>
        ///// Clears the specified rectangle.  The replacement color will be transparent.
        ///// </summary>
        ///// <param name="self">The IGeoLayer to clear</param>
        ///// <param name="region">The Geographic region to clear</param>
        //public static void Clear(this IMapLayer self, IEnvelope region)
        //{
        //    Clear(self, region, Color.Transparent);
        //}

        ///// <summary>
        ///// Clears the specified rectangle.  The replacement color will be transparent.
        ///// </summary>
        ///// <param name="self">The IGeoLayer to clear</param>
        ///// <param name="regions">The Geographic regions to clear</param>
        //public static void Clear(this IMapLayer self, List<IEnvelope> regions)
        //{
        //    Clear(self, regions, Color.Transparent);
        //}

        ///// <summary>
        ///// Clears the specified rectangle.  The replacement color will be transparent.
        ///// </summary>
        ///// <param name="self">The IGeoLayer to clear</param>
        ///// <param name="regions">The Geographic regions to clear</param>
        ///// <param name="color">The color to use when clearing content</param>
        //public static void Clear(this IMapLayer self, List<IEnvelope> regions, Color color)
        //{
        //    MapArgs args = self.CreateGeoArgs();
        //    List<Rectangle> rectangles = new List<Rectangle>();
        //    foreach (IEnvelope region in regions)
        //    {
        //        if (region != null)
        //        {
        //            Rectangle rectangle = args.ProjToPixel(region);
        //            if (rectangle.IsEmpty == false)
        //            {
        //                rectangles.Add(rectangle);
        //            }
        //        }
        //    }
        //    if (rectangles.Count > 0)
        //    {
        //        self.Clear(rectangles, color);
        //    }
        //}

        ///// <summary>
        ///// This overload is useful for dealing with selections.  It doesn't change the
        ///// buffer size or geographic extent, but will attempt to draw features in the
        ///// specified regions.  If the buffer is undefined, the DefaultBufferSize
        ///// will be used.
        ///// </summary>
        ///// <param name="self">The IGeoLayer to draw</param>
        ///// <param name="regions">The list of geographic regions to draw</param>
        //public static void DrawRegions(this IMapLayer self, List<IEnvelope> regions)
        //{
        //    IEnvelope fullExtent = self.BufferEnvelope;

        //    if (fullExtent == null || fullExtent.IsNull)
        //    {
        //        foreach (IEnvelope region in regions)
        //        {
        //            if (region != null)
        //            {
        //                fullExtent.ExpandToInclude(region);
        //            }
        //        }
        //    }
        //    if (fullExtent != null && fullExtent.IsNull == false)
        //    {
        //        MapArgs args = CreateGeoArgs(self, fullExtent);
        //        self.DrawRegions(args, regions);
        //    }

        //}

        ///// <summary>
        ///// Clears and re-draws the content for the entire buffer, using the current buffer
        ///// dimensions (or the DefaultBufferSize if the buffer doesn't exist)
        ///// </summary>
        ///// <param name="self">The IGeoLayer to intitialize</param>
        //public static void Initialize(this IMapLayer self)
        //{
        //    if (self.BufferEnvelope == null || self.BufferEnvelope.IsNull)
        //    {
        //        IMapFrame mg = self.MapFrame as IMapFrame;
        //        if (mg != null)
        //        {
        //            self.BufferEnvelope = mg.Extents;
        //            self.BufferRectangle = mg.ClientRectangle;
        //        }
        //        else
        //        {
        //            IMapLabelLayer lblLyr = self as IMapLabelLayer;
        //            if (lblLyr != null)
        //            {
        //                self.BufferEnvelope = lblLyr.FeatureLayer.BufferEnvelope;
        //                self.BufferRectangle = lblLyr.FeatureLayer.BufferRectangle;
        //            }
        //            else
        //            {
        //                self.BufferEnvelope = self.Envelope;
        //                self.BufferRectangle = new Rectangle(0, 0, 1000, 1000);
        //            }

        //        }
        //    }
        //    MapArgs args = CreateGeoArgs(self);
        //    if (args == null) return;
        //    self.Initialize(args, self.BufferEnvelope);
        //}

        ///// <summary>
        ///// Begins the process of drawing, specifying a single geographic region to draw.
        ///// </summary>
        ///// <param name="self">The IGeoLayer to initialize</param>
        ///// <param name="args">A GeoArgs specifying the rectangular dimensions</param>
        ///// <param name="region">A list of the geographic regions to draw content to</param>
        //public static void Initialize(this IMapLayer self, MapArgs args, IEnvelope region)
        //{
        //    self.Initialize(args, new List<IEnvelope> { region });
        //}

        ///// <summary>
        ///// Creates a GeoArgs parameter from the current buffer and extent.  If the current
        ///// buffer is not available, the DefaultBufferSize is used.  If the BufferExtent
        ///// is not defined, then will throw an exception.
        ///// </summary>
        ///// <param name="self"></param>
        ///// <returns></returns>
        //public static MapArgs CreateGeoArgs(this IMapLayer self)
        //{
        //    if (self.BufferEnvelope == null || self.BufferEnvelope.IsNull) return null;
        //    return CreateGeoArgs(self, self.BufferEnvelope);
        //}

        ///// <summary>
        ///// Creates a GeoArgs parameter that matches teh
        ///// </summary>
        ///// <param name="self">This IGeoLayer</param>
        ///// <param name="geographicExtent">The geographic extent that should be drawn to the buffer</param>
        ///// <returns>A GeoArgs with the appropriate dimensions.</returns>
        ///// <exception cref="ArgumentNullException">geoGraphicExtent cannot be null</exception>
        //public static MapArgs CreateGeoArgs(this IMapLayer self, IEnvelope geographicExtent)
        //{
        //    if (geographicExtent == null || geographicExtent.IsNull) return null;
        //    return new MapArgs(self.BufferRectangle, geographicExtent);
        //}

        ///// <summary>
        ///// Pans the image for this layer.  Instead of drawing entirely new content just the
        ///// newly revealed area is re-drawn.
        ///// </summary>
        ///// <param name="self">The IGeoLayer to apply the panning technique to</param>
        ///// <param name="shift">A Point showing the amount to shift in pixels</param>
        ///// <returns>The rectangular regions that were updated</returns>
        //public static List<Rectangle> Pan(this IMapLayer self, Point shift)
        //{
        //    // If this layer is a group, then pan each of its members instead
        //    IGroup grp = self as IGroup;
        //    if (grp != null)
        //    {
        //        List<Rectangle> rects = new List<Rectangle>();
        //        foreach (IMapLayer layer in grp.Layers)
        //        {
        //            rects = layer.Pan(shift);
        //        }
        //        return rects;
        //    }

        //    // This is only a valid opperation if the current buffer exists
        //    if (self.Buffer == null) return null;

        //    Rectangle currentClip;
        //    int width = self.Buffer.Width;
        //    int height = self.Buffer.Height;

        //    Image backBuffer = new Bitmap(self.Buffer.Width, self.Buffer.Height);

        //    Graphics g = Graphics.FromImage(backBuffer);
        //    g.DrawImageUnscaled(self.Buffer, -shift.X, -shift.Y);
        //    g.Dispose();

        //    if (self.BackBuffer != null && self.BackBuffer != self.Buffer) self.BackBuffer.Dispose();
        //    self.BackBuffer = backBuffer;

        //    List<IEnvelope> clipRegions = new List<IEnvelope>();
        //    List<Rectangle> clipRectangles = new List<Rectangle>();
        //    Rectangle bufferRect = new Rectangle(0, 0, width, height);
        //    MapArgs args = new MapArgs(bufferRect, self.BufferEnvelope);
        //    double dx = shift.X * self.BufferEnvelope.Width / self.Buffer.Width;
        //    double dy = -shift.Y * self.BufferEnvelope.Height / self.Buffer.Height;
        //    self.BufferEnvelope.Translate(dx, dy);
        //    if (shift.X < 0)
        //    {
        //        currentClip = new Rectangle(0, 0, Math.Abs(shift.X), height);
        //    }
        //    else
        //    {
        //        currentClip = new Rectangle(width - Math.Abs(shift.X), 0, Math.Abs(shift.X), height);
        //    }
        //    if (currentClip.IsEmpty == false)
        //    {
        //        clipRectangles.Add(currentClip);
        //        clipRegions.Add(args.PixelToProj(currentClip));
        //    }

        //    if (shift.Y < 0)
        //    {
        //        // Top
        //        if (shift.X < 0)
        //        {
        //            currentClip = new Rectangle(Math.Abs(shift.X), 0, width - Math.Abs(shift.X), Math.Abs(shift.Y));
        //        }
        //        else
        //        {
        //            currentClip = new Rectangle(0, 0, width - Math.Abs(shift.X), Math.Abs(shift.Y));
        //        }
        //    }
        //    else
        //    {
        //        // Bottom
        //        if (shift.X < 0)
        //        {
        //            currentClip = new Rectangle(Math.Abs(shift.X), height - Math.Abs(shift.Y), width - Math.Abs(shift.X), Math.Abs(shift.Y));
        //        }
        //        else
        //        {
        //            currentClip = new Rectangle(0, height - Math.Abs(shift.Y), width - Math.Abs(shift.X), Math.Abs(shift.Y));
        //        }
        //    }
        //    if (currentClip.IsEmpty == false)
        //    {
        //        clipRectangles.Add(currentClip);
        //        clipRegions.Add(args.PixelToProj(currentClip));
        //    }

        //    if (clipRegions.Count > 0)
        //    {
        //        // Draw features to the new regions.
        //        self.DrawRegions(args, clipRegions);

        //    }
        //    // Go ahead and swap the buffer for this layer to the front.
        //    self.FinishDrawing();

        //    IMapFeatureLayer mfl = self as IMapFeatureLayer;
        //    if (mfl != null)
        //    {
        //        if (mfl.LabelLayer != null) mfl.LabelLayer.Pan(shift);
        //    }

        //    // Don't call OnBufferChanged here because we would normally be updating all the layers at once.
        //    // We call the OnBufferChanged from the MapFrame.
        //    return clipRectangles;

        //}

        ///// <summary>
        ///// Gets the rectangle bounds for the specified regions, and returns a list of rectangles
        ///// where the rectangle is the intersection of the specified list of regions and the
        ///// current extent.  If none of the regions are in the image extent, this returns
        ///// a list with no members.
        ///// </summary>
        ///// <param name="self"></param>
        ///// <param name="regions"></param>
        ///// <returns></returns>
        //public static List<Rectangle> GetRectangles(this IMapLayer self, List<IEnvelope> regions)
        //{
        //   // MapArgs args = CreateGeoArgs(self);
        //    List<Rectangle> result = new List<Rectangle>();
        //    foreach (IEnvelope region in regions)
        //    {
        //        if (region != null)
        //        {
        //            Rectangle rectangle = self.MapFrame.ProjToPixel(region);
        //            rectangle.Intersect(self.MapFrame.);
        //            if (rectangle.IsEmpty == false)
        //            {
        //                if (rectangle.Width > 0 && rectangle.Height > 0)
        //                {
        //                    result.Add(rectangle);
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// Given an arbitrary set of rectangles, this obtains the geographic regions for areas
        ///// as the intersection of the specified rectangles with this drawing bounds, transformed
        ///// into geographic coordinates.  If none of the rectangles are in the drawing extent,
        ///// this returns an empty list.
        ///// </summary>
        ///// <param name="self"></param>
        ///// <param name="clipRectangles"></param>
        ///// <returns></returns>
        //public static List<IEnvelope> GetEnvelopes(this IMapLayer self, List<Rectangle> clipRectangles)
        //{
        //    MapArgs args = self.CreateGeoArgs();
        //    List<IEnvelope> result = new List<IEnvelope>();
        //    foreach (Rectangle rectangle in clipRectangles)
        //    {
        //        rectangle.Intersect(args.ImageRectangle);
        //        if (rectangle.IsEmpty == false)
        //        {
        //            IEnvelope region = args.PixelToProj(rectangle);
        //            result.Add(region);
        //        }
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// Creates an overload where preserve is automatically set to true.
        ///// </summary>
        ///// <param name="self">The IGeoLayer where drawing will take place.</param>
        //public static void StartDrawing(this IMapLayer self)
        //{
        //    self.StartDrawing(true);
        //}

        #endregion
    }
}