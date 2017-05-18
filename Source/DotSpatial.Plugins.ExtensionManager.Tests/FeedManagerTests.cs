// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Linq;
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;

namespace DotSpatial.Plugins.ExtensionManager.Tests
{
    /// <summary>
    /// Tests for the feed manager.
    /// </summary>
    [TestClass]
    public class FeedManagerTests
    {
        #region Methods

        /// <summary>
        /// Checks that a newly added feed is returned by GetFeeds.
        /// </summary>
        [TestMethod]
        public void AddedFeedAppearsInGetFeeds()
        {
            FeedManager.ClearFeeds();

            Feed feed = new Feed
            {
                Name = "sample feed",
                Url = "https://example.com"
            };
            FeedManager.Add(feed);

            var expected = FeedManager.GetFeeds();
            foreach (var item in expected)
            {
                if (item.Name.Equals(feed.Name, StringComparison.OrdinalIgnoreCase) && item.Url.Equals(feed.Url, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            Assert.Fail("The feed was not returned by GetFeeds.");
        }

        /// <summary>
        /// Checks that GetFeeds doesn't return null values.
        /// </summary>
        [TestMethod]
        public void GetFeedsReturnsNonNullValue()
        {
            var expected = FeedManager.GetFeeds();
            Assert.IsNotNull(expected);
        }

        /// <summary>
        /// Checks that a feed with an invalid url indicates that it is invalid.
        /// </summary>
        [TestMethod]
        public void InvalidUrlReturnsNotValid()
        {
            FeedManager.ClearFeeds();

            Feed feed = new Feed
            {
                Name = "sample feed",
                Url = "htt:// example.com"
            };

            Assert.IsFalse(feed.IsValid());
        }

        /// <summary>
        /// Checks that removing a feed that is not in the list does nothing.
        /// </summary>
        [TestMethod]
        public void RemoveFeedThatIsNotInTheListDoesNothing()
        {
            FeedManager.ClearFeeds();

            Feed feed = new Feed
            {
                Name = "sample feed",
                Url = "https://example.com"
            };
            FeedManager.Add(feed);

            var before = FeedManager.GetFeeds();

            FeedManager.Remove(feed);

            var after = FeedManager.GetFeeds();
            Assert.AreEqual(before.Count(), after.Count(), "The number of feeds should be the same if an attempt is made to remove a feed that is not in the list.");
        }

        #endregion
    }
}