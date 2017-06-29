// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
        /// <summary>
        /// Represents the number of decimals to preserve after a 0.
        /// </summary>
        private byte _decimalCount;

        /// <summary>
        /// The length of a field in bytes
        /// </summary>
        private byte _length;

        #region Properties

        /// <summary>
        /// Creates a new default field empty field - needed for datatable copy and clone methods.
        /// </summary>
        public Field()
        {

        }

        /// <summary>
        /// Creates a new default field given the specified DataColumn.  Numeric types
        /// default to a size of 255, but will be shortened during the save opperation.
        /// The default decimal count for double and long is 0, for Currency is 2, for float is
        /// 3, and for double is 8.  These can be changed by changing the DecimalCount property.
        /// </summary>
        /// <param name="inColumn">A System.Data.DataColumn to create a Field from</param>
        public Field(DataColumn inColumn)
            : base(inColumn.ColumnName, inColumn.DataType, inColumn.Expression, inColumn.ColumnMapping)
        {
            SetupDecimalCount();
            if (inColumn.DataType == typeof(string))
            {
                _length = inColumn.MaxLength <= 254 ? (byte) inColumn.MaxLength : (byte) 254;
            }
            else if (inColumn.DataType == typeof(DateTime))
            {
                _length = 8;
            }
            else if (inColumn.DataType == typeof(bool))
            {
                _length = 1;
            }
        }

        /// <summary>
        /// Creates a new instance of a field given only a column name
        /// </summary>
        /// <param name="inColumnName">The string Column Name for the new field</param>
        public Field(string inColumnName)
            : base(inColumnName)
        {
            // can't setup decimal count without a data type
        }

        /// <summary>
        /// Creates a new Field with a specific name for a specified data type
        /// </summary>
        /// <param name="inColumnName">The string name of the column</param>
        /// <param name="inDataType">The System.Type describing the datatype of the field</param>
        public Field(string inColumnName, Type inDataType)
            : base(inColumnName, inDataType)
        {
            SetupDecimalCount();
        }

        /// <summary>
        /// Creates a new field with a specific name and using a simplified enumeration of possible types.
        /// </summary>
        /// <param name="inColumnName">the string column name.</param>
        /// <param name="type">The type enumeration that clarifies which basic data type to use.</param>
        public Field(string inColumnName, FieldDataType type)
            : base(inColumnName)
        {
            if (type == FieldDataType.Double) DataType = typeof(double);
            if (type == FieldDataType.Integer) DataType = typeof(int);
            if (type == FieldDataType.String) DataType = typeof(string);
        }

        /*
         * Field type:
            C   –   Character
            Y   –   Currency
            N   –   Numeric
            F   –   Float
            D   –   Date
            T   –   DateTime
            B   –   Double
            I   –   Integer
            L   –   Logical
            M   –   Memo
            G   –   General
            C   –   Character (binary)
            M   –   Memo (binary)
            P   –   Picture
         */

        /// <summary>
        /// This creates a new instance.  Since the data type is
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="typeCode"></param>
        /// <param name="length"></param>
        /// <param name="decimalCount"></param>
        public Field(string columnName, char typeCode, byte length, byte decimalCount)
            : base(columnName)
        {
            Length = length;
            ColumnName = columnName;
            DecimalCount = decimalCount;
            // Date
            if (typeCode == 'D')
            {
                // date
                DataType = typeof(DateTime);
                return;
            }
            if (typeCode == 'T')
            {
                // date time
                DataType = typeof(DateTime);
                return;
            }
            if (typeCode == 'L')
            {
                DataType = typeof(bool);
                return;
            }
            if (typeCode == 'B')
            {
                DataType = typeof(byte[]);
                return;
            }
            if (typeCode == 'F')
            {
                DataType = typeof(float);
                return;
            }
            if (typeCode == 'N')
            {
                // The strategy here is to assign the smallest type that we KNOW will be large enough
                // to hold any value with the length (in digits) and characters.
                // even though double can hold as high a value as a "Number" can go, it can't
                // preserve the extraordinary 255 digit precision that a Number has.  The strategy
                // is to assess the length in characters and assign a numeric type where no
                // value may exist outside the range.  (They can always change the datatype later.)

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

                if (decimalCount > 14)
                {
                    // we know this has too many significant digits to fit in a double.
                    DataType = typeof(string);
                }

                // Singles  -3.402823E+38 to 3.402823E+38
                // Doubles -1.79769313486232E+308 to 1.79769313486232E+308
                // Decimals -79228162514264337593543950335 to 79228162514264337593543950335

                // Doubles have the range to handle any number with the 255 character size,
                // but won't preserve the precision that is possible.  It is still
                // preferable to have a numeric type in 99% of cases, and double is the easiest.

                DataType = typeof(double);
                Length = length;

                return;
            }
            // Type code is either C or not recognized, in which case we will just end up with a string
            // representation of whatever the characters are.

            DataType = typeof(String);
            MaxLength = length;
        }

        /// <summary>
        /// This is the single character dBase code.  Only some of these are supported with Esri.
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
                if (DataType == typeof(float)) return 'F';
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
        /// As far as dbf fields are concerned, all numeric datatypes use the same
        /// database number format.
        /// </summary>
        public byte DecimalCount
        {
            get
            {
                return _decimalCount;
            }
            set
            {
                _decimalCount = value;
            }
        }

        /// <summary>
        /// The character length of the field
        /// </summary>
        public byte Length
        {
            get
            {
                return _length;
            }
            set
            {
                _length = value;
            }
        }

        /// <summary>
        /// The offset of the field on a row in the file
        /// </summary>
        public int DataAddress { get; set; }

        ///<summary>
        /// Number Converter associated with this field.
        ///</summary>
        public NumberConverter NumberConverter { get; set; }

        /// <summary>
        /// Internal method that decides an appropriate decimal count, given a data column
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
                //_decimalCount = (byte)40;  // Singles  -3.402823E+38 to 3.402823E+38
                //_length = (byte)40;
                _length = 18;
                _decimalCount = 6;
                return;
            }
            if (DataType == typeof(double))
            {
                //_decimalCount = (byte)255; // Doubles -1.79769313486232E+308 to 1.79769313486232E+308
                //_length = (byte)255;
                _length = 18;
                _decimalCount = 9;
                return;
            }
            if (DataType == typeof(decimal))
            {
                _decimalCount = 9; // Decimals -79228162514264337593543950335 to 79228162514264337593543950335
                _length = 18;
                return;
            }
            if (DataType == typeof(byte))
            {
                // 0 to 255
                _decimalCount = 0;
                _length = 3;
                return;
            }
            if (DataType == typeof(short))
            {
                // -32768 to 32767
                _decimalCount = 0;
                _length = 6;
                return;
            }
            if (DataType == typeof(int))
            {
                // -2147483648 to 2147483647
                _decimalCount = 0;
                _length = 11;
                return;
            }
            if (DataType == typeof(long))
            {
                // -9223372036854775808 to -9223372036854775807
                _decimalCount = 0;
                _length = 20;
                return;
            }
        }

        #endregion
    }
}
