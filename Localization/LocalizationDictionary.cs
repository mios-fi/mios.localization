using System;
using System.Collections.Generic;

namespace Mios.Localization {
	public class LocalizationDictionary {
		private readonly IEqualityComparer<string> comparer;
		private readonly List<string> keys;
		private readonly HashSet<string> locales;
		private readonly Dictionary<string,Dictionary<string,string>> dictionaries;
		public LocalizationDictionary() : this(StringComparer.InvariantCultureIgnoreCase) {
		}
		public LocalizationDictionary(IEqualityComparer<string> comparer) {
			this.comparer = comparer;
			keys = new List<string>();
			locales = new HashSet<string>(comparer);
			dictionaries = new Dictionary<string, Dictionary<string, string>>(comparer);
		}
		public string this[string locale, string key] {
			get {
				Dictionary<string, string> dictionary;
				if(!dictionaries.TryGetValue(locale, out dictionary)) {
					return null;
				}
				string value;
				if(dictionary.TryGetValue(key, out value)) {
					return value;
				}
				return null;
			}
			set {
				locales.Add(locale);
				if(!keys.Contains(key.ToLowerInvariant())) {
					keys.Add(key.ToLowerInvariant());
				}
				Dictionary<string, string> dictionary;
				if(!dictionaries.TryGetValue(locale, out dictionary)) {
					dictionaries.Add(locale, dictionary = new Dictionary<string, string>(comparer));
				}
				dictionary[key] = value;
			}
		}
		public IDictionary<string, string> InLocale(string locale) {
			Dictionary<string, string> dictionary;
			if(!dictionaries.TryGetValue(locale, out dictionary)) {
				dictionary = new Dictionary<string, string>(comparer);
			}
			return new ReadOnlyDictionary<string, string>(dictionary);
		}
		public IEnumerable<string> Keys {
			get { return keys; }
		}
		public IEnumerable<string> Locales {
			get { return locales; }
		}
	}
}
