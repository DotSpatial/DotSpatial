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

namespace DotSpatial.Serialization
{
    /// <summary>
    /// This class contains details on how to serialize a field or property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class SerializeAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance of the SerializeAttribute.
        /// </summary>
        /// <param name="name">The name to use when serializing the associated member.</param>
        public SerializeAttribute(string name)
        {
            Name = name;
            ConstructorArgumentIndex = -1;
        }

        /// <summary>
        /// The name to use when serializing the associated member.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The constructor argument index that the associated member represents.
        /// The default value is -1, which indicates that the associated member is
        /// not used as a constructor argument.
        /// </summary>
        public int ConstructorArgumentIndex { get; set; }

        /// <summary>
        /// The type of the formatter to use for the associated value.
        /// The type must be derived from <see cref="SerializationFormatter"/>.
        /// </summary>
        public Type Formatter { get; set; }
    }
}