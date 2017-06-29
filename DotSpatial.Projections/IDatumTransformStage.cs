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
namespace DotSpatial.Projections
{
    /// <summary>
    /// DatumTransformStage interface
    /// </summary>
    public interface IDatumTransformStage
    {
        /// <summary>
        /// String identifier of the datum from which the transform proceeds
        /// </summary>
        string FromDatum { get; }

        /// <summary>
        /// String identifier of the datum to which the transform proceeds
        /// </summary>
        string ToDatum { get; }

        /// <summary>
        /// Transform method; one of: GeoCentric, Coordinate_Frame, Position_Vector, NADCON, HARN, NTv2
        /// </summary>
        TransformMethod Method { get; }

        /// <summary>
        /// Detla X of geocentric origin, in meters
        /// </summary>
        double DeltaX { get; }

        /// <summary>
        /// Delta Y of geocentric origin, in meters
        /// </summary>
        double DeltaY { get; }

        /// <summary>
        /// Delta Z of geocentric origin, in meters
        /// </summary>
        double DeltaZ { get; }

        /// <summary>
        /// Rotation about the geocentric X-axis, in arc-seconds
        /// </summary>
        double RotateX { get; }

        /// <summary>
        /// Rotation about the geocentric Y-axis, in arc-seconds
        /// </summary>
        double RotateY { get; }

        /// <summary>
        /// Rotation about the geocentric Z-axis, in arc-seconds
        /// </summary>
        double RotateZ { get; }

        /// <summary>
        /// Delta scale in parts-per-million. Actaul scale is 1+DS/1000000.
        /// </summary>
        double DeltaScale { get; }

        /// <summary>
        /// If Method is one of: NADCON, HARN, or NTv2, this field contains the name of the grid shift table
        /// </summary>
        string GridShiftTable { get; }

        /// <summary>
        /// True if the table must be used in the direction opposite that specified by the From/To Datum properties
        /// </summary>
        bool ApplyTableInverse { get; }

        /// <summary>
        /// Spheriod on which the From datum is based
        /// </summary>
        Spheroid FromSpheroid { get; }

        /// <summary>
        /// Spheriod on which the To datum is based
        /// </summary>
        Spheroid ToSpheroid { get; }
    }
}