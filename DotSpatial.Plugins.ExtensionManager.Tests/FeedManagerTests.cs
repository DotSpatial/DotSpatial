using System;
using System.Linq;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using ClassCleanup = NUnit.Framework.TestFixtureTearDownAttribute;
using ClassInitialize = NUnit.Framework.TestFixtureSetUpAttribute;
using Assert = NUnit.Framework.Assert;

namespace DotSpatial.Plugins.ExtensionManager.Tests
{
    [TestClass]
    public class FeedManagerTests
    {
        [TestMethod]
        public void GetFeedsReturnsNonNullValue()
        {
            var expected = FeedManager.GetFeeds();
            Assert.IsNotNull(expected);
        }

        [TestMethod]
        public void AddedFeedAppearsinGetFeeds()
        {
            FeedManager.ClearFeeds();

            Feed feed = new Feed();
            feed.Name = "sample feed";
            feed.Url = "https://example.com";
            FeedManager.Add(feed);

            var expected = FeedManager.GetFeeds();
            foreach (var item in expected)
            {
                if (item.Name.Equals(feed.Name, StringComparison.OrdinalIgnoreCase) &&
                    item.Url.Equals(feed.Url, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }
            Assert.Fail("The feed was not returned by GetFeeds.");
        }

        [TestMethod]
        public void RemoveFeedThatIsNotInTheListDoesNothing()
        {
            FeedManager.ClearFeeds();

            Feed feed = new Feed();
            feed.Name = "sample feed";
            feed.Url = "https://example.com";
            FeedManager.Add(feed);

            var before = FeedManager.GetFeeds();

            FeedManager.Remove(feed);

            var after = FeedManager.GetFeeds();
            Assert.AreEqual(before.Count(), after.Count(), "The number of feeds should be the same if an attempt is made to remove a feed that is not in the list.");
        }

        [TestMethod]
        public void InvalidUrlReturnsNotValid()
        {
            FeedManager.ClearFeeds();

            Feed feed = new Feed();
            feed.Name = "sample feed";
            feed.Url = "htt://example.com";

            Assert.IsFalse(feed.IsValid());
        }
    }
}