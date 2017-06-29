// ********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/31/2008 9:46:39 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Topology.KDTree
{
    /// <summary>
    /// NegativeInvalidException
    /// </summary>
    public class NegativeArgumentException : ArgumentException
    {
        /// <summary>
        /// This creates a new instance of an exception that occurs if a negative value was passed as an argument and this is invalid
        /// </summary>
        /// <param name="parameterName">The name of the parameter that was negative</param>
        public NegativeArgumentException(string parameterName)
            : base(TopologyText.ArgumentCannotBeNegative_S.Replace("%S", parameterName))
        {
        }
    }
}