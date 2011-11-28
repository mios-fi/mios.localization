using System;

namespace Mios.Localization {
	public class NullLocalizer {
		public static LocalizedString Instance(string key, params object[] args) {
			return new LocalizedString(String.Format(key,args), null);
		}
	}
}
