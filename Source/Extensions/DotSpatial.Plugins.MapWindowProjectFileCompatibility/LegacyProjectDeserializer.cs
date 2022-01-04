// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Controls;

namespace DotSpatial.Plugins.MapWindowProjectFileCompatibility
{
    /// <summary>
    /// Used to Deserialize mwprj files from MapWindow 4 and earlier.
    /// </summary>
    public class LegacyProjectDeserializer
    {
        #region Fields

        private readonly IMap _map;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LegacyProjectDeserializer"/> class.
        /// </summary>
        /// <param name="map">The map.</param>
        public LegacyProjectDeserializer(IMap map)
        {
            _map = map;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Opens the MW4 style project file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void OpenFile(string fileName)
        {
            dynamic parser = DynamicXMLNode.Load(fileName);

            switch ((string)parser["type"])
            {
                case "projectfile":
                    new ProjectFileVer1Deserializer(_map).Deserialize(parser);
                    break;
                case "projectfile.2":
                    new ProjectFileVer2Deserializer(_map).Deserialize(parser);
                    break;
                default:
                    throw new Exception("Unknown project file format");
            }
        }

        #endregion
    }
}