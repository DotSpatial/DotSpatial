// ********************************************************************************************************
// Product Name: DotSpatial.Serialization.dll
// Description:  A module that supports common functions like serialization.
// ********************************************************************************************************
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
        #region Fields

        private static readonly Dictionary<Type, SerializationMap> TypeToSerializationMap = new Dictionary<Type, SerializationMap>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes static members of the <see cref="SerializationMap"/> class.
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

                if (mapInstance != null) TypeToSerializationMap[mapInstance.ForType] = mapInstance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationMap"/> class.
        /// </summary>
        /// <param name="forType">The type associated with this SerializationMap.</param>
        protected SerializationMap(Type forType)
        {
            ForType = forType;
            FindSerializableMembers(forType);

            // Update the mapping dictionary
            TypeToSerializationMap[forType] = this;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type that this serialization map instance is associated with.
        /// </summary>
        public Type ForType { get; private set; }

        /// <summary>
        /// Gets the members that participate in serialization.
        /// </summary>
        public List<SerializationMapEntry> Members { get; } = new List<SerializationMapEntry>();

        #endregion

        #region Methods

        /// <summary>
        /// This overload is used for objects that are marked with the <see cref="SerializeAttribute"/>.
        /// </summary>
        /// <param name="type">The type to generate a <c>SerializationMap</c> for.</param>
        /// <returns>A new serialization map that can be used to serialize the given type.</returns>
        public static SerializationMap FromType(Type type)
        {
            var cachedMap = GetCachedSerializationMap(type);
            if (cachedMap != null) return cachedMap;

            return new SerializationMap(type);
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
            Members.Add(result);
            return result;
        }

        private static bool FieldIsConstructorArgument(FieldInfo fi)
        {
            foreach (var fieldAttribute in fi.GetCustomAttributes(typeof(SerializeAttribute), true))
            {
                var sa = (SerializeAttribute)fieldAttribute;
                return sa.ConstructorArgumentIndex >= 0;
            }

            return false;
        }

        private static SerializationMap GetCachedSerializationMap(Type type)
        {
            // Try to find a cached instance first
            if (TypeToSerializationMap.ContainsKey(type)) return TypeToSerializationMap[type];

            return null;
        }

        private void AddSerializableMembers(IEnumerable<MemberInfo> members)
        {
            foreach (var memberInfo in members)
            {
                var attribute = memberInfo.GetCustomAttributes(typeof(SerializeAttribute), true).Cast<SerializeAttribute>().FirstOrDefault();
                MemberInfo info = memberInfo;
                if (attribute != null && !Members.Any(mi => mi.Member.Name == info.Name && mi.Member.DeclaringType == info.DeclaringType && mi.Member.MemberType == info.MemberType))
                {
                    Members.Add(new SerializationMapEntry(memberInfo, attribute));
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

        private void FindSerializableFields(Type type, BindingFlags bindingFlags)
        {
            AddSerializableMembers(type.GetFields(bindingFlags).Where(fi => !fi.IsInitOnly || FieldIsConstructorArgument(fi)));
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

        #endregion
    }
}