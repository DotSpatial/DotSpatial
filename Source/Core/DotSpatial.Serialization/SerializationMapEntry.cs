// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Reflection;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// Contains the information needed to serialize a single property or field.
    /// </summary>
    public class SerializationMapEntry
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationMapEntry"/> class.
        /// </summary>
        /// <param name="memberInfo">Info about the field or property to serialize.</param>
        /// <param name="attribute">Details about how to serialize this member.</param>
        public SerializationMapEntry(MemberInfo memberInfo, SerializeAttribute attribute)
        {
            Member = memberInfo;
            Attribute = attribute;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the attribute that contains more details about how to serialize this member.
        /// </summary>
        public SerializeAttribute Attribute { get; private set; }

        /// <summary>
        /// Gets the <see cref="FieldInfo"/> or <see cref="PropertyInfo"/> instance for the associated member.
        /// </summary>
        public MemberInfo Member { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Assigns the constructor argument index of this entry's attribute.
        /// </summary>
        /// <param name="index">The constructor argument index to assign.</param>
        /// <returns>The SerializationMapEntry.</returns>
        public SerializationMapEntry AsConstructorArgument(int index)
        {
            Attribute.ConstructorArgumentIndex = index;
            return this;
        }

        /// <summary>
        /// Overrides the normal equality test to take into account serialization.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if both are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj is not SerializationMapEntry other) return false;

            return other.Member == Member && other.Attribute == Attribute;
        }

        /// <summary>
        /// Returns a numeric hash code austensibly uniquishly controlled by the member and attribute hash code.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return Member.GetHashCode() ^ Attribute.GetHashCode();
        }

        /// <summary>
        /// Assigns the formatter value of this entry's attribute.
        /// </summary>
        /// <param name="formatterType">The serialization formatter to use for the associated member.</param>
        /// <returns>The SerializationMapEntry.</returns>
        public SerializationMapEntry WithFormatterType(Type formatterType)
        {
            Attribute.Formatter = formatterType;
            return this;
        }

        #endregion
    }
}