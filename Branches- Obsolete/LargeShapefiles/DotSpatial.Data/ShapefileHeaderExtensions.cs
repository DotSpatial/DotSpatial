using System;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    public static class ShapefileHeaderExtensions
    {
        /// <summary>
        /// Generates a new envelope based on the extents of this shapefile.
        /// </summary>
        /// <returns>An Envelope</returns>
        public static IEnvelope ToEnvelope(this ShapefileHeader header)
        {
            if (header == null) throw new ArgumentNullException("header");

            var env = new Envelope(header.Xmin, header.Xmax, header.Ymin, header.Ymax, header.Zmin, header.Zmax);
            env.Minimum.M = header.Mmin;
            env.Maximum.M = header.Mmax;
            return env;
        }

        /// <summary>
        /// Generates a new extent from the shape header.  This will infer the whether the ExtentMZ, ExtentM
        /// or Extent class is the best implementation.  Casting is required to access the higher
        /// values from the Extent return type.
        /// </summary>
        /// <returns>Extent, which can be Extent, ExtentM, or ExtentMZ</returns>
        public static Extent ToExtent(this ShapefileHeader header)
        {
            if (header == null) throw new ArgumentNullException("header");

            if (header.ShapeType == ShapeType.MultiPointZ ||
                header.ShapeType == ShapeType.PointZ ||
                header.ShapeType == ShapeType.PolygonZ ||
                header.ShapeType == ShapeType.PolyLineZ)
            {
                return new ExtentMZ(header.Xmin, header.Ymin, header.Mmin, header.Zmin, header.Xmax, header.Ymax, header.Mmax, header.Zmax);
            }
            if (header.ShapeType == ShapeType.MultiPointM ||
                header.ShapeType == ShapeType.PointM ||
                header.ShapeType == ShapeType.PolygonM ||
                header.ShapeType == ShapeType.PolyLineM)
            {
                return new ExtentM(header.Xmin, header.Ymin, header.Mmin, header.Xmax, header.Ymax, header.Mmax);
            }
            return new Extent(header.Xmin, header.Ymin, header.Xmax, header.Ymax);

        }

        /// <summary>
        /// The shape type is assumed to be fixed, and will control how the input extent is treated as far
        /// as M and Z values, rather than updating the shape type based on the extent.
        /// </summary>
        public static void SetExtent(this ShapefileHeader header, IExtent extent)
        {
            IExtentZ zExt = extent as ExtentMZ;
            IExtentM mExt = extent as ExtentM;
            if ((header.ShapeType == ShapeType.MultiPointZ ||
                 header.ShapeType == ShapeType.PointZ ||
                 header.ShapeType == ShapeType.PolygonZ ||
                 header.ShapeType == ShapeType.PolyLineZ))
            {
                if (zExt == null || extent.HasZ == false)
                {
                    header.Zmin = double.MaxValue;
                    header.Zmax = double.MinValue;
                }
                else
                {
                    header.Zmin = zExt.MinZ;
                    header.Zmax = zExt.MaxZ;
                }
            }
            if (header.ShapeType == ShapeType.MultiPointM ||
                header.ShapeType == ShapeType.PointM ||
                header.ShapeType == ShapeType.PolygonM ||
                header.ShapeType == ShapeType.PolyLineM)
            {
                if (mExt == null || extent.HasM == false)
                {
                    header.Mmin = double.MaxValue;
                    header.Mmax = double.MinValue;
                }
                else
                {
                    header.Mmin = mExt.MinM;
                    header.Mmax = mExt.MaxM;
                }
            }
            header.Xmin = extent.MinX;
            header.Xmax = extent.MaxX;
            header.Ymin = extent.MinY;
            header.Ymax = extent.MaxY;
        }
    }
}