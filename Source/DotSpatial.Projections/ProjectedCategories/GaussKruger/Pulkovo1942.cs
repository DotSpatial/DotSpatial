// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:37:24 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.ProjectedCategories.GaussKruger
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for Pulkovo1942.
    /// </summary>
    public class Pulkovo1942 : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM102E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM105E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM108E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM111E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM114E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM117E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM120E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM123E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM126E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM129E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM132E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM135E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM138E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM141E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM144E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM147E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM150E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM153E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM156E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM159E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM162E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM165E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM168E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM168W;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM171E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM171W;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM174E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM174W;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM177E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM177W;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM180E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM21E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM24E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM27E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM30E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM33E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM36E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM39E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM42E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM45E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM48E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM51E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM54E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM57E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM60E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM63E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM66E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM69E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM72E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM75E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM78E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM81E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM84E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM87E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM90E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM93E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM96E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKCM99E;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone10;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone11;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone12;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone13;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone14;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone15;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone16;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone17;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone18;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone19;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone20;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone21;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone22;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone23;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone24;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone25;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone26;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone27;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone28;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone29;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone30;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone31;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone32;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone33;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone34;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone35;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone36;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone37;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone38;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone39;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone40;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone41;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone42;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone43;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone44;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone45;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone46;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone47;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone48;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone49;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone50;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone51;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone52;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone53;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone54;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone55;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone56;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone57;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone58;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone59;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone60;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone61;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone62;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone63;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone64;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone7;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone8;
        public readonly ProjectionInfo Pulkovo19423DegreeGKZone9;
        public readonly ProjectionInfo Pulkovo1942CS63ZoneA1;
        public readonly ProjectionInfo Pulkovo1942CS63ZoneA2;
        public readonly ProjectionInfo Pulkovo1942CS63ZoneA3;
        public readonly ProjectionInfo Pulkovo1942CS63ZoneA4;
        public readonly ProjectionInfo Pulkovo1942CS63ZoneC0;
        public readonly ProjectionInfo Pulkovo1942CS63ZoneC1;
        public readonly ProjectionInfo Pulkovo1942CS63ZoneC2;
        public readonly ProjectionInfo Pulkovo1942CS63ZoneK2;
        public readonly ProjectionInfo Pulkovo1942CS63ZoneK3;
        public readonly ProjectionInfo Pulkovo1942CS63ZoneK4;
        public readonly ProjectionInfo Pulkovo1942GKZone10;
        public readonly ProjectionInfo Pulkovo1942GKZone10N;
        public readonly ProjectionInfo Pulkovo1942GKZone11;
        public readonly ProjectionInfo Pulkovo1942GKZone11N;
        public readonly ProjectionInfo Pulkovo1942GKZone12;
        public readonly ProjectionInfo Pulkovo1942GKZone12N;
        public readonly ProjectionInfo Pulkovo1942GKZone13;
        public readonly ProjectionInfo Pulkovo1942GKZone13N;
        public readonly ProjectionInfo Pulkovo1942GKZone14;
        public readonly ProjectionInfo Pulkovo1942GKZone14N;
        public readonly ProjectionInfo Pulkovo1942GKZone15;
        public readonly ProjectionInfo Pulkovo1942GKZone15N;
        public readonly ProjectionInfo Pulkovo1942GKZone16;
        public readonly ProjectionInfo Pulkovo1942GKZone16N;
        public readonly ProjectionInfo Pulkovo1942GKZone17;
        public readonly ProjectionInfo Pulkovo1942GKZone17N;
        public readonly ProjectionInfo Pulkovo1942GKZone18;
        public readonly ProjectionInfo Pulkovo1942GKZone18N;
        public readonly ProjectionInfo Pulkovo1942GKZone19;
        public readonly ProjectionInfo Pulkovo1942GKZone19N;
        public readonly ProjectionInfo Pulkovo1942GKZone2;
        public readonly ProjectionInfo Pulkovo1942GKZone20;
        public readonly ProjectionInfo Pulkovo1942GKZone20N;
        public readonly ProjectionInfo Pulkovo1942GKZone21;
        public readonly ProjectionInfo Pulkovo1942GKZone21N;
        public readonly ProjectionInfo Pulkovo1942GKZone22;
        public readonly ProjectionInfo Pulkovo1942GKZone22N;
        public readonly ProjectionInfo Pulkovo1942GKZone23;
        public readonly ProjectionInfo Pulkovo1942GKZone23N;
        public readonly ProjectionInfo Pulkovo1942GKZone24;
        public readonly ProjectionInfo Pulkovo1942GKZone24N;
        public readonly ProjectionInfo Pulkovo1942GKZone25;
        public readonly ProjectionInfo Pulkovo1942GKZone25N;
        public readonly ProjectionInfo Pulkovo1942GKZone26;
        public readonly ProjectionInfo Pulkovo1942GKZone26N;
        public readonly ProjectionInfo Pulkovo1942GKZone27;
        public readonly ProjectionInfo Pulkovo1942GKZone27N;
        public readonly ProjectionInfo Pulkovo1942GKZone28;
        public readonly ProjectionInfo Pulkovo1942GKZone28N;
        public readonly ProjectionInfo Pulkovo1942GKZone29;
        public readonly ProjectionInfo Pulkovo1942GKZone29N;
        public readonly ProjectionInfo Pulkovo1942GKZone2N;
        public readonly ProjectionInfo Pulkovo1942GKZone3;
        public readonly ProjectionInfo Pulkovo1942GKZone30;
        public readonly ProjectionInfo Pulkovo1942GKZone30N;
        public readonly ProjectionInfo Pulkovo1942GKZone31;
        public readonly ProjectionInfo Pulkovo1942GKZone31N;
        public readonly ProjectionInfo Pulkovo1942GKZone32;
        public readonly ProjectionInfo Pulkovo1942GKZone32N;
        public readonly ProjectionInfo Pulkovo1942GKZone3N;
        public readonly ProjectionInfo Pulkovo1942GKZone4;
        public readonly ProjectionInfo Pulkovo1942GKZone4N;
        public readonly ProjectionInfo Pulkovo1942GKZone5;
        public readonly ProjectionInfo Pulkovo1942GKZone5N;
        public readonly ProjectionInfo Pulkovo1942GKZone6;
        public readonly ProjectionInfo Pulkovo1942GKZone6N;
        public readonly ProjectionInfo Pulkovo1942GKZone7;
        public readonly ProjectionInfo Pulkovo1942GKZone7N;
        public readonly ProjectionInfo Pulkovo1942GKZone8;
        public readonly ProjectionInfo Pulkovo1942GKZone8N;
        public readonly ProjectionInfo Pulkovo1942GKZone9;
        public readonly ProjectionInfo Pulkovo1942GKZone9N;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Pulkovo1942.
        /// </summary>
        public Pulkovo1942()
        {
            Pulkovo19423DegreeGKCM102E = ProjectionInfo.FromEpsgCode(2610).SetNames("Pulkovo_1942_3_Degree_GK_CM_102E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM105E = ProjectionInfo.FromEpsgCode(2611).SetNames("Pulkovo_1942_3_Degree_GK_CM_105E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM108E = ProjectionInfo.FromEpsgCode(2612).SetNames("Pulkovo_1942_3_Degree_GK_CM_108E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM111E = ProjectionInfo.FromEpsgCode(2613).SetNames("Pulkovo_1942_3_Degree_GK_CM_111E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM114E = ProjectionInfo.FromEpsgCode(2614).SetNames("Pulkovo_1942_3_Degree_GK_CM_114E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM117E = ProjectionInfo.FromEpsgCode(2615).SetNames("Pulkovo_1942_3_Degree_GK_CM_117E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM120E = ProjectionInfo.FromEpsgCode(2616).SetNames("Pulkovo_1942_3_Degree_GK_CM_120E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM123E = ProjectionInfo.FromEpsgCode(2617).SetNames("Pulkovo_1942_3_Degree_GK_CM_123E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM126E = ProjectionInfo.FromEpsgCode(2618).SetNames("Pulkovo_1942_3_Degree_GK_CM_126E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM129E = ProjectionInfo.FromEpsgCode(2619).SetNames("Pulkovo_1942_3_Degree_GK_CM_129E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM132E = ProjectionInfo.FromEpsgCode(2620).SetNames("Pulkovo_1942_3_Degree_GK_CM_132E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM135E = ProjectionInfo.FromEpsgCode(2621).SetNames("Pulkovo_1942_3_Degree_GK_CM_135E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM138E = ProjectionInfo.FromEpsgCode(2622).SetNames("Pulkovo_1942_3_Degree_GK_CM_138E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM141E = ProjectionInfo.FromEpsgCode(2623).SetNames("Pulkovo_1942_3_Degree_GK_CM_141E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM144E = ProjectionInfo.FromEpsgCode(2624).SetNames("Pulkovo_1942_3_Degree_GK_CM_144E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM147E = ProjectionInfo.FromEpsgCode(2625).SetNames("Pulkovo_1942_3_Degree_GK_CM_147E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM150E = ProjectionInfo.FromEpsgCode(2626).SetNames("Pulkovo_1942_3_Degree_GK_CM_150E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM153E = ProjectionInfo.FromEpsgCode(2627).SetNames("Pulkovo_1942_3_Degree_GK_CM_153E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM156E = ProjectionInfo.FromEpsgCode(2628).SetNames("Pulkovo_1942_3_Degree_GK_CM_156E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM159E = ProjectionInfo.FromEpsgCode(2629).SetNames("Pulkovo_1942_3_Degree_GK_CM_159E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM162E = ProjectionInfo.FromEpsgCode(2630).SetNames("Pulkovo_1942_3_Degree_GK_CM_162E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM165E = ProjectionInfo.FromEpsgCode(2631).SetNames("Pulkovo_1942_3_Degree_GK_CM_165E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM168E = ProjectionInfo.FromEpsgCode(2632).SetNames("Pulkovo_1942_3_Degree_GK_CM_168E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM168W = ProjectionInfo.FromEpsgCode(2640).SetNames("Pulkovo_1942_3_Degree_GK_CM_168W", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM171E = ProjectionInfo.FromEpsgCode(2633).SetNames("Pulkovo_1942_3_Degree_GK_CM_171E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM171W = ProjectionInfo.FromEpsgCode(2639).SetNames("Pulkovo_1942_3_Degree_GK_CM_171W", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM174E = ProjectionInfo.FromEpsgCode(2634).SetNames("Pulkovo_1942_3_Degree_GK_CM_174E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM174W = ProjectionInfo.FromEpsgCode(2638).SetNames("Pulkovo_1942_3_Degree_GK_CM_174W", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM177E = ProjectionInfo.FromEpsgCode(2635).SetNames("Pulkovo_1942_3_Degree_GK_CM_177E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM177W = ProjectionInfo.FromEpsgCode(2637).SetNames("Pulkovo_1942_3_Degree_GK_CM_177W", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM180E = ProjectionInfo.FromEpsgCode(2636).SetNames("Pulkovo_1942_3_Degree_GK_CM_180E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM21E = ProjectionInfo.FromEpsgCode(2582).SetNames("Pulkovo_1942_3_Degree_GK_CM_21E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM24E = ProjectionInfo.FromEpsgCode(2583).SetNames("Pulkovo_1942_3_Degree_GK_CM_24E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM27E = ProjectionInfo.FromEpsgCode(2584).SetNames("Pulkovo_1942_3_Degree_GK_CM_27E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM30E = ProjectionInfo.FromEpsgCode(2585).SetNames("Pulkovo_1942_3_Degree_GK_CM_30E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM33E = ProjectionInfo.FromEpsgCode(2586).SetNames("Pulkovo_1942_3_Degree_GK_CM_33E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM36E = ProjectionInfo.FromEpsgCode(2587).SetNames("Pulkovo_1942_3_Degree_GK_CM_36E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM39E = ProjectionInfo.FromEpsgCode(2588).SetNames("Pulkovo_1942_3_Degree_GK_CM_39E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM42E = ProjectionInfo.FromEpsgCode(2589).SetNames("Pulkovo_1942_3_Degree_GK_CM_42E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM45E = ProjectionInfo.FromEpsgCode(2590).SetNames("Pulkovo_1942_3_Degree_GK_CM_45E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM48E = ProjectionInfo.FromEpsgCode(2591).SetNames("Pulkovo_1942_3_Degree_GK_CM_48E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM51E = ProjectionInfo.FromEpsgCode(2592).SetNames("Pulkovo_1942_3_Degree_GK_CM_51E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM54E = ProjectionInfo.FromEpsgCode(2593).SetNames("Pulkovo_1942_3_Degree_GK_CM_54E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM57E = ProjectionInfo.FromEpsgCode(2594).SetNames("Pulkovo_1942_3_Degree_GK_CM_57E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM60E = ProjectionInfo.FromEpsgCode(2595).SetNames("Pulkovo_1942_3_Degree_GK_CM_60E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM63E = ProjectionInfo.FromEpsgCode(2596).SetNames("Pulkovo_1942_3_Degree_GK_CM_63E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM66E = ProjectionInfo.FromEpsgCode(2597).SetNames("Pulkovo_1942_3_Degree_GK_CM_66E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM69E = ProjectionInfo.FromEpsgCode(2598).SetNames("Pulkovo_1942_3_Degree_GK_CM_69E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM72E = ProjectionInfo.FromEpsgCode(2599).SetNames("Pulkovo_1942_3_Degree_GK_CM_72E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM75E = ProjectionInfo.FromEpsgCode(2601).SetNames("Pulkovo_1942_3_Degree_GK_CM_75E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM78E = ProjectionInfo.FromEpsgCode(2602).SetNames("Pulkovo_1942_3_Degree_GK_CM_78E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM81E = ProjectionInfo.FromEpsgCode(2603).SetNames("Pulkovo_1942_3_Degree_GK_CM_81E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM84E = ProjectionInfo.FromEpsgCode(2604).SetNames("Pulkovo_1942_3_Degree_GK_CM_84E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM87E = ProjectionInfo.FromEpsgCode(2605).SetNames("Pulkovo_1942_3_Degree_GK_CM_87E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM90E = ProjectionInfo.FromEpsgCode(2606).SetNames("Pulkovo_1942_3_Degree_GK_CM_90E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM93E = ProjectionInfo.FromEpsgCode(2607).SetNames("Pulkovo_1942_3_Degree_GK_CM_93E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM96E = ProjectionInfo.FromEpsgCode(2608).SetNames("Pulkovo_1942_3_Degree_GK_CM_96E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKCM99E = ProjectionInfo.FromEpsgCode(2609).SetNames("Pulkovo_1942_3_Degree_GK_CM_99E", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone10 = ProjectionInfo.FromEpsgCode(2526).SetNames("Pulkovo_1942_3_Degree_GK_Zone_10", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone11 = ProjectionInfo.FromEpsgCode(2527).SetNames("Pulkovo_1942_3_Degree_GK_Zone_11", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone12 = ProjectionInfo.FromEpsgCode(2528).SetNames("Pulkovo_1942_3_Degree_GK_Zone_12", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone13 = ProjectionInfo.FromEpsgCode(2529).SetNames("Pulkovo_1942_3_Degree_GK_Zone_13", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone14 = ProjectionInfo.FromEpsgCode(2530).SetNames("Pulkovo_1942_3_Degree_GK_Zone_14", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone15 = ProjectionInfo.FromEpsgCode(2531).SetNames("Pulkovo_1942_3_Degree_GK_Zone_15", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone16 = ProjectionInfo.FromEpsgCode(2532).SetNames("Pulkovo_1942_3_Degree_GK_Zone_16", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone17 = ProjectionInfo.FromEpsgCode(2533).SetNames("Pulkovo_1942_3_Degree_GK_Zone_17", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone18 = ProjectionInfo.FromEpsgCode(2534).SetNames("Pulkovo_1942_3_Degree_GK_Zone_18", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone19 = ProjectionInfo.FromEpsgCode(2535).SetNames("Pulkovo_1942_3_Degree_GK_Zone_19", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone20 = ProjectionInfo.FromEpsgCode(2536).SetNames("Pulkovo_1942_3_Degree_GK_Zone_20", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone21 = ProjectionInfo.FromEpsgCode(2537).SetNames("Pulkovo_1942_3_Degree_GK_Zone_21", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone22 = ProjectionInfo.FromEpsgCode(2538).SetNames("Pulkovo_1942_3_Degree_GK_Zone_22", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone23 = ProjectionInfo.FromEpsgCode(2539).SetNames("Pulkovo_1942_3_Degree_GK_Zone_23", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone24 = ProjectionInfo.FromEpsgCode(2540).SetNames("Pulkovo_1942_3_Degree_GK_Zone_24", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone25 = ProjectionInfo.FromEpsgCode(2541).SetNames("Pulkovo_1942_3_Degree_GK_Zone_25", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone26 = ProjectionInfo.FromEpsgCode(2542).SetNames("Pulkovo_1942_3_Degree_GK_Zone_26", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone27 = ProjectionInfo.FromEpsgCode(2543).SetNames("Pulkovo_1942_3_Degree_GK_Zone_27", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone28 = ProjectionInfo.FromEpsgCode(2544).SetNames("Pulkovo_1942_3_Degree_GK_Zone_28", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone29 = ProjectionInfo.FromEpsgCode(2545).SetNames("Pulkovo_1942_3_Degree_GK_Zone_29", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone30 = ProjectionInfo.FromEpsgCode(2546).SetNames("Pulkovo_1942_3_Degree_GK_Zone_30", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone31 = ProjectionInfo.FromEpsgCode(2547).SetNames("Pulkovo_1942_3_Degree_GK_Zone_31", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone32 = ProjectionInfo.FromEpsgCode(2548).SetNames("Pulkovo_1942_3_Degree_GK_Zone_32", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone33 = ProjectionInfo.FromEpsgCode(2549).SetNames("Pulkovo_1942_3_Degree_GK_Zone_33", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone34 = ProjectionInfo.FromEpsgCode(2551).SetNames("Pulkovo_1942_3_Degree_GK_Zone_34", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone35 = ProjectionInfo.FromEpsgCode(2552).SetNames("Pulkovo_1942_3_Degree_GK_Zone_35", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone36 = ProjectionInfo.FromEpsgCode(2553).SetNames("Pulkovo_1942_3_Degree_GK_Zone_36", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone37 = ProjectionInfo.FromEpsgCode(2554).SetNames("Pulkovo_1942_3_Degree_GK_Zone_37", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone38 = ProjectionInfo.FromEpsgCode(2555).SetNames("Pulkovo_1942_3_Degree_GK_Zone_38", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone39 = ProjectionInfo.FromEpsgCode(2556).SetNames("Pulkovo_1942_3_Degree_GK_Zone_39", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone40 = ProjectionInfo.FromEpsgCode(2557).SetNames("Pulkovo_1942_3_Degree_GK_Zone_40", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone41 = ProjectionInfo.FromEpsgCode(2558).SetNames("Pulkovo_1942_3_Degree_GK_Zone_41", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone42 = ProjectionInfo.FromEpsgCode(2559).SetNames("Pulkovo_1942_3_Degree_GK_Zone_42", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone43 = ProjectionInfo.FromEpsgCode(2560).SetNames("Pulkovo_1942_3_Degree_GK_Zone_43", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone44 = ProjectionInfo.FromEpsgCode(2561).SetNames("Pulkovo_1942_3_Degree_GK_Zone_44", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone45 = ProjectionInfo.FromEpsgCode(2562).SetNames("Pulkovo_1942_3_Degree_GK_Zone_45", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone46 = ProjectionInfo.FromEpsgCode(2563).SetNames("Pulkovo_1942_3_Degree_GK_Zone_46", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone47 = ProjectionInfo.FromEpsgCode(2564).SetNames("Pulkovo_1942_3_Degree_GK_Zone_47", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone48 = ProjectionInfo.FromEpsgCode(2565).SetNames("Pulkovo_1942_3_Degree_GK_Zone_48", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone49 = ProjectionInfo.FromEpsgCode(2566).SetNames("Pulkovo_1942_3_Degree_GK_Zone_49", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone50 = ProjectionInfo.FromEpsgCode(2567).SetNames("Pulkovo_1942_3_Degree_GK_Zone_50", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone51 = ProjectionInfo.FromEpsgCode(2568).SetNames("Pulkovo_1942_3_Degree_GK_Zone_51", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone52 = ProjectionInfo.FromEpsgCode(2569).SetNames("Pulkovo_1942_3_Degree_GK_Zone_52", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone53 = ProjectionInfo.FromEpsgCode(2570).SetNames("Pulkovo_1942_3_Degree_GK_Zone_53", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone54 = ProjectionInfo.FromEpsgCode(2571).SetNames("Pulkovo_1942_3_Degree_GK_Zone_54", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone55 = ProjectionInfo.FromEpsgCode(2572).SetNames("Pulkovo_1942_3_Degree_GK_Zone_55", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone56 = ProjectionInfo.FromEpsgCode(2573).SetNames("Pulkovo_1942_3_Degree_GK_Zone_56", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone57 = ProjectionInfo.FromEpsgCode(2574).SetNames("Pulkovo_1942_3_Degree_GK_Zone_57", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone58 = ProjectionInfo.FromEpsgCode(2575).SetNames("Pulkovo_1942_3_Degree_GK_Zone_58", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone59 = ProjectionInfo.FromEpsgCode(2576).SetNames("Pulkovo_1942_3_Degree_GK_Zone_59", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone60 = ProjectionInfo.FromEpsgCode(2577).SetNames("Pulkovo_1942_3_Degree_GK_Zone_60", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone61 = ProjectionInfo.FromEpsgCode(2578).SetNames("Pulkovo_1942_3_Degree_GK_Zone_61", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone62 = ProjectionInfo.FromEpsgCode(2579).SetNames("Pulkovo_1942_3_Degree_GK_Zone_62", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone63 = ProjectionInfo.FromEpsgCode(2580).SetNames("Pulkovo_1942_3_Degree_GK_Zone_63", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone64 = ProjectionInfo.FromEpsgCode(2581).SetNames("Pulkovo_1942_3_Degree_GK_Zone_64", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone7 = ProjectionInfo.FromEpsgCode(2523).SetNames("Pulkovo_1942_3_Degree_GK_Zone_7", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone8 = ProjectionInfo.FromEpsgCode(2524).SetNames("Pulkovo_1942_3_Degree_GK_Zone_8", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo19423DegreeGKZone9 = ProjectionInfo.FromEpsgCode(2525).SetNames("Pulkovo_1942_3_Degree_GK_Zone_9", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942CS63ZoneA1 = ProjectionInfo.FromEpsgCode(2935).SetNames("Pulkovo_1942_CS63_Zone_A1", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942CS63ZoneA2 = ProjectionInfo.FromEpsgCode(2936).SetNames("Pulkovo_1942_CS63_Zone_A2", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942CS63ZoneA3 = ProjectionInfo.FromEpsgCode(2937).SetNames("Pulkovo_1942_CS63_Zone_A3", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942CS63ZoneA4 = ProjectionInfo.FromEpsgCode(2938).SetNames("Pulkovo_1942_CS63_Zone_A4", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942CS63ZoneC0 = ProjectionInfo.FromEpsgCode(3350).SetNames("Pulkovo_1942_CS63_Zone_C0", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942CS63ZoneC1 = ProjectionInfo.FromEpsgCode(3351).SetNames("Pulkovo_1942_CS63_Zone_C1", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942CS63ZoneC2 = ProjectionInfo.FromEpsgCode(3352).SetNames("Pulkovo_1942_CS63_Zone_C2", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942CS63ZoneK2 = ProjectionInfo.FromEpsgCode(2939).SetNames("Pulkovo_1942_CS63_Zone_K2", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942CS63ZoneK3 = ProjectionInfo.FromEpsgCode(2940).SetNames("Pulkovo_1942_CS63_Zone_K3", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942CS63ZoneK4 = ProjectionInfo.FromEpsgCode(2941).SetNames("Pulkovo_1942_CS63_Zone_K4", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone10 = ProjectionInfo.FromEpsgCode(28410).SetNames("Pulkovo_1942_GK_Zone_10", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone10N = ProjectionInfo.FromEpsgCode(28470).SetNames("Pulkovo_1942_GK_Zone_10N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone11 = ProjectionInfo.FromEpsgCode(28411).SetNames("Pulkovo_1942_GK_Zone_11", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone11N = ProjectionInfo.FromEpsgCode(28471).SetNames("Pulkovo_1942_GK_Zone_11N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone12 = ProjectionInfo.FromEpsgCode(28412).SetNames("Pulkovo_1942_GK_Zone_12", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone12N = ProjectionInfo.FromEpsgCode(28472).SetNames("Pulkovo_1942_GK_Zone_12N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone13 = ProjectionInfo.FromEpsgCode(28413).SetNames("Pulkovo_1942_GK_Zone_13", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone13N = ProjectionInfo.FromEpsgCode(28473).SetNames("Pulkovo_1942_GK_Zone_13N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone14 = ProjectionInfo.FromEpsgCode(28414).SetNames("Pulkovo_1942_GK_Zone_14", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone14N = ProjectionInfo.FromEpsgCode(28474).SetNames("Pulkovo_1942_GK_Zone_14N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone15 = ProjectionInfo.FromEpsgCode(28415).SetNames("Pulkovo_1942_GK_Zone_15", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone15N = ProjectionInfo.FromEpsgCode(28475).SetNames("Pulkovo_1942_GK_Zone_15N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone16 = ProjectionInfo.FromEpsgCode(28416).SetNames("Pulkovo_1942_GK_Zone_16", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone16N = ProjectionInfo.FromEpsgCode(28476).SetNames("Pulkovo_1942_GK_Zone_16N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone17 = ProjectionInfo.FromEpsgCode(28417).SetNames("Pulkovo_1942_GK_Zone_17", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone17N = ProjectionInfo.FromEpsgCode(28477).SetNames("Pulkovo_1942_GK_Zone_17N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone18 = ProjectionInfo.FromEpsgCode(28418).SetNames("Pulkovo_1942_GK_Zone_18", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone18N = ProjectionInfo.FromEpsgCode(28478).SetNames("Pulkovo_1942_GK_Zone_18N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone19 = ProjectionInfo.FromEpsgCode(28419).SetNames("Pulkovo_1942_GK_Zone_19", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone19N = ProjectionInfo.FromEpsgCode(28479).SetNames("Pulkovo_1942_GK_Zone_19N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone2 = ProjectionInfo.FromEpsgCode(28402).SetNames("Pulkovo_1942_GK_Zone_2", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone20 = ProjectionInfo.FromEpsgCode(28420).SetNames("Pulkovo_1942_GK_Zone_20", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone20N = ProjectionInfo.FromEpsgCode(28480).SetNames("Pulkovo_1942_GK_Zone_20N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone21 = ProjectionInfo.FromEpsgCode(28421).SetNames("Pulkovo_1942_GK_Zone_21", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone21N = ProjectionInfo.FromEpsgCode(28481).SetNames("Pulkovo_1942_GK_Zone_21N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone22 = ProjectionInfo.FromEpsgCode(28422).SetNames("Pulkovo_1942_GK_Zone_22", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone22N = ProjectionInfo.FromEpsgCode(28482).SetNames("Pulkovo_1942_GK_Zone_22N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone23 = ProjectionInfo.FromEpsgCode(28423).SetNames("Pulkovo_1942_GK_Zone_23", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone23N = ProjectionInfo.FromEpsgCode(28483).SetNames("Pulkovo_1942_GK_Zone_23N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone24 = ProjectionInfo.FromEpsgCode(28424).SetNames("Pulkovo_1942_GK_Zone_24", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone24N = ProjectionInfo.FromEpsgCode(28484).SetNames("Pulkovo_1942_GK_Zone_24N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone25 = ProjectionInfo.FromEpsgCode(28425).SetNames("Pulkovo_1942_GK_Zone_25", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone25N = ProjectionInfo.FromEpsgCode(28485).SetNames("Pulkovo_1942_GK_Zone_25N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone26 = ProjectionInfo.FromEpsgCode(28426).SetNames("Pulkovo_1942_GK_Zone_26", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone26N = ProjectionInfo.FromEpsgCode(28486).SetNames("Pulkovo_1942_GK_Zone_26N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone27 = ProjectionInfo.FromEpsgCode(28427).SetNames("Pulkovo_1942_GK_Zone_27", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone27N = ProjectionInfo.FromEpsgCode(28487).SetNames("Pulkovo_1942_GK_Zone_27N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone28 = ProjectionInfo.FromEpsgCode(28428).SetNames("Pulkovo_1942_GK_Zone_28", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone28N = ProjectionInfo.FromEpsgCode(28488).SetNames("Pulkovo_1942_GK_Zone_28N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone29 = ProjectionInfo.FromEpsgCode(28429).SetNames("Pulkovo_1942_GK_Zone_29", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone29N = ProjectionInfo.FromEpsgCode(28489).SetNames("Pulkovo_1942_GK_Zone_29N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone2N = ProjectionInfo.FromEpsgCode(28462).SetNames("Pulkovo_1942_GK_Zone_2N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone3 = ProjectionInfo.FromEpsgCode(28403).SetNames("Pulkovo_1942_GK_Zone_3", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone30 = ProjectionInfo.FromEpsgCode(28430).SetNames("Pulkovo_1942_GK_Zone_30", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone30N = ProjectionInfo.FromEpsgCode(28490).SetNames("Pulkovo_1942_GK_Zone_30N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone31 = ProjectionInfo.FromEpsgCode(28431).SetNames("Pulkovo_1942_GK_Zone_31", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone31N = ProjectionInfo.FromEpsgCode(28491).SetNames("Pulkovo_1942_GK_Zone_31N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone32 = ProjectionInfo.FromEpsgCode(28432).SetNames("Pulkovo_1942_GK_Zone_32", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone32N = ProjectionInfo.FromEpsgCode(28492).SetNames("Pulkovo_1942_GK_Zone_32N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone3N = ProjectionInfo.FromEpsgCode(28463).SetNames("Pulkovo_1942_GK_Zone_3N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone4 = ProjectionInfo.FromEpsgCode(28404).SetNames("Pulkovo_1942_GK_Zone_4", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone4N = ProjectionInfo.FromEpsgCode(28464).SetNames("Pulkovo_1942_GK_Zone_4N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone5 = ProjectionInfo.FromEpsgCode(28405).SetNames("Pulkovo_1942_GK_Zone_5", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone5N = ProjectionInfo.FromEpsgCode(28465).SetNames("Pulkovo_1942_GK_Zone_5N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone6 = ProjectionInfo.FromEpsgCode(28406).SetNames("Pulkovo_1942_GK_Zone_6", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone6N = ProjectionInfo.FromEpsgCode(28466).SetNames("Pulkovo_1942_GK_Zone_6N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone7 = ProjectionInfo.FromEpsgCode(28407).SetNames("Pulkovo_1942_GK_Zone_7", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone7N = ProjectionInfo.FromEpsgCode(28467).SetNames("Pulkovo_1942_GK_Zone_7N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone8 = ProjectionInfo.FromEpsgCode(28408).SetNames("Pulkovo_1942_GK_Zone_8", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone8N = ProjectionInfo.FromEpsgCode(28468).SetNames("Pulkovo_1942_GK_Zone_8N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone9 = ProjectionInfo.FromEpsgCode(28409).SetNames("Pulkovo_1942_GK_Zone_9", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942GKZone9N = ProjectionInfo.FromEpsgCode(28469).SetNames("Pulkovo_1942_GK_Zone_9N", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
        }

        #endregion
    }
}

#pragma warning restore 1591