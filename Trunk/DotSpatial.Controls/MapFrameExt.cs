// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from DotSpatial.Controls.dll version 1.0.2011.2
//
// The Initial Developer of this Original Code is Jiri Kadlec. Created 2/15/2011 9:30:52 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Extension methods for accessing the layers (including nested layers) and groups (including nested groups) of the
    /// map frame
    /// </summary>
    public static class MapFrameExt
    {
        /// <summary>
        /// Gets all layers of the map frame including layers which are nested
        /// within groups. The group objects themselves are not included in this list,
        /// but all FeatureLayers, RasterLayers, ImageLayers and other layers are included.
        /// </summary>
        /// <returns>The list of the layers</returns>
        public static List<ILayer> GetAllLayers(this IMapFrame mapFrame)
        {
            List<ILayer> layerList = new List<ILayer>();
            GetNestedLayers(mapFrame, layerList);
            return layerList;
        }

        private static void GetNestedLayers(IMapGroup grp, List<ILayer> layerList)
        {
            //initialize the layer list if required
            if (layerList == null) layerList = new List<ILayer>();

            //recursive function -- all nested groups and layers are considered
            foreach (IMapLayer lyr in grp.Layers)
            {
                grp = lyr as IMapGroup;
                if (grp != null)
                {
                    GetNestedLayers(grp, layerList);
                }
                else
                {
                    layerList.Add(lyr);
                }
            }
        }

        /// <summary>
        /// Gets all map groups in the map including the nested groups
        /// </summary>
        /// <returns>the list of the groups</returns>
        public static List<IMapGroup> GetAllGroups(this IMapFrame mapFrame)
        {
            List<IMapGroup> groupList = new List<IMapGroup>();
            GetNestedGroups(mapFrame, groupList);
            return groupList;
        }

        private static void GetNestedGroups(IMapGroup grp, List<IMapGroup> groupList)
        {
            //initialize the layer list if required
            if (groupList == null) groupList = new List<IMapGroup>();

            //recursive function -- all nested groups and layers are considered
            foreach (ILayer lyr in grp.Layers)
            {
                grp = lyr as IMapGroup;
                if (grp != null)
                {
                    GetNestedGroups(grp, groupList);
                    groupList.Add(grp);
                }
            }
        }
    }
}