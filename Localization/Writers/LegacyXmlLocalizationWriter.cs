using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Mios.Localization.Writers {
  public class LegacyXmlLocalizationWriter {
		public void Write(ILocalizationDictionary dictionary, string path) {
			var document = File.Exists(path)?XDocument.Load(path):null;
			using(var writer = new StreamWriter(path)) {
				Write(dictionary, document, writer);
			}
		}
		public void Write(ILocalizationDictionary dictionary, TextWriter writer) {
			Write(dictionary, null, writer);
		}

		static void Write(ILocalizationDictionary dictionary, XDocument document, TextWriter writer) {
			document = document ?? new XDocument();
			var root = document.Root;
			if(root==null) {
				document.Add(root = new XElement("page"));
			}
			root.Elements("key").Remove();
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