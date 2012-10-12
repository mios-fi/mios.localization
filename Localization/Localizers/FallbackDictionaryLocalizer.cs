using System;
using System.Linq;

namespace Mios.Localization.Localizers {
	public class FallbackDictionaryLocalizer {
		LocalizationDictionary dictionary;
		public Localizer DefaultLocalizer { get; set; }
		private  string[] locales;
		public FallbackDictionaryLocalizer(LocalizationDictionary dictionary, params string[] locales) {
		  if(dictionary == null) throw new ArgumentNullException("dictionary");
		  if(locales == null) throw new ArgumentNullException("locales");
      DefaultLocalizer = NullLocalizer.Instance;
      this.dictionary = dictionary;
			this.locales = locales;
		}
		public LocalizedString Localize(string original, params object[] args) {
			if(original == null) return new LocalizedString();
			var localized = locales
				.Select(t => dictionary[t, original])
				.FirstOrDefault(t => t != null);
			if(localized == null) {
				return DefaultLocalizer(original, args);
			}
			localized = String.Format(localized, args);
			return new LocalizedString(String.Format(original, args), localized);
		}
	}
}
