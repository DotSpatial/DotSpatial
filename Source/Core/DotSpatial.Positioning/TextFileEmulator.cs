// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.IO;
using System.Text;
using System.Threading;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Emulator that sources it's data from a text file
    /// </summary>
    public class TextFileEmulator : Emulator
    {
        /// <summary>
        ///
        /// </summary>
        private readonly string _filePath;
        /// <summary>
        ///
        /// </summary>
        private StreamReader _reader;

        /// <summary>
        /// Creates a Text File Emulator from the specified file path with a default read interval of 400 seconds
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public TextFileEmulator(string filePath)
            : this(filePath, TimeSpan.FromSeconds(400))
        { }

        /// <summary>
        /// Creates a Text File Emulator from the specified file path with the specified read interval
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="readInterval">The read interval.</param>
        public TextFileEmulator(string filePath, TimeSpan readInterval)
        {
            _filePath = filePath;
            ReadInterval = readInterval;
        }

        /// <summary>
        /// Gets or sets the ReadInterval for the Text File Emulator
        /// </summary>
        /// <value>The read interval.</value>
        public TimeSpan ReadInterval { get; set; }

        /// <summary>
        /// OnEmulation event handler
        /// </summary>
        protected override void OnEmulation()
        {
            // Are we at the end of the file?
            if (_reader == null || _reader.EndOfStream)
            {
                // Yes.  Re-open it from the beginning
                FileStream stream = new(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                _reader = new StreamReader(stream);
            }

            // Read a line from the file
            string line = _reader.ReadLine();

            // Don't write to the buffer if it's full
            if (line != null)
            {
                if (ReadBuffer.Count + line.Length > ReadBuffer.Capacity)
                {
                    return;
                }

                // Write the string
                ReadBuffer.AddRange(Encoding.ASCII.GetBytes(line));
            }

            // Sleep
            Thread.Sleep(ReadInterval);
        }
    }
}