using System;
using System.Collections.Generic;
using Mios.Localization.Utilities;

namespace Mios.Localization {
	public interface ILocalizationDictionary {
		string this[string locale, string key] { get; set; }
		IEnumerable<string> Keys { get; }
		IEnumerable<string> Locales { get; }
		IDictionary<string, string> InLocale(string locale);
	}

	public class LocalizationDictionary : ILocalizationDictionary {
		private readonly IEqualityComparer<string> comparer;
		private readonly List<string> keys;
		private readonly HashSet<string> localesHash;
		private readonly HashSet<string> keysHash;
		private readonly Dictionary<string, Dictionary<string, string>> dictionaries;
		public LocalizationDictionary() : this(StringComparer.InvariantCulture) {
		}
		public LocalizationDictionary(IEqualityComparer<string> comparer) {
			this.comparer = comparer;
			keys = new List<string>();
			localesHash = new HashSet<string>(comparer);
			keysHash = new HashSet<string>(comparer);
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
				localesHash.Add(locale);
				if(!keysHash.Contains(key)) {
					keysHash.Add(key);
					keys.Add(key);
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
			get { return localesHash; }
		}
	}
}
