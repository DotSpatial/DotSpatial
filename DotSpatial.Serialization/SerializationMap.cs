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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// This class stores the reflected data required for serialization.
    /// </summary>
    public class SerializationMap
    {
        private static readonly Dictionary<Type, SerializationMap> _typeToSerializationMap = new Dictionary<Type, SerializationMap>();

        private readonly List<SerializationMapEntry> _memberInfos = new List<SerializationMapEntry>();

        /// <summary>
        /// The static constructor creates a dictionary that maps an object type to a specific SerializationMap instance.
        /// </summary>
        static SerializationMap()
        {
            var mapTypes = ReflectionHelper.FindDerivedClasses(typeof(SerializationMap));

            foreach (var type in mapTypes)
            {
                SerializationMap mapInstance;

                try
                {
                    mapInstance = (SerializationMap)type.Assembly.CreateInstance(type.FullName);
                }
                catch
                {
                    continue;
                }

                if (mapInstance != null)
                    _typeToSerializationMap[mapInstance.ForType] = mapInstance;
            }
        }

        /// <summary>
        /// Creates a new instance of the SerializationMap class.
        /// </summary>
        /// <param name="forType">The type associated with this SerializationMap.</param>
        protected SerializationMap(Type forType)
        {
            ForType = forType;
            FindSerializableMembers(forType);

            // Update the mapping dictionary
            _typeToSerializationMap[forType] = this;
        }

        /// <summary>
        /// The type that this serialization map instance is associated with.
        /// </summary>
        public Type ForType { get; private set; }

        /// <summary>
        /// The members that participate in serialization.
        /// </summary>
        public List<SerializationMapEntry> Members
        {
            get { return _memberInfos; }
        }

        /// <summary>
        /// This overload is used for objects that are marked with the <see cref="SerializeAttribute"/>.
        /// </summary>
        /// <param name="type">The type to generate a <c>SerializationMap</c> for.</param>
        /// <returns>A new serialization map that can be used to serialize the given type.</returns>
        public static SerializationMap FromType(Type type)
        {
            var cachedMap = GetCachedSerializationMap(type);
            if (cachedMap != null)
                return cachedMap;

            return new SerializationMap(type);
        }

        private static SerializationMap GetCachedSerializationMap(Type type)
        {
            // Try to find a cached instance first
            if (_typeToSerializationMap.ContainsKey(type))
                return _typeToSerializationMap[type];

            return null;
        }

        /// <summary>
        /// The forward method that will serialize a property or field with the specified name.
        /// The name does not have to be the same as the name of the member.
        /// </summary>
        /// <param name="memberInfo">The property or field information to serialize</param>
        /// <param name="name">The name to remove</param>
        /// <returns>The Serialization Map Entry created by the serialize method</returns>
        protected SerializationMapEntry Serialize(MemberInfo memberInfo, string name)
        {
            SerializationMapEntry result = new SerializationMapEntry(memberInfo, new SerializeAttribute(name));
            _memberInfos.Add(result);
            return result;
        }

        private void FindSerializableMembers(Type type)
        {
            FindPrivateSerializableMembers(type);
            FindPublicSerializableMemebers(type);
        }

        private void FindSerializableProperties(Type type, BindingFlags bindingFlags)
        {
            AddSerializableMembers(type.GetProperties(bindingFlags));
        }

        private void FindSerializableFields(Type type, BindingFlags bindingFlags)
        {
            AddSerializableMembers(type.GetFields(bindingFlags).Where(fi => (!fi.IsInitOnly || FieldIsConstructorArgument(fi))).Cast<MemberInfo>());
        }

        private static bool FieldIsConstructorArgument(FieldInfo fi)
        {
            foreach (var fieldAttribute in fi.GetCustomAttributes(typeof(SerializeAttribute), true))
            {
                var sa = (SerializeAttribute) fieldAttribute;
                return sa.ConstructorArgumentIndex >= 0;
            }
            return false;
        }

        private void AddSerializableMembers(IEnumerable<MemberInfo> members)
        {
            foreach (var memberInfo in members)
            {
                var attribute = memberInfo.GetCustomAttributes(typeof(SerializeAttribute), true).Cast<SerializeAttribute>().FirstOrDefault();
                MemberInfo info = memberInfo;
                if (attribute != null &&
                    !_memberInfos.Any(mi => mi.Member.Name == info.Name &&
                                            mi.Member.DeclaringType == info.DeclaringType &&
                                            mi.Member.MemberType == info.MemberType))
                {
                    _memberInfos.Add(new SerializationMapEntry(memberInfo, attribute));
                }
            }
        }

        private void FindPrivateSerializableMembers(Type type)
        {
            Type baseType = type.BaseType;

            Stack<Type> typesToSerialize = new Stack<Type>();
            typesToSerialize.Push(type);

            while (baseType != null && !baseType.Equals(typeof(object)))
            {
                typesToSerialize.Push(baseType);
                baseType = baseType.BaseType;
            }

            while (typesToSerialize.Count > 0)
            {
                type = typesToSerialize.Pop();
                FindSerializableFields(type, BindingFlags.Instance | BindingFlags.NonPublic);
                FindSerializableProperties(type, BindingFlags.Instance | BindingFlags.NonPublic);
            }
        }

        private void FindPublicSerializableMemebers(Type type)
        {
            FindSerializableFields(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            FindSerializableProperties(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
        }
    }

    ///// <summary>
    ///// Generic type used to make creating custom mappings a little bit easier.
    ///// </summary>
    ///// <typeparam name="T">The type that this map will be associated with.</typeparam>
    //public class SerializationMap<T> : SerializationMap
    //{
    //    /// <summary>
    //    /// Constructs a new instance of the generic SerializationMap class
    //    /// </summary>
    //    protected SerializationMap() : base(typeof(T))
    //    {
    //    }

    //    /// <summary>
    //    /// Specifies that a field or property should be serialize
    //    /// </summary>
    //    /// <param name="expression">An expression that will yield a <see cref="MemberInfo"/>.</param>
    //    /// <param name="name">The name to use when serializing this member.</param>
    //    /// <returns>A <see cref="SerializationMapEntry"/> representing the member specified by <paramref name="expression"/>.</returns>
    //    protected SerializationMapEntry Serialize(Expression<Func<T, object>> expression, string name)
    //    {
    //        MemberExpression memberExpression = null;

    //        if (expression.Body.NodeType == ExpressionType.Convert)
    //        {
    //            memberExpression = ((UnaryExpression)expression.Body).Operand as MemberExpression;
    //        }
    //        else if (expression.Body.NodeType == ExpressionType.MemberAccess)
    //        {
    //            memberExpression = expression.Body as MemberExpression;
    //        }

    //        if (memberExpression == null)
    //            throw new ArgumentException("This type of expression not supported.", "expression");

    //        MemberInfo memberInfo = memberExpression.Member;
    //        Debug.Assert(memberInfo != null);

    //        // Replace an entry if the specified member has already been mapped one way or another.
    //        SerializeAttribute attribute = memberInfo.GetCustomAttributes(typeof(SerializeAttribute), true).
    //            Cast<SerializeAttribute>().
    //            FirstOrDefault();
    //        if (attribute != null)
    //            Members.RemoveAll(m => m.Member.Equals(memberInfo));

    //        SerializationMapEntry result = new SerializationMapEntry(memberInfo, new SerializeAttribute(name));
    //        Members.Add(result);
    //        return result;
    //    }
    //}
}