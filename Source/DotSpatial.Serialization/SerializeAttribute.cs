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
            UseCase = UseCases.ConstructorOnly;
        }

        /// <summary>
        /// UseCases contains all the cases in which a constructor property can be used.
        /// </summary>
        public enum UseCases
        {
            /// <summary>
            /// Use the property only as an argument of the constructor.
            /// </summary>
            ConstructorOnly = 0,

            /// <summary>
            /// Use the property only as an argument of the static method.
            /// </summary>
            StaticMethodOnly = 1,

            /// <summary>
            /// Use the property as an argument for the constructor as well as for the static method.
            /// </summary>
            Both = 2
        }

        /// <summary>
        /// Gets the name to use when serializing the associated member.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the constructor argument index that the associated member represents.
        /// The default value is -1, which indicates that the associated member is not used as a constructor argument.
        /// If this value is larger than 0 and no UseCase is set ConstructorOnly is automatically assumed for backwards compatibility.
        /// </summary>
        public int ConstructorArgumentIndex { get; set; }

        /// <summary>
        /// Gets or sets the type of the formatter to use for the associated value.
        /// The type must be derived from <see cref="SerializationFormatter"/>.
        /// </summary>
        public Type Formatter { get; set; }

        /// <summary>
        /// Gets or sets the name of the static method that can be used to construct a new instance in case of classes that are not able to construct a working version of themselves in the constructor.
        /// This should be attached to a boolean property that also indicates whether the static constructor should be used via IndicatesStaticConstructor.
        /// </summary>
        public string StaticConstructorMethodName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the static method should be used to construct the instance. This may only be used on a single boolean property. Also add the StaticConstructorMethodName to the same boolean property.
        /// </summary>
        public bool UseStaticConstructor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the argument can be used as constructor argument for the static method only, the constructor only, in both cases or not at all.
        /// The None case should only be set if ConstructorArgumentIndex is -1.
        /// </summary>
        public UseCases UseCase { get; set; }
    }
}