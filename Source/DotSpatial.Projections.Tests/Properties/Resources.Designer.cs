﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DotSpatial.Projections.Tests.Properties {
    using System;
    
    
    /// <summary>
    ///   Une classe de ressource fortement typée destinée, entre autres, à la consultation des chaînes localisées.
    /// </summary>
    // Cette classe a été générée automatiquement par la classe StronglyTypedResourceBuilder
    // à l'aide d'un outil, tel que ResGen ou Visual Studio.
    // Pour ajouter ou supprimer un membre, modifiez votre fichier .ResX, puis réexécutez ResGen
    // avec l'option /str ou régénérez votre projet VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Retourne l'instance ResourceManager mise en cache utilisée par cette classe.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DotSpatial.Projections.Tests.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Remplace la propriété CurrentUICulture du thread actuel pour toutes
        ///   les recherches de ressources à l'aide de cette classe de ressource fortement typée.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à PROJCS[&quot;WGS_1984_Web_Mercator_Auxiliary_Sphere&quot;,
        ///	GEOGCS[&quot;GCS_WGS_1984&quot;,
        ///		DATUM[&quot;D_WGS_1984&quot;,
        ///			SPHEROID[&quot;WGS_1984&quot;, 6378137, 298.257223562997]
        ///		],
        ///		PRIMEM[&quot;Greenwich&quot;, 0],
        ///		UNIT[&quot;Degree&quot;, 0.0174532925199433]
        ///	],
        ///	PROJECTION[&quot;Mercator_Auxiliary_Sphere&quot;],
        ///	PARAMETER[&quot;False_Easting&quot;, 0],
        ///	PARAMETER[&quot;False_Northing&quot;, 0],
        ///	PARAMETER[&quot;Central_Meridian&quot;, 0],
        ///	PARAMETER[&quot;Standard_Parallel_1&quot;, 0],
        ///	PARAMETER[&quot;Scale_Factor&quot;, 1],
        ///	PARAMETER[&quot;Auxiliary_Sphere_Type&quot;, 0.0],
        ///	UNIT[&quot;Meter&quot;, 1]
        ///]
        ///.
        /// </summary>
        internal static string FormattedProjectionFile {
            get {
                return ResourceManager.GetString("FormattedProjectionFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à PROJCS[&quot;WGS_1984_Web_Mercator_Auxiliary_Sphere&quot;,GEOGCS[&quot;GCS_WGS_1984&quot;,DATUM[&quot;D_WGS_1984&quot;,SPHEROID[&quot;WGS_1984&quot;,6378137,298.257223562997]],PRIMEM[&quot;Greenwich&quot;,0],UNIT[&quot;Degree&quot;,0.0174532925199433]], PROJECTION[&quot;Mercator_Auxiliary_Sphere&quot;],PARAMETER[&quot;False_Easting&quot;,0],PARAMETER[&quot;False_Northing&quot;,0],PARAMETER[&quot;Central_Meridian&quot;,0],PARAMETER[&quot;Standard_Parallel_1&quot;,0],PARAMETER[&quot;Scale_Factor&quot;,1],PARAMETER[&quot;Auxiliary_Sphere_Type&quot;,0.0],UNIT[&quot;Meter&quot;,1]]
        ///.
        /// </summary>
        internal static string StandardProjectionFile {
            get {
                return ResourceManager.GetString("StandardProjectionFile", resourceCulture);
            }
        }
    }
}
