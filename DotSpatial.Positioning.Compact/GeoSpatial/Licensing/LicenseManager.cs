#if PocketPC && !DesignTime
using System;
using System.Collections;
using System.Text;
using System.Reflection;

namespace GeoFramework.Licensing
{
    public static class LicenseManager
    {
        /// <summary>
        /// Indicates the host environment of the object being licenses.
        /// </summary>
        public static LicenseUsageMode UsageMode
        {
            get
            {
                // Figure out if we're in design-mode or not
#if DesignTime
			    return LicenseUsageMode.Designtime;
#else
                return LicenseUsageMode.Runtime;
#endif

            }
        }
    }
}

#endif