using System.IO;
using GeoAPI.Geometries;
using GeoAPI.IO;
using NetTopologySuite.IO;

namespace DotSpatial.Plugins.SpatiaLite
{
    /// <summary>
    /// Helper class for reading binary data from the SpatiaLite database
    /// </summary>
    public class SpatiaLiteWkbReader : WKBReader
    {
        /// <summary>
        /// Reads SpatiaLite data from the given stream. This is also called by Read(byte[] data).
        /// </summary>
        /// <param name="stream">Stream that should be used to read the data.</param>
        /// <returns>IGeometry that is contained in the given stream.</returns>
        public override IGeometry Read(Stream stream)
        {
            //specialized Read() method for SpatiaLite
            using (stream)
            {
                //read first byte
                BinaryReader reader = null;
                var startByte = stream.ReadByte(); //must be "0"
                var byteOrder = (ByteOrder)stream.ReadByte();

                try
                {
                    reader = (byteOrder == ByteOrder.BigEndian) ? new BEBinaryReader(stream) : new BinaryReader(stream);

                    int srid = reader.ReadInt32();
                    double mbrMinX = reader.ReadDouble();
                    double mbrMinY = reader.ReadDouble();
                    double mbrMaxX = reader.ReadDouble();
                    double mbrMaxY = reader.ReadDouble();
                    byte mbrEnd = reader.ReadByte();

                    return Read(reader);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
        }
    }
}