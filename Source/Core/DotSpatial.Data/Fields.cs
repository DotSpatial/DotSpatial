// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// Manage the Fields associated with a dbase.
    /// </summary>
    public class Fields
    {
        #region Fields

        private readonly Dictionary<string, Field> _nameLookup = new();
        private readonly Dictionary<int, Field> _posLookup = new();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Fields"/> class and updates the individual field data addresses.
        /// </summary>
        /// <param name="fields">Fields that get managed by this.</param>
        public Fields(IEnumerable<Field> fields)
        {
            foreach (Field field in fields)
            {
                Add(field);
            }

            UpdateDataAddresses();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of fields.
        /// </summary>
        public int Count => _posLookup.Count;

        #endregion

        #region Indexers

        /// <summary>
        /// Gets the field by ordinal.
        /// </summary>
        /// <param name="pos">Position of the field.</param>
        /// <returns>The field at the given position.</returns>
        public Field this[int pos] => _posLookup[pos];

        /// <summary>
        /// Retrieve a field by name.
        /// </summary>
        /// <param name="name">Name of the field.</param>
        /// <returns>Field with the given name.</returns>
        public Field this[string name] => _nameLookup[name];

        #endregion

        #region Methods

        /// <summary>
        /// Adds a field.
        /// </summary>
        /// <param name="field">Field that gets added.</param>
        public void Add(Field field)
        {
            _posLookup.Add(field.Ordinal, field);
            _nameLookup.Add(field.ColumnName, field);
        }

        /// <summary>
        /// Updates the DataAddress of each field.
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

        #endregion
    }
}