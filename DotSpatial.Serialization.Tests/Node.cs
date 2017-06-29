using System.Collections.Generic;
using DotSpatial.Serialization;

namespace MapWindow.Tests.XML.TestData
{
    public class Node
    {
        [Serialize("Data")]
        private object _data;

        [Serialize("Nodes")]
        private List<Node> _nodes;

        public Node()
        {
            _data = null;
            _nodes = new List<Node>();
        }

        public Node(object data)
        {
            _data = data;
            _nodes = new List<Node>();
        }

        public Node(object data, List<Node> nodes)
        {
            _data = data;
            _nodes = nodes;
        }

        public object Data
        {
            get { return _data; }
        }

        public List<Node> Nodes
        {
            get { return _nodes; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (_data == null)
                return ((Node)obj).Data == null;

            return _data.Equals(((Node)obj).Data);
        }

        public override int GetHashCode()
        {
            if (_data == null)
                return _nodes.Count;
            
            return _data.GetHashCode() ^ _nodes.Count;
        }
    }
}