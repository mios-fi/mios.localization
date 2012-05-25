using System.IO;
using System.Xml.Linq;
using Mios.Localization.Implementations;
using Xunit;

namespace Mios.Localization.Tests {
	public class XmlDictionaryHandlerTests {
		[Fact]
		public void WritesKeysToStream() {
			var dic = new LocalizationDictionary();
			dic["fi", "first"] = "ensimmäinen";
			dic["sv", "first"] = "första";
			dic["sv-fi", "first"] = "först";
			var writer = new StringWriter();
			new XmlDictionaryHandler().Write(dic, writer);
			var doc = XDocument.Parse(writer.ToString());
			var e = doc.Root.Element("key");
			Assert.Equal("first", e.Attribute("id").Value);
			Assert.Equal("ensimmäinen", e.Element("fi").Value);
			Assert.Equal("första", e.Element("sv").Value);
			Assert.Equal("först", e.Element("svFI").Value);
		}
		[Fact]
		public void RemapsLocaleStrings() {
			const string source = @"<?xml version=""1.0""?><page><key id=""first""><fi>ensimmäinen</fi><sv>första</sv><svFI>först</svFI></key></page>";
			var dic = new XmlDictionaryHandler().Read(new StringReader(source));
			Assert.Equal("först",dic["sv-fi", "first"]);
		}
	}
}
