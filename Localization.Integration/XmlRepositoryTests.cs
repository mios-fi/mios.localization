using Mios.Localization;
using Xunit;
using System.Globalization;

namespace Mios.Localization.Integration {
	public class XmlRepositoryTests {
		[Fact]
		public void CanLoadFromRootDirectory() {
			var rep = new XmlResourceRepository("language");
			Assert.Equal("Yksi", rep.GetString("first;one", CultureInfo.CreateSpecificCulture("fi")));
		}
		[Fact]
		public void CanLoadFromSubDirectory() {
			var rep = new XmlResourceRepository("language");
			Assert.Equal("Två", rep.GetString("home\\second;two", CultureInfo.CreateSpecificCulture("sv")));
		}
		[Fact]
		public void CanLoadFromRootDirectoryWithFullLocale() {
			var rep = new XmlResourceRepository("language");
			Assert.Equal("Två", rep.GetString("home\\second;two", CultureInfo.CreateSpecificCulture("sv-fi")));
		}
	}
}
