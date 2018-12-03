// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// Responsible for mapping a Type to a FieldTypeCharacter
    /// </summary>
    public interface IFieldTypeCharacterMapper
    {
        /// <summary>
        /// Maps a Type to a FieldTypeCharacter
        /// </summary>
        /// <param name="type">A Type to convert to the char FieldTypeCharacter</param>
        /// <returns>The FieldTypeCharacter assosiated with the given type.</returns>
        char Map(Type type);
    }
}