using System.Globalization;
using System.IO;
using Xunit;

namespace Mios.Localization.Tests {
	public class XmlResourceSetTest {

		private XmlResourceSet CreateSimpleSet(CultureInfo cultureInfo) {
			return new XmlResourceSet(new StringReader(@"<?xml version=""1.0""?>
				<page>
					<key id=""First"">
						<fi>Ensimmäinen</fi>
						<sv>Första</sv>
					</key>
					<key id=""Second"">
						<fi>Toinen</fi>
						<sv>Andra</sv>
					</key>
				</page>"), cultureInfo);
		}
		[Fact]
		public void GetSimple_ValueAvailable_ReturnsValue() {
			var set = CreateSimpleSet(new CultureInfo("sv"));
			Assert.Equal("Första", set.Get("First"));
			Assert.Equal("Andra", set.Get("Second"));
		}
		[Fact]
		public void GetSimple_NoValueAvailable_ReturnsNull() {
			var set = CreateSimpleSet(new CultureInfo("sv-fi"));
			Assert.Null(set.Get("NOTFOUND"));
		}


		private XmlResourceSet CreateRegionSet(CultureInfo cultureInfo) {
			return new XmlResourceSet(new StringReader(@"<?xml version=""1.0""?>
				<page>
					<key id=""bun"">
						<fi>sämpylä</fi>
						<sv>fralla</sv>
						<svFI>semla</svFI>
					</key>
					<key id=""bread"">
						<fi>leipä</fi>
						<sv>bröd</sv>
					</key>
				</page>"), cultureInfo);
		}

		[Fact]
		public void GetNeutralCulture_available_ReturnsNeutralCultureValue() {
			var set = CreateRegionSet(new CultureInfo("sv"));
			Assert.Equal("fralla", set.Get("bun"));
		}

		[Fact]
		public void GetRegionSpecificCulture_unavailable_ReturnsNeutralCultureValue() {
			XmlResourceSet set;
			set = CreateRegionSet(new CultureInfo("sv-fi"));
			Assert.Equal("bröd", set.Get("bread"));
		}

		[Fact]
		public void GetRegionSpecificCulture_available_ReturnsRegionSpecificCultureValue() {
			var set = CreateRegionSet(new CultureInfo("sv-fi"));
			Assert.Equal("semla", set.Get("bun"));
		}
	}
}
