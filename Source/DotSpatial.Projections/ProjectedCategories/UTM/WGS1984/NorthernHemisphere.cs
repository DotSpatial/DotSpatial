// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:11:31 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.ProjectedCategories.UTM.WGS1984
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for NorthernHemisphere.
    /// </summary>
    public class NorthernHemisphere : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo WGS1984BLMZone14NUSFeet;
        public readonly ProjectionInfo WGS1984BLMZone15NUSFeet;
        public readonly ProjectionInfo WGS1984BLMZone16NUSFeet;
        public readonly ProjectionInfo WGS1984BLMZone17NUSFeet;
        // public readonly ProjectionInfo WGS1984ComplexUTMZone20N;
        // public readonly ProjectionInfo WGS1984ComplexUTMZone21N;
        // public readonly ProjectionInfo WGS1984ComplexUTMZone22N;
        // public readonly ProjectionInfo WGS1984ComplexUTMZone23N;
        // public readonly ProjectionInfo WGS1984ComplexUTMZone24N;
        // public readonly ProjectionInfo WGS1984ComplexUTMZone25N;
        // public readonly ProjectionInfo WGS1984ComplexUTMZone26N;
        // public readonly ProjectionInfo WGS1984ComplexUTMZone27N;
        // public readonly ProjectionInfo WGS1984ComplexUTMZone28N;
        // public readonly ProjectionInfo WGS1984ComplexUTMZone29N;
        // public readonly ProjectionInfo WGS1984ComplexUTMZone30N;
        public readonly ProjectionInfo WGS1984UTMZone10N;
        public readonly ProjectionInfo WGS1984UTMZone11N;
        public readonly ProjectionInfo WGS1984UTMZone12N;
        public readonly ProjectionInfo WGS1984UTMZone13N;
        public readonly ProjectionInfo WGS1984UTMZone14N;
        public readonly ProjectionInfo WGS1984UTMZone15N;
        public readonly ProjectionInfo WGS1984UTMZone16N;
        public readonly ProjectionInfo WGS1984UTMZone17N;
        public readonly ProjectionInfo WGS1984UTMZone18N;
        public readonly ProjectionInfo WGS1984UTMZone19N;
        public readonly ProjectionInfo WGS1984UTMZone1N;
        public readonly ProjectionInfo WGS1984UTMZone20N;
        public readonly ProjectionInfo WGS1984UTMZone21N;
        public readonly ProjectionInfo WGS1984UTMZone22N;
        public readonly ProjectionInfo WGS1984UTMZone23N;
        public readonly ProjectionInfo WGS1984UTMZone24N;
        public readonly ProjectionInfo WGS1984UTMZone25N;
        public readonly ProjectionInfo WGS1984UTMZone26N;
        public readonly ProjectionInfo WGS1984UTMZone27N;
        public readonly ProjectionInfo WGS1984UTMZone28N;
        public readonly ProjectionInfo WGS1984UTMZone29N;
        public readonly ProjectionInfo WGS1984UTMZone2N;
        public readonly ProjectionInfo WGS1984UTMZone30N;
        public readonly ProjectionInfo WGS1984UTMZone31N;
        public readonly ProjectionInfo WGS1984UTMZone32N;
        public readonly ProjectionInfo WGS1984UTMZone33N;
        public readonly ProjectionInfo WGS1984UTMZone34N;
        public readonly ProjectionInfo WGS1984UTMZone35N;
        public readonly ProjectionInfo WGS1984UTMZone36N;
        public readonly ProjectionInfo WGS1984UTMZone37N;
        public readonly ProjectionInfo WGS1984UTMZone38N;
        public readonly ProjectionInfo WGS1984UTMZone39N;
        public readonly ProjectionInfo WGS1984UTMZone3N;
        public readonly ProjectionInfo WGS1984UTMZone40N;
        public readonly ProjectionInfo WGS1984UTMZone41N;
        public readonly ProjectionInfo WGS1984UTMZone42N;
        public readonly ProjectionInfo WGS1984UTMZone43N;
        public readonly ProjectionInfo WGS1984UTMZone44N;
        public readonly ProjectionInfo WGS1984UTMZone45N;
        public readonly ProjectionInfo WGS1984UTMZone46N;
        public readonly ProjectionInfo WGS1984UTMZone47N;
        public readonly ProjectionInfo WGS1984UTMZone48N;
        public readonly ProjectionInfo WGS1984UTMZone49N;
        public readonly ProjectionInfo WGS1984UTMZone4N;
        public readonly ProjectionInfo WGS1984UTMZone50N;
        public readonly ProjectionInfo WGS1984UTMZone51N;
        public readonly ProjectionInfo WGS1984UTMZone52N;
        public readonly ProjectionInfo WGS1984UTMZone53N;
        public readonly ProjectionInfo WGS1984UTMZone54N;
        public readonly ProjectionInfo WGS1984UTMZone55N;
        public readonly ProjectionInfo WGS1984UTMZone56N;
        public readonly ProjectionInfo WGS1984UTMZone57N;
        public readonly ProjectionInfo WGS1984UTMZone58N;
        public readonly ProjectionInfo WGS1984UTMZone59N;
        public readonly ProjectionInfo WGS1984UTMZone5N;
        public readonly ProjectionInfo WGS1984UTMZone60N;
        public readonly ProjectionInfo WGS1984UTMZone6N;
        public readonly ProjectionInfo WGS1984UTMZone7N;
        public readonly ProjectionInfo WGS1984UTMZone8N;
        public readonly ProjectionInfo WGS1984UTMZone9N;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NorthernHemisphere.
        /// </summary>
        public NorthernHemisphere()
        {
            WGS1984BLMZone14NUSFeet = ProjectionInfo.FromEpsgCode(32664).SetNames("WGS_1984_BLM_Zone_14N_ftUS", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984BLMZone15NUSFeet = ProjectionInfo.FromEpsgCode(32665).SetNames("WGS_1984_BLM_Zone_15N_ftUS", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984BLMZone16NUSFeet = ProjectionInfo.FromEpsgCode(32666).SetNames("WGS_1984_BLM_Zone_16N_ftUS", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984BLMZone17NUSFeet = ProjectionInfo.FromEpsgCode(32667).SetNames("WGS_1984_BLM_Zone_17N_ftUS", "GCS_WGS_1984", "D_WGS_1984");
            // WGS1984ComplexUTMZone20N = ProjectionInfo.FromAuthorityCode("ESRI", 102570).SetNames("WGS_1984_Complex_UTM_Zone_20N", "GCS_WGS_1984", "D_WGS_1984"); // projection not found
            // WGS1984ComplexUTMZone21N = ProjectionInfo.FromAuthorityCode("ESRI", 102571).SetNames("WGS_1984_Complex_UTM_Zone_21N", "GCS_WGS_1984", "D_WGS_1984"); // projection not found
            // WGS1984ComplexUTMZone22N = ProjectionInfo.FromAuthorityCode("ESRI", 102572).SetNames("WGS_1984_Complex_UTM_Zone_22N", "GCS_WGS_1984", "D_WGS_1984"); // projection not found
            // WGS1984ComplexUTMZone23N = ProjectionInfo.FromAuthorityCode("ESRI", 102573).SetNames("WGS_1984_Complex_UTM_Zone_23N", "GCS_WGS_1984", "D_WGS_1984"); // projection not found
            // WGS1984ComplexUTMZone24N = ProjectionInfo.FromAuthorityCode("ESRI", 102574).SetNames("WGS_1984_Complex_UTM_Zone_24N", "GCS_WGS_1984", "D_WGS_1984"); // projection not found
            // WGS1984ComplexUTMZone25N = ProjectionInfo.FromAuthorityCode("ESRI", 102575).SetNames("WGS_1984_Complex_UTM_Zone_25N", "GCS_WGS_1984", "D_WGS_1984"); // projection not found
            // WGS1984ComplexUTMZone26N = ProjectionInfo.FromAuthorityCode("ESRI", 102576).SetNames("WGS_1984_Complex_UTM_Zone_26N", "GCS_WGS_1984", "D_WGS_1984"); // projection not found
            // WGS1984ComplexUTMZone27N = ProjectionInfo.FromAuthorityCode("ESRI", 102577).SetNames("WGS_1984_Complex_UTM_Zone_27N", "GCS_WGS_1984", "D_WGS_1984"); // projection not found
            // WGS1984ComplexUTMZone28N = ProjectionInfo.FromAuthorityCode("ESRI", 102578).SetNames("WGS_1984_Complex_UTM_Zone_28N", "GCS_WGS_1984", "D_WGS_1984"); // projection not found
            // WGS1984ComplexUTMZone29N = ProjectionInfo.FromAuthorityCode("ESRI", 102579).SetNames("WGS_1984_Complex_UTM_Zone_29N", "GCS_WGS_1984", "D_WGS_1984"); // projection not found
            // WGS1984ComplexUTMZone30N = ProjectionInfo.FromAuthorityCode("ESRI", 102580).SetNames("WGS_1984_Complex_UTM_Zone_30N", "GCS_WGS_1984", "D_WGS_1984"); // projection not found
            WGS1984UTMZone10N = ProjectionInfo.FromEpsgCode(32610).SetNames("WGS_1984_UTM_Zone_10N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone11N = ProjectionInfo.FromEpsgCode(32611).SetNames("WGS_1984_UTM_Zone_11N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone12N = ProjectionInfo.FromEpsgCode(32612).SetNames("WGS_1984_UTM_Zone_12N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone13N = ProjectionInfo.FromEpsgCode(32613).SetNames("WGS_1984_UTM_Zone_13N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone14N = ProjectionInfo.FromEpsgCode(32614).SetNames("WGS_1984_UTM_Zone_14N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone15N = ProjectionInfo.FromEpsgCode(32615).SetNames("WGS_1984_UTM_Zone_15N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone16N = ProjectionInfo.FromEpsgCode(32616).SetNames("WGS_1984_UTM_Zone_16N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone17N = ProjectionInfo.FromEpsgCode(32617).SetNames("WGS_1984_UTM_Zone_17N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone18N = ProjectionInfo.FromEpsgCode(32618).SetNames("WGS_1984_UTM_Zone_18N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone19N = ProjectionInfo.FromEpsgCode(32619).SetNames("WGS_1984_UTM_Zone_19N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone1N = ProjectionInfo.FromEpsgCode(32601).SetNames("WGS_1984_UTM_Zone_1N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone20N = ProjectionInfo.FromEpsgCode(32620).SetNames("WGS_1984_UTM_Zone_20N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone21N = ProjectionInfo.FromEpsgCode(32621).SetNames("WGS_1984_UTM_Zone_21N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone22N = ProjectionInfo.FromEpsgCode(32622).SetNames("WGS_1984_UTM_Zone_22N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone23N = ProjectionInfo.FromEpsgCode(32623).SetNames("WGS_1984_UTM_Zone_23N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone24N = ProjectionInfo.FromEpsgCode(32624).SetNames("WGS_1984_UTM_Zone_24N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone25N = ProjectionInfo.FromEpsgCode(32625).SetNames("WGS_1984_UTM_Zone_25N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone26N = ProjectionInfo.FromEpsgCode(32626).SetNames("WGS_1984_UTM_Zone_26N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone27N = ProjectionInfo.FromEpsgCode(32627).SetNames("WGS_1984_UTM_Zone_27N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone28N = ProjectionInfo.FromEpsgCode(32628).SetNames("WGS_1984_UTM_Zone_28N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone29N = ProjectionInfo.FromEpsgCode(32629).SetNames("WGS_1984_UTM_Zone_29N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone2N = ProjectionInfo.FromEpsgCode(32602).SetNames("WGS_1984_UTM_Zone_2N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone30N = ProjectionInfo.FromEpsgCode(32630).SetNames("WGS_1984_UTM_Zone_30N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone31N = ProjectionInfo.FromEpsgCode(32631).SetNames("WGS_1984_UTM_Zone_31N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone32N = ProjectionInfo.FromEpsgCode(32632).SetNames("WGS_1984_UTM_Zone_32N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone33N = ProjectionInfo.FromEpsgCode(32633).SetNames("WGS_1984_UTM_Zone_33N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone34N = ProjectionInfo.FromEpsgCode(32634).SetNames("WGS_1984_UTM_Zone_34N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone35N = ProjectionInfo.FromEpsgCode(32635).SetNames("WGS_1984_UTM_Zone_35N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone36N = ProjectionInfo.FromEpsgCode(32636).SetNames("WGS_1984_UTM_Zone_36N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone37N = ProjectionInfo.FromEpsgCode(32637).SetNames("WGS_1984_UTM_Zone_37N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone38N = ProjectionInfo.FromEpsgCode(32638).SetNames("WGS_1984_UTM_Zone_38N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone39N = ProjectionInfo.FromEpsgCode(32639).SetNames("WGS_1984_UTM_Zone_39N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone3N = ProjectionInfo.FromEpsgCode(32603).SetNames("WGS_1984_UTM_Zone_3N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone40N = ProjectionInfo.FromEpsgCode(32640).SetNames("WGS_1984_UTM_Zone_40N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone41N = ProjectionInfo.FromEpsgCode(32641).SetNames("WGS_1984_UTM_Zone_41N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone42N = ProjectionInfo.FromEpsgCode(32642).SetNames("WGS_1984_UTM_Zone_42N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone43N = ProjectionInfo.FromEpsgCode(32643).SetNames("WGS_1984_UTM_Zone_43N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone44N = ProjectionInfo.FromEpsgCode(32644).SetNames("WGS_1984_UTM_Zone_44N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone45N = ProjectionInfo.FromEpsgCode(32645).SetNames("WGS_1984_UTM_Zone_45N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone46N = ProjectionInfo.FromEpsgCode(32646).SetNames("WGS_1984_UTM_Zone_46N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone47N = ProjectionInfo.FromEpsgCode(32647).SetNames("WGS_1984_UTM_Zone_47N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone48N = ProjectionInfo.FromEpsgCode(32648).SetNames("WGS_1984_UTM_Zone_48N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone49N = ProjectionInfo.FromEpsgCode(32649).SetNames("WGS_1984_UTM_Zone_49N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone4N = ProjectionInfo.FromEpsgCode(32604).SetNames("WGS_1984_UTM_Zone_4N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone50N = ProjectionInfo.FromEpsgCode(32650).SetNames("WGS_1984_UTM_Zone_50N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone51N = ProjectionInfo.FromEpsgCode(32651).SetNames("WGS_1984_UTM_Zone_51N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone52N = ProjectionInfo.FromEpsgCode(32652).SetNames("WGS_1984_UTM_Zone_52N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone53N = ProjectionInfo.FromEpsgCode(32653).SetNames("WGS_1984_UTM_Zone_53N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone54N = ProjectionInfo.FromEpsgCode(32654).SetNames("WGS_1984_UTM_Zone_54N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone55N = ProjectionInfo.FromEpsgCode(32655).SetNames("WGS_1984_UTM_Zone_55N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone56N = ProjectionInfo.FromEpsgCode(32656).SetNames("WGS_1984_UTM_Zone_56N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone57N = ProjectionInfo.FromEpsgCode(32657).SetNames("WGS_1984_UTM_Zone_57N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone58N = ProjectionInfo.FromEpsgCode(32658).SetNames("WGS_1984_UTM_Zone_58N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone59N = ProjectionInfo.FromEpsgCode(32659).SetNames("WGS_1984_UTM_Zone_59N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone5N = ProjectionInfo.FromEpsgCode(32605).SetNames("WGS_1984_UTM_Zone_5N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone60N = ProjectionInfo.FromEpsgCode(32660).SetNames("WGS_1984_UTM_Zone_60N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone6N = ProjectionInfo.FromEpsgCode(32606).SetNames("WGS_1984_UTM_Zone_6N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone7N = ProjectionInfo.FromEpsgCode(32607).SetNames("WGS_1984_UTM_Zone_7N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone8N = ProjectionInfo.FromEpsgCode(32608).SetNames("WGS_1984_UTM_Zone_8N", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone9N = ProjectionInfo.FromEpsgCode(32609).SetNames("WGS_1984_UTM_Zone_9N", "GCS_WGS_1984", "D_WGS_1984");
        }

        #endregion
    }
}

#pragma warning restore 1591