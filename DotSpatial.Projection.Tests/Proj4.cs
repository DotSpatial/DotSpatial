using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using NUnit.Framework;
public class Proj4
{
    //Because Proj.4 is not a COM component, we must declare each function
    //that we will be using from the external library.
    //proj.dll must be included in the bin directory of the calling program
    //the nad initialization folder should also be present on the computer.
    [DllImport("proj.dll", CharSet = CharSet.Ansi)]
    private static extern IntPtr pj_init_plus(string init);
    [DllImport("proj.dll")]
    private static extern void pj_free(IntPtr pointer);
    [DllImport("proj.dll")]
    private static extern int pj_transform(IntPtr srcPrj, IntPtr destPrj, int nPoints, int offset, ref double x, ref double y, ref double z);
    [DllImport("proj.dll")]
    private static extern int pj_is_latlong(IntPtr coordPointer);

    private const double RAD_TO_DEG = 57.2957795130823;
    private const double DEG_TO_RAD = 0.0174532925199433;
    private static bool _convertToDegrees;
    private static bool _convertToRadians;
    private static IntPtr _srcPrj;
    private static IntPtr _destPrj;

    /// <summary>
    /// Projects a 2D point from one coordinate system to another
    /// using Proj.4 function: pj_transform.
    /// </summary>
    /// <param name="x">X value of point to project.</param>
    /// <param name="y">Y value of point to project.</param>
    /// <param name="srcPrj4String">Source projection string (proj4 format)</param>
    /// <param name="destPrj4String">Destination projection string (proj4 format)</param>
    public static void ProjectPoint(ref double x, ref double y, string srcPrj4String, string destPrj4String)
    {
        try
        {
            InitializeGlobalVariables(ref srcPrj4String, ref destPrj4String);
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
    private static void InitializeGlobalVariables(ref string sourceProj, ref string destProj)
    {
        _destPrj = pj_init_plus(destProj);
        Assert.True(_destPrj != IntPtr.Zero, "Could not initialize proj.dll for output: At least one parameter in the destination projection is incorrect. Projection: " + destProj);

        _convertToDegrees = pj_is_latlong(_destPrj) != 0;

        _srcPrj = pj_init_plus(sourceProj);
        Assert.True(_srcPrj != IntPtr.Zero, "Could not initialize proj.dll for output: At least one parameter in the source projection is incorrect. Projection: " + sourceProj);

        _convertToRadians = pj_is_latlong(_srcPrj) != 0;
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
    private static void ConvertAndProjectPoint(ref double x, ref double y)
    {
        if (_convertToRadians)
        {
            x *= DEG_TO_RAD;
            y *= DEG_TO_RAD;
        }
        double z = 0;
        int retcode = pj_transform(_srcPrj, _destPrj, 1, 0, ref x, ref y, ref z);
        if (retcode != 0)
        {
            throw new DotSpatial.Projections.ProjectionException(retcode);
        }
        if (!_convertToDegrees) return;
        x *= RAD_TO_DEG;
        y *= RAD_TO_DEG;
    }


    /// <summary>
    /// Releases memory used by the srcPrj and destPrj structures.
    /// </summary>
    private static void FreeProjPointers()
    {
        if (_srcPrj != IntPtr.Zero)
        {
            try
            {
                pj_free(_srcPrj);
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
                pj_free(_destPrj);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            _destPrj = IntPtr.Zero;
        }
    }

    
}

