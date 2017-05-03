using System;
using System.IO;

namespace DotSpatial.Data
{
    /// <summary>
    /// Extension methods for <see cref="BinaryReader"/> and <see cref="BinaryWriter"/> classes.
    /// </summary>
    public static class BinaryReaderWriterExtensions
    {
        /// <summary>
        /// Writes the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="writer">The writer.</param>
        /// <param name="num">The value.</param>
        public static void Write<T>(this BinaryWriter writer, T num)
        {
            if (typeof(T) == typeof(byte))
                writer.Write((byte)(object)num);
            else if (typeof(T) == typeof(double))
                writer.Write((double)(object)num);
            else if (typeof(T) == typeof(decimal))
                writer.Write((decimal)(object)num);
            else if (typeof(T) == typeof(short))
                writer.Write((short)(object)num);
            else if (typeof(T) == typeof(int))
                writer.Write((int)(object)num);
            else if (typeof(T) == typeof(long))
                writer.Write((long)(object)num);
            else if (typeof(T) == typeof(float))
                writer.Write((float)(object)num);
            else if (typeof(T) == typeof(bool))
                writer.Write((bool)(object)num);
            else
                throw new NotSupportedException("Unable to write type - " + typeof(T));
        }

        /// <summary>
        /// Reads the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the return value.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns>The read value with the given type.</returns>
        public static T Read<T>(this BinaryReader reader)
        {
            object ret = null;

            if (typeof(T) == typeof(byte))
                ret = reader.ReadByte();
            else if (typeof(T) == typeof(double))
                ret = reader.ReadDouble();
            else if (typeof(T) == typeof(decimal))
                ret = reader.ReadDecimal();
            else if (typeof(T) == typeof(short))
                ret = reader.ReadInt16();
            else if (typeof(T) == typeof(int))
                ret = reader.ReadInt32();
            else if (typeof(T) == typeof(long))
                ret = reader.ReadInt64();
            else if (typeof(T) == typeof(float))
                ret = reader.ReadSingle();
            else if (typeof(T) == typeof(bool))
                ret = reader.ReadBoolean();

            if (ret == null)
                throw new NotSupportedException("Unable to read type - " + typeof(T));

            return (T)ret;
        }
    }
}