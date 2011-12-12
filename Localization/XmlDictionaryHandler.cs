using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Mios.Localization {
	public class XmlDictionaryHandler : ILocalizationReader, ILocalizationWriter {
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

		public ILocalizationDictionary Read(string path) {
			if(!File.Exists(path)) return null;
			return Read(XDocument.Load(path));
		}
		public ILocalizationDictionary Read(TextReader reader) {
			return Read(XDocument.Load(reader));
		}
		static ILocalizationDictionary Read(XDocument document) {
			var root = document.Root;
			if(root == null) throw new ArgumentException("Document does not contain required root element");
			var dictionary = new LocalizationDictionary();
			foreach(var keyElement in root.Elements("key")) {
				var attribute = keyElement.Attribute("id");
				if(attribute==null) throw new ArgumentException("Element does not contain required 'id' attribute");
				var id = attribute.Value;
				foreach(var entryElement in keyElement.Elements()) {
					var locale = entryElement.Name.LocalName;
					dictionary[FromCompressedLocale(locale), id] = entryElement.Value;
				}
			}
			return dictionary;
		}

		private static string FromCompressedLocale(string locale) {
			if(locale.Length!=4) return locale.ToLowerInvariant();
			return
				locale.Substring(0, 2).ToLowerInvariant() + 
				"-" + 
				locale.Substring(2, 2).ToLowerInvariant();
		}
		private static string ToCompressedLocale(string locale) {
			if(locale.Length!=5) return locale.ToLowerInvariant();
			return
				locale.Substring(0, 2).ToLowerInvariant() +
				locale.Substring(3, 2).ToUpperInvariant();
		}
	}
}