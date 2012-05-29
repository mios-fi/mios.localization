using System.Linq;
using Xunit;

namespace Mios.Localization.Tests {
	public class LocalizationDictionaryTests {
		[Fact]
		public void AddPerservesKeyNameCase() {
			var dictionary = new LocalizationDictionary();
			dictionary["fi-fi", "UPPERCASE"] = "true";
			dictionary["fi-fi", "lowercase"] = "true";
			var keys = dictionary.Keys.ToArray();
			Assert.Contains("UPPERCASE", keys);
			Assert.Contains("lowercase", keys);
		}
		[Fact]
		public void AddPerservesKeyOrder() {
			var dictionary = new LocalizationDictionary();
			dictionary["fi-fi", "UPPERCASE"] = "true";
			dictionary["sv-se", "lowercase"] = "true";
			dictionary["fi-se", "UPPERCASE"] = "true";
			var keys = dictionary.Keys.ToArray();
			Assert.Equal(new [] { "UPPERCASE", "lowercase"}, keys.ToArray());
		}
	}
}
