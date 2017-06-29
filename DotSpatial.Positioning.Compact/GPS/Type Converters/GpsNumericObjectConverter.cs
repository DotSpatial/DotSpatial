using System;
using System.Collections.Generic;
using System.Text;

namespace GeoFramework.Design
{
    public abstract class GpsNumericObjectConverter : GeoFrameworkNumericObjectConverter
    {

        protected override Version HandledAssemblyVersion
        {
            get
            {
                return new Version("2.3.1.0");
            }
        }

        protected override string HandledAssemblyName
        {
            get
            {
#if PocketPC
                return "GeoFramework.Gps.PocketPC, Culture=neutral, Version=" + HandledAssemblyVersion.ToString(4) + ", PublicKeyToken=3ed3cdf4fdda3400";
#else
                return "GeoFramework.Gps, Culture=neutral, Version=" + HandledAssemblyVersion.ToString(4) + ", PublicKeyToken=3ed3cdf4fdda3400";
#endif
            }
        }
    }
}
