// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// This class contains details on how to serialize a field or property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class SerializeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializeAttribute"/> class.
        /// </summary>
        /// <param name="name">The name to use when serializing the associated member.</param>
        public SerializeAttribute(string name)
        {
            Name = name;
            ConstructorArgumentIndex = -1;
        }

        /// <summary>
        /// Gets the name to use when serializing the associated member.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the constructor argument index that the associated member represents.
        /// The default value is -1, which indicates that the associated member is
        /// not used as a constructor argument.
        /// </summary>
        public int ConstructorArgumentIndex { get; set; }

        /// <summary>
        /// Gets or sets the type of the formatter to use for the associated value.
        /// The type must be derived from <see cref="SerializationFormatter"/>.
        /// </summary>
        public Type Formatter { get; set; }
    }
}