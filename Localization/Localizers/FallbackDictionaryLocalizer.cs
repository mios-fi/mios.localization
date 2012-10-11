using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mios.Localization.Localizers {
	public class FallbackDictionaryLocalizer {
		LocalizationDictionary dictionary;
		public Localizer DefaultLocalizer { get; set; }
		private  string[] locales;
		public FallbackDictionaryLocalizer(LocalizationDictionary dictionary, params string[] locales) {
			this.dictionary = dictionary;
			this.locales = locales;
		}
		public LocalizedString Localize(string original, params object[] args) {
			if(original == null) return new LocalizedString();
			string localized = locales
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
