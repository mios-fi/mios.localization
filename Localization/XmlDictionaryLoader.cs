using System;
using System.IO;
using System.Xml.Linq;

namespace Mios.Localization {
	public class XmlDictionaryLoader : ILocalizationLoader {
		public LocalizationDictionary Load(string path) {
			if(!File.Exists(path)) return null;
			return Load(XDocument.Load(path));
		}
		public LocalizationDictionary Load(TextReader reader) {
			return Load(XDocument.Load(reader));
		}
		static LocalizationDictionary Load(XDocument document) {
			var root = document.Root;
			if(root == null) throw new ArgumentException("Document does not contain required root element");
			var dictionary = new LocalizationDictionary();
			foreach(var keyElement in root.Elements("key")) {
				var key = GetKey(keyElement);
				foreach(var entryElement in keyElement.Elements()) {
					dictionary[entryElement.Name.ToString(), key] = entryElement.Value;
				}
			}
			return dictionary;
		}
		private static string GetKey(XElement t) {
			var attribute = t.Attribute("id");
			if(attribute==null) throw new ArgumentException("Element does not contain required 'id' attribute");
			return attribute.Value;
		}
	}
}