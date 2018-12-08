// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// Manages the Default Mappings of Fields
    /// </summary>
    public class FieldTypeCharacterMapperManager
    {
        /// <summary>
        /// Gets or sets the IFieldTypeCharacterMapper that the Field will use to Map a Type to a FieldTypeCharacter.
        /// </summary>
        public static IFieldTypeCharacterMapper Mapper { get; set; } = new FieldTypeCharacterMapper();
    }
}
