// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:23:00 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.GeographicCategories.SolarSystem
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for Uranus.
    /// </summary>
    public class Uranus : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Ariel2000;
        public readonly ProjectionInfo Belinda2000;
        public readonly ProjectionInfo Bianca2000;
        public readonly ProjectionInfo Cordelia2000;
        public readonly ProjectionInfo Cressida2000;
        public readonly ProjectionInfo Desdemona2000;
        public readonly ProjectionInfo Juliet2000;
        public readonly ProjectionInfo Miranda2000;
        public readonly ProjectionInfo Oberon2000;
        public readonly ProjectionInfo Ophelia2000;
        public readonly ProjectionInfo Portia2000;
        public readonly ProjectionInfo Puck2000;
        public readonly ProjectionInfo Rosalind2000;
        public readonly ProjectionInfo Titania2000;
        public readonly ProjectionInfo Umbriel2000;
        public readonly ProjectionInfo Uranus2000;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Uranus.
        /// </summary>
        public Uranus()
        {
            Ariel2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104945).SetNames("", "GCS_Ariel_2000", "D_Ariel_2000"); // missing
            Belinda2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104946).SetNames("", "GCS_Belinda_2000", "D_Belinda_2000"); // missing
            Bianca2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104947).SetNames("", "GCS_Bianca_2000", "D_Bianca_2000"); // missing
            Cordelia2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104948).SetNames("", "GCS_Cordelia_2000", "D_Cordelia_2000"); // missing
            Cressida2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104949).SetNames("", "GCS_Cressida_2000", "D_Cressida_2000"); // missing
            Desdemona2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104950).SetNames("", "GCS_Desdemona_2000", "D_Desdemona_2000"); // missing
            Juliet2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104951).SetNames("", "GCS_Juliet_2000", "D_Juliet_2000"); // missing
            Miranda2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104952).SetNames("", "GCS_Miranda_2000", "D_Miranda_2000"); // missing
            Oberon2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104953).SetNames("", "GCS_Oberon_2000", "D_Oberon_2000"); // missing
            Ophelia2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104954).SetNames("", "GCS_Ophelia_2000", "D_Ophelia_2000"); // missing
            Portia2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104955).SetNames("", "GCS_Portia_2000", "D_Portia_2000"); // missing
            Puck2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104956).SetNames("", "GCS_Puck_2000", "D_Puck_2000"); // missing
            Rosalind2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104957).SetNames("", "GCS_Rosalind_2000", "D_Rosalind_2000"); // missing
            Titania2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104958).SetNames("", "GCS_Titania_2000", "D_Titania_2000"); // missing
            Umbriel2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104959).SetNames("", "GCS_Umbriel_2000", "D_Umbriel_2000"); // missing
            Uranus2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104944).SetNames("", "GCS_Uranus_2000", "D_Uranus_2000"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591