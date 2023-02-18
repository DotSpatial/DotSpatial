// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests
{
    static class FunctionLoader
    {
        [DllImport("Kernel32.dll")]
        private static extern IntPtr LoadLibrary(string path);

        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        public static Delegate LoadFunction<T>(string dllPath, string functionName)
        {
            IntPtr hModule = LoadLibrary(dllPath);
            IntPtr functionAddress = GetProcAddress(hModule, functionName);
            return Marshal.GetDelegateForFunctionPointer(functionAddress, typeof(T));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Proj4
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr pj_init_plus_delegate(string init);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void pj_free_delegate(IntPtr pointer);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int pj_transform_delegate(IntPtr srcPrj, IntPtr destPrj, int nPoints, int offset, ref double x, ref double y, ref double z);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int pj_is_latlong_delegate(IntPtr coordPointer);

        private static readonly pj_init_plus_delegate _pj_init_plus;
        private static readonly pj_free_delegate _pj_free;
        private static readonly pj_transform_delegate _pj_transform;
        private static readonly pj_is_latlong_delegate _pj_is_latlong;

        static Proj4()
        {
            // init proj delegates
            string basePath = (IntPtr.Size == 8) ? "x64" : "x86";
            string path = System.IO.Path.Combine(Common.AbsolutePath(basePath), "proj.dll");

            _pj_init_plus = (pj_init_plus_delegate)FunctionLoader.LoadFunction<pj_init_plus_delegate>(path, "pj_init_plus");
            _pj_free = (pj_free_delegate)FunctionLoader.LoadFunction<pj_free_delegate>(path, "pj_free");
            _pj_transform = (pj_transform_delegate)FunctionLoader.LoadFunction<pj_transform_delegate>(path, "pj_transform");
            _pj_is_latlong = (pj_is_latlong_delegate)FunctionLoader.LoadFunction<pj_is_latlong_delegate>(path, "pj_is_latlong");
        }

        private const double RAD_TO_DEG = 57.2957795130823;
        private const double DEG_TO_RAD = 0.0174532925199433;
        private bool _convertToDegrees;
        private bool _convertToRadians;
        private IntPtr _srcPrj;
        private IntPtr _destPrj;

        /// <summary>
        /// Projects a 2D point from one coordinate system to another
        /// using Proj.4 function: pj_transform.
        /// </summary>
        /// <param name="x">X value of point to project.</param>
        /// <param name="y">Y value of point to project.</param>
        /// <param name="srcPrj4String">Source projection string (proj4 format)</param>
        /// <param name="destPrj4String">Destination projection string (proj4 format)</param>
        public void ProjectPoint(ref double x, ref double y, string srcPrj4String, string destPrj4String)
        {
            try
            {
                InitializeGlobalVariables(srcPrj4String, destPrj4String);
                ConvertAndProjectPoint(ref x, ref y);
            }
            finally
            {
                FreeProjPointers();
            }
        }

        /// <summary>
        /// Sets the global projection variables: srcPrj, destPrj, convertToRadians, and convertToDegrees.
        /// </summary>
        /// <param name="sourceProj">The proj4 source projection.</param>
        /// <param name="destProj">The proj4 destination projection.</param>
        private void InitializeGlobalVariables(string sourceProj, string destProj)
        {
            _destPrj = _pj_init_plus(destProj);
            Assert.True(_destPrj != IntPtr.Zero, "Could not initialize proj.dll for output: At least one parameter in the destination projection is incorrect. Projection: " + destProj);

            _convertToDegrees = _pj_is_latlong(_destPrj) != 0;

            _srcPrj = _pj_init_plus(sourceProj);
            Assert.True(_srcPrj != IntPtr.Zero, "Could not initialize proj.dll for output: At least one parameter in the source projection is incorrect. Projection: " + sourceProj);

            _convertToRadians = _pj_is_latlong(_srcPrj) != 0;
        }

        /// <summary>
        /// Projects a 2D point from one coordinate system to another
        /// using Proj.4 function: pj_transform.
        /// This function assumes that the caller has already checked to see
        /// if conversion to radians or degrees is necessary, then performs
        /// the necessary conversion before or after projecting the point.
        /// </summary>
        /// <param name="x">X value of point to project.</param>
        /// <param name="y">Y value of point to project.</param>
        private void ConvertAndProjectPoint(ref double x, ref double y)
        {
            if (_convertToRadians)
            {
                x *= DEG_TO_RAD;
                y *= DEG_TO_RAD;
            }

            double z = 0;
            int retcode = _pj_transform(_srcPrj, _destPrj, 1, 0, ref x, ref y, ref z);
            if (retcode != 0)
            {
                throw new ProjectionException(retcode);
            }

            if (!_convertToDegrees)
            {
                return;
            }

            x *= RAD_TO_DEG;
            y *= RAD_TO_DEG;
        }

        /// <summary>
        /// Releases memory used by the srcPrj and destPrj structures.
        /// </summary>
        private void FreeProjPointers()
        {
            if (_srcPrj != IntPtr.Zero)
            {
                try
                {
                    _pj_free(_srcPrj);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

                _srcPrj = IntPtr.Zero;
            }

            if (_destPrj != IntPtr.Zero)
            {
                try
                {
                    _pj_free(_destPrj);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

                _destPrj = IntPtr.Zero;
            }
        }
    }
}