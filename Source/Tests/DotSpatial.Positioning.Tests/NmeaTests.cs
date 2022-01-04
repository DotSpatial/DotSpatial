// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace DotSpatial.Positioning.Tests
{
    /// <summary>
    /// Tests for NMEA sentences.
    /// </summary>
    [TestFixture]
    internal class NmeaTests
    {
        #region Methods

        /// <summary>
        /// Checks whether the build GpggkSentence equals the Gpggk string it was build from.
        /// </summary>
        [Test]
        public void GpggkSentenceFromString()
        {
            GpggkSentence sentence = new GpggkSentence("$GPGGK,113616.00,041006,4724.5248557,N,00937.1063064,E,3,12,1.7,EHT1171.742,M*6D");

            Assert.AreEqual("$GPGGK,113616.00,041006,4724.5248557,N,00937.1063064,E,3,12,1.7,EHT1171.742,M*6D", sentence.Sentence);
            Assert.AreEqual(FixQuality.PulsePerSecond, sentence.FixQuality);
            Assert.AreEqual(12, sentence.SatellitesInUse);
            Assert.AreEqual(new DilutionOfPrecision(1.7F), sentence.PositionDilutionOfPrecision);

            Latitude l = new Latitude(47, 24.5248557, LatitudeHemisphere.North);
            Assert.AreEqual(l, sentence.Position.Latitude);

            Longitude l2 = new Longitude(9, 37.1063064, LongitudeHemisphere.East);
            Assert.AreEqual(l2, sentence.Position.Longitude);

            Distance d = new Distance(1171.742, DistanceUnit.Meters).ToLocalUnitType();
            Assert.AreEqual(d, sentence.AltitudeAboveEllipsoid);

            DateTime date = new DateTime(2006, 10, 04, 11, 36, 16, DateTimeKind.Utc);
            Assert.AreEqual(date, sentence.UtcDateTime);
        }

        /// <summary>
        /// Checks whether the build GpgllSentence equals the Gpgll objects it was build from.
        /// </summary>
        [Test]
        public void GpgllSentenceFromObject()
        {
            TimeSpan ts = new TimeSpan(22, 54, 44);
            Latitude l = new Latitude(49, 16.45, LatitudeHemisphere.North);
            Longitude l2 = new Longitude(123, 11.12, LongitudeHemisphere.West);

            GpgllSentence sentence = new GpgllSentence(new Position(l, l2), ts, FixStatus.Fix);
            Assert.AreEqual("$GPGLL,4916.4500,N,12311.1200,W,225444.000,A*2F", sentence.Sentence);
            Assert.AreEqual(l, sentence.Position.Latitude);
            Assert.AreEqual(l2, sentence.Position.Longitude);
            Assert.AreEqual(FixStatus.Fix, sentence.FixStatus);
            Assert.AreEqual(new TimeSpan(22, 54, 44), sentence.UtcTime);
        }

        /// <summary>
        /// Checks whether the build GpgllSentence equals the Gpgll string it was build from.
        /// </summary>
        [Test]
        public void GpgllSentenceFromString()
        {
            Latitude l = new Latitude(49, 16.45, LatitudeHemisphere.North);
            Longitude l2 = new Longitude(123, 11.12, LongitudeHemisphere.West);

            GpgllSentence sentence = new GpgllSentence("$GPGLL,4916.45,N,12311.12,W,225444,A*1D");
            Assert.AreEqual("$GPGLL,4916.45,N,12311.12,W,225444,A*1D", sentence.Sentence);
            Assert.AreEqual(l, sentence.Position.Latitude);
            Assert.AreEqual(l2, sentence.Position.Longitude);
            Assert.AreEqual(FixStatus.Fix, sentence.FixStatus);
            Assert.AreEqual(new TimeSpan(22, 54, 44), sentence.UtcTime);
        }

        /// <summary>
        /// Checks whether the build GpgsaSentence equals the Gpgsa objects it was build from.
        /// </summary>
        [Test]
        public void GpgsaSentenceFromObjects()
        {
            List<Satellite> sateliteList = new List<Satellite>
                                           {
                                               new Satellite(19, new Azimuth(30.3), new Elevation(10.5), new SignalToNoiseRatio(50), true),
                                               new Satellite(28, new Azimuth(40.3), new Elevation(70.4), new SignalToNoiseRatio(50), true),
                                               new Satellite(14, new Azimuth(50.3), new Elevation(40.2), new SignalToNoiseRatio(50), true),
                                               new Satellite(18, new Azimuth(60.3), new Elevation(30.6), new SignalToNoiseRatio(50), true),
                                               new Satellite(27, new Azimuth(160.3), new Elevation(70.0), new SignalToNoiseRatio(50), true),
                                               new Satellite(22, new Azimuth(260.3), new Elevation(70.0), new SignalToNoiseRatio(50), true),
                                               new Satellite(31, new Azimuth(200.3), new Elevation(90.3), new SignalToNoiseRatio(50), true),
                                               new Satellite(39, new Azimuth(10.3), new Elevation(20.3), new SignalToNoiseRatio(50), true),
                                               new Satellite(24, new Azimuth(5.3), new Elevation(40.7), new SignalToNoiseRatio(50), true),
                                           };

            GpgsaSentence sentence = new GpgsaSentence(FixMode.Manual, FixMethod.Fix3D, sateliteList, new DilutionOfPrecision(1.2F), new DilutionOfPrecision(1.2F), new DilutionOfPrecision(1.2F));
            Assert.AreEqual("$GPGSA,M,3,19,28,14,18,27,22,31,39,24,,,,1.2,1.2,1.2*38", sentence.Sentence);
            Assert.AreEqual(FixMode.Manual, sentence.FixMode);
            Assert.AreEqual(FixMethod.Fix3D, sentence.FixMethod);

            DilutionOfPrecision dil = new DilutionOfPrecision(1.2F);
            Assert.AreEqual(dil, sentence.HorizontalDilutionOfPrecision);
            Assert.AreEqual(dil, sentence.PositionDilutionOfPrecision);
            Assert.AreEqual(dil, sentence.VerticalDilutionOfPrecision);
            Assert.AreEqual(sateliteList.Count, sentence.FixedSatellites.Count);

            for (int i = 0; i < Math.Min(sentence.FixedSatellites.Count, sateliteList.Count); i++)
            {
                Assert.AreEqual(sateliteList[i].PseudorandomNumber, sentence.FixedSatellites[i].PseudorandomNumber);
            }
        }

        /// <summary>
        /// Checks whether the build GpgsaSentence equals the Gpgsa string it was build from.
        /// </summary>
        [Test]
        public void GpgsaSentenceFromString()
        {
            GpgsaSentence sentence = new GpgsaSentence("$GPGSA,A,3,04,05,,09,12,,,24,,,,,2.5,1.3,2.1*39");
            Assert.AreEqual("$GPGSA,A,3,04,05,,09,12,,,24,,,,,2.5,1.3,2.1*39", sentence.Sentence);
            Assert.AreEqual(FixMode.Automatic, sentence.FixMode);
            Assert.AreEqual(FixMethod.Fix3D, sentence.FixMethod);
            Assert.AreEqual(new DilutionOfPrecision(2.5F), sentence.PositionDilutionOfPrecision);
            Assert.AreEqual(new DilutionOfPrecision(1.3F), sentence.HorizontalDilutionOfPrecision);
            Assert.AreEqual(new DilutionOfPrecision(2.1F), sentence.VerticalDilutionOfPrecision);
            Assert.AreEqual(5, sentence.FixedSatellites.Count);
            Assert.AreEqual(4, sentence.FixedSatellites[0].PseudorandomNumber);
            Assert.AreEqual(5, sentence.FixedSatellites[1].PseudorandomNumber);
            Assert.AreEqual(9, sentence.FixedSatellites[2].PseudorandomNumber);
            Assert.AreEqual(12, sentence.FixedSatellites[3].PseudorandomNumber);
            Assert.AreEqual(24, sentence.FixedSatellites[4].PseudorandomNumber);
        }

        /// <summary>
        /// Checks whether the build GpgsvSentence equals the Gpgsv objects it was build from.
        /// </summary>
        [Test]
        public void GpgsvSentenceFromObject()
        {
            List<Satellite> sats = new List<Satellite>
                                   {
                                       new Satellite(2, new Azimuth(282.00), new Elevation(59.00), new SignalToNoiseRatio(0)),
                                       new Satellite(3, new Azimuth(287.00), new Elevation(42.00), new SignalToNoiseRatio(0)),
                                       new Satellite(6, new Azimuth(94.00), new Elevation(16.00), new SignalToNoiseRatio(0)),
                                       new Satellite(15, new Azimuth(90.00), new Elevation(80.00), new SignalToNoiseRatio(48))
                                   };

            GpgsvSentence sentence = new GpgsvSentence(2, 1, 8, sats);
            Assert.AreEqual("$GPGSV,2,1,8,02,59,282,00,03,42,287,00,06,16,094,00,15,80,090,48*49", sentence.Sentence);
            Assert.AreEqual(2, sentence.TotalMessageCount);
            Assert.AreEqual(1, sentence.CurrentMessageNumber);
            Assert.AreEqual(8, sentence.SatellitesInView);
            Assert.AreEqual(sats.Count, sentence.Satellites.Count);

            for (int i = 0; i < sats.Count; i++)
            {
                Assert.AreEqual(sats[i], sentence.Satellites[i]);
            }
        }

        /// <summary>
        /// Checks whether the build GpgsvSentence equals the Gpgsv string it was build from.
        /// </summary>
        [Test]
        public void GpgsvSentenceFromString()
        {
            List<Satellite> sats = new List<Satellite>
                                   {
                                       new Satellite(2, new Azimuth(282.00), new Elevation(59.00), new SignalToNoiseRatio(0)),
                                       new Satellite(3, new Azimuth(287.00), new Elevation(42.00), new SignalToNoiseRatio(0)),
                                       new Satellite(6, new Azimuth(94.00), new Elevation(16.00), new SignalToNoiseRatio(0)),
                                       new Satellite(15, new Azimuth(90.00), new Elevation(80.00), new SignalToNoiseRatio(48))
                                   };

            GpgsvSentence sentence = new GpgsvSentence("$GPGSV,2,1,08,02,59,282,00,03,42,287,00,06,16,094,00,15,80,090,48*79");
            Assert.AreEqual("$GPGSV,2,1,08,02,59,282,00,03,42,287,00,06,16,094,00,15,80,090,48*79", sentence.Sentence);
            Assert.AreEqual(2, sentence.TotalMessageCount);
            Assert.AreEqual(1, sentence.CurrentMessageNumber);
            Assert.AreEqual(8, sentence.SatellitesInView);
            Assert.AreEqual(sats.Count, sentence.Satellites.Count);
            for (int i = 0; i < sats.Count; i++)
            {
                Assert.AreEqual(sats[i], sentence.Satellites[i]);
            }
        }

        /// <summary>
        /// Checks whether the build GprmcSentence equals the Gprmc objects it was build from.
        /// </summary>
        [Test]
        public void GprmcSentenceFromObjects()
        {
            Latitude l = new Latitude(51, 33.82, LatitudeHemisphere.North);
            Longitude l2 = new Longitude(0, 42.24, LongitudeHemisphere.West);
            Position p = new Position(l, l2);
            DateTime date = new DateTime(2004, 06, 13, 22, 05, 16, DateTimeKind.Utc);
            Speed s = new Speed(173.8, SpeedUnit.Knots);
            Azimuth a = new Azimuth(231.8);
            Longitude mv = new Longitude(4.2, LongitudeHemisphere.West);

            GprmcSentence sentence = new GprmcSentence(date, true, p, s, a, mv);
            Assert.AreEqual("$GPRMC,220516.000,A,5133.8200,N,00042.2400,W,173.8,231.8,130604,4.2,W*67", sentence.Sentence);

            Assert.AreEqual(FixStatus.Fix, sentence.FixStatus);
            Assert.AreEqual(l, sentence.Position.Latitude);
            Assert.AreEqual(l2, sentence.Position.Longitude);
            Assert.AreEqual(s, sentence.Speed);
            Assert.AreEqual(a, sentence.Bearing);
            Assert.AreEqual(mv, sentence.MagneticVariation);
            Assert.AreEqual(date, sentence.UtcDateTime);
        }

        /// <summary>
        /// Checks whether the build GprmcSentence equals the Gprmc string it was build from.
        /// </summary>
        [Test]
        public void GprmcSentenceFromString()
        {
            GprmcSentence sentence = new GprmcSentence("$GPRMC,194530.000,A,3051.8007,N,10035.9989,W,1.49,111.67,310714,,,A*74");
            Assert.AreEqual("$GPRMC,194530.000,A,3051.8007,N,10035.9989,W,1.49,111.67,310714,,,A*74", sentence.Sentence);
            Assert.AreEqual(FixStatus.Fix, sentence.FixStatus);

            Latitude l = new Latitude(30, 51.8007, LatitudeHemisphere.North);
            Assert.AreEqual(l, sentence.Position.Latitude);

            Longitude l2 = new Longitude(100, 35.9989, LongitudeHemisphere.West);
            Assert.AreEqual(l2, sentence.Position.Longitude);

            Assert.AreEqual(new Speed(1.49, SpeedUnit.Knots), sentence.Speed);
            Assert.AreEqual(111.67, sentence.Bearing.DecimalDegrees);
            Assert.AreEqual(Longitude.Invalid, sentence.MagneticVariation);

            DateTime date = new DateTime(2014, 07, 31, 19, 45, 30, DateTimeKind.Utc);
            Assert.AreEqual(date, sentence.UtcDateTime);
        }

        /// <summary>
        /// Checks whether the build GpvtgSentence equals the Gpvtg string it was build from.
        /// </summary>
        [Test]
        public void GpvtgSentenceFromString()
        {
            GpvtgSentence sentence = new GpvtgSentence("$GPVTG,054.7,T,034.4,M,005.5,N,010.2,K*48");
            Assert.AreEqual("$GPVTG,054.7,T,034.4,M,005.5,N,010.2,K*48", sentence.Sentence);
            Assert.AreEqual(new Speed(5.5, SpeedUnit.Knots), sentence.Speed);
            Assert.AreEqual(54.7, sentence.Bearing.DecimalDegrees);
        }

        /// <summary>
        /// Checks whether the build PgrmfSentence equals the Pgrmf string it was build from.
        /// </summary>
        [Test]
        public void PgrmfSentenceFromString()
        {
            Latitude l = new Latitude(1, 0.5, LatitudeHemisphere.North);
            Longitude l2 = new Longitude(1, 23.0, LongitudeHemisphere.West);
            Position p = new Position(l, l2);
            DateTime dt = new DateTime(2003, 12, 4, 21, 59, 48);
            Azimuth a = new Azimuth(62.0);
            Speed s = new Speed(0, SpeedUnit.KilometersPerHour);

            PgrmfSentence sentence = new PgrmfSentence("$PGRMF,223,424801,041203,215948,13,0100.5000,N,00123.0000,W,A,2,0,62,2,1*35");
            Assert.AreEqual("$PGRMF,223,424801,041203,215948,13,0100.5000,N,00123.0000,W,A,2,0,62,2,1*35", sentence.Sentence);

            Assert.AreEqual(dt, sentence.UtcDateTime);
            Assert.AreEqual(p, sentence.Position);
            Assert.AreEqual(FixMode.Automatic, sentence.FixMode);
            Assert.AreEqual(FixMethod.Fix3D, sentence.FixMethod);
            Assert.AreEqual(s, sentence.Speed);
            Assert.AreEqual(a, sentence.Bearing);
            Assert.AreEqual(new DilutionOfPrecision(2), sentence.PositionDilutionOfPrecision);

            /*
             * 2.2.3
                GPS Fix Data Sentence (PGRMF)
                $PGRMF,<1>,<2>,<3>,<4>,<5>,<6>,<7>,<8>,<9>,<10>,<11>,<12>,<13>,<14>,<15>*hh<CR><LF>
               <0>  GPS week number (0 - 1023)
               <1>  GPS seconds (0 - 604799)
               <2>  UTC date of position fix, ddmmyy format
               <3>  UTC time of position fix, hhmmss format
               <4>  GPS leap second count
               <5>  Latitude, ddmm.mmmm format (leading zeros will be transmitted)
               <6>  Latitude hemisphere, N or S
               <7>  Longitude, dddmm.mmmm format (leading zeros will be transmitted)
               <8>  Longitude hemisphere, E or W
               <9>  Mode, M = manual, A = automatic
               <10> Fix type, 0 = no fix, 1 = 2D fix, 2 = 3D fix
               <11> Speed over ground, 0 to 1051 kilometers/hour
               <12> Course over ground, 0 to 359 degrees, true
               <13> Position dilution of precision, 0 to 9 (rounded to nearest integer value)
               <14> Time dilution of precision, 0 to 9 (rounded to nearest integer value)
               *hh <CR><LF>
             */
        }

        /// <summary>
        /// Checks whether the GpgsvSentence that was build from reading a Gprmc string with NmeaReader contains all the objects it should.
        /// </summary>
        [Test]
        public void ReadGprmcWithNmeaReader()
        {
            using (MemoryStream mStrm = new MemoryStream(Encoding.ASCII.GetBytes("$GPRMC,220516.000,A,5133.8200,N,00042.2400,W,173.8,231.8,130604,4.2,W*67")))
            {
                NmeaReader nr = new NmeaReader(mStrm);
                NmeaSentence ns = nr.ReadTypedSentence();
                GprmcSentence sentence = ns as GprmcSentence;
                Assert.AreNotEqual(null, sentence);
                if (sentence == null) return;

                Assert.AreEqual("$GPRMC,220516.000,A,5133.8200,N,00042.2400,W,173.8,231.8,130604,4.2,W*67", sentence.Sentence);

                Latitude l = new Latitude(51, 33.82, LatitudeHemisphere.North);
                Longitude l2 = new Longitude(0, 42.24, LongitudeHemisphere.West);
                DateTime date = new DateTime(2004, 06, 13, 22, 05, 16, DateTimeKind.Utc);
                Speed s = new Speed(173.8, SpeedUnit.Knots);
                Azimuth a = new Azimuth(231.8);
                Longitude mv = new Longitude(4.2, LongitudeHemisphere.West);

                Assert.AreEqual(FixStatus.Fix, sentence.FixStatus);
                Assert.AreEqual(l, sentence.Position.Latitude);
                Assert.AreEqual(l2, sentence.Position.Longitude);
                Assert.AreEqual(s, sentence.Speed);
                Assert.AreEqual(a, sentence.Bearing);
                Assert.AreEqual(mv, sentence.MagneticVariation);
                Assert.AreEqual(date, sentence.UtcDateTime);
            }
        }

        #endregion
    }
}