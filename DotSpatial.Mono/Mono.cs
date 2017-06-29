using System;

namespace DotSpatial.Mono
{
    public static class Mono
    {
        public static bool IsRunningOnMono()
        {
            Type t = Type.GetType("Mono.Runtime");
            return (t != null);
        }
    }
}
