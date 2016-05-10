using System.Collections.Generic;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// This contains a list of types that were moved from one assembly to another when DotSpatial.Topology was replaced by NetTopologySuite/GeoAPI.
    /// This is used to make sure that the old dspx files that contain types from DotSpatial.Topology can be loaded using NetTopologySuite.
    /// </summary>
    public class MovedTypes
    {
        /// <summary>
        /// This contains all types that were moved when DotSpatial.Topology was replaced by NetTopologySuite/GeoAPI.
        /// </summary>
        public List<TypeMoveDefintion> Types;

        /// <summary>
        /// List with TypeMoveDefinitions of Namespaces/Apis that were moved.
        /// </summary>
        public MovedTypes()
        {
            Types = new List<TypeMoveDefintion>
            {
             new TypeMoveDefintion("DotSpatial.Topology", "GeoAPI.Geometries","DotSpatial.Topology", "GeoAPI"),
             new TypeMoveDefintion("DotSpatial.Topology", "NetTopologySuite.Geometries","DotSpatial.Topology", "NetTopologySuite"),
             new TypeMoveDefintion("DotSpatial.Topology", "DotSpatial.NTSExtension","DotSpatial.Topology", "DotSpatial.NTSExtension")
            };
        }
    }

    /// <summary>
    /// Class that defines a move from an old namespace to a new namespace.
    /// </summary>
    public class TypeMoveDefintion
    {
        private readonly string _from;
        private readonly string _to;
        private readonly string _fromApi;
        private readonly string _toApi;


        /// <summary>
        /// Creates a new TypeMoveDefinition.
        /// </summary>
        /// <param name="from">Old namespace that should be replaced.</param>
        /// <param name="to">Namespace that should replace the old namespace.</param>
        /// <param name="fromApi">Name of the Api that should be replaced.</param>
        /// <param name="toApi">Name of the Api that replaces the old Api name.</param>
        public TypeMoveDefintion(string from, string to, string fromApi, string toApi)
        {
            _from = from;
            _to = to;
            _fromApi = fromApi;
            _toApi = toApi;
        }

        /// <summary>
        /// Changes the old string to the new type.
        /// </summary>
        /// <param name="oldString"></param>
        /// <returns>String with new type.</returns>
        public string MoveType(string oldString)
        {
            string[] splits = oldString.Split(',');

            if (splits.Length >= 2)
            {
                return splits[0].Replace(_from, _to) + "," + splits[1].Replace(_fromApi, _toApi); 
            }
            return null;
        }

        /// <summary>
        /// Checks if the old string contains the from value of this definition to find out whether we have to apply this move. 
        /// </summary>
        /// <param name="oldString">String with the old definition.</param>
        /// <returns>True, if this move should be done.</returns>
        public bool IsApplicable(string oldString)
        {
            return oldString.StartsWith(_from);
        }
    }
}
