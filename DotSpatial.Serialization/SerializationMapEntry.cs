// ********************************************************************************************************
// Product Name: DotSpatial.Serialization.dll
// Description:  A module that supports common functions like serialization.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Darrel Brown. Created 9/10/2009
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |-----------------|---------|---------------------------------------------------------------------
// |      Name       |  Date   |                        Comments
// |-----------------|---------|----------------------------------------------------------------------
//
// ********************************************************************************************************

using System;
using System.Reflection;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// Contains the information needed to serialize a single property or field.
    /// </summary>
    public class SerializationMapEntry
    {
        /// <summary>
        /// Creates a new SerializationMapEntry instance.
        /// </summary>
        /// <param name="memberInfo">Info about the field or property to serialize.</param>
        /// <param name="attribute">Details about how to serialize this member.</param>
        public SerializationMapEntry(MemberInfo memberInfo, SerializeAttribute attribute)
        {
            Member = memberInfo;
            Attribute = attribute;
        }

        /// <summary>
        /// The <see cref="FieldInfo"/> or <see cref="PropertyInfo"/> instance for the associated member.
        /// </summary>
        public MemberInfo Member { get; private set; }

        /// <summary>
        /// The attribute that contains more details about how to serialize this member.
        /// </summary>
        public SerializeAttribute Attribute { get; private set; }

        /// <summary>
        /// Assigns the constructor argument index of this entry's attribute.
        /// </summary>
        /// <param name="index">The constructor argument index to assign</param>
        public SerializationMapEntry AsConstructorArgument(int index)
        {
            Attribute.ConstructorArgumentIndex = index;
            return this;
        }

        /// <summary>
        /// Assigns the formatter value of this entry's attribute.
        /// </summary>
        /// <param name="formatterType">The serialization formatter to use for the associated member.</param>
        public SerializationMapEntry WithFormatterType(Type formatterType)
        {
            Attribute.Formatter = formatterType;
            return this;
        }

        /// <summary>
        /// Overrides the normal equality test to take into account serialization
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var other = obj as SerializationMapEntry;
            if (other == null)
                return false;

            return other.Member == Member && other.Attribute == Attribute;
        }

        /// <summary>
        /// Returns a numeric hash code austensibly uniquishly controlled by the member and attribute hash code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Member.GetHashCode() ^ Attribute.GetHashCode();
        }
    }
}