// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:41:10 PM
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
    /// This class contains predefined CoordinateSystems for Xian1980.
    /// </summary>
    public class Xian1980 : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Xian19803DegreeGKCM102E;
        public readonly ProjectionInfo Xian19803DegreeGKCM105E;
        public readonly ProjectionInfo Xian19803DegreeGKCM108E;
        public readonly ProjectionInfo Xian19803DegreeGKCM111E;
        public readonly ProjectionInfo Xian19803DegreeGKCM114E;
        public readonly ProjectionInfo Xian19803DegreeGKCM117E;
        public readonly ProjectionInfo Xian19803DegreeGKCM120E;
        public readonly ProjectionInfo Xian19803DegreeGKCM123E;
        public readonly ProjectionInfo Xian19803DegreeGKCM126E;
        public readonly ProjectionInfo Xian19803DegreeGKCM129E;
        public readonly ProjectionInfo Xian19803DegreeGKCM132E;
        public readonly ProjectionInfo Xian19803DegreeGKCM135E;
        public readonly ProjectionInfo Xian19803DegreeGKCM75E;
        public readonly ProjectionInfo Xian19803DegreeGKCM78E;
        public readonly ProjectionInfo Xian19803DegreeGKCM81E;
        public readonly ProjectionInfo Xian19803DegreeGKCM84E;
        public readonly ProjectionInfo Xian19803DegreeGKCM87E;
        public readonly ProjectionInfo Xian19803DegreeGKCM90E;
        public readonly ProjectionInfo Xian19803DegreeGKCM93E;
        public readonly ProjectionInfo Xian19803DegreeGKCM96E;
        public readonly ProjectionInfo Xian19803DegreeGKCM99E;
        public readonly ProjectionInfo Xian19803DegreeGKZone25;
        public readonly ProjectionInfo Xian19803DegreeGKZone26;
        public readonly ProjectionInfo Xian19803DegreeGKZone27;
        public readonly ProjectionInfo Xian19803DegreeGKZone28;
        public readonly ProjectionInfo Xian19803DegreeGKZone29;
        public readonly ProjectionInfo Xian19803DegreeGKZone30;
        public readonly ProjectionInfo Xian19803DegreeGKZone31;
        public readonly ProjectionInfo Xian19803DegreeGKZone32;
        public readonly ProjectionInfo Xian19803DegreeGKZone33;
        public readonly ProjectionInfo Xian19803DegreeGKZone34;
        public readonly ProjectionInfo Xian19803DegreeGKZone35;
        public readonly ProjectionInfo Xian19803DegreeGKZone36;
        public readonly ProjectionInfo Xian19803DegreeGKZone37;
        public readonly ProjectionInfo Xian19803DegreeGKZone38;
        public readonly ProjectionInfo Xian19803DegreeGKZone39;
        public readonly ProjectionInfo Xian19803DegreeGKZone40;
        public readonly ProjectionInfo Xian19803DegreeGKZone41;
        public readonly ProjectionInfo Xian19803DegreeGKZone42;
        public readonly ProjectionInfo Xian19803DegreeGKZone43;
        public readonly ProjectionInfo Xian19803DegreeGKZone44;
        public readonly ProjectionInfo Xian19803DegreeGKZone45;
        public readonly ProjectionInfo Xian1980GKCM105E;
        public readonly ProjectionInfo Xian1980GKCM111E;
        public readonly ProjectionInfo Xian1980GKCM117E;
        public readonly ProjectionInfo Xian1980GKCM123E;
        public readonly ProjectionInfo Xian1980GKCM129E;
        public readonly ProjectionInfo Xian1980GKCM135E;
        public readonly ProjectionInfo Xian1980GKCM75E;
        public readonly ProjectionInfo Xian1980GKCM81E;
        public readonly ProjectionInfo Xian1980GKCM87E;
        public readonly ProjectionInfo Xian1980GKCM93E;
        public readonly ProjectionInfo Xian1980GKCM99E;
        public readonly ProjectionInfo Xian1980GKZone13;
        public readonly ProjectionInfo Xian1980GKZone14;
        public readonly ProjectionInfo Xian1980GKZone15;
        public readonly ProjectionInfo Xian1980GKZone16;
        public readonly ProjectionInfo Xian1980GKZone17;
        public readonly ProjectionInfo Xian1980GKZone18;
        public readonly ProjectionInfo Xian1980GKZone19;
        public readonly ProjectionInfo Xian1980GKZone20;
        public readonly ProjectionInfo Xian1980GKZone21;
        public readonly ProjectionInfo Xian1980GKZone22;
        public readonly ProjectionInfo Xian1980GKZone23;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Xian1980.
        /// </summary>
        public Xian1980()
        {
            Xian19803DegreeGKCM102E = ProjectionInfo.FromEpsgCode(2379).SetNames("Xian_1980_3_Degree_GK_CM_102E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKCM105E = ProjectionInfo.FromEpsgCode(2380).SetNames("Xian_1980_3_Degree_GK_CM_105E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKCM108E = ProjectionInfo.FromEpsgCode(2381).SetNames("Xian_1980_3_Degree_GK_CM_108E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKCM111E = ProjectionInfo.FromEpsgCode(2382).SetNames("Xian_1980_3_Degree_GK_CM_111E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKCM114E = ProjectionInfo.FromEpsgCode(2383).SetNames("Xian_1980_3_Degree_GK_CM_114E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKCM117E = ProjectionInfo.FromEpsgCode(2384).SetNames("Xian_1980_3_Degree_GK_CM_117E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKCM120E = ProjectionInfo.FromEpsgCode(2385).SetNames("Xian_1980_3_Degree_GK_CM_120E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKCM123E = ProjectionInfo.FromEpsgCode(2386).SetNames("Xian_1980_3_Degree_GK_CM_123E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKCM126E = ProjectionInfo.FromEpsgCode(2387).SetNames("Xian_1980_3_Degree_GK_CM_126E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKCM129E = ProjectionInfo.FromEpsgCode(2388).SetNames("Xian_1980_3_Degree_GK_CM_129E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKCM132E = ProjectionInfo.FromEpsgCode(2389).SetNames("Xian_1980_3_Degree_GK_CM_132E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKCM135E = ProjectionInfo.FromEpsgCode(2390).SetNames("Xian_1980_3_Degree_GK_CM_135E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKCM75E = ProjectionInfo.FromEpsgCode(2370).SetNames("Xian_1980_3_Degree_GK_CM_75E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKCM78E = ProjectionInfo.FromEpsgCode(2371).SetNames("Xian_1980_3_Degree_GK_CM_78E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKCM81E = ProjectionInfo.FromEpsgCode(2372).SetNames("Xian_1980_3_Degree_GK_CM_81E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKCM84E = ProjectionInfo.FromEpsgCode(2373).SetNames("Xian_1980_3_Degree_GK_CM_84E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKCM87E = ProjectionInfo.FromEpsgCode(2374).SetNames("Xian_1980_3_Degree_GK_CM_87E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKCM90E = ProjectionInfo.FromEpsgCode(2375).SetNames("Xian_1980_3_Degree_GK_CM_90E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKCM93E = ProjectionInfo.FromEpsgCode(2376).SetNames("Xian_1980_3_Degree_GK_CM_93E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKCM96E = ProjectionInfo.FromEpsgCode(2377).SetNames("Xian_1980_3_Degree_GK_CM_96E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKCM99E = ProjectionInfo.FromEpsgCode(2378).SetNames("Xian_1980_3_Degree_GK_CM_99E", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone25 = ProjectionInfo.FromEpsgCode(2349).SetNames("Xian_1980_3_Degree_GK_Zone_25", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone26 = ProjectionInfo.FromEpsgCode(2350).SetNames("Xian_1980_3_Degree_GK_Zone_26", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone27 = ProjectionInfo.FromEpsgCode(2351).SetNames("Xian_1980_3_Degree_GK_Zone_27", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone28 = ProjectionInfo.FromEpsgCode(2352).SetNames("Xian_1980_3_Degree_GK_Zone_28", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone29 = ProjectionInfo.FromEpsgCode(2353).SetNames("Xian_1980_3_Degree_GK_Zone_29", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone30 = ProjectionInfo.FromEpsgCode(2354).SetNames("Xian_1980_3_Degree_GK_Zone_30", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone31 = ProjectionInfo.FromEpsgCode(2355).SetNames("Xian_1980_3_Degree_GK_Zone_31", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone32 = ProjectionInfo.FromEpsgCode(2356).SetNames("Xian_1980_3_Degree_GK_Zone_32", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone33 = ProjectionInfo.FromEpsgCode(2357).SetNames("Xian_1980_3_Degree_GK_Zone_33", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone34 = ProjectionInfo.FromEpsgCode(2358).SetNames("Xian_1980_3_Degree_GK_Zone_34", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone35 = ProjectionInfo.FromEpsgCode(2359).SetNames("Xian_1980_3_Degree_GK_Zone_35", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone36 = ProjectionInfo.FromEpsgCode(2360).SetNames("Xian_1980_3_Degree_GK_Zone_36", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone37 = ProjectionInfo.FromEpsgCode(2361).SetNames("Xian_1980_3_Degree_GK_Zone_37", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone38 = ProjectionInfo.FromEpsgCode(2362).SetNames("Xian_1980_3_Degree_GK_Zone_38", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone39 = ProjectionInfo.FromEpsgCode(2363).SetNames("Xian_1980_3_Degree_GK_Zone_39", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone40 = ProjectionInfo.FromEpsgCode(2364).SetNames("Xian_1980_3_Degree_GK_Zone_40", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone41 = ProjectionInfo.FromEpsgCode(2365).SetNames("Xian_1980_3_Degree_GK_Zone_41", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone42 = ProjectionInfo.FromEpsgCode(2366).SetNames("Xian_1980_3_Degree_GK_Zone_42", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone43 = ProjectionInfo.FromEpsgCode(2367).SetNames("Xian_1980_3_Degree_GK_Zone_43", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone44 = ProjectionInfo.FromEpsgCode(2368).SetNames("Xian_1980_3_Degree_GK_Zone_44", "GCS_Xian_1980", "D_Xian_1980");
            Xian19803DegreeGKZone45 = ProjectionInfo.FromEpsgCode(2369).SetNames("Xian_1980_3_Degree_GK_Zone_45", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKCM105E = ProjectionInfo.FromEpsgCode(2343).SetNames("Xian_1980_GK_CM_105E", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKCM111E = ProjectionInfo.FromEpsgCode(2344).SetNames("Xian_1980_GK_CM_111E", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKCM117E = ProjectionInfo.FromEpsgCode(2345).SetNames("Xian_1980_GK_CM_117E", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKCM123E = ProjectionInfo.FromEpsgCode(2346).SetNames("Xian_1980_GK_CM_123E", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKCM129E = ProjectionInfo.FromEpsgCode(2347).SetNames("Xian_1980_GK_CM_129E", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKCM135E = ProjectionInfo.FromEpsgCode(2348).SetNames("Xian_1980_GK_CM_135E", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKCM75E = ProjectionInfo.FromEpsgCode(2338).SetNames("Xian_1980_GK_CM_75E", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKCM81E = ProjectionInfo.FromEpsgCode(2339).SetNames("Xian_1980_GK_CM_81E", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKCM87E = ProjectionInfo.FromEpsgCode(2340).SetNames("Xian_1980_GK_CM_87E", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKCM93E = ProjectionInfo.FromEpsgCode(2341).SetNames("Xian_1980_GK_CM_93E", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKCM99E = ProjectionInfo.FromEpsgCode(2342).SetNames("Xian_1980_GK_CM_99E", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKZone13 = ProjectionInfo.FromEpsgCode(2327).SetNames("Xian_1980_GK_Zone_13", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKZone14 = ProjectionInfo.FromEpsgCode(2328).SetNames("Xian_1980_GK_Zone_14", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKZone15 = ProjectionInfo.FromEpsgCode(2329).SetNames("Xian_1980_GK_Zone_15", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKZone16 = ProjectionInfo.FromEpsgCode(2330).SetNames("Xian_1980_GK_Zone_16", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKZone17 = ProjectionInfo.FromEpsgCode(2331).SetNames("Xian_1980_GK_Zone_17", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKZone18 = ProjectionInfo.FromEpsgCode(2332).SetNames("Xian_1980_GK_Zone_18", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKZone19 = ProjectionInfo.FromEpsgCode(2333).SetNames("Xian_1980_GK_Zone_19", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKZone20 = ProjectionInfo.FromEpsgCode(2334).SetNames("Xian_1980_GK_Zone_20", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKZone21 = ProjectionInfo.FromEpsgCode(2335).SetNames("Xian_1980_GK_Zone_21", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKZone22 = ProjectionInfo.FromEpsgCode(2336).SetNames("Xian_1980_GK_Zone_22", "GCS_Xian_1980", "D_Xian_1980");
            Xian1980GKZone23 = ProjectionInfo.FromEpsgCode(2337).SetNames("Xian_1980_GK_Zone_23", "GCS_Xian_1980", "D_Xian_1980");
        }

        #endregion
    }
}

#pragma warning restore 1591