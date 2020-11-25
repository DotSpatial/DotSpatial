namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    /// As is extractor (the default extractor) always can extract and returns string as is.
    /// </summary>
    public class AsIsFileNameExtractor : IOgrDataReaderFileNameExtractor
    {
        /// <summary>
        /// Checks if conversion is possible by the extractor
        /// </summary>
        /// <param name="rawConnectionConfig">The connection as passed in to the ogrreader</param>
        /// <returns>If conversion can be executed</returns>
        public bool CanExtractFileName(string rawConnectionConfig)
        {
            return true;
        }

        /// <summary>
        /// Executes the extraction
        /// </summary>
        /// <param name="rawConnectionConfig">The connection as passed in to the ogrreader</param>
        /// <returns>If conversion can be executed</returns>
        public string ExtractFileName(string rawConnectionConfig)
        {
            return rawConnectionConfig;
        }
    }
}