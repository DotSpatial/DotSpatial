using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

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
        public void InValidFeedReturnsNotValid()
        {
            Feed feed = new Feed();
            feed.Name = "sample feed";
            feed.Url = "htt://example.com";
            FeedManager.Add(feed);

            Assert.IsFalse(feed.IsValid());
        }
    }
}