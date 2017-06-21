using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotSpatial.Mono
{
    public class Mono
    {
        public static bool IsRunningOnMono()
        {
            Type t = Type.GetType("Mono.Runtime");
            return (t != null);
        }
    }
}
