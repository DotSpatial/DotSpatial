﻿using System;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;
using GisSharpBlog.NetTopologySuite.IO;
using NUnit.Framework;

namespace GisSharpBlog.NetTopologySuite.Tests.Various
{
    [TestFixture]
    public class Issue37Tests
    {
        private readonly IGeometryFactory factory = GeometryFactory.Default;

        private WKTReader reader;        

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            reader = new WKTReader(factory);
        }

        [Test]
        public void Difference()
        {
            var geom1 = reader.Read(@"POLYGON((-17445.395049241037
7027956.74246328,-17043.826645147976 7129954.0712792575,17589.339716189785
7130067.1462710546,39761.968883175316 7126950.9864603812,25010.654925657538
7021989.9337616432,-17445.395049241037
7027956.74246328),(30821.194551129327 7063334.0714834519,37105.041271255621
7108045.9641759554,35410.746940282996 7107926.8247523149,31448.55806386597
7107298.5311717829,27554.224237946335 7106327.9096971015,23756.521966153046
7105022.126693625,20083.582138775229 7103390.830451577,16562.699326805414
7101446.0861854088,13220.144029514759 7099202.2944224058,10080.978700195139
7096676.09311996,7168.878465985913 7093886.243930826,4505.9575566306048
7090853.5031201011,2112.6025589346814 7087600.4777273331,7.3137134966055184
7084151.4676621584,-1793.4444372516909 7080532.2945225,-3275.3816608225288
7076770.1180302547,-4426.5084553028482
7072893.2410903079,-5237.248788301129
7068930.9045935431,-3404.9524430905353
7068639.0074323155,-1572.4133228090652
7068347.8751512486,260.36825319335821 7068057.5079015717,2093.391965481344
7067767.90583437,3926.6574945381153 7067479.06910061,5760.1645207611382
7067190.9978511157,7593.9127244658039 7066903.6922365744,9427.9017858850675
7066617.1524075512,11262.131385167513 7066331.3785144677,13096.601202378995
7066046.3707076181,14931.310917502315 7065762.1291371593,16766.260210437264
7065478.6539531145,18601.448761000269 7065195.9453053745,20436.876248926485
7064914.0033436986,22272.542353865396 7064632.8282177085,24108.446755386525
7064352.4200768927,25944.589132975067 7064072.7790706065,27780.9691660335
7063793.9053480756,29617.586533883361 7063515.7990583824,30821.194551129327
7063334.0714834519))");
            Assert.IsNotNull(geom1);
            Assert.IsTrue(geom1.IsValid);

            var geom2 = reader.Read(@"POLYGON((14518.078277259594
7023464.5692419875,11497.562933872843 7025662.3046554783,8431.9481214536681
7028335.26856397,5617.0474384991358 7031264.45683008,3074.235016295675
7034427.0837329868,822.64580979566688
7037798.5997495521,-1120.9633072352574
7041352.8931691349,-2742.3294252505157 7045062.50264867,-4029.7759410829603
7048898.8386645,-4974.2811578741557 7052832.4117995631,-5569.5232447888984
7056833.0658134529,-5811.90194162676 7060870.21347627,-5700.5376509662119
7064913.0732023632,-5237.248788301129
7068930.9045935431,-3404.9524430905353
7068639.0074323155,-1572.4133228090652
7068347.8751512486,260.36825319335821 7068057.5079015717,2093.391965481344
7067767.90583437,3926.6574945381153 7067479.06910061,5760.1645207611382
7067190.9978511157,7593.9127244658039 7066903.6922365744,9427.9017858850675
7066617.1524075512,11262.131385167513 7066331.3785144677,13096.601202378995
7066046.3707076181,14931.310917502315 7065762.1291371593,16766.260210437264
7065478.6539531145,18601.448761000269 7065195.9453053745,20436.876248926485
7064914.0033436986,22272.542353865396 7064632.8282177085,24108.446755386525
7064352.4200768927,25944.589132975067 7064072.7790706065,27780.9691660335
7063793.9053480756,29617.586533883361 7063515.7990583824,30821.194551129327
7063334.0714834519,25010.654925657538 7021989.9337616432,14518.078277259594
7023464.5692419875))");
            Assert.IsNotNull(geom2);
            Assert.IsTrue(geom2.IsValid);

            Exception exception = null;
            try
            {
                geom1.Difference(geom2);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(TopologyException), exception);

            var buf1 = geom1.Buffer(0);
            Assert.IsNotNull(buf1);
            Assert.IsTrue(buf1.IsValid);

            var buf2 = geom2.Buffer(0);
            Assert.IsNotNull(buf2);
            Assert.IsTrue(buf2.IsValid);

            var result = buf1.Difference(buf2);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
        }
    }
}
