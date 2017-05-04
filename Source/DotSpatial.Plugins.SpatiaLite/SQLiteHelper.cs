using System.Data.SQLite;
using System.IO;

namespace DotSpatial.Plugins.SpatiaLite
{
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
    /// This class contains methods for working with the SQLite database.
    /// </summary>
    public static class SqLiteHelper
    {
        #region Methods

        /// <summary>
        /// Check if the path is a valid SQLite database.
        /// This function returns false, if the SQLite db file doesn't exist or if the file size is 0 Bytes.
        /// </summary>
        /// <param name="dbPath">Path of the SQLite db.</param>
        /// <returns>False if the SQLite db file doesn't exist or it's size is 0 bytes.</returns>
        public static bool DatabaseExists(string dbPath)
        {
            if (!File.Exists(dbPath))
            {
                return false;
            }

            FileInfo dbFileInfo = new FileInfo(dbPath);
            return dbFileInfo.Length != 0;
        }

        /// <summary>
        /// To get the full SQLite connection string given the SQLite database path.
        /// </summary>
        /// <param name="dbFileName">Name of the SQLite db file.</param>
        /// <returns>The connection string.</returns>
        public static string GetSqLiteConnectionString(string dbFileName)
        {
            SQLiteConnectionStringBuilder conn = new SQLiteConnectionStringBuilder
            {
                DataSource = dbFileName,
                Version = 3,
                FailIfMissing = true
            };
            conn.Add("Compress", true);

            return conn.ConnectionString;
        }

        /// <summary>
        /// To get the SQLite database path given the SQLite connection string.
        /// </summary>
        /// <param name="sqliteConnString">The connection string.</param>
        /// <returns>The file name.</returns>
        public static string GetSqLiteFileName(string sqliteConnString)
        {
            SQLiteConnectionStringBuilder conn = new SQLiteConnectionStringBuilder(sqliteConnString);
            return conn.DataSource;
        }
        #endregion
    }
}