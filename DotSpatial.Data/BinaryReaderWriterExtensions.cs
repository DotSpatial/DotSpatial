using System;
using System.IO;

namespace DotSpatial.Data
{
    /// <summary>
    ///
    /// </summary>
    public static class BinaryReaderWriterExtensions
    {
        /// <summary>
        /// Writes the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="writer">The writer.</param>
        /// <param name="num">The value.</param>
        public static void Write<T>(this BinaryWriter writer, T num)
        {
            if (typeof(T) == typeof(Byte))
                writer.Write((Byte)(object)num);
            else if (typeof(T) == typeof(Double))
                writer.Write((Double)(object)num);
            else if (typeof(T) == typeof(Decimal))
                writer.Write((Decimal)(object)num);
            else if (typeof(T) == typeof(Int16))
                writer.Write((Int16)(object)num);
            else if (typeof(T) == typeof(Int32))
                writer.Write((Int32)(object)num);
            else if (typeof(T) == typeof(Int64))
                writer.Write((Int64)(object)num);
            else if (typeof(T) == typeof(Single))
                writer.Write((Single)(object)num);
            else if (typeof(T) == typeof(Boolean))
                writer.Write((Boolean)(object)num);
            else
            {
                throw new NotSupportedException("Unable to write type - " + typeof(T));
            }
        }

        /// <summary>
        /// Reads the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static T Read<T>(this BinaryReader reader)
        {
            object ret = null;

            if (typeof(T) == typeof(Byte))
                ret = reader.ReadByte();
            else if (typeof(T) == typeof(Double))
                ret = reader.ReadDouble();
            else if (typeof(T) == typeof(Decimal))
                ret = reader.ReadDecimal();
            else if (typeof(T) == typeof(Int16))
                ret = reader.ReadInt16();
            else if (typeof(T) == typeof(Int32))
                ret = reader.ReadInt32();
            else if (typeof(T) == typeof(Int64))
                ret = reader.ReadInt64();
            else if (typeof(T) == typeof(Single))
                ret = reader.ReadSingle();
            else if (typeof(T) == typeof(Boolean))
                ret = reader.ReadBoolean();

            if (ret == null)
                throw new NotSupportedException("Unable to read type - " + typeof(T));

            return (T)ret;
        }
    }
}