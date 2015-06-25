using System;
using MonoMac.AppKit;

namespace DotSpatial.Mono
{
    public static class Mono
    {
        public static bool IsRunningOnMono()
        {
            Type t = Type.GetType("Mono.Runtime");
            return (t != null);
        }

        public static bool IsRunningOnMonoMac()
        {
            try
            {
                var initTest = NSApplication.SharedApplication;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
