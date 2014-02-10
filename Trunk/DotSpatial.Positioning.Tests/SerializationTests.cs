using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using NUnit.Framework;

namespace DotSpatial.Positioning.Tests
{
    [TestFixture]
    class SerializationTests
    {
        [Test]
        public void DeserializeComplexObject()
        {
            // Create sample data
            var sourceData1 = new SampleData1
            {
                Position = new Position(new Latitude(10), new Longitude(10)),
                Position2 = new Position(new Latitude(15), new Longitude(25)),
                Distance = new Distance(100, DistanceUnit.Kilometers),
                Distance2 = new Distance(150, DistanceUnit.Feet),
                Angle = new Angle(15, 16, 17),
                Angle2 = new Angle(17, 18, 19),
                Area = new Area(10, AreaUnit.Acres),
                Area2 = new Area(15, AreaUnit.SquareCentimeters),
                Azimuth = new Azimuth(10, 15, 16),
                Azimuth2 = new Azimuth(11, 12, 13),
                CartesianPoint = new CartesianPoint(new Distance(10, DistanceUnit.Meters),
                    new Distance(11, DistanceUnit.Meters),
                    new Distance(12, DistanceUnit.StatuteMiles)),
                CartesianPoint2 = new CartesianPoint(new Distance(20, DistanceUnit.Meters),
                    new Distance(21, DistanceUnit.Meters),
                    new Distance(22, DistanceUnit.StatuteMiles)),
                Elevation = new Elevation(10, 11, 12),
                Elevation2 = new Elevation(20, 21, 22),
                Ellipsoid = new Ellipsoid("name", new Distance(10, DistanceUnit.Meters), 10),
                Ellipsoid2 = new Ellipsoid("name2", new Distance(11, DistanceUnit.Meters), 11),
                GeographicRectangle = new GeographicRectangle(new Latitude(20), new Longitude(15),
                    new Latitude(10), new Longitude(25)),
                GeographicRectangle2 = new GeographicRectangle(new Latitude(20), new Longitude(15),
                    new Latitude(10), new Longitude(25)),
                Latitude = new Latitude(17),
                Latitude2 = new Latitude(18),
                Longitude = new Longitude(17),
                Longitude2 = new Longitude(18),
                PointD = new PointD(10, 11),
                PointD2 = new PointD(12, 13),
                Position3D = new Position3D(new Distance(10, DistanceUnit.Meters), new Latitude(10), new Longitude(11)),
                Position3D2 = new Position3D(new Distance(11, DistanceUnit.Meters), new Latitude(12), new Longitude(13)),
                Radian = new Radian(10),
                Radian2 = new Radian(11),
                SizeD = new SizeD(new PointD(10, 11)),
                SizeD2 = new SizeD(new PointD(11, 12)),
                Speed = new Speed(10, SpeedUnit.FeetPerSecond),
                Speed2 = new Speed(11, SpeedUnit.Knots),
                Velocity = new Velocity(new Speed(10, SpeedUnit.Knots), 1),
                Velocity2 = new Velocity(new Speed(11, SpeedUnit.Knots), 2),
            };

            var sb = new StringBuilder();
            var memoryString = new StringWriter(sb);

            // Serialize it to a xml file
            var serializer = new XmlSerializer(typeof(SampleData1));
            serializer.Serialize(memoryString, sourceData1);

            // Read it back from the file
            var tr = new StringReader(sb.ToString());
            var targetData1 = (SampleData1)serializer.Deserialize(tr);

            foreach (var propertyInfo in typeof(SampleData1).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var expected = propertyInfo.GetValue(sourceData1, null);
                var actual = propertyInfo.GetValue(targetData1, null);
                Assert.AreEqual(expected, actual,
                    "Wrong Deserialization for " + propertyInfo.Name + " for type " + propertyInfo.PropertyType);
            }
        }
    }

    /// <summary>
    /// This class contains all ISerializable classes
    /// </summary>
    public class SampleData1
    {
// ReSharper disable UnusedAutoPropertyAccessor.Global
        public Position Position { get; set; }
        public Position Position2 { get; set; }
        public Distance Distance { get; set; }
        public Distance Distance2 { get; set; }
        public Angle Angle { get; set; }
        public Angle Angle2 { get; set; }
        public Area Area { get; set; }
        public Area Area2 { get; set; }
        public Azimuth Azimuth { get; set; }
        public Azimuth Azimuth2 { get; set; }
        public CartesianPoint CartesianPoint { get; set; }
        public CartesianPoint CartesianPoint2 { get; set; }
        public Elevation Elevation { get; set; }
        public Elevation Elevation2 { get; set; }
        public Ellipsoid Ellipsoid { get; set; }
        public Ellipsoid Ellipsoid2 { get; set; }
        public GeographicRectangle GeographicRectangle { get; set; }
        public GeographicRectangle GeographicRectangle2 { get; set; }
        public Latitude Latitude { get; set; }
        public Latitude Latitude2 { get; set; }
        public Longitude Longitude { get; set; }
        public Longitude Longitude2 { get; set; }
        public PointD PointD { get; set; }
        public PointD PointD2 { get; set; }
        public Position3D Position3D { get; set; }
        public Position3D Position3D2 { get; set; }
        public Radian Radian { get; set; }
        public Radian Radian2 { get; set; }
        public SizeD SizeD { get; set; }
        public SizeD SizeD2 { get; set; }
        public Speed Speed { get; set; }
        public Speed Speed2 { get; set; }
        public Velocity Velocity { get; set; }
        public Velocity Velocity2 { get; set; }
// ReSharper restore UnusedAutoPropertyAccessor.Global
    }
}