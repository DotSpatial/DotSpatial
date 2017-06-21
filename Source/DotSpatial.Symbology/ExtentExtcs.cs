// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/13/2010 7:40:48 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Extension methods for extent that are related to System.Drawing, and not data.
    /// </summary>
    public static class ExtentExt
    {
        /// <summary>
        /// This method assumes that there was a direct correlation between this envelope and the original
        /// rectangle.  This reproportions this window to match the specified newRectangle.
        /// </summary>
        /// <param name="self">The original envelope</param>
        /// <param name="original">The original rectangle </param>
        /// <param name="newRectangle">The new rectangle</param>
        /// <returns>A new IEnvelope </returns>
        public static Extent Reproportion(this Extent self, Rectangle original, Rectangle newRectangle)
        {
            double dx = self.Width * (newRectangle.X - original.X) / original.Width;
            double dy = self.Height * (newRectangle.Y - original.Y) / original.Height;
            double w = (self.Width * newRectangle.Width / original.Width);
            double h = (self.Height * newRectangle.Height / original.Height);
            return new Extent(self.X + dx, self.Y + dy - h, self.X + dx + w, self.Y + dy);
        }
    }
}