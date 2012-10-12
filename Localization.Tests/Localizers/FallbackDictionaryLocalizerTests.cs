using Mios.Localization.Localizers;
using Xunit;

namespace Mios.Localization.Tests.Localizers {
	public class FallbackDictionaryLocalizerTests {
		[Fact]
		public void ShouldReturnLocalizationFromDictionary() {
			var dictionary = new LocalizationDictionary { 
				{"sv-se","k","Key"}
			};
			var t = new FallbackDictionaryLocalizer(dictionary, "sv-se");
			Assert.Equal("Key", t.Localize("k"));
		}
    [Fact]
    public void ShouldReturnValueFromDefaultLocalizerIfNotFound() {
      var dictionary = new LocalizationDictionary();
      var t = new FallbackDictionaryLocalizer(dictionary, "sv-se") {
        DefaultLocalizer = (k, p) => new LocalizedString(k, "default")
      };
      Assert.Equal("default", t.Localize("k"));
    }
    [Fact]
    public void ShouldReturnEmptyLocalizedStringIfKeyIsNull() {
      var dictionary = new LocalizationDictionary();
      var t = new FallbackDictionaryLocalizer(dictionary, "sv-se");
      Assert.Null(t.Localize(null).Localization);
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