// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System;

namespace DotSpatial.Projections
{
    /// <summary>
    /// Validation exception for Esri string.
    /// </summary>
    public class InvalidEsriFormatException : ArgumentException
    {
        private readonly string _projectionString;

        /// <summary>
        /// Initializes a new instance of an invlaidEsriFormatException.
        /// </summary>
        /// <param name="projectionString">The text string that failed validation.</param>
        public InvalidEsriFormatException(string projectionString)
            : base(
                "The expression " + projectionString +
                " could not be parsed as a valid Esri string.  Esri strings are denoted by characteristics like the GEOGCS[ tag."
                )
        {
            _projectionString = projectionString;
        }

        /// <summary>
        /// Gets the string that had the invalid format.
        /// </summary>
        public string ProjectionString
        {
            get { return _projectionString; }
        }
    }
}