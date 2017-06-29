// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from http://gps3.codeplex.com/ version 3.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GPS.Net 3.0
// | Shade1974 (Ted Dunsford) | 10/22/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************
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
        ///
        /// </summary>
        private TimeSpan _readInterval;

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
            _readInterval = readInterval;
        }

        /// <summary>
        /// Gets or sets the ReadInterval for the Text File Emulator
        /// </summary>
        /// <value>The read interval.</value>
        public TimeSpan ReadInterval
        {
            get { return _readInterval; }
            set { _readInterval = value; }
        }

        /// <summary>
        /// OnEmulation event handler
        /// </summary>
        protected override void OnEmulation()
        {
            // Are we at the end of the file?
            if (_reader == null || _reader.EndOfStream)
            {
                // Yes.  Re-open it from the beginning
                FileStream stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                _reader = new StreamReader(stream);
            }

            // Read a line from the file
            string line = _reader.ReadLine();

            // Don't write to the buffer if it's full
            if (line != null)
            {
                if (ReadBuffer.Count + line.Length > ReadBuffer.Capacity)
                    return;

                // Write the string
                ReadBuffer.AddRange(Encoding.ASCII.GetBytes(line));
            }

            // Sleep
#if PocketPC
            Thread.Sleep((int)_ReadInterval.TotalMilliseconds);
#else
            Thread.Sleep(_readInterval);
#endif
        }

        // TODO: We should be able to get rid of this. Not used internally anymore.
        //public override Emulator Clone()
        //{
        //    // Return a copy of this emulator
        //    return new TextFileEmulator(_FilePath);
        //}
    }
}