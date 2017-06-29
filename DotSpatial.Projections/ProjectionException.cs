// ********************************************************************************************************
// Product Name: DotSpatial.Projection
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
    /// <summary>
    /// ProjectionErrorCodes
    /// </summary>
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
            switch (val)
            {
                case 1:
                    return ProjectionMessages.Err1_NoArguments;
                case 2:
                    return ProjectionMessages.Err2_NoOptions;
                case 3:
                    return ProjectionMessages.Err3_NoColon;
                case 4:
                    return ProjectionMessages.Err4_NotNamed;
                case 5:
                    return ProjectionMessages.Err5_UknownProjection;
                case 6:
                    return ProjectionMessages.Err6_EffectiveEccentricity1;
                case 7:
                    return ProjectionMessages.Err7_UknownUnitID;
                case 8:
                    return ProjectionMessages.Err8_Invalidboolean;
                case 9:
                    return ProjectionMessages.Err9_UknownEllipse;
                case 10:
                    return ProjectionMessages.Err10_ZeroRecFlattening;
                case 11:
                    return ProjectionMessages.Err11_LatitudeOutOfBounds;
                case 12:
                    return ProjectionMessages.Err12_ESquareNegative;
                case 13:
                    return ProjectionMessages.Err13_NoMajorRadius;
                case 14:
                    return ProjectionMessages.Err14_LatLonOutOfBounds;
                case 15:
                    return ProjectionMessages.Err15_InvalidXY;
                case 16:
                    return ProjectionMessages.Err16_ImproperDMS;
                case 17:
                    return ProjectionMessages.Err17_NonConvergentRMeridDist;
                case 18:
                    return ProjectionMessages.Err18_NonConvergentRPhi2;
                case 19:
                    return ProjectionMessages.Err19_TrigException;
                case 20:
                    return ProjectionMessages.Err20_ToleranceConditionError;
                case 21:
                    return ProjectionMessages.Err21_ConicLatitudeError;
                case 22:
                    return ProjectionMessages.Err22_Lat1TooLarge;
                case 23:
                    return ProjectionMessages.Err23_Lat1TooSmall;
                case 24:
                    return ProjectionMessages.Err24_Lat_tsTooLarge;
                case 25:
                    return ProjectionMessages.Err25_NoControlPtSeparation;
                case 26:
                    return ProjectionMessages.Err26_ProjectionNotRotated;
                case 27:
                    return ProjectionMessages.Err27_WorMTooSmall;
                case 28:
                    return ProjectionMessages.Err28_LsatOutOfBounds;
                case 29:
                    return ProjectionMessages.Err29_PathNotInRange;
                case 30:
                    return ProjectionMessages.Err30_HTooSmall;
                case 31:
                    return ProjectionMessages.Err31_KTooSmall;
                case 32:
                    return ProjectionMessages.Err32_LatOutOfBounds;
                case 33:
                    return ProjectionMessages.Err33_InvalidLatitudes;
                case 34:
                    return ProjectionMessages.Err34_EllipticalRequired;
                case 35:
                    return ProjectionMessages.Err35_InvalidUTMZone;
                case 36:
                    return ProjectionMessages.Err36_TchebyException;
                case 37:
                    return ProjectionMessages.Err37_ProjNotFound;
                case 38:
                    return ProjectionMessages.Err38_CorrectionNotFound;
                case 39:
                    return ProjectionMessages.Err39_NorMnotSpecified;
                case 40:
                    return ProjectionMessages.Err40_InvalidN;
                case 41:
                    return ProjectionMessages.Err41_Lat1OrLat2Missing;
                case 42:
                    return ProjectionMessages.Err42_Lat1EqualsLat2;
                case 43:
                    return ProjectionMessages.Err43_MeanLatError;
                case 44:
                    return ProjectionMessages.Err44_CoordinateUnreadable;
                case 45:
                    return ProjectionMessages.Err45_GeocentricMissingZ;
                case 46:
                    return ProjectionMessages.Err46_UknownPMID;
            }
            return "Unspecified";
        }
    }
}