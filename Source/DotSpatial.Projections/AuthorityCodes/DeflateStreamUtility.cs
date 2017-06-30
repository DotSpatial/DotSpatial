using System;
using System.IO;
using System.IO.Compression;

namespace DotSpatial.Projections
{
    /// <summary>
    /// Tiny program to deflate DotSpatial.Projection resource streams.
    /// </summary>
    public class DeflateStreamUtility
    {
        public static void Main(string[] args)
        {
            foreach (var s in args)
            {
                if (!File.Exists(s))
                {
                    Console.WriteLine(@"File not found: " + s);
                    continue;
                }

                using (FileStream fs = File.OpenRead(s))
                {
                    string sout = Path.ChangeExtension(s, "ds");
                    using (DeflateStream ds = new DeflateStream(File.OpenWrite(sout), CompressionMode.Compress, false))
                    {
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);
                        ds.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }
    }
}