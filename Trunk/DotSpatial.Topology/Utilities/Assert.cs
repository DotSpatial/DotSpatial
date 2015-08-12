// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
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
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System;

namespace DotSpatial.Topology.Utilities
{
    /// <summary>
    /// A utility for making programming assertions.
    /// </summary>
    public static class Assert
    {
        #region Methods

        public static void IsEquals(Object expectedValue, Object actualValue)
        {
            IsEquals(expectedValue, actualValue, null);
        }

        public static void IsEquals(Object expectedValue, Object actualValue, string message)
        {
            if (actualValue.Equals(expectedValue))
                return;
            string s = message != null ? ": " + message : String.Empty;
            string format = String.Format("Expected {0} but encountered {1}{2}", expectedValue, actualValue, s);
            throw new AssertionFailedException(format);
        }

        public static void IsTrue(bool assertion)
        {
            IsTrue(assertion, null);
        }

        public static void IsTrue(bool assertion, string message)
        {
            if (assertion) return;
            if (message == null)
                throw new AssertionFailedException();
            throw new AssertionFailedException(message);
        }

        public static void ShouldNeverReachHere()
        {
            ShouldNeverReachHere(null);
        }

        public static void ShouldNeverReachHere(string message)
        {
            string s = (message != null ? ": " + message : String.Empty);
            string format = String.Format("Should never reach here{0}", s);
            throw new AssertionFailedException(format);
        }

        #endregion
    }
}