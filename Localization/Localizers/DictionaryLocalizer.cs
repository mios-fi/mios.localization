using System;
using System.Collections.Generic;

namespace Mios.Localization.Localizers {
	public class DictionaryLocalizer {
		private readonly IDictionary<string, string> source;
		public DictionaryLocalizer(IDictionary<string,string> source) {
			this.source = source;
		}
		public LocalizedString Localize(string original, params object[] args) {
			if(original==null) return new LocalizedString(); 
			string localized;
			source.TryGetValue(original, out localized);
			if(localized!=null) {
				localized = String.Format(localized, args);
			}
			return new LocalizedString(String.Format(original, args), localized);
		}
	}
}