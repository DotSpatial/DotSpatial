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
//
// ********************************************************************************************************

using System;
using System.Reflection;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// A class that works like an enumeration to define several constants.
    /// </summary>
    public static class XmlConstants
    {
        // Element names
        /// <summary>
        /// dictionary
        /// </summary>
        public const string DICTIONARY = "dictionary";
        /// <summary>
        /// entry
        /// </summary>
        public const string DICTIONARY_ENTRY = "entry";
        /// <summary>
        /// key
        /// </summary>
        public const string DICTIONARY_KEY = "key";
        /// <summary>
        /// value
        /// </summary>
        public const string DICTIONARY_VALUE = "value";
        /// <summary>
        /// enum
        /// </summary>
        public const string ENUM = "enum";
        /// <summary>
        /// list
        /// </summary>
        public const string LIST = "list";
        /// <summary>
        /// object
        /// </summary>
        public const string OBJECT = "object";
        /// <summary>
        /// primitive
        /// </summary>
        public const string PRIMITIVE = "primitive";
        /// <summary>
        /// string
        /// </summary>
        public const string STRING = "string";
        /// <summary>
        /// root
        /// </summary>
        public const string ROOT = "root";
        /// <summary>
        /// member
        /// </summary>
        public const string MEMBER = "member";
        /// <summary>
        /// item
        /// </summary>
        public const string ITEM = "item";
        /// <summary>
        /// types
        /// </summary>
        public const string TYPE_CACHE = "types";

        // Attribute names
        /// <summary>
        /// type
        /// </summary>
        public const string TYPE_ID = "type";
        /// <summary>
        /// value
        /// </summary>
        public const string VALUE = "value";
        /// <summary>
        /// arg
        /// </summary>
        public const string ARG = "arg";
        /// <summary>
        /// formatter
        /// </summary>
        public const string FORMATTER = "formatter";
        /// <summary>
        /// name
        /// </summary>
        public const string NAME = "name";
        /// <summary>
        /// key
        /// </summary>
        public const string KEY = "key";

        // Object reference constants
        /// <summary>
        /// id
        /// </summary>
        public const string ID = "id";
        /// <summary>
        /// ref
        /// </summary>
        public const string REF = "ref";

        /// <summary>
        /// Returns a System.Type that corresponds to the MemberInfo,
        /// regardless of whether the member is a field or property.
        /// </summary>
        /// <param name="memberInfo">The base class that can be either a FieldInfo or PropertyInfo</param>
        /// <returns>The System.Type</returns>
        public static Type GetMemberType(MemberInfo memberInfo)
        {
            if (memberInfo is PropertyInfo)
                return ((PropertyInfo)memberInfo).PropertyType;

            return ((FieldInfo)memberInfo).FieldType;
        }
    }
}