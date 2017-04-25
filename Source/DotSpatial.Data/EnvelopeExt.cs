// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created Before 2010.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// |   Ted Dunsford       |  6/30/2010  |  Moved to DotSpatial
// ********************************************************************************************************

using System.Drawing;
using DotSpatial.NTSExtension;
using GeoAPI.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// This class handles some extension methods that also require System.Drawing.
    /// </summary>
    public static class EnvelopeExt
    {
        /// <summary>
        /// This method assumes that there was a direct correlation between this envelope and the original
        /// rectangle.  This reproportions this window to match the specified newRectangle.
        /// </summary>
        /// <param name="self">The original envelope</param>
        /// <param name="original">The original rectangle </param>
        /// <param name="newRectangle">The new rectangle</param>
        /// <returns>A new IEnvelope </returns>
        public static Envelope Reproportion(this Envelope self, Rectangle original, Rectangle newRectangle)
        {
            double dx = self.Width * (newRectangle.X - original.X) / original.Width;
            double dy = self.Height * (newRectangle.Y - original.Y) / original.Height;
            double w = self.Width * newRectangle.Width / original.Width;
            double h = self.Height * newRectangle.Height / original.Height;
            return new Envelope(self.MinX + dx, self.MinX + dx + w, self.MinY + dy - h, self.MinY + dy);
        }

        /// <summary>
        /// This allows the creation of the correct kind of Extent class from an Envelope, which can contain
        /// M or Z values.
        /// </summary>
        /// <param name="self">The Envelope to convert into an Extent.</param>
        /// <returns>The extent that was created from the envelope.</returns>
        public static Extent ToExtent(this Envelope self)
        {
            if (self.HasZ())
            {
                // regardless of whether it has M, we need an MZExtent.
                return new ExtentMZ(self);
            }

            return self.HasM() ? new ExtentM(self) : new Extent(self);
        }
    }
}