// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Reflection;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// A class that works like an enumeration to define several constants.
    /// </summary>
    public static class XmlConstants
    {
        #region Fields

        /// <summary>
        /// arg
        /// </summary>
        public const string Arg = "arg";

        /// <summary>
        /// dictionary
        /// </summary>
        public const string Dictionary = "dictionary";

        /// <summary>
        /// entry
        /// </summary>
        public const string DictionaryEntry = "entry";

        /// <summary>
        /// key
        /// </summary>
        public const string DictionaryKey = "key";

        /// <summary>
        /// value
        /// </summary>
        public const string DictionaryValue = "value";

        /// <summary>
        /// enum
        /// </summary>
        public const string Enum = "enum";

        /// <summary>
        /// formatter
        /// </summary>
        public const string Formatter = "formatter";

        /// <summary>
        /// id
        /// </summary>
        public const string Id = "id";

        /// <summary>
        /// item
        /// </summary>
        public const string Item = "item";

        /// <summary>
        /// key
        /// </summary>
        public const string Key = "key";

        /// <summary>
        /// list
        /// </summary>
        public const string List = "list";

        /// <summary>
        /// member
        /// </summary>
        public const string Member = "member";

        /// <summary>
        /// name
        /// </summary>
        public const string Name = "name";

        /// <summary>
        /// object
        /// </summary>
        public const string Object = "object";

        /// <summary>
        /// primitive
        /// </summary>
        public const string Primitive = "primitive";

        /// <summary>
        /// ref
        /// </summary>
        public const string Ref = "ref";

        /// <summary>
        /// root
        /// </summary>
        public const string Root = "root";

        /// <summary>
        /// string
        /// </summary>
        public const string String = "string";

        /// <summary>
        /// types
        /// </summary>
        public const string TypeCache = "types";

        /// <summary>
        /// type
        /// </summary>
        public const string TypeId = "type";

        /// <summary>
        /// value
        /// </summary>
        public const string Value = "value";

        #endregion

        #region Methods

        /// <summary>
        /// Returns a System.Type that corresponds to the MemberInfo, regardless of whether the member is a field or property.
        /// </summary>
        /// <param name="memberInfo">The base class that can be either a FieldInfo or PropertyInfo</param>
        /// <returns>The System.Type</returns>
        public static Type GetMemberType(MemberInfo memberInfo)
        {
            var info = memberInfo as PropertyInfo;
            return info != null ? info.PropertyType : ((FieldInfo)memberInfo).FieldType;
        }

        #endregion
    }
}