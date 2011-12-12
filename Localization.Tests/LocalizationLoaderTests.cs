using System.IO;
using Xunit;

namespace Mios.Localization.Tests {
	public class LocalizationLoaderTests {
		[Fact]
		public void ReturnsFirstAvailableResult() {
			new LocalizationReader {
				new TextDictionaryHandler().Filter(t=>Path.GetExtension(t)==".txt"),
				new EmbeddedTextDictionaryHandler().Filter(t=>Path.GetExtension(t)==".cshtml"),
				new XmlDictionaryHandler().Filter(t=>Path.GetExtension(t)==".xml")
			}.WithVirtualPathsMapped();
		}
	}
}
