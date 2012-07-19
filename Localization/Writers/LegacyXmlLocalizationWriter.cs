using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Mios.Localization.Writers {
  public class LegacyXmlLocalizationWriter {
    public void Write(ILocalizationDictionary dictionary, string path) {
			using(var writer = new StreamWriter(path)) {
				Write(dictionary, writer);
			}
		}
		public void Write(ILocalizationDictionary dictionary, TextWriter writer) {
			var document = new XDocument();
			var root = new XElement("page");
			document.Add(root);
			root.Add(dictionary.Keys
				.Select(key => {
					var e = new XElement("key");
					e.Add(dictionary.Locales.Select(locale => 
						new XElement(ToCompressedLocale(locale), dictionary[locale, key])));
					e.SetAttributeValue("id", key);
					return e;
				}));
			document.Save(writer);
		}

    private static string ToCompressedLocale(string locale) {
			if(locale.Length!=5) return locale.ToLowerInvariant();
			return
				locale.Substring(0, 2).ToLowerInvariant() +
				locale.Substring(3, 2).ToUpperInvariant();
		}
	}
}