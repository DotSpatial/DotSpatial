// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Serialization.Tests
{
    /// <summary>
    /// An object with an integer as member.
    /// </summary>
    public class ObjectWithIntMember
    {
        #region Fields

        [Serialize("Number", Formatter = typeof(TestHexFormatter), ConstructorArgumentIndex = 0)]
        private int _number;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectWithIntMember"/> class.
        /// </summary>
        /// <param name="number">The number.</param>
        public ObjectWithIntMember(int number)
        {
            _number = number;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        public int Number
        {
            get
            {
                return _number;
            }

            set
            {
                _number = value;
            }
        }

        #endregion
    }
}