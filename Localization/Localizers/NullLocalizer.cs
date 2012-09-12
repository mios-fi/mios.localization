using System;
using System.Linq;

namespace Mios.Localization.Localizers {
	public class NullLocalizer {
		public static LocalizedString Instance(string key, params object[] args) {
      var parameters = args.Any() 
        ? "["+String.Join(",",args.Select(t=>(t??String.Empty).ToString()).ToArray())+"]"
        : String.Empty;
			return new LocalizedString(key+parameters, null);
		}
	}
}
