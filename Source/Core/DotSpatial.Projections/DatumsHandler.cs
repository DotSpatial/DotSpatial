using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

[assembly: InternalsVisibleTo("DotSpatial.Projections.Tests")]
namespace DotSpatial.Projections
{
    internal class DatumsHandler
    {
        private Dictionary<string, DatumEntry> _entries;

        public void Initialize()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            string fileName = null;
            var currentAssemblyLocation = currentAssembly.Location;
            if (!string.IsNullOrEmpty(currentAssemblyLocation))
            {
                fileName = Path.Combine(Path.GetDirectoryName(currentAssemblyLocation), "datums.xml");
            }
            using (var datumStream = File.Exists(fileName)
                ? File.OpenRead(fileName)
                : DeflateStreamReader.DecodeEmbeddedResource("DotSpatial.Projections.XML.datums.xml.ds"))
            {

                var xmlSerializer = new XmlSerializer(typeof(DatumEntries));
                var entries = (DatumEntries)xmlSerializer.Deserialize(datumStream);
                _entries = entries.Items.ToDictionary(_ => _.Name, _ => _);
            }
        }

        public DatumEntry this[string name]
        {
            get
            {
                var ent = _entries;
                if (ent == null) return null;

                if (ent.TryGetValue(name, out DatumEntry de))
                    return de;
                return null;
            }
        }

        public int Count
        {
            get
            {
                var ent = _entries;
                return ent == null ? 0 : ent.Count;
            }
        }
    }

    [XmlRoot("Datums")]
    public class DatumEntries
    {
        public DatumEntries()
        {
            Items = new List<DatumEntry>();
        }

        [XmlElement("Datum")]
        public List<DatumEntry> Items { get; set; }
    }

    public class DatumEntry
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Type")]
        public DatumType Type { get; set; }

        [XmlAttribute("P1")]
        public string P1 { get; set; }

        [XmlAttribute("P2")]
        public string P2 { get; set; }

        [XmlAttribute("P3")]
        public string P3 { get; set; }

        [XmlAttribute("P4")]
        public string P4 { get; set; }

        [XmlAttribute("P5")]
        public string P5 { get; set; }

        [XmlAttribute("P6")]
        public string P6 { get; set; }

        [XmlAttribute("P7")]
        public string P7 { get; set; }

        [XmlAttribute("Shift")]
        public string Shift { get; set; }
    }
}