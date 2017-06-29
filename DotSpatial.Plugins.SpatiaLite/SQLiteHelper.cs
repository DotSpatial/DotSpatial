using System;
using System.Data.SQLite;
using System.IO;
using System.Runtime.Serialization;

namespace DotSpatial.Plugins.SpatiaLite
{
    /// <summary>
    /// This class contains methods for working with the
    /// SQLite database
    /// </summary>
    public static class SQLiteHelper
    {
        /// <summary>
        /// To get the SQLite database path given the SQLite connection string
        /// </summary>
        public static string GetSQLiteFileName(string sqliteConnString)
        {
            SQLiteConnectionStringBuilder conn = new SQLiteConnectionStringBuilder(sqliteConnString);
            return conn.DataSource;
        }

        /// <summary>
        /// To get the full SQLite connection string given the SQLite database path
        /// </summary>
        public static string GetSQLiteConnectionString(string dbFileName)
        {
            // returns "Data Source= " + dbFileName + ";New=False;Compress=True;Version=3";
            SQLiteConnectionStringBuilder conn = new SQLiteConnectionStringBuilder();
            conn.DataSource = dbFileName;
            conn.Version = 3;
            conn.FailIfMissing = true;
            conn.Add("Compress", true);

            return conn.ConnectionString;
        }

        /// <summary>
        /// Utility function for copying byte array to stream object
        /// </summary>
        /// <param name="input">the input stream object</param>
        /// <param name="output">the output stream object</param>
        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8192];

            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }

        /// <summary>
        /// Check if the path is a valid SQLite database
        /// This function returns false, if the SQLite db
        /// file doesn't exist or if the file size is 0 Bytes
        /// </summary>
        public static bool DatabaseExists(string dbPath)
        {
            if (!File.Exists(dbPath))
            {
                return false;
            }
            else
            {
                FileInfo dbFileInfo = new FileInfo(dbPath);
                if (dbFileInfo.Length == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }

    /// <summary>
    /// The type of SQLite database (data repositor or metadata cache)
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// Data repository sqlite database
        /// </summary>
        DefaulDatabase,
        /// <summary>
        /// Metadata cache SQLite database
        /// </summary>
        MetadataCacheDatabase
    }

    /// <summary>
    /// This exception occurs in case of invalid database schema
    /// </summary>
    public class InvalidDatabaseSchemaException : Exception
    {
        /// <summary>
        /// new instance of invalid database schema exception
        /// </summary>
        public InvalidDatabaseSchemaException()
        {
        }

        /// <summary>
        /// invalid database schema exception with message
        /// </summary>
        /// <param name="message">the error message</param>
        public InvalidDatabaseSchemaException(string message)
            : this(message, null)
        {
        }

        /// <summary>
        /// invalid database schema exception with message and inner exception
        /// </summary>
        /// <param name="message">the error messsage</param>
        /// <param name="inner">the inner exception</param>
        public InvalidDatabaseSchemaException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// invalid database schema exception with serialization info and streaming context
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">streaming context</param>
        protected InvalidDatabaseSchemaException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}