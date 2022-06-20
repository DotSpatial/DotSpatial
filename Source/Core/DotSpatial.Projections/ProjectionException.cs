// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/23/2009 1:44:00 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;

namespace DotSpatial.Projections
{
    public class ProjectionException : Exception
    {
        private readonly int _errorCode;

        /// <summary>
        /// Creates a new projection exception with the appropriate message code
        /// </summary>
        /// <param name="errorCode"></param>
        public ProjectionException(int errorCode)
            : base(GetMessage(errorCode))
        {
            _errorCode = errorCode;
        }

        /// <summary>
        /// Creates a new projection exception with the specified message
        /// </summary>
        /// <param name="message"></param>
        public ProjectionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Gets the error code that was used when this exception was created
        /// </summary>
        public int ErrorCode
        {
            get { return _errorCode; }
        }

        /// <summary>
        /// Returns a string message given the correct numeric code.
        /// </summary>
        /// <param name="projectionCode"></param>
        /// <returns></returns>
        public static string GetMessage(int projectionCode)
        {
            int val = Math.Abs(projectionCode);
            return val switch
            {
                1 => ProjectionMessages.Err1_NoArguments,
                2 => ProjectionMessages.Err2_NoOptions,
                3 => ProjectionMessages.Err3_NoColon,
                4 => ProjectionMessages.Err4_NotNamed,
                5 => ProjectionMessages.Err5_UknownProjection,
                6 => ProjectionMessages.Err6_EffectiveEccentricity1,
                7 => ProjectionMessages.Err7_UknownUnitID,
                8 => ProjectionMessages.Err8_Invalidboolean,
                9 => ProjectionMessages.Err9_UknownEllipse,
                10 => ProjectionMessages.Err10_ZeroRecFlattening,
                11 => ProjectionMessages.Err11_LatitudeOutOfBounds,
                12 => ProjectionMessages.Err12_ESquareNegative,
                13 => ProjectionMessages.Err13_NoMajorRadius,
                14 => ProjectionMessages.Err14_LatLonOutOfBounds,
                15 => ProjectionMessages.Err15_InvalidXY,
                16 => ProjectionMessages.Err16_ImproperDMS,
                17 => ProjectionMessages.Err17_NonConvergentRMeridDist,
                18 => ProjectionMessages.Err18_NonConvergentRPhi2,
                19 => ProjectionMessages.Err19_TrigException,
                20 => ProjectionMessages.Err20_ToleranceConditionError,
                21 => ProjectionMessages.Err21_ConicLatitudeError,
                22 => ProjectionMessages.Err22_Lat1TooLarge,
                23 => ProjectionMessages.Err23_Lat1TooSmall,
                24 => ProjectionMessages.Err24_Lat_tsTooLarge,
                25 => ProjectionMessages.Err25_NoControlPtSeparation,
                26 => ProjectionMessages.Err26_ProjectionNotRotated,
                27 => ProjectionMessages.Err27_WorMTooSmall,
                28 => ProjectionMessages.Err28_LsatOutOfBounds,
                29 => ProjectionMessages.Err29_PathNotInRange,
                30 => ProjectionMessages.Err30_HTooSmall,
                31 => ProjectionMessages.Err31_KTooSmall,
                32 => ProjectionMessages.Err32_LatOutOfBounds,
                33 => ProjectionMessages.Err33_InvalidLatitudes,
                34 => ProjectionMessages.Err34_EllipticalRequired,
                35 => ProjectionMessages.Err35_InvalidUTMZone,
                36 => ProjectionMessages.Err36_TchebyException,
                37 => ProjectionMessages.Err37_ProjNotFound,
                38 => ProjectionMessages.Err38_CorrectionNotFound,
                39 => ProjectionMessages.Err39_NorMnotSpecified,
                40 => ProjectionMessages.Err40_InvalidN,
                41 => ProjectionMessages.Err41_Lat1OrLat2Missing,
                42 => ProjectionMessages.Err42_Lat1EqualsLat2,
                43 => ProjectionMessages.Err43_MeanLatError,
                44 => ProjectionMessages.Err44_CoordinateUnreadable,
                45 => ProjectionMessages.Err45_GeocentricMissingZ,
                46 => ProjectionMessages.Err46_UknownPMID,
                _ => "Unspecified",
            };
        }
    }
}