// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Normally, cloning an object starts with MemberwiseClone, which
    /// creates a shallow copy of the object. For any members that
    /// derive from the Descriptor, however, any public properties
    /// or fields that implement ICloneable are copied (deep copy behavior).
    /// This is not always desirable, even if the member CAN be copied.
    /// This attribute causes the deep copy behavior to skip over
    /// properties marked with this attribute.
    /// </summary>
    public class ShallowCopy : Attribute
    {
    }
}