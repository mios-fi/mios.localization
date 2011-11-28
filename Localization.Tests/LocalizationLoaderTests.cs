using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Mios.Localization.Tests {
	public class LocalizationLoaderTests {
		[Fact]
		public void ReturnsFirstAvailableResult() {
			var loader = new LocalizationLoader {
				new TextDictionaryLoader().Filter(t=>Path.GetExtension(t)==".txt"),
				new EmbeddedTextDictionaryLoader().Filter(t=>Path.GetExtension(t)==".cshtml"),
				new XmlDictionaryLoader().Filter(t=>Path.GetExtension(t)==".xml")
			}.WithVirtualPathsMapped();
		}
	}
}
