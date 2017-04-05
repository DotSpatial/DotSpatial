// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:35:25 PM
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
    /// This class contains predefined CoordinateSystems for Beijing1954.
    /// </summary>
    public class Beijing1954 : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Beijing19543DegreeGKCM102E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM105E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM108E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM111E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM114E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM117E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM120E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM123E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM126E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM129E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM132E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM135E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM75E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM78E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM81E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM84E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM87E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM90E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM93E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM96E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM99E;
        public readonly ProjectionInfo Beijing19543DegreeGKZone25;
        public readonly ProjectionInfo Beijing19543DegreeGKZone26;
        public readonly ProjectionInfo Beijing19543DegreeGKZone27;
        public readonly ProjectionInfo Beijing19543DegreeGKZone28;
        public readonly ProjectionInfo Beijing19543DegreeGKZone29;
        public readonly ProjectionInfo Beijing19543DegreeGKZone30;
        public readonly ProjectionInfo Beijing19543DegreeGKZone31;
        public readonly ProjectionInfo Beijing19543DegreeGKZone32;
        public readonly ProjectionInfo Beijing19543DegreeGKZone33;
        public readonly ProjectionInfo Beijing19543DegreeGKZone34;
        public readonly ProjectionInfo Beijing19543DegreeGKZone35;
        public readonly ProjectionInfo Beijing19543DegreeGKZone36;
        public readonly ProjectionInfo Beijing19543DegreeGKZone37;
        public readonly ProjectionInfo Beijing19543DegreeGKZone38;
        public readonly ProjectionInfo Beijing19543DegreeGKZone39;
        public readonly ProjectionInfo Beijing19543DegreeGKZone40;
        public readonly ProjectionInfo Beijing19543DegreeGKZone41;
        public readonly ProjectionInfo Beijing19543DegreeGKZone42;
        public readonly ProjectionInfo Beijing19543DegreeGKZone43;
        public readonly ProjectionInfo Beijing19543DegreeGKZone44;
        public readonly ProjectionInfo Beijing19543DegreeGKZone45;
        public readonly ProjectionInfo Beijing1954GKZone13;
        public readonly ProjectionInfo Beijing1954GKZone13N;
        public readonly ProjectionInfo Beijing1954GKZone14;
        public readonly ProjectionInfo Beijing1954GKZone14N;
        public readonly ProjectionInfo Beijing1954GKZone15;
        public readonly ProjectionInfo Beijing1954GKZone15N;
        public readonly ProjectionInfo Beijing1954GKZone16;
        public readonly ProjectionInfo Beijing1954GKZone16N;
        public readonly ProjectionInfo Beijing1954GKZone17;
        public readonly ProjectionInfo Beijing1954GKZone17N;
        public readonly ProjectionInfo Beijing1954GKZone18;
        public readonly ProjectionInfo Beijing1954GKZone18N;
        public readonly ProjectionInfo Beijing1954GKZone19;
        public readonly ProjectionInfo Beijing1954GKZone19N;
        public readonly ProjectionInfo Beijing1954GKZone20;
        public readonly ProjectionInfo Beijing1954GKZone20N;
        public readonly ProjectionInfo Beijing1954GKZone21;
        public readonly ProjectionInfo Beijing1954GKZone21N;
        public readonly ProjectionInfo Beijing1954GKZone22;
        public readonly ProjectionInfo Beijing1954GKZone22N;
        public readonly ProjectionInfo Beijing1954GKZone23;
        public readonly ProjectionInfo Beijing1954GKZone23N;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Beijing1954.
        /// </summary>
        public Beijing1954()
        {
            Beijing19543DegreeGKCM102E = ProjectionInfo.FromEpsgCode(2431).SetNames("Beijing_1954_3_Degree_GK_CM_102E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKCM105E = ProjectionInfo.FromEpsgCode(2432).SetNames("Beijing_1954_3_Degree_GK_CM_105E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKCM108E = ProjectionInfo.FromEpsgCode(2433).SetNames("Beijing_1954_3_Degree_GK_CM_108E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKCM111E = ProjectionInfo.FromEpsgCode(2434).SetNames("Beijing_1954_3_Degree_GK_CM_111E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKCM114E = ProjectionInfo.FromEpsgCode(2435).SetNames("Beijing_1954_3_Degree_GK_CM_114E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKCM117E = ProjectionInfo.FromEpsgCode(2436).SetNames("Beijing_1954_3_Degree_GK_CM_117E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKCM120E = ProjectionInfo.FromEpsgCode(2437).SetNames("Beijing_1954_3_Degree_GK_CM_120E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKCM123E = ProjectionInfo.FromEpsgCode(2438).SetNames("Beijing_1954_3_Degree_GK_CM_123E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKCM126E = ProjectionInfo.FromEpsgCode(2439).SetNames("Beijing_1954_3_Degree_GK_CM_126E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKCM129E = ProjectionInfo.FromEpsgCode(2440).SetNames("Beijing_1954_3_Degree_GK_CM_129E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKCM132E = ProjectionInfo.FromEpsgCode(2441).SetNames("Beijing_1954_3_Degree_GK_CM_132E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKCM135E = ProjectionInfo.FromEpsgCode(2442).SetNames("Beijing_1954_3_Degree_GK_CM_135E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKCM75E = ProjectionInfo.FromEpsgCode(2422).SetNames("Beijing_1954_3_Degree_GK_CM_75E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKCM78E = ProjectionInfo.FromEpsgCode(2423).SetNames("Beijing_1954_3_Degree_GK_CM_78E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKCM81E = ProjectionInfo.FromEpsgCode(2424).SetNames("Beijing_1954_3_Degree_GK_CM_81E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKCM84E = ProjectionInfo.FromEpsgCode(2425).SetNames("Beijing_1954_3_Degree_GK_CM_84E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKCM87E = ProjectionInfo.FromEpsgCode(2426).SetNames("Beijing_1954_3_Degree_GK_CM_87E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKCM90E = ProjectionInfo.FromEpsgCode(2427).SetNames("Beijing_1954_3_Degree_GK_CM_90E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKCM93E = ProjectionInfo.FromEpsgCode(2428).SetNames("Beijing_1954_3_Degree_GK_CM_93E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKCM96E = ProjectionInfo.FromEpsgCode(2429).SetNames("Beijing_1954_3_Degree_GK_CM_96E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKCM99E = ProjectionInfo.FromEpsgCode(2430).SetNames("Beijing_1954_3_Degree_GK_CM_99E", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone25 = ProjectionInfo.FromEpsgCode(2401).SetNames("Beijing_1954_3_Degree_GK_Zone_25", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone26 = ProjectionInfo.FromEpsgCode(2402).SetNames("Beijing_1954_3_Degree_GK_Zone_26", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone27 = ProjectionInfo.FromEpsgCode(2403).SetNames("Beijing_1954_3_Degree_GK_Zone_27", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone28 = ProjectionInfo.FromEpsgCode(2404).SetNames("Beijing_1954_3_Degree_GK_Zone_28", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone29 = ProjectionInfo.FromEpsgCode(2405).SetNames("Beijing_1954_3_Degree_GK_Zone_29", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone30 = ProjectionInfo.FromEpsgCode(2406).SetNames("Beijing_1954_3_Degree_GK_Zone_30", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone31 = ProjectionInfo.FromEpsgCode(2407).SetNames("Beijing_1954_3_Degree_GK_Zone_31", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone32 = ProjectionInfo.FromEpsgCode(2408).SetNames("Beijing_1954_3_Degree_GK_Zone_32", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone33 = ProjectionInfo.FromEpsgCode(2409).SetNames("Beijing_1954_3_Degree_GK_Zone_33", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone34 = ProjectionInfo.FromEpsgCode(2410).SetNames("Beijing_1954_3_Degree_GK_Zone_34", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone35 = ProjectionInfo.FromEpsgCode(2411).SetNames("Beijing_1954_3_Degree_GK_Zone_35", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone36 = ProjectionInfo.FromEpsgCode(2412).SetNames("Beijing_1954_3_Degree_GK_Zone_36", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone37 = ProjectionInfo.FromEpsgCode(2413).SetNames("Beijing_1954_3_Degree_GK_Zone_37", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone38 = ProjectionInfo.FromEpsgCode(2414).SetNames("Beijing_1954_3_Degree_GK_Zone_38", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone39 = ProjectionInfo.FromEpsgCode(2415).SetNames("Beijing_1954_3_Degree_GK_Zone_39", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone40 = ProjectionInfo.FromEpsgCode(2416).SetNames("Beijing_1954_3_Degree_GK_Zone_40", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone41 = ProjectionInfo.FromEpsgCode(2417).SetNames("Beijing_1954_3_Degree_GK_Zone_41", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone42 = ProjectionInfo.FromEpsgCode(2418).SetNames("Beijing_1954_3_Degree_GK_Zone_42", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone43 = ProjectionInfo.FromEpsgCode(2419).SetNames("Beijing_1954_3_Degree_GK_Zone_43", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone44 = ProjectionInfo.FromEpsgCode(2420).SetNames("Beijing_1954_3_Degree_GK_Zone_44", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing19543DegreeGKZone45 = ProjectionInfo.FromEpsgCode(2421).SetNames("Beijing_1954_3_Degree_GK_Zone_45", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone13 = ProjectionInfo.FromEpsgCode(21413).SetNames("Beijing_1954_GK_Zone_13", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone13N = ProjectionInfo.FromEpsgCode(21473).SetNames("Beijing_1954_GK_Zone_13N", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone14 = ProjectionInfo.FromEpsgCode(21414).SetNames("Beijing_1954_GK_Zone_14", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone14N = ProjectionInfo.FromEpsgCode(21474).SetNames("Beijing_1954_GK_Zone_14N", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone15 = ProjectionInfo.FromEpsgCode(21415).SetNames("Beijing_1954_GK_Zone_15", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone15N = ProjectionInfo.FromEpsgCode(21475).SetNames("Beijing_1954_GK_Zone_15N", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone16 = ProjectionInfo.FromEpsgCode(21416).SetNames("Beijing_1954_GK_Zone_16", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone16N = ProjectionInfo.FromEpsgCode(21476).SetNames("Beijing_1954_GK_Zone_16N", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone17 = ProjectionInfo.FromEpsgCode(21417).SetNames("Beijing_1954_GK_Zone_17", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone17N = ProjectionInfo.FromEpsgCode(21477).SetNames("Beijing_1954_GK_Zone_17N", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone18 = ProjectionInfo.FromEpsgCode(21418).SetNames("Beijing_1954_GK_Zone_18", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone18N = ProjectionInfo.FromEpsgCode(21478).SetNames("Beijing_1954_GK_Zone_18N", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone19 = ProjectionInfo.FromEpsgCode(21419).SetNames("Beijing_1954_GK_Zone_19", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone19N = ProjectionInfo.FromEpsgCode(21479).SetNames("Beijing_1954_GK_Zone_19N", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone20 = ProjectionInfo.FromEpsgCode(21420).SetNames("Beijing_1954_GK_Zone_20", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone20N = ProjectionInfo.FromEpsgCode(21480).SetNames("Beijing_1954_GK_Zone_20N", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone21 = ProjectionInfo.FromEpsgCode(21421).SetNames("Beijing_1954_GK_Zone_21", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone21N = ProjectionInfo.FromEpsgCode(21481).SetNames("Beijing_1954_GK_Zone_21N", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone22 = ProjectionInfo.FromEpsgCode(21422).SetNames("Beijing_1954_GK_Zone_22", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone22N = ProjectionInfo.FromEpsgCode(21482).SetNames("Beijing_1954_GK_Zone_22N", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone23 = ProjectionInfo.FromEpsgCode(21423).SetNames("Beijing_1954_GK_Zone_23", "GCS_Beijing_1954", "D_Beijing_1954");
            Beijing1954GKZone23N = ProjectionInfo.FromEpsgCode(21483).SetNames("Beijing_1954_GK_Zone_23N", "GCS_Beijing_1954", "D_Beijing_1954");
        }

        #endregion
    }
}

#pragma warning restore 1591