// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:38:49 PM
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
    /// This class contains predefined CoordinateSystems for Pulkovo1995.
    /// </summary>
    public class Pulkovo1995 : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM102E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM105E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM108E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM111E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM114E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM117E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM120E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM123E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM126E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM129E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM132E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM135E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM138E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM141E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM144E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM147E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM150E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM153E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM156E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM159E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM162E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM165E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM168E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM168W;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM171E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM171W;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM174E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM174W;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM177E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM177W;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM180E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM21E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM24E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM27E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM30E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM33E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM36E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM39E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM42E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM45E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM48E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM51E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM54E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM57E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM60E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM63E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM66E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM69E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM72E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM75E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM78E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM81E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM84E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM87E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM90E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM93E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM96E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM99E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone10;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone11;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone12;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone13;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone14;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone15;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone16;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone17;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone18;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone19;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone20;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone21;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone22;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone23;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone24;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone25;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone26;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone27;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone28;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone29;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone30;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone31;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone32;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone33;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone34;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone35;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone36;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone37;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone38;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone39;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone40;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone41;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone42;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone43;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone44;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone45;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone46;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone47;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone48;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone49;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone50;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone51;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone52;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone53;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone54;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone55;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone56;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone57;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone58;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone59;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone60;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone61;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone62;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone63;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone64;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone7;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone8;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone9;
        public readonly ProjectionInfo Pulkovo1995GKZone10;
        public readonly ProjectionInfo Pulkovo1995GKZone10N;
        public readonly ProjectionInfo Pulkovo1995GKZone11;
        public readonly ProjectionInfo Pulkovo1995GKZone11N;
        public readonly ProjectionInfo Pulkovo1995GKZone12;
        public readonly ProjectionInfo Pulkovo1995GKZone12N;
        public readonly ProjectionInfo Pulkovo1995GKZone13;
        public readonly ProjectionInfo Pulkovo1995GKZone13N;
        public readonly ProjectionInfo Pulkovo1995GKZone14;
        public readonly ProjectionInfo Pulkovo1995GKZone14N;
        public readonly ProjectionInfo Pulkovo1995GKZone15;
        public readonly ProjectionInfo Pulkovo1995GKZone15N;
        public readonly ProjectionInfo Pulkovo1995GKZone16;
        public readonly ProjectionInfo Pulkovo1995GKZone16N;
        public readonly ProjectionInfo Pulkovo1995GKZone17;
        public readonly ProjectionInfo Pulkovo1995GKZone17N;
        public readonly ProjectionInfo Pulkovo1995GKZone18;
        public readonly ProjectionInfo Pulkovo1995GKZone18N;
        public readonly ProjectionInfo Pulkovo1995GKZone19;
        public readonly ProjectionInfo Pulkovo1995GKZone19N;
        public readonly ProjectionInfo Pulkovo1995GKZone2;
        public readonly ProjectionInfo Pulkovo1995GKZone20;
        public readonly ProjectionInfo Pulkovo1995GKZone20N;
        public readonly ProjectionInfo Pulkovo1995GKZone21;
        public readonly ProjectionInfo Pulkovo1995GKZone21N;
        public readonly ProjectionInfo Pulkovo1995GKZone22;
        public readonly ProjectionInfo Pulkovo1995GKZone22N;
        public readonly ProjectionInfo Pulkovo1995GKZone23;
        public readonly ProjectionInfo Pulkovo1995GKZone23N;
        public readonly ProjectionInfo Pulkovo1995GKZone24;
        public readonly ProjectionInfo Pulkovo1995GKZone24N;
        public readonly ProjectionInfo Pulkovo1995GKZone25;
        public readonly ProjectionInfo Pulkovo1995GKZone25N;
        public readonly ProjectionInfo Pulkovo1995GKZone26;
        public readonly ProjectionInfo Pulkovo1995GKZone26N;
        public readonly ProjectionInfo Pulkovo1995GKZone27;
        public readonly ProjectionInfo Pulkovo1995GKZone27N;
        public readonly ProjectionInfo Pulkovo1995GKZone28;
        public readonly ProjectionInfo Pulkovo1995GKZone28N;
        public readonly ProjectionInfo Pulkovo1995GKZone29;
        public readonly ProjectionInfo Pulkovo1995GKZone29N;
        public readonly ProjectionInfo Pulkovo1995GKZone2N;
        public readonly ProjectionInfo Pulkovo1995GKZone3;
        public readonly ProjectionInfo Pulkovo1995GKZone30;
        public readonly ProjectionInfo Pulkovo1995GKZone30N;
        public readonly ProjectionInfo Pulkovo1995GKZone31;
        public readonly ProjectionInfo Pulkovo1995GKZone31N;
        public readonly ProjectionInfo Pulkovo1995GKZone32;
        public readonly ProjectionInfo Pulkovo1995GKZone32N;
        public readonly ProjectionInfo Pulkovo1995GKZone3N;
        public readonly ProjectionInfo Pulkovo1995GKZone4;
        public readonly ProjectionInfo Pulkovo1995GKZone4N;
        public readonly ProjectionInfo Pulkovo1995GKZone5;
        public readonly ProjectionInfo Pulkovo1995GKZone5N;
        public readonly ProjectionInfo Pulkovo1995GKZone6;
        public readonly ProjectionInfo Pulkovo1995GKZone6N;
        public readonly ProjectionInfo Pulkovo1995GKZone7;
        public readonly ProjectionInfo Pulkovo1995GKZone7N;
        public readonly ProjectionInfo Pulkovo1995GKZone8;
        public readonly ProjectionInfo Pulkovo1995GKZone8N;
        public readonly ProjectionInfo Pulkovo1995GKZone9;
        public readonly ProjectionInfo Pulkovo1995GKZone9N;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Pulkovo1995.
        /// </summary>
        public Pulkovo1995()
        {
            Pulkovo19953DegreeGKCM102E = ProjectionInfo.FromEpsgCode(2726).SetNames("Pulkovo_1995_3_Degree_GK_CM_102E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM105E = ProjectionInfo.FromEpsgCode(2727).SetNames("Pulkovo_1995_3_Degree_GK_CM_105E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM108E = ProjectionInfo.FromEpsgCode(2728).SetNames("Pulkovo_1995_3_Degree_GK_CM_108E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM111E = ProjectionInfo.FromEpsgCode(2729).SetNames("Pulkovo_1995_3_Degree_GK_CM_111E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM114E = ProjectionInfo.FromEpsgCode(2730).SetNames("Pulkovo_1995_3_Degree_GK_CM_114E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM117E = ProjectionInfo.FromEpsgCode(2731).SetNames("Pulkovo_1995_3_Degree_GK_CM_117E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM120E = ProjectionInfo.FromEpsgCode(2732).SetNames("Pulkovo_1995_3_Degree_GK_CM_120E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM123E = ProjectionInfo.FromEpsgCode(2733).SetNames("Pulkovo_1995_3_Degree_GK_CM_123E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM126E = ProjectionInfo.FromEpsgCode(2734).SetNames("Pulkovo_1995_3_Degree_GK_CM_126E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM129E = ProjectionInfo.FromEpsgCode(2735).SetNames("Pulkovo_1995_3_Degree_GK_CM_129E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM132E = ProjectionInfo.FromEpsgCode(2738).SetNames("Pulkovo_1995_3_Degree_GK_CM_132E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM135E = ProjectionInfo.FromEpsgCode(2739).SetNames("Pulkovo_1995_3_Degree_GK_CM_135E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM138E = ProjectionInfo.FromEpsgCode(2740).SetNames("Pulkovo_1995_3_Degree_GK_CM_138E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM141E = ProjectionInfo.FromEpsgCode(2741).SetNames("Pulkovo_1995_3_Degree_GK_CM_141E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM144E = ProjectionInfo.FromEpsgCode(2742).SetNames("Pulkovo_1995_3_Degree_GK_CM_144E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM147E = ProjectionInfo.FromEpsgCode(2743).SetNames("Pulkovo_1995_3_Degree_GK_CM_147E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM150E = ProjectionInfo.FromEpsgCode(2744).SetNames("Pulkovo_1995_3_Degree_GK_CM_150E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM153E = ProjectionInfo.FromEpsgCode(2745).SetNames("Pulkovo_1995_3_Degree_GK_CM_153E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM156E = ProjectionInfo.FromEpsgCode(2746).SetNames("Pulkovo_1995_3_Degree_GK_CM_156E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM159E = ProjectionInfo.FromEpsgCode(2747).SetNames("Pulkovo_1995_3_Degree_GK_CM_159E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM162E = ProjectionInfo.FromEpsgCode(2748).SetNames("Pulkovo_1995_3_Degree_GK_CM_162E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM165E = ProjectionInfo.FromEpsgCode(2749).SetNames("Pulkovo_1995_3_Degree_GK_CM_165E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM168E = ProjectionInfo.FromEpsgCode(2750).SetNames("Pulkovo_1995_3_Degree_GK_CM_168E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM168W = ProjectionInfo.FromEpsgCode(2758).SetNames("Pulkovo_1995_3_Degree_GK_CM_168W", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM171E = ProjectionInfo.FromEpsgCode(2751).SetNames("Pulkovo_1995_3_Degree_GK_CM_171E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM171W = ProjectionInfo.FromEpsgCode(2757).SetNames("Pulkovo_1995_3_Degree_GK_CM_171W", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM174E = ProjectionInfo.FromEpsgCode(2752).SetNames("Pulkovo_1995_3_Degree_GK_CM_174E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM174W = ProjectionInfo.FromEpsgCode(2756).SetNames("Pulkovo_1995_3_Degree_GK_CM_174W", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM177E = ProjectionInfo.FromEpsgCode(2753).SetNames("Pulkovo_1995_3_Degree_GK_CM_177E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM177W = ProjectionInfo.FromEpsgCode(2755).SetNames("Pulkovo_1995_3_Degree_GK_CM_177W", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM180E = ProjectionInfo.FromEpsgCode(2754).SetNames("Pulkovo_1995_3_Degree_GK_CM_180E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM21E = ProjectionInfo.FromEpsgCode(2699).SetNames("Pulkovo_1995_3_Degree_GK_CM_21E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM24E = ProjectionInfo.FromEpsgCode(2700).SetNames("Pulkovo_1995_3_Degree_GK_CM_24E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM27E = ProjectionInfo.FromEpsgCode(2701).SetNames("Pulkovo_1995_3_Degree_GK_CM_27E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM30E = ProjectionInfo.FromEpsgCode(2702).SetNames("Pulkovo_1995_3_Degree_GK_CM_30E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM33E = ProjectionInfo.FromEpsgCode(2703).SetNames("Pulkovo_1995_3_Degree_GK_CM_33E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM36E = ProjectionInfo.FromEpsgCode(2704).SetNames("Pulkovo_1995_3_Degree_GK_CM_36E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM39E = ProjectionInfo.FromEpsgCode(2705).SetNames("Pulkovo_1995_3_Degree_GK_CM_39E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM42E = ProjectionInfo.FromEpsgCode(2706).SetNames("Pulkovo_1995_3_Degree_GK_CM_42E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM45E = ProjectionInfo.FromEpsgCode(2707).SetNames("Pulkovo_1995_3_Degree_GK_CM_45E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM48E = ProjectionInfo.FromEpsgCode(2708).SetNames("Pulkovo_1995_3_Degree_GK_CM_48E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM51E = ProjectionInfo.FromEpsgCode(2709).SetNames("Pulkovo_1995_3_Degree_GK_CM_51E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM54E = ProjectionInfo.FromEpsgCode(2710).SetNames("Pulkovo_1995_3_Degree_GK_CM_54E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM57E = ProjectionInfo.FromEpsgCode(2711).SetNames("Pulkovo_1995_3_Degree_GK_CM_57E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM60E = ProjectionInfo.FromEpsgCode(2712).SetNames("Pulkovo_1995_3_Degree_GK_CM_60E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM63E = ProjectionInfo.FromEpsgCode(2713).SetNames("Pulkovo_1995_3_Degree_GK_CM_63E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM66E = ProjectionInfo.FromEpsgCode(2714).SetNames("Pulkovo_1995_3_Degree_GK_CM_66E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM69E = ProjectionInfo.FromEpsgCode(2715).SetNames("Pulkovo_1995_3_Degree_GK_CM_69E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM72E = ProjectionInfo.FromEpsgCode(2716).SetNames("Pulkovo_1995_3_Degree_GK_CM_72E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM75E = ProjectionInfo.FromEpsgCode(2717).SetNames("Pulkovo_1995_3_Degree_GK_CM_75E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM78E = ProjectionInfo.FromEpsgCode(2718).SetNames("Pulkovo_1995_3_Degree_GK_CM_78E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM81E = ProjectionInfo.FromEpsgCode(2719).SetNames("Pulkovo_1995_3_Degree_GK_CM_81E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM84E = ProjectionInfo.FromEpsgCode(2720).SetNames("Pulkovo_1995_3_Degree_GK_CM_84E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM87E = ProjectionInfo.FromEpsgCode(2721).SetNames("Pulkovo_1995_3_Degree_GK_CM_87E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM90E = ProjectionInfo.FromEpsgCode(2722).SetNames("Pulkovo_1995_3_Degree_GK_CM_90E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM93E = ProjectionInfo.FromEpsgCode(2723).SetNames("Pulkovo_1995_3_Degree_GK_CM_93E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM96E = ProjectionInfo.FromEpsgCode(2724).SetNames("Pulkovo_1995_3_Degree_GK_CM_96E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKCM99E = ProjectionInfo.FromEpsgCode(2725).SetNames("Pulkovo_1995_3_Degree_GK_CM_99E", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone10 = ProjectionInfo.FromEpsgCode(2644).SetNames("Pulkovo_1995_3_Degree_GK_Zone_10", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone11 = ProjectionInfo.FromEpsgCode(2645).SetNames("Pulkovo_1995_3_Degree_GK_Zone_11", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone12 = ProjectionInfo.FromEpsgCode(2646).SetNames("Pulkovo_1995_3_Degree_GK_Zone_12", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone13 = ProjectionInfo.FromEpsgCode(2647).SetNames("Pulkovo_1995_3_Degree_GK_Zone_13", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone14 = ProjectionInfo.FromEpsgCode(2648).SetNames("Pulkovo_1995_3_Degree_GK_Zone_14", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone15 = ProjectionInfo.FromEpsgCode(2649).SetNames("Pulkovo_1995_3_Degree_GK_Zone_15", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone16 = ProjectionInfo.FromEpsgCode(2650).SetNames("Pulkovo_1995_3_Degree_GK_Zone_16", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone17 = ProjectionInfo.FromEpsgCode(2651).SetNames("Pulkovo_1995_3_Degree_GK_Zone_17", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone18 = ProjectionInfo.FromEpsgCode(2652).SetNames("Pulkovo_1995_3_Degree_GK_Zone_18", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone19 = ProjectionInfo.FromEpsgCode(2653).SetNames("Pulkovo_1995_3_Degree_GK_Zone_19", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone20 = ProjectionInfo.FromEpsgCode(2654).SetNames("Pulkovo_1995_3_Degree_GK_Zone_20", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone21 = ProjectionInfo.FromEpsgCode(2655).SetNames("Pulkovo_1995_3_Degree_GK_Zone_21", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone22 = ProjectionInfo.FromEpsgCode(2656).SetNames("Pulkovo_1995_3_Degree_GK_Zone_22", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone23 = ProjectionInfo.FromEpsgCode(2657).SetNames("Pulkovo_1995_3_Degree_GK_Zone_23", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone24 = ProjectionInfo.FromEpsgCode(2658).SetNames("Pulkovo_1995_3_Degree_GK_Zone_24", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone25 = ProjectionInfo.FromEpsgCode(2659).SetNames("Pulkovo_1995_3_Degree_GK_Zone_25", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone26 = ProjectionInfo.FromEpsgCode(2660).SetNames("Pulkovo_1995_3_Degree_GK_Zone_26", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone27 = ProjectionInfo.FromEpsgCode(2661).SetNames("Pulkovo_1995_3_Degree_GK_Zone_27", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone28 = ProjectionInfo.FromEpsgCode(2662).SetNames("Pulkovo_1995_3_Degree_GK_Zone_28", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone29 = ProjectionInfo.FromEpsgCode(2663).SetNames("Pulkovo_1995_3_Degree_GK_Zone_29", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone30 = ProjectionInfo.FromEpsgCode(2664).SetNames("Pulkovo_1995_3_Degree_GK_Zone_30", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone31 = ProjectionInfo.FromEpsgCode(2665).SetNames("Pulkovo_1995_3_Degree_GK_Zone_31", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone32 = ProjectionInfo.FromEpsgCode(2666).SetNames("Pulkovo_1995_3_Degree_GK_Zone_32", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone33 = ProjectionInfo.FromEpsgCode(2667).SetNames("Pulkovo_1995_3_Degree_GK_Zone_33", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone34 = ProjectionInfo.FromEpsgCode(2668).SetNames("Pulkovo_1995_3_Degree_GK_Zone_34", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone35 = ProjectionInfo.FromEpsgCode(2669).SetNames("Pulkovo_1995_3_Degree_GK_Zone_35", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone36 = ProjectionInfo.FromEpsgCode(2670).SetNames("Pulkovo_1995_3_Degree_GK_Zone_36", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone37 = ProjectionInfo.FromEpsgCode(2671).SetNames("Pulkovo_1995_3_Degree_GK_Zone_37", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone38 = ProjectionInfo.FromEpsgCode(2672).SetNames("Pulkovo_1995_3_Degree_GK_Zone_38", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone39 = ProjectionInfo.FromEpsgCode(2673).SetNames("Pulkovo_1995_3_Degree_GK_Zone_39", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone40 = ProjectionInfo.FromEpsgCode(2674).SetNames("Pulkovo_1995_3_Degree_GK_Zone_40", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone41 = ProjectionInfo.FromEpsgCode(2675).SetNames("Pulkovo_1995_3_Degree_GK_Zone_41", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone42 = ProjectionInfo.FromEpsgCode(2676).SetNames("Pulkovo_1995_3_Degree_GK_Zone_42", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone43 = ProjectionInfo.FromEpsgCode(2677).SetNames("Pulkovo_1995_3_Degree_GK_Zone_43", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone44 = ProjectionInfo.FromEpsgCode(2678).SetNames("Pulkovo_1995_3_Degree_GK_Zone_44", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone45 = ProjectionInfo.FromEpsgCode(2679).SetNames("Pulkovo_1995_3_Degree_GK_Zone_45", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone46 = ProjectionInfo.FromEpsgCode(2680).SetNames("Pulkovo_1995_3_Degree_GK_Zone_46", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone47 = ProjectionInfo.FromEpsgCode(2681).SetNames("Pulkovo_1995_3_Degree_GK_Zone_47", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone48 = ProjectionInfo.FromEpsgCode(2682).SetNames("Pulkovo_1995_3_Degree_GK_Zone_48", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone49 = ProjectionInfo.FromEpsgCode(2683).SetNames("Pulkovo_1995_3_Degree_GK_Zone_49", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone50 = ProjectionInfo.FromEpsgCode(2684).SetNames("Pulkovo_1995_3_Degree_GK_Zone_50", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone51 = ProjectionInfo.FromEpsgCode(2685).SetNames("Pulkovo_1995_3_Degree_GK_Zone_51", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone52 = ProjectionInfo.FromEpsgCode(2686).SetNames("Pulkovo_1995_3_Degree_GK_Zone_52", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone53 = ProjectionInfo.FromEpsgCode(2687).SetNames("Pulkovo_1995_3_Degree_GK_Zone_53", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone54 = ProjectionInfo.FromEpsgCode(2688).SetNames("Pulkovo_1995_3_Degree_GK_Zone_54", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone55 = ProjectionInfo.FromEpsgCode(2689).SetNames("Pulkovo_1995_3_Degree_GK_Zone_55", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone56 = ProjectionInfo.FromEpsgCode(2690).SetNames("Pulkovo_1995_3_Degree_GK_Zone_56", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone57 = ProjectionInfo.FromEpsgCode(2691).SetNames("Pulkovo_1995_3_Degree_GK_Zone_57", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone58 = ProjectionInfo.FromEpsgCode(2692).SetNames("Pulkovo_1995_3_Degree_GK_Zone_58", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone59 = ProjectionInfo.FromEpsgCode(2693).SetNames("Pulkovo_1995_3_Degree_GK_Zone_59", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone60 = ProjectionInfo.FromEpsgCode(2694).SetNames("Pulkovo_1995_3_Degree_GK_Zone_60", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone61 = ProjectionInfo.FromEpsgCode(2695).SetNames("Pulkovo_1995_3_Degree_GK_Zone_61", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone62 = ProjectionInfo.FromEpsgCode(2696).SetNames("Pulkovo_1995_3_Degree_GK_Zone_62", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone63 = ProjectionInfo.FromEpsgCode(2697).SetNames("Pulkovo_1995_3_Degree_GK_Zone_63", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone64 = ProjectionInfo.FromEpsgCode(2698).SetNames("Pulkovo_1995_3_Degree_GK_Zone_64", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone7 = ProjectionInfo.FromEpsgCode(2641).SetNames("Pulkovo_1995_3_Degree_GK_Zone_7", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone8 = ProjectionInfo.FromEpsgCode(2642).SetNames("Pulkovo_1995_3_Degree_GK_Zone_8", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo19953DegreeGKZone9 = ProjectionInfo.FromEpsgCode(2643).SetNames("Pulkovo_1995_3_Degree_GK_Zone_9", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone10 = ProjectionInfo.FromEpsgCode(20010).SetNames("Pulkovo_1995_GK_Zone_10", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone10N = ProjectionInfo.FromEpsgCode(20070).SetNames("Pulkovo_1995_GK_Zone_10N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone11 = ProjectionInfo.FromEpsgCode(20011).SetNames("Pulkovo_1995_GK_Zone_11", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone11N = ProjectionInfo.FromEpsgCode(20071).SetNames("Pulkovo_1995_GK_Zone_11N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone12 = ProjectionInfo.FromEpsgCode(20012).SetNames("Pulkovo_1995_GK_Zone_12", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone12N = ProjectionInfo.FromEpsgCode(20072).SetNames("Pulkovo_1995_GK_Zone_12N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone13 = ProjectionInfo.FromEpsgCode(20013).SetNames("Pulkovo_1995_GK_Zone_13", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone13N = ProjectionInfo.FromEpsgCode(20073).SetNames("Pulkovo_1995_GK_Zone_13N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone14 = ProjectionInfo.FromEpsgCode(20014).SetNames("Pulkovo_1995_GK_Zone_14", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone14N = ProjectionInfo.FromEpsgCode(20074).SetNames("Pulkovo_1995_GK_Zone_14N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone15 = ProjectionInfo.FromEpsgCode(20015).SetNames("Pulkovo_1995_GK_Zone_15", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone15N = ProjectionInfo.FromEpsgCode(20075).SetNames("Pulkovo_1995_GK_Zone_15N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone16 = ProjectionInfo.FromEpsgCode(20016).SetNames("Pulkovo_1995_GK_Zone_16", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone16N = ProjectionInfo.FromEpsgCode(20076).SetNames("Pulkovo_1995_GK_Zone_16N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone17 = ProjectionInfo.FromEpsgCode(20017).SetNames("Pulkovo_1995_GK_Zone_17", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone17N = ProjectionInfo.FromEpsgCode(20077).SetNames("Pulkovo_1995_GK_Zone_17N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone18 = ProjectionInfo.FromEpsgCode(20018).SetNames("Pulkovo_1995_GK_Zone_18", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone18N = ProjectionInfo.FromEpsgCode(20078).SetNames("Pulkovo_1995_GK_Zone_18N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone19 = ProjectionInfo.FromEpsgCode(20019).SetNames("Pulkovo_1995_GK_Zone_19", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone19N = ProjectionInfo.FromEpsgCode(20079).SetNames("Pulkovo_1995_GK_Zone_19N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone2 = ProjectionInfo.FromAuthorityCode("EPSG", 20002).SetNames("Pulkovo_1995_GK_Zone_2", "GCS_Pulkovo_1995", "D_Pulkovo_1995"); // missing
            Pulkovo1995GKZone20 = ProjectionInfo.FromEpsgCode(20020).SetNames("Pulkovo_1995_GK_Zone_20", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone20N = ProjectionInfo.FromEpsgCode(20080).SetNames("Pulkovo_1995_GK_Zone_20N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone21 = ProjectionInfo.FromEpsgCode(20021).SetNames("Pulkovo_1995_GK_Zone_21", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone21N = ProjectionInfo.FromEpsgCode(20081).SetNames("Pulkovo_1995_GK_Zone_21N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone22 = ProjectionInfo.FromEpsgCode(20022).SetNames("Pulkovo_1995_GK_Zone_22", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone22N = ProjectionInfo.FromEpsgCode(20082).SetNames("Pulkovo_1995_GK_Zone_22N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone23 = ProjectionInfo.FromEpsgCode(20023).SetNames("Pulkovo_1995_GK_Zone_23", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone23N = ProjectionInfo.FromEpsgCode(20083).SetNames("Pulkovo_1995_GK_Zone_23N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone24 = ProjectionInfo.FromEpsgCode(20024).SetNames("Pulkovo_1995_GK_Zone_24", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone24N = ProjectionInfo.FromEpsgCode(20084).SetNames("Pulkovo_1995_GK_Zone_24N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone25 = ProjectionInfo.FromEpsgCode(20025).SetNames("Pulkovo_1995_GK_Zone_25", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone25N = ProjectionInfo.FromEpsgCode(20085).SetNames("Pulkovo_1995_GK_Zone_25N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone26 = ProjectionInfo.FromEpsgCode(20026).SetNames("Pulkovo_1995_GK_Zone_26", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone26N = ProjectionInfo.FromEpsgCode(20086).SetNames("Pulkovo_1995_GK_Zone_26N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone27 = ProjectionInfo.FromEpsgCode(20027).SetNames("Pulkovo_1995_GK_Zone_27", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone27N = ProjectionInfo.FromEpsgCode(20087).SetNames("Pulkovo_1995_GK_Zone_27N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone28 = ProjectionInfo.FromEpsgCode(20028).SetNames("Pulkovo_1995_GK_Zone_28", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone28N = ProjectionInfo.FromEpsgCode(20088).SetNames("Pulkovo_1995_GK_Zone_28N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone29 = ProjectionInfo.FromEpsgCode(20029).SetNames("Pulkovo_1995_GK_Zone_29", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone29N = ProjectionInfo.FromEpsgCode(20089).SetNames("Pulkovo_1995_GK_Zone_29N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone2N = ProjectionInfo.FromAuthorityCode("EPSG", 20062).SetNames("Pulkovo_1995_GK_Zone_2N", "GCS_Pulkovo_1995", "D_Pulkovo_1995"); // missing
            Pulkovo1995GKZone3 = ProjectionInfo.FromAuthorityCode("EPSG", 20003).SetNames("Pulkovo_1995_GK_Zone_3", "GCS_Pulkovo_1995", "D_Pulkovo_1995"); // missing
            Pulkovo1995GKZone30 = ProjectionInfo.FromEpsgCode(20030).SetNames("Pulkovo_1995_GK_Zone_30", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone30N = ProjectionInfo.FromEpsgCode(20090).SetNames("Pulkovo_1995_GK_Zone_30N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone31 = ProjectionInfo.FromEpsgCode(20031).SetNames("Pulkovo_1995_GK_Zone_31", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone31N = ProjectionInfo.FromEpsgCode(20091).SetNames("Pulkovo_1995_GK_Zone_31N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone32 = ProjectionInfo.FromEpsgCode(20032).SetNames("Pulkovo_1995_GK_Zone_32", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone32N = ProjectionInfo.FromEpsgCode(20092).SetNames("Pulkovo_1995_GK_Zone_32N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone3N = ProjectionInfo.FromAuthorityCode("EPSG", 20063).SetNames("Pulkovo_1995_GK_Zone_3N", "GCS_Pulkovo_1995", "D_Pulkovo_1995"); // missing
            Pulkovo1995GKZone4 = ProjectionInfo.FromEpsgCode(20004).SetNames("Pulkovo_1995_GK_Zone_4", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone4N = ProjectionInfo.FromEpsgCode(20064).SetNames("Pulkovo_1995_GK_Zone_4N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone5 = ProjectionInfo.FromEpsgCode(20005).SetNames("Pulkovo_1995_GK_Zone_5", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone5N = ProjectionInfo.FromEpsgCode(20065).SetNames("Pulkovo_1995_GK_Zone_5N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone6 = ProjectionInfo.FromEpsgCode(20006).SetNames("Pulkovo_1995_GK_Zone_6", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone6N = ProjectionInfo.FromEpsgCode(20066).SetNames("Pulkovo_1995_GK_Zone_6N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone7 = ProjectionInfo.FromEpsgCode(20007).SetNames("Pulkovo_1995_GK_Zone_7", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone7N = ProjectionInfo.FromEpsgCode(20067).SetNames("Pulkovo_1995_GK_Zone_7N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone8 = ProjectionInfo.FromEpsgCode(20008).SetNames("Pulkovo_1995_GK_Zone_8", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone8N = ProjectionInfo.FromEpsgCode(20068).SetNames("Pulkovo_1995_GK_Zone_8N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone9 = ProjectionInfo.FromEpsgCode(20009).SetNames("Pulkovo_1995_GK_Zone_9", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Pulkovo1995GKZone9N = ProjectionInfo.FromEpsgCode(20069).SetNames("Pulkovo_1995_GK_Zone_9N", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
        }

        #endregion
    }
}

#pragma warning restore 1591