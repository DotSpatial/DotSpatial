using System.Collections.Generic;
using System.Linq;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Contains extension methods for layer collections.
    /// </summary>
    public static class LayerCollectionExensions
    {
        /// <summary>
        /// Given a base name, this increments a number for appending
        /// if the name already exists in the collection.
        /// </summary>
        /// <param name="layers">Layers collection.</param>
        /// <param name="baseName">The string base name to start with</param>
        /// <returns>The base name modified by a number making it unique in the collection</returns>
        public static string UnusedName(this ICollection<ILayer> layers, string baseName)
        {
            int i = 1;
            string name = baseName;
            bool duplicateExists = layers.Any(item => item.LegendText == baseName);
            while (duplicateExists)
            {
                duplicateExists = false;
                name = baseName + i;
                foreach (ILayer item in layers)
                {
                    if (item.LegendText != name) continue;
                    duplicateExists = true;
                    break;
                }
                i++;
            }
            return name;
        }

        public static string UnusedName(this IEnumerable<ILayer> layers, string baseName)
        {
            int i = 1;
            string name = baseName;
            bool duplicateExists = layers.Any(item => item.LegendText == baseName);
            while (duplicateExists)
            {
                duplicateExists = false;
                name = baseName + i;
                foreach (ILayer item in layers)
                {
                    if (item.LegendText != name) continue;
                    duplicateExists = true;
                    break;
                }
                i++;
            }
            return name;
        }
    }
}