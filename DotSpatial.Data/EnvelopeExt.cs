// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created Before 2010.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// |   Ted Dunsford       |  6/30/2010  |  Moved to DotSpatial
// ********************************************************************************************************

using System.Drawing;
using DotSpatial.Topology;

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
        public static IEnvelope Reproportion(this IEnvelope self, Rectangle original, Rectangle newRectangle)
        {
            double dx = self.Width * (newRectangle.X - original.X) / original.Width;
            double dy = self.Height * (newRectangle.Y - original.Y) / original.Height;
            double w = (self.Width * newRectangle.Width / original.Width);
            double h = (self.Height * newRectangle.Height / original.Height);
            double[] ext = { self.X + dx, self.X + dx + w, self.Y + dy - h, self.Y + dy };
            return new Envelope(ext);
        }

        /// <summary>
        /// This allows the creation of the correct kind of Extent class from an Envelope, which can contain
        /// M or Z values.
        /// </summary>
        /// <param name="self">The Envelope to convert into an Extent.</param>
        /// <returns></returns>
        public static Extent ToExtent(this IEnvelope self)
        {
            if (self.HasZ())
            {
                // regardless of whether it has M, we need an MZExtent.
                return new ExtentMZ(self);
            }
            if (self.HasM())
            {
                return new ExtentM(self);
            }
            return new Extent(self);
        }
    }
}