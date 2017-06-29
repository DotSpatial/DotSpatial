namespace DotSpatial.Projections
{
    public interface ICoordinateSystemCategoryHolder
    {
        /// <summary>
        /// Gets an array of all the names of the coordinate system categories
        /// in this collection of systems.
        /// </summary>
        string[] Names { get; }


        /// <summary>
        /// Given the string name, this will return the specified coordinate category
        /// </summary>
        /// <param name="name">Category name</param>
        /// <returns>Coordinate system category</returns>
        CoordinateSystemCategory GetCategory(string name);
    }
}