using System;
using System.Collections;
using System.Collections.Generic;

namespace Mios.Localization {
  public class LocalizationDictionary : ILocalizationDictionary, IEnumerable {
		private readonly IEqualityComparer<string> comparer;
		private readonly List<string> keys;
		private readonly HashSet<string> localesHash;
		private readonly HashSet<string> keysHash;
		private readonly Dictionary<string, Dictionary<string, string>> dictionaries;

    public IList<Include> Includes { get; protected set; }

    public struct Include {
      public string Path { get; set; }
      public string Prefix { get; set; }
    }

    public LocalizationDictionary() : this(StringComparer.OrdinalIgnoreCase) {
		}
		public LocalizationDictionary(IEqualityComparer<string> comparer) {
      Includes = new List<Include>();
			this.comparer = comparer;
			keys = new List<string>();
			localesHash = new HashSet<string>(comparer);
			keysHash = new HashSet<string>(comparer);
			dictionaries = new Dictionary<string, Dictionary<string, string>>(comparer);
		}
		public void Add(string locale, string key, string value) {
			this[locale, key] = value;
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
		public IEnumerator GetEnumerator() {
			yield break;
		}
	}
}
