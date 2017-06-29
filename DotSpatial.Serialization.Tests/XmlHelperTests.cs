
using NUnit.Framework;
using DotSpatial.Serialization;

namespace MapWindow.Tests.XML
{
	[TestFixture]
	public class XmlHelperTests
	{
		[Test]
		public void TestEscapeInvalidCharacters()
		{
			const string input = "<>'\"&";
			string result = XmlHelper.EscapeInvalidCharacters(input);

			Assert.AreEqual("&lt;&gt;&apos;&quot;&amp;", result);
		}

		[Test]
		public void TestEscapeInvalidCharactersWhenAlreadyEscaped()
		{
			const string input = "&lt;&gt;&apos;&quot;&amp;";
			string result = XmlHelper.EscapeInvalidCharacters(input);

			Assert.AreEqual("&lt;&gt;&apos;&quot;&amp;", result);
		}

		[Test]
		public void TestUnEscapeInvalidCharacters()
		{
			const string input = "&lt;&gt;&apos;&quot;&amp;";
			string result = XmlHelper.UnEscapeInvalidCharacters(input);

			Assert.AreEqual("<>'\"&", result);
		}
	}
}