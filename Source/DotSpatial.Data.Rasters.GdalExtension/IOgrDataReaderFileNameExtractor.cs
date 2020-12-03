namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    /// Sometimes the file name isn't exactly the same as the connection string passed to the OgrReader
    /// This interface proxies the conversion
    /// </summary>
    public interface IOgrDataReaderFileNameExtractor
    {
        /// <summary>
        /// Checks if conversion is possible by the extractor
        /// </summary>
        /// <param name="rawConnectionConfig">The connection as passed in to the ogrreader</param>
        /// <returns>If conversion can be executed</returns>
        bool CanExtractFileName(string rawConnectionConfig);

        /// <summary>
        /// Executes the extraction
        /// </summary>
        /// <param name="rawConnectionConfig">The connection as passed in to the ogrreader</param>
        /// <returns>If conversion can be executed</returns>
        string ExtractFileName(string rawConnectionConfig);
    }
}
