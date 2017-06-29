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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:07:15 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.ProjectedCategories
{
    /// <summary>
    /// UtmNad1927
    /// </summary>
    public class UtmNad1927 : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo NAD1927UTMZone10N;
        public readonly ProjectionInfo NAD1927UTMZone11N;
        public readonly ProjectionInfo NAD1927UTMZone12N;
        public readonly ProjectionInfo NAD1927UTMZone13N;
        public readonly ProjectionInfo NAD1927UTMZone14N;
        public readonly ProjectionInfo NAD1927UTMZone15N;
        public readonly ProjectionInfo NAD1927UTMZone16N;
        public readonly ProjectionInfo NAD1927UTMZone17N;
        public readonly ProjectionInfo NAD1927UTMZone18N;
        public readonly ProjectionInfo NAD1927UTMZone19N;
        public readonly ProjectionInfo NAD1927UTMZone1N;
        public readonly ProjectionInfo NAD1927UTMZone20N;
        public readonly ProjectionInfo NAD1927UTMZone21N;
        public readonly ProjectionInfo NAD1927UTMZone22N;
        public readonly ProjectionInfo NAD1927UTMZone2N;
        public readonly ProjectionInfo NAD1927UTMZone3N;
        public readonly ProjectionInfo NAD1927UTMZone4N;
        public readonly ProjectionInfo NAD1927UTMZone59N;
        public readonly ProjectionInfo NAD1927UTMZone5N;
        public readonly ProjectionInfo NAD1927UTMZone60N;
        public readonly ProjectionInfo NAD1927UTMZone6N;
        public readonly ProjectionInfo NAD1927UTMZone7N;
        public readonly ProjectionInfo NAD1927UTMZone8N;
        public readonly ProjectionInfo NAD1927UTMZone9N;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of UtmNad1927
        /// </summary>
        public UtmNad1927()
        {
            NAD1927UTMZone10N = ProjectionInfo.FromProj4String("+proj=utm +zone=10 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone11N = ProjectionInfo.FromProj4String("+proj=utm +zone=11 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone12N = ProjectionInfo.FromProj4String("+proj=utm +zone=12 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone13N = ProjectionInfo.FromProj4String("+proj=utm +zone=13 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone14N = ProjectionInfo.FromProj4String("+proj=utm +zone=14 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone15N = ProjectionInfo.FromProj4String("+proj=utm +zone=15 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone16N = ProjectionInfo.FromProj4String("+proj=utm +zone=16 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone17N = ProjectionInfo.FromProj4String("+proj=utm +zone=17 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone18N = ProjectionInfo.FromProj4String("+proj=utm +zone=18 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone19N = ProjectionInfo.FromProj4String("+proj=utm +zone=19 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone1N = ProjectionInfo.FromProj4String("+proj=utm +zone=1 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone20N = ProjectionInfo.FromProj4String("+proj=utm +zone=20 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone21N = ProjectionInfo.FromProj4String("+proj=utm +zone=21 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone22N = ProjectionInfo.FromProj4String("+proj=utm +zone=22 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone2N = ProjectionInfo.FromProj4String("+proj=utm +zone=2 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone3N = ProjectionInfo.FromProj4String("+proj=utm +zone=3 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone4N = ProjectionInfo.FromProj4String("+proj=utm +zone=4 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone59N = ProjectionInfo.FromProj4String("+proj=utm +zone=59 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone5N = ProjectionInfo.FromProj4String("+proj=utm +zone=5 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone60N = ProjectionInfo.FromProj4String("+proj=utm +zone=60 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone6N = ProjectionInfo.FromProj4String("+proj=utm +zone=6 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone7N = ProjectionInfo.FromProj4String("+proj=utm +zone=7 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone8N = ProjectionInfo.FromProj4String("+proj=utm +zone=8 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone9N = ProjectionInfo.FromProj4String("+proj=utm +zone=9 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");

            NAD1927UTMZone10N.Name = "NAD_1927_UTM_Zone_10N";
            NAD1927UTMZone11N.Name = "NAD_1927_UTM_Zone_11N";
            NAD1927UTMZone12N.Name = "NAD_1927_UTM_Zone_12N";
            NAD1927UTMZone13N.Name = "NAD_1927_UTM_Zone_13N";
            NAD1927UTMZone14N.Name = "NAD_1927_UTM_Zone_14N";
            NAD1927UTMZone15N.Name = "NAD_1927_UTM_Zone_15N";
            NAD1927UTMZone16N.Name = "NAD_1927_UTM_Zone_16N";
            NAD1927UTMZone17N.Name = "NAD_1927_UTM_Zone_17N";
            NAD1927UTMZone18N.Name = "NAD_1927_UTM_Zone_18N";
            NAD1927UTMZone19N.Name = "NAD_1927_UTM_Zone_19N";
            NAD1927UTMZone1N.Name = "NAD_1927_UTM_Zone_1N";
            NAD1927UTMZone20N.Name = "NAD_1927_UTM_Zone_20N";
            NAD1927UTMZone21N.Name = "NAD_1927_UTM_Zone_21N";
            NAD1927UTMZone22N.Name = "NAD_1927_UTM_Zone_22N";
            NAD1927UTMZone2N.Name = "NAD_1927_UTM_Zone_2N";
            NAD1927UTMZone3N.Name = "NAD_1927_UTM_Zone_3N";
            NAD1927UTMZone4N.Name = "NAD_1927_UTM_Zone_4N";
            NAD1927UTMZone59N.Name = "NAD_1927_UTM_Zone_59N";
            NAD1927UTMZone5N.Name = "NAD_1927_UTM_Zone_5N";
            NAD1927UTMZone60N.Name = "NAD_1927_UTM_Zone_60N";
            NAD1927UTMZone6N.Name = "NAD_1927_UTM_Zone_6N";
            NAD1927UTMZone7N.Name = "NAD_1927_UTM_Zone_7N";
            NAD1927UTMZone8N.Name = "NAD_1927_UTM_Zone_8N";
            NAD1927UTMZone9N.Name = "NAD_1927_UTM_Zone_9N";

            NAD1927UTMZone10N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone11N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone12N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone13N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone14N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone15N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone16N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone17N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone18N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone19N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone1N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone20N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone21N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone22N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone2N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone3N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone4N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone59N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone5N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone60N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone6N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone7N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone8N.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927UTMZone9N.GeographicInfo.Name = "GCS_North_American_1927";

            NAD1927UTMZone10N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone11N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone12N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone13N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone14N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone15N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone16N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone17N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone18N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone19N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone1N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone20N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone21N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone22N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone2N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone3N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone4N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone59N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone5N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone60N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone6N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone7N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone8N.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927UTMZone9N.GeographicInfo.Datum.Name = "D_North_American_1927";
        }

        #endregion
    }
}

#pragma warning restore 1591