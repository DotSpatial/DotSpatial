
//using System;
//using GeoAPI.Geometries;

//namespace DotSpatial.NTSExtension
//{
//   public class Envelope : GeoAPI.Geometries.Envelope
//    {

//        /// <summary>
//        /// Creates an <c>Envelope</c> for a region defined by maximum and minimum values.
//        /// </summary>
//        /// <param name="x1">The first x-value.</param>
//        /// <param name="x2">The second x-value.</param>
//        /// <param name="y1">The first y-value.</param>
//        /// <param name="y2">The second y-value.</param>
//        /// <param name="z1">The first z-value.</param>
//        /// <param name="z2">The second z-value.</param>
//        /// <param name="m1"></param>
//        /// <param name="m2"/>
//        public Envelope(double x1, double x2, double y1, double y2, double z1 = double.NaN, double z2 = double.NaN, double m1 = double.NaN, double m2 = double.NaN)
//            : base(x1,x2,y1,y2)
//        {
//            MinM = Math.Min(m1, m2);
//            MaxM = Math.Max(m1, m2);
//            MinZ = Math.Min(z1, z2);
//            MaxZ = Math.Max(z1, z2);
//        }

//        private double _minM = double.NaN, _minZ = double.NaN, _maxM = double.NaN, _maxZ = double.NaN;
//        public double MinM
//        {
//            get { return _minM; }
//            set { _minM = value; }
//        }
//        public double MaxM
//        {
//            get { return _maxM; }
//            set { _maxM = value; }
//        }

//        public double MinZ
//        {
//            get { return _minZ; }
//            set { _minZ = value; }
//        }
//        public double MaxZ
//        {
//            get { return _maxZ; }
//            set { _maxZ = value; }
//        }

//        /// <summary>
//        /// True only if the M coordinates are not NaN and the max is greater than the min.
//        /// </summary>
//        /// <returns>Boolean</returns>
//        public bool HasM()
//        {
//            if (double.IsNaN(MinM) || double.IsNaN(MaxM)) return false;
//            return !(MinM > MaxM);
//        }

//        /// <summary>
//        /// True only of the Z ordinates are not NaN and the max is greater than the min.
//        /// </summary>
//        /// <returns>Boolean</returns>
//        public bool HasZ()
//        {
//            if (double.IsNaN(MinZ) || double.IsNaN(MaxZ)) return false;
//            return !(MinZ > MaxZ);
//        }


//    }
//}
