using System;

namespace DotSpatial.Topology.Utilities
{
    /// <summary>
    /// Utility functions to report memory usage.
    /// </summary>
    /// <author>mbdavis</author>
    public class Memory
    {
        #region Constant Fields

        public const double GB = 1073741824;
        public const double KB = 1024;
        public const double MB = 1048576;

        #endregion

        #region Properties

        public static long Total
        {
            get
            {
                return GC.GetTotalMemory(true);
            }
        }

        public static String TotalString
        {
            get { return Format(Total); }
        }

        #endregion

        #region Methods

        public static String Format(long mem)
        {
            if (mem < 2 * KB)
                return mem + " bytes";
            if (mem < 2 * MB)
                return Round(mem / KB) + " KB";
            if (mem < 2 * GB)
                return Round(mem / MB) + " MB";
            return Round(mem / GB) + " GB";
        }

        public static double Round(double d)
        {
            return Math.Ceiling(d * 100) / 100;
        }

        #endregion
    }
}