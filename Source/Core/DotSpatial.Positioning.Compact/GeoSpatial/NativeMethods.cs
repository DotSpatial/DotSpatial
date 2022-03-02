#if PocketPC

using System;
using System.Text;
using System.Runtime.InteropServices;

namespace GeoFramework
{
    internal static class NativeMethods
    {
        #region Host Platform Determination

        [DllImport("coredll.dll", EntryPoint = "SystemParametersInfo", SetLastError = true)]
        public extern static Boolean GetSystemParameterString(uint sysParam, UInt32 bufferSize, StringBuilder stringBuffer, Boolean updateWinIni);

        #endregion
    }
}

#endif
