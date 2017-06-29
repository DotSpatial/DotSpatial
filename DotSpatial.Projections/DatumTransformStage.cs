// ********************************************************************************************************
// Product Name: DotSpatial.Projections
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
//
// The Initial Developer of this Original Code is Steve Riddell. Created 5/27/2011 1:04:00 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// ********************************************************************************************************

using System;

namespace DotSpatial.Projections
{
    /// <summary>
    /// Datum Transform Type: GridShift, Param3, or Param7
    /// </summary>
    public enum TransformMethod
    {
        /// <summary>Grid shift table</summary>
        GridShift,
        /// <summary>3-parameter geocentric transform: delat x, delta y, and delta z</summary>
        Param3,
        /// <summary>7-parameter geocentric tranform: delta x,y,z, plus rotataion of x,y,z axes, and delta scale</summary>
        Param7
    };

    /// <summary>
    /// DatumTransfromStage implementation
    /// </summary>
    [Serializable]
    public class DatumTransformStage : IDatumTransformStage
    {
        /// <summary>True if the table must be used in the direction opposite that specified by the From/To Datum properties</summary>
        protected bool ApplyTableInv;

        /// <summary>Delta scale in parts-per-million. Actaul scale is 1+DS/1000000.</summary>
        protected double Ds;

        /// <summary>Detla X of geocentric origin, in meters</summary>
        protected double Dx;

        /// <summary>Delta Y of geocentric origin, in meters</summary>
        protected double Dy;

        /// <summary>Delta Z of geocentric origin, in meters</summary>
        protected double Dz;

        /// <summary>String identifer of datum from which the transform proceeds</summary>
        protected string From;

        /// <summary>Spheriod on which the From datum is based</summary>
        protected Spheroid FromSph;

        /// <summary>Rotation about the geocentric X-axis, in arc-seconds</summary>
        protected double Rx;

        /// <summary>Rotation about the geocentric Y-axis, in arc-seconds</summary>
        protected double Ry;

        /// <summary>Rotation about the geocentric Z-axis, in arc-seconds</summary>
        protected double Rz;

        /// <summary>If Method is one of: NADCON, HARN, or NTv2, this field contains the name of the grid shift table</summary>
        protected string TableName;

        /// <summary>Transform method; one of: GridShift, Param3, or Param7</summary>
        protected TransformMethod Tm;

        /// <summary>String identifier of the datum to which the transform proceeds</summary>
        protected string To;

        /// <summary>Spheriod on which the To datum is based</summary>
        protected Spheroid ToSph;

        /// <summary>
        /// DatumTranformStage constructor for a geocentric transform
        /// </summary>
        public DatumTransformStage(string strFrom, string strTo, Spheroid sphFrom, Spheroid sphTo, double dx, double dy, double dz)
        {
            From = strFrom;
            To = strTo;
            FromSph = sphFrom;
            ToSph = sphTo;
            Dx = dx;
            Dy = dy;
            Dz = dz;
            Tm = TransformMethod.Param3;
        }

        /// <summary>
        /// Parmeterless constructor
        /// </summary>
        public DatumTransformStage() { }

        /// <summary>
        /// DatumTranformStage constructor for a gridshift transform
        /// </summary>
        public DatumTransformStage(string strFrom, string strTo, Spheroid sphFrom, Spheroid sphTo, string strTableName, bool bApplyTableInv)
        {
            From = strFrom;
            To = strTo;
            FromSph = sphFrom;
            ToSph = sphTo;
            Tm = TransformMethod.GridShift;
            TableName = strTableName;
            ApplyTableInv = bApplyTableInv;
        }

        /// <summary>
        /// DatumTranformStage constructor for a 7-parameter transform
        /// </summary>
        public DatumTransformStage(string strFrom, string strTo, Spheroid sphFrom, Spheroid sphTo, double dx, double dy, double dz, double rx, double ry, double rz, double ds)
        {
            From = strFrom;
            To = strTo;
            FromSph = sphFrom;
            ToSph = sphTo;
            Tm = TransformMethod.Param7;
            Dx = dx;
            Dy = dy;
            Dz = dz;
            Rx = rx;
            Ry = ry;
            Rz = rz;
            Ds = ds;
        }

        /// <summary>
        /// DatumTransfromStage copy constructor
        /// </summary>
        /// <param name="dts"></param>
        public DatumTransformStage(DatumTransformStage dts)
        {
            From = dts.From;
            To = dts.To;
            FromSph = dts.FromSph;
            ToSph = dts.ToSph;
            Tm = dts.Tm;
            Dx = dts.Dx;
            Dy = dts.Dy;
            Dz = dts.Dz;
            Rx = dts.Rx;
            Ry = dts.Ry;
            Rz = dts.Rz;
            Ds = dts.Ds;
            TableName = dts.TableName;
            ApplyTableInv = dts.ApplyTableInv;
        }

        #region IDatumTransformStage Members

        /// <summary>
        /// Source datum
        /// </summary>
        public string FromDatum
        {
            get { return From; }
        }

        /// <summary>
        /// Destination datum
        /// </summary>
        public string ToDatum
        {
            get { return To; }
        }

        /// <summary>
        /// TranformMethod to be used
        /// </summary>
        public TransformMethod Method
        {
            get { return Tm; }
        }

        /// <summary>
        /// geocentric X offset
        /// </summary>
        public double DeltaX
        {
            get { return Dx; }
        }

        /// <summary>
        /// geocentric Y offset
        /// </summary>
        public double DeltaY
        {
            get { return Dy; }
        }

        /// <summary>
        /// geocentric Z offset
        /// </summary>
        public double DeltaZ
        {
            get { return Dz; }
        }

        /// <summary>
        /// rotation about X axis in arc-seconds
        /// </summary>
        public double RotateX
        {
            get { return Rx; }
        }

        /// <summary>
        /// rotation about Y axis in arc-seconds
        /// </summary>
        public double RotateY
        {
            get { return Ry; }
        }

        /// <summary>
        /// rotation about Z axis in arc-seconds
        /// </summary>
        public double RotateZ
        {
            get { return Rz; }
        }

        /// <summary>
        /// delta scale in ppm
        /// </summary>
        public double DeltaScale
        {
            get { return Ds; }
        }

        /// <summary>
        /// if type is one of: NADCON, HARN, or NTv2, GridShiftTable is the file base name
        /// </summary>
        public string GridShiftTable
        {
            get { return TableName; }
        }

        /// <summary>
        /// If false, use inverse of table
        /// </summary>
        public bool ApplyTableInverse
        {
            get { return ApplyTableInv; }
        }

        /// <summary>
        /// Spheroid of source datum
        /// </summary>
        public Spheroid FromSpheroid
        {
            get { return FromSph; }
        }

        /// <summary>
        /// Spheroid of destination datum
        /// </summary>
        public Spheroid ToSpheroid
        {
            get { return ToSph; }
        }

        #endregion
    }
}