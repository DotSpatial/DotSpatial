using System.Collections.Generic;
using System.Linq;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Contains extension methods for layer collections.
    /// </summary>
    public static class LayerCollectionExensions
    {
        #region Methods

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

        /// <summary>
        /// Finds a name that is based on baseName and is not yet used for an existing layer.
        /// </summary>
        /// <param name="layers">Layers to check for the name.</param>
        /// <param name="baseName">Name that should be checked.</param>
        /// <returns>The name if it is not yet used, otherwise the name combined with a number.</returns>
        public static string UnusedName(this IEnumerable<ILayer> layers, string baseName)
        {
            int i = 1;
            string name = baseName;
            var layerList = layers as IList<ILayer> ?? layers.ToList();
            bool duplicateExists = layerList.Any(item => item.LegendText == baseName);
            while (duplicateExists)
            {
                duplicateExists = false;
                name = baseName + i;
                foreach (ILayer item in layerList)
                {
                    if (item.LegendText != name) continue;

                    duplicateExists = true;
                    break;
                }

                i++;
            }

            return name;
        }

        #endregion
    }
}