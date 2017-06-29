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
// The Original Code is DotSpatial
//
// The Initial Developer of this Original Code is Kyle Ellison. Created 06/24/2011.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |-----------------|----------|---------------------------------------------------------------------
// |      Name       |  Date    |                        Comments
// |-----------------|----------|----------------------------------------------------------------------
// |-----------------|----------|----------------------------------------------------------------------
// ********************************************************************************************************

using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// Manage the Fields associated with a dbase
    /// </summary>
    public class Fields
    {
        private readonly Dictionary<string, Field> _nameLookup = new Dictionary<string, Field>();
        private readonly Dictionary<int, Field> _posLookup = new Dictionary<int, Field>();

        /// <summary>
        /// Construct Fields and update the individual field data addresses
        /// </summary>
        /// <param name="fields"></param>
        public Fields(IEnumerable<Field> fields)
        {
            foreach (Field field in fields)
            {
                Add(field);
            }

            UpdateDataAddresses();
        }

        /// <summary>
        /// Retrieve field by ordinal
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Field this[int pos]
        {
            get { return _posLookup[pos]; }
        }

        /// <summary>
        /// Retrieve field by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Field this[string name]
        {
            get { return _nameLookup[name]; }
        }

        /// <summary>
        /// Get number of fields
        /// </summary>
        public int Count
        {
            get { return _posLookup.Count; }
        }

        /// <summary>
        /// Add a field
        /// </summary>
        /// <param name="field"></param>
        public void Add(Field field)
        {
            _posLookup.Add(field.Ordinal, field);
            _nameLookup.Add(field.ColumnName, field);
        }

        /// <summary>
        /// Update the DataAddress of each field
        /// </summary>
        public void UpdateDataAddresses()
        {
            int count = _posLookup.Count;
            int dataAddress = 0;
            for (int pos = 0; pos < count; pos++)
            {
                Field field = _posLookup[pos];
                field.DataAddress = dataAddress;
                dataAddress += field.Length;
            }
        }
    }
}