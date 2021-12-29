// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.IO;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace DotSpatial.Plugins.SpatiaLite
{
    /// <summary>
    /// Helper class for reading binary data from the SpatiaLite database.
    /// </summary>
    public class SpatiaLiteWkbReader : WKBReader
    {
        /// <summary>
        /// Reads SpatiaLite data from the given stream. This is also called by Read(byte[] data).
        /// </summary>
        /// <param name="stream">Stream that should be used to read the data.</param>
        /// <returns>IGeometry that is contained in the given stream.</returns>
        public override Geometry Read(Stream stream)
        {
            // specialized Read() method for SpatiaLite
            using (stream)
            {
                // read first byte
                BiEndianBinaryReader reader = null;
                var startByte = stream.ReadByte(); // must be "0"
                var byteOrder = (ByteOrder)stream.ReadByte();

                try
                {
                    reader = new BiEndianBinaryReader(stream, byteOrder);

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
                    reader?.Close();
                }
            }
        }
    }
}