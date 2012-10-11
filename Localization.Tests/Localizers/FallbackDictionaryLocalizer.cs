using Mios.Localization.Localizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Mios.Localization.Tests.Localizers {
	public class FallbackDictionaryLocalizerTests {
		[Fact]
		public void ShouldReturnLocalizationFromDictionary() {
			var dictionary = new LocalizationDictionary { 
				{"sv-se","k","Key"}
			};
			var t = new FallbackDictionaryLocalizer(dictionary, "sv-se");
			Assert.Equal("Key", t.Localize("k", "sv-se"));
		}
		[Fact]
		public void ShouldReturnValueFromDefaultLocalizerIfNotFound() {
			var dictionary = new LocalizationDictionary();
			var t = new FallbackDictionaryLocalizer(dictionary, "sv-se") { 
				DefaultLocalizer = (k,p) => new LocalizedString(k,"default")
			};
			Assert.Equal("default", t.Localize("k", "sv-se"));
		}
		[Fact]
		public void ShouldReturnFallbackLocalizationIfFullLocaleNotFound() {
			var dictionary = new LocalizationDictionary { 
				{"sv","k","Fallback"}
			};
			var t = new FallbackDictionaryLocalizer(dictionary, "sv-se","sv");
			Assert.Equal("Fallback", t.Localize("k"));
		}
	}
}