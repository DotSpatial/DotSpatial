// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
//
// The Original Code is from http://geoframework.codeplex.com/ version 2.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GeoFrameworks 2.0
// | Shade1974 (Ted Dunsford) | 10/21/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************

namespace DotSpatial.Positioning
{
#if Framework40
    /// <summary>
    /// Facilitates the creation of a deep copy of an object.
    /// </summary>
    /// <typeparam name="T">The destination type for the ICloneable interface.</typeparam>
    public interface ICloneable<out T>
    {
        /// <summary>
        /// Creates a deep copy of the object.
        /// </summary>
        /// <returns></returns>
        T Clone();
    }
#else
    public interface ICloneable<T>
    {
        T Clone();
    }
#endif
}