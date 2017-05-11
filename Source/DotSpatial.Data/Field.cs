// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Data;

namespace DotSpatial.Data
{
    /// <summary>
    /// This represents the column information for one column of a shapefile.
    /// This specifies precision as well as the typical column information.
    /// </summary>
    public class Field : DataColumn
    {
        #region Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="Field"/> class that is empty.
        /// This is needed for datatable copy and clone methods.
        /// </summary>
        public Field()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Field"/> class.
        /// Creates a new default field given the specified DataColumn. Numeric types
        /// default to a size of 255, but will be shortened during the save opperation.
        /// The default decimal count for double and long is 0, for Currency is 2, for float is
        /// 3, and for double is 8. These can be changed by changing the DecimalCount property.
        /// </summary>
        /// <param name="inColumn">A System.Data.DataColumn to create a Field from</param>
        public Field(DataColumn inColumn)
            : base(inColumn.ColumnName, inColumn.DataType, inColumn.Expression, inColumn.ColumnMapping)
        {
            SetupDecimalCount();
            if (inColumn.DataType == typeof(string))
            {
                Length = inColumn.MaxLength <= 254 ? (byte)inColumn.MaxLength : (byte)254;
            }
            else if (inColumn.DataType == typeof(DateTime))
            {
                Length = 8;
            }
            else if (inColumn.DataType == typeof(bool))
            {
                Length = 1;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Field"/> class, as type string, using the specified column name.
        /// </summary>
        /// <param name="inColumnName">The string name of the column.</param>
        public Field(string inColumnName)
            : base(inColumnName)
        {
            // can't setup decimal count without a data type
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Field"/> class with a specific name for a specified data type.
        /// </summary>
        /// <param name="inColumnName">The string name of the column.</param>
        /// <param name="inDataType">The System.Type describing the datatype of the field</param>
        public Field(string inColumnName, Type inDataType)
            : base(inColumnName, inDataType)
        {
            SetupDecimalCount();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Field"/> class with a specific name and using a simplified enumeration of possible types.
        /// </summary>
        /// <param name="inColumnName">The string name of the column.</param>
        /// <param name="type">The type enumeration that clarifies which basic data type to use.</param>
        public Field(string inColumnName, FieldDataType type)
            : base(inColumnName)
        {
            if (type == FieldDataType.Double) DataType = typeof(double);
            if (type == FieldDataType.Integer) DataType = typeof(int);
            if (type == FieldDataType.String) DataType = typeof(string);
        }

        /*
         * Field Types Specified by the dBASE Format Specifications
         *
         * http://www.dbase.com/Knowledgebase/INT/db7_file_fmt.htm
         *
         * Symbol |  Data  Type  | Description
         * -------+--------------+-------------------------------------------------------------------------------------
         *   B    |       Binary | 10 digits representing a .DBT block number. The number is stored as a string, right justified and padded with blanks.
         *   C    |    Character | All OEM code page characters - padded with blanks to the width of the field.
         *   D    |         Date | 8 bytes - date stored as a string in the format YYYYMMDD.
         *   N    |      Numeric | Number stored as a string, right justified, and padded with blanks to the width of the field.
         *   L    |      Logical | 1 byte - initialized to 0x20 (space) otherwise T or F.
         *   M    |         Memo | 10 digits (bytes) representing a .DBT block number. The number is stored as a string, right justified and padded with blanks.
         *   @    |    Timestamp | 8 bytes - two longs, first for date, second for time. The date is the number of days since  01/01/4713 BC. Time is hours * 3600000L + minutes * 60000L + Seconds * 1000L
         *   I    |         Long | 4 bytes. Leftmost bit used to indicate sign, 0 negative.
         *   +    |Autoincrement | Same as a Long
         *   F    |        Float | Number stored as a string, right justified, and padded with blanks to the width of the field.
         *   O    |       Double | 8 bytes - no conversions, stored as a double.
         *   G    |          OLE | 10 digits (bytes) representing a .DBT block number. The number is stored as a string, right justified and padded with blanks.
         */

        /// <summary>
        /// Initializes a new instance of the <see cref="Field"/> class.
        /// </summary>
        /// <param name="columnName">The string name of the column.</param>
        /// <param name="typeCode">A code specified by the dBASE Format Specifications that indicates what type should be used.</param>
        /// <param name="length">The character length of the field.</param>
        /// <param name="decimalCount">Represents the number of decimals to preserve after a 0.</param>
        public Field(string columnName, char typeCode, byte length, byte decimalCount)
            : base(columnName)
        {
            Length = length;
            ColumnName = columnName;
            DecimalCount = decimalCount;

            // Date or Timestamp
            if (typeCode == 'D' || typeCode == '@')
            {
                // date
                DataType = typeof(DateTime);
                return;
            }

            if (typeCode == 'L')
            {
                DataType = typeof(bool);
                return;
            }

            // Long or AutoIncrement
            if (typeCode == 'I' || typeCode == '+')
            {
                DataType = typeof(int);
                return;
            }

            if (typeCode == 'O')
            {
                DataType = typeof(double);
                return;
            }

            if (typeCode == 'N' || typeCode == 'B' || typeCode == 'M' || typeCode == 'F' || typeCode == 'G')
            {
                // The strategy here is to assign the smallest type that we KNOW will be large enough
                // to hold any value with the length (in digits) and characters.
                // even though double can hold as high a value as a "Number" can go, it can't
                // preserve the extraordinary 255 digit precision that a Number has. The strategy
                // is to assess the length in characters and assign a numeric type where no
                // value may exist outside the range. (They can always change the datatype later.)

                // The basic encoding we are using here
                if (decimalCount == 0)
                {
                    if (length < 3)
                    {
                        // 0 to 255
                        DataType = typeof(byte);
                        return;
                    }

                    if (length < 5)
                    {
                        // -32768 to 32767
                        DataType = typeof(short); // Int16
                        return;
                    }

                    if (length < 10)
                    {
                        // -2147483648 to 2147483647
                        DataType = typeof(int); // Int32
                        return;
                    }

                    if (length < 19)
                    {
                        // -9223372036854775808 to -9223372036854775807
                        DataType = typeof(long); // Int64
                        return;
                    }
                }

                if (decimalCount > 15)
                {
                    // we know this has too many significant digits to fit in a double:
                    // a double can hold 15-16 significant digits: https://msdn.microsoft.com/en-us/library/678hzkk9.aspx
                    DataType = typeof(string);
                    MaxLength = length;
                    return;
                }

                // Singles  -3.402823E+38 to 3.402823E+38
                // Doubles -1.79769313486232E+308 to 1.79769313486232E+308
                // Decimals -79228162514264337593543950335 to 79228162514264337593543950335

                // Doubles have the range to handle any number with the 255 character size,
                // but won't preserve the precision that is possible. It is still
                // preferable to have a numeric type in 99% of cases, and double is the easiest.
                DataType = typeof(double);

                return;
            }

            // Type code is either C or not recognized, in which case we will just end up with a string
            // representation of whatever the characters are.
            DataType = typeof(string);
            MaxLength = length;
        }

        /// <summary>
        /// Gets the single character dBase code. Only some of these are supported with Esri.
        /// C - Character (Chars, Strings, objects - as ToString(), and structs - as  )
        /// D - Date (DateTime)
        /// T - Time (DateTime)
        /// N - Number (Short, Integer, Long, Float, Double, byte)
        /// L - Logic (True-False, Yes-No)
        /// F - Float
        /// B - Double
        /// </summary>
        public char TypeCharacter
        {
            get
            {
                if (DataType == typeof(bool)) return 'L';
                if (DataType == typeof(DateTime)) return 'D';

                // We are using numeric in most cases here, because that is the format most compatible with other
                // Applications
                if (DataType == typeof(float)) return 'N';
                if (DataType == typeof(double)) return 'N';
                if (DataType == typeof(decimal)) return 'N';
                if (DataType == typeof(byte)) return 'N';
                if (DataType == typeof(short)) return 'N';
                if (DataType == typeof(int)) return 'N';
                if (DataType == typeof(long)) return 'N';

                // The default is to store it as a string type
                return 'C';
            }
        }

        /// <summary>
        /// Gets or sets the number of places to keep after the 0 in number formats.
        /// As far as dbf fields are concerned, all numeric datatypes use the same database number format.
        /// </summary>
        public byte DecimalCount { get; set; }

        /// <summary>
        /// Gets or sets the length of the field in bytes.
        /// </summary>
        public byte Length { get; set; }

        /// <summary>
        /// Gets or sets the offset of the field on a row in the file
        /// </summary>
        public int DataAddress { get; set; }

        /// <summary>
        /// Gets or sets the Number Converter associated with this field.
        /// </summary>
        public NumberConverter NumberConverter { get; set; }

        /// <summary>
        /// Internal method that decides an appropriate decimal count, given a data column.
        /// </summary>
        private void SetupDecimalCount()
        {
            // Going this way, we want a large enough decimal count to hold any of the possible numeric values.
            // We will try to make the length large enough to hold any values, but some doubles simply will be
            // too large to be stored in this format, so we will throw exceptions if that happens later.

            // These sizes represent the "maximized" length and decimal counts that will be shrunk in order
            // to fit the data before saving.
            if (DataType == typeof(float))
            {
                //// _decimalCount = (byte)40;  // Singles  -3.402823E+38 to 3.402823E+38
                //// _length = (byte)40;
                Length = 18;
                DecimalCount = 6;
                return;
            }

            if (DataType == typeof(double))
            {
                //// _decimalCount = (byte)255; // Doubles -1.79769313486232E+308 to 1.79769313486232E+308
                //// _length = (byte)255;
                Length = 18;
                DecimalCount = 9;
                return;
            }

            if (DataType == typeof(decimal))
            {
                Length = 18;
                DecimalCount = 9; // Decimals -79228162514264337593543950335 to 79228162514264337593543950335
                return;
            }

            if (DataType == typeof(byte))
            {
                // 0 to 255
                DecimalCount = 0;
                Length = 3;
                return;
            }

            if (DataType == typeof(short))
            {
                // -32768 to 32767
                Length = 6;
                DecimalCount = 0;
                return;
            }

            if (DataType == typeof(int))
            {
                // -2147483648 to 2147483647
                Length = 11;
                DecimalCount = 0;
                return;
            }

            if (DataType == typeof(long))
            {
                // -9223372036854775808 to -9223372036854775807
                Length = 20;
                DecimalCount = 0;
            }
        }

        #endregion
    }
}
