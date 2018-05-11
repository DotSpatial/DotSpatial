using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotSpatial.Data
{
    /// <summary>
    /// Manages the Default Mappings of Fields
    /// </summary>
    public class FieldTypeCharacterMapperManager
    {
        /// <summary>
        /// The IFieldTypeCharacterMapper that the Field will use to Map a Type to a FieldTypeCharacter
        /// </summary>
        public static IFieldTypeCharacterMapper Mapper { get; set; } = new FieldTypeCharacterMapper();
    }
}
