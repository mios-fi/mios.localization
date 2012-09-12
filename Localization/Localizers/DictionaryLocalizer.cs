using System;
using System.Collections.Generic;

namespace Mios.Localization.Localizers {
	public class DictionaryLocalizer {
		private readonly IDictionary<string, string> source;
	  private readonly Localizer defaultLocalizer;

	  public DictionaryLocalizer(IDictionary<string, string> source) : this(source, NullLocalizer.Instance) {
    }
	  public DictionaryLocalizer(IDictionary<string,string> source, Localizer defaultLocalizer) {
	    this.source = source;
	    this.defaultLocalizer = defaultLocalizer;
	  }

	  public LocalizedString Localize(string original, params object[] args) {
			if(original==null) return new LocalizedString(); 
			string localized;
      if(!source.TryGetValue(original, out localized)) {
        return defaultLocalizer(original, args);
      }
			if(localized!=null) {
				localized = String.Format(localized, args);
			}
			return new LocalizedString(String.Format(original, args), localized);
		}
	}
}