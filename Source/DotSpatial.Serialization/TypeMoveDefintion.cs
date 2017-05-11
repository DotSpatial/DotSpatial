namespace DotSpatial.Serialization
{
    /// <summary>
    /// Class that defines a move from an old namespace to a new namespace.
    /// </summary>
    public class TypeMoveDefintion
    {
        #region Fields

        private readonly string _from;
        private readonly string _fromApi;
        private readonly string _to;
        private readonly string _toApi;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeMoveDefintion"/> class.
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

        #endregion

        #region Methods

        /// <summary>
        /// Checks if the old string contains the from value of this definition to find out whether we have to apply this move.
        /// </summary>
        /// <param name="oldString">String with the old definition.</param>
        /// <returns>True, if this move should be done.</returns>
        public bool IsApplicable(string oldString)
        {
            return oldString.StartsWith(_from);
        }

        /// <summary>
        /// Changes the old string to the new type.
        /// </summary>
        /// <param name="oldString">The old string.</param>
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

        #endregion
    }
}