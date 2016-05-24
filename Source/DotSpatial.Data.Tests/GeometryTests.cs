using System.Collections.Generic;
using System.Linq;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    public class GeometryTests
    {
        [Test(Description = @"Checks that the resulting binary array doesn't have 4 unnecessary 0 bytes at the end. (https://github.com/DotSpatial/DotSpatial/issues/475)")]
        public void ToBinaryWithout4Zeros()
        {
            List<Coordinate> coords = new List<Coordinate>
            {
                new Coordinate(-84.6976494321476000, 41.7477691708729000),
                new Coordinate(-84.6402221482214000, 41.7477691708729000),
                new Coordinate(-84.6402221482214000, 41.6997514050474000),
                new Coordinate(-84.6974494321476000, 41.6997514050474000),
                new Coordinate(-84.6974494321476000, 41.6519336392220000),
                new Coordinate(-84.7548767160738000, 41.6519336392220000),
                new Coordinate(-84.7548767160738000, 41.6999514050475000),
                new Coordinate(-84.6976494321476000, 41.6999514050475000),
                new Coordinate(-84.6976494321476000, 41.7477691708729000),
            };

            Polygon p = new Polygon(new LinearRing(coords.ToArray()));
            var bin = p.ToBinary();

            Assert.AreEqual(157, bin.Length);
            Assert.AreNotEqual(0, bin[153]);
            Assert.AreNotEqual(0, bin[154]);
            Assert.AreNotEqual(0, bin[155]);
            Assert.AreNotEqual(0, bin[156]);
        }

        [Test(Description = @"https://github.com/DotSpatial/DotSpatial/issues/746")]
        [Ignore] // todo: Unignore after updating GeoAPI
        public void OverlapWith2XyEqualCoordinatesInARow()
        {
            #region cList definitions
            var cList1 = new List<Coordinate>()
            {
                new Coordinate(404907.751, 6471787.332, 107.90599999702),
                new Coordinate(404907.751, 6471787.332, 107.906000000003),
                new Coordinate(404907.748, 6471786.75, 107.913),
                new Coordinate(404909.313, 6471780.353, 107.992000000013),
                new Coordinate(404910.342, 6471775.076, 108.057000000001),
                new Coordinate(404910.739, 6471768.872, 108.131999999998)
            };

            var cList2 = new List<Coordinate>()
            {
                new Coordinate(403643.38, 6471428.533, 105.391000000003),
                new Coordinate(403655.126, 6471432.67, 105.509999999995),
                new Coordinate(403681.732, 6471442.614, 105.774999999994),
                new Coordinate(403709.495, 6471452.217, 106.097999999998),
                new Coordinate(403768.351, 6471471.859, 106.842000000004),
                new Coordinate(403808.526, 6471486.045, 107.251000000018),
                new Coordinate(403842.265, 6471497.695, 107.502999999997),
                new Coordinate(403872.106, 6471507.932, 107.569000000003),
                new Coordinate(403885.109, 6471512.253, 107.570000000022),
                new Coordinate(403897.809, 6471516.798, 107.570000000022),
                new Coordinate(403908.806, 6471520.209, 107.561000000002),
                new Coordinate(403920.496, 6471523.102, 107.538),
                new Coordinate(403936, 6471527.101, 107.50900000002),
                new Coordinate(403951.27, 6471530.917, 107.479000000007),
                new Coordinate(403967.298, 6471533.838, 107.379000000001),
                new Coordinate(403983.781, 6471536.211, 107.342000000004),
                new Coordinate(404001.67, 6471538.223, 107.372000000003),
                new Coordinate(404008.845, 6471539.031, 107.376000000004),
                new Coordinate(404038.665, 6471541.303, 107.657000000007),
                new Coordinate(404068.718, 6471544.306, 107.566000000021),
                new Coordinate(404102.126, 6471547.656, 107.320000000022),
                new Coordinate(404115.592, 6471549.174, 107.200000000012),
                new Coordinate(404120.202, 6471549.815, 107.165000000008),
                new Coordinate(404170.485, 6471555.509, 107.023000000016),
                new Coordinate(404218.371, 6471564.16, 107.841),
                new Coordinate(404266.142, 6471579.021, 107.308000000019),
                new Coordinate(404275.645, 6471581.107, 107.547999999995),
                new Coordinate(404295.22, 6471586.933, 107.798999999999),
                new Coordinate(404312.662, 6471594.065, 108.028999999995),
                new Coordinate(404345.322, 6471606.214, 107.884000000005),
                new Coordinate(404362.72, 6471612.33, 106.939000000013),
                new Coordinate(404370.226, 6471614.968, 106.691000000006),
                new Coordinate(404394.423, 6471623.278, 106.396999999997),
                new Coordinate(404411.605, 6471629.948, 106.494000000035),
                new Coordinate(404438.912, 6471639.042, 106.926000000007),
                new Coordinate(404470.538, 6471649.984, 107.447000000015),
                new Coordinate(404510.51, 6471663.29, 107.570000000022),
                new Coordinate(404546.025, 6471674.752, 107.732000000018),
                new Coordinate(404575.88, 6471685.013, 107.75900000002),
                new Coordinate(404613.138, 6471697.079, 107.875),
                new Coordinate(404657.753, 6471711.892, 108.015000000014),
                new Coordinate(404694.853, 6471724.396, 108.180000000008),
                new Coordinate(404725.929, 6471734.583, 108.377000000008),
                new Coordinate(404747.434, 6471741.897, 107.820000000022),
                new Coordinate(404756.761, 6471745.069, 108.397000000026),
                new Coordinate(404784.852, 6471754.261, 108.407000000007),
                new Coordinate(404808.738, 6471762.267, 108.550000000017),
                new Coordinate(404817.941, 6471764.939, 108.603000000003),
                new Coordinate(404827.475, 6471767.412, 108.656000000003),
                new Coordinate(404833.218, 6471768.981, 108.688000000009),
                new Coordinate(404840.24, 6471770.585, 108.747000000018),
                new Coordinate(404845.942, 6471771.198, 108.801000000021),
                new Coordinate(404852.977, 6471772.115, 108.843999999997),
                new Coordinate(404869.14, 6471772.1, 108.830000000002),
                new Coordinate(404901.213, 6471769.834, 108.304000000018),
                new Coordinate(404910.739, 6471768.872, 108.131999999998),
                new Coordinate(404946.338, 6471765.28, 107.558000000019),
                new Coordinate(404975.086, 6471762.622, 107.116999999998),
                new Coordinate(404988.827, 6471761.632, 106.909),
                new Coordinate(405006.055, 6471760.454, 106.739000000016),
                new Coordinate(405014.995, 6471759.967, 106.701000000015),
                new Coordinate(405023.925, 6471759.909, 106.707000000009),
                new Coordinate(405036.662, 6471760.04, 106.755000000019),
                new Coordinate(405044.804, 6471760.663, 106.801000000021),
                new Coordinate(405054.064, 6471762.011, 106.864000000001),
                new Coordinate(405065.696, 6471765.191, 106.889999999999),
                new Coordinate(405074.274, 6471767.871, 106.885000000009),
                new Coordinate(405083.813, 6471771.568, 106.864000000001),
                new Coordinate(405096.038, 6471776.334, 106.837),
                new Coordinate(405106.617, 6471781.635, 106.884000000005),
                new Coordinate(405118.888, 6471787.111, 107.055999999997),
                new Coordinate(405129.981, 6471791.946, 107.317999999999),
                new Coordinate(405138.014, 6471795.089, 107.555999999997),
                new Coordinate(405146.024, 6471797.846, 108.165000000008),
                new Coordinate(405156.669, 6471801.315, 108.717000000033),
                new Coordinate(405162.439, 6471802.921, 108.905000000028),
                new Coordinate(405173.434, 6471805.332, 109.151000000027),
                new Coordinate(405182.863, 6471807.124, 109.29300000002),
                new Coordinate(405193.114, 6471807.968, 109.389999999999),
                new Coordinate(405199.57, 6471808.162, 109.418000000005),
                new Coordinate(405208.054, 6471808.07, 109.455000000016),
                new Coordinate(405218.548, 6471807.231, 109.498999999996),
                new Coordinate(405230.21, 6471804.613, 109.516000000018),
                new Coordinate(405247.744, 6471800.078, 109.555999999997),
                new Coordinate(405270.247, 6471794.156, 109.677000000011),
                new Coordinate(405290.459, 6471788.619, 109.775999999998),
                new Coordinate(405312.549, 6471782.554, 109.862000000008),
                new Coordinate(405333.078, 6471777.104, 110.082999999999),
                new Coordinate(405352.259, 6471772.052, 110.505000000019),
                new Coordinate(405376.555, 6471765.785, 110.932000000015),
                new Coordinate(405394.879, 6471760.736, 111.134000000005),
                new Coordinate(405408.007, 6471757.722, 111.225999999995),
                new Coordinate(405419.143, 6471754.867, 111.043999999994),
                new Coordinate(405426.504, 6471752.98, 110.864000000001),
                new Coordinate(405433.29, 6471751.502, 110.760999999999),
                new Coordinate(405440.937, 6471750.169, 110.66700000003),
                new Coordinate(405450.667, 6471748.827, 110.593000000023),
                new Coordinate(405457.744, 6471747.706, 109.881999999998),
                new Coordinate(405466.238, 6471747.146, 109.263000000021),
                new Coordinate(405479.163, 6471746.813, 109.058999999994),
                new Coordinate(405484.798, 6471747.37, 109.072),
                new Coordinate(405492.145, 6471748.097, 109.193000000014),
                new Coordinate(405506.915, 6471750.644, 109.894),
                new Coordinate(405517.918, 6471752.623, 110.432000000015),
                new Coordinate(405524.734, 6471754.088, 110.551000000021),
                new Coordinate(405537.049, 6471758.859, 111.246000000014),
                new Coordinate(405550.662, 6471764.231, 111.52800000002),
                new Coordinate(405557.071, 6471767.121, 111.591),
                new Coordinate(405564.819, 6471770.614, 111.693000000014),
                new Coordinate(405581.163, 6471778.507, 111.893000000011),
                new Coordinate(405587.978, 6471781.798, 111.906000000003),
                new Coordinate(405616.121, 6471794.269, 111.628000000012),
                new Coordinate(405653.686, 6471811.788, 111.955000000016),
                new Coordinate(405682.556, 6471825.184, 111.534),
                new Coordinate(405710.314, 6471837.575, 110.535000000018),
                new Coordinate(405751.313, 6471857.474, 111.078999999998),
                new Coordinate(405784.196, 6471872.651, 110.188000000009),
                new Coordinate(405833.614, 6471895.717, 110.558000000019),
                new Coordinate(405862.643, 6471909.973, 110.286999999997),
                new Coordinate(405889.271, 6471922.241, 109.948000000033),
                new Coordinate(405900.484723792, 6471927.2667716, 109.918001569153),
            };

            #endregion

            var feature1 = new LineString(cList1.ToArray());
            var feature2 = new LineString(cList2.ToArray());

            Assert.DoesNotThrow(() => feature1.Overlaps(feature2));
        }

    }
}
