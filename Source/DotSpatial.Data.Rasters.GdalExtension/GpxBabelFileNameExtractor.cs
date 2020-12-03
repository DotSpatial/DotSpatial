using System;
using System.Globalization;
using System.IO;

namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    /// Extracts file name from a GpxBabel connection config.
    /// </summary>
    public class GpsBabelFileNameExtractor : IOgrDataReaderFileNameExtractor
    {
        private string _extractedFileName = null;

        /// <summary>
        /// Checks if conversion is possible by the extractor
        /// </summary>
        /// <param name="rawConnectionConfig">The connection as passed in to the ogrreader</param>
        /// <returns>If conversion can be executed</returns>
        public bool CanExtractFileName(string rawConnectionConfig)
        {
            return ExtractFromConnString(rawConnectionConfig) != null;
        }

        /// <summary>
        /// Executes the extraction
        /// </summary>
        /// <param name="rawConnectionConfig">The connection as passed in to the ogrreader</param>
        /// <returns>If conversion can be executed</returns>
        public string ExtractFileName(string rawConnectionConfig)
        {
            if (_extractedFileName == null && !CanExtractFileName(rawConnectionConfig))
                throw new ArgumentException($"{this.GetType().Name} could not extract the filename from {rawConnectionConfig}! Check if pattern folows (GPSBabel:gpsbabel_file_format[,gpsbabel_format_option]:[features=[waypoints,][tracks,][routes]:]filename*). As described in https://gdal.org/drivers/vector/gpsbabel.html" , "rawConnectionConfig");
            return _extractedFileName;
        }

        /// <summary>
        /// Sets _extractedFileName and returns extracted value if successfull
        /// </summary>
        /// <param name="rawConnectionConfig">The connection as passed in to the ogrreader</param>
        /// <returns>Extracted file name or null if unable to extract</returns>
        private string ExtractFromConnString(string rawConnectionConfig)
        {
            try
            {
                if (!rawConnectionConfig.StartsWith("gpsbabel:", true, CultureInfo.InvariantCulture))
                    return null;
                var filename = rawConnectionConfig.Remove(0, rawConnectionConfig.LastIndexOf(":", StringComparison.Ordinal)-1);
                _extractedFileName = new FileInfo(filename).FullName; //piggy back on fileinfo parser.
                return _extractedFileName;
            }
            catch (Exception ex) when (ex is ArgumentOutOfRangeException || ex is ArgumentException)
            {
                return null;
            }
        }
    }
}