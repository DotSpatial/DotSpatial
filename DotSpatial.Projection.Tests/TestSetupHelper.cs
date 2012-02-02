using System;
using System.Configuration;
using System.IO;

namespace DotSpatial.Projection.Tests
{
    public static class TestSetupHelper
    {
        public static void CopyProj4()
        {
            //var asr = new AppSettingsReader();
            //var proj4Path = (string)asr.GetValue("Proj4Path", typeof (string));
            //if (string.IsNullOrEmpty(proj4Path) || !File.Exists(proj4Path))
            //    throw new ApplicationException("You must set a valid path to proj.dll in app.config");
            //if (!File.Exists("proj.dll") && File.Exists(proj4Path))
            //    File.Copy(proj4Path, "proj.dll");
        }
    }
}