using System.Globalization;
using Xunit;

namespace Mios.Localization.Tests {
	public class CultureInfoExtensionsTests {
		[Fact]
		public void GetRegion_neutralCulture_ShouldReturnNull() {
			Assert.Null(new CultureInfo("sv").GetRegion());
		}
		[Fact]
		public void GetRegion_regionalCulture_ShouldReturnTwoLetterLowercaseCodeOfRegion() {
			Assert.Equal("fi", new CultureInfo("sv-fi").GetRegion());
			Assert.Equal("se", new CultureInfo("sv-se").GetRegion());
		}
	}
}
