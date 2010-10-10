using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Mios.Localization {
	public class XmlResourceSet {
		private readonly IDictionary<string,string> dictionary;
		public XmlResourceSet(string fileName, CultureInfo culture) {
			dictionary = LoadResources(XDocument.Load(fileName), culture);
		}
		public XmlResourceSet(TextReader reader, CultureInfo culture) {
			dictionary = LoadResources(XDocument.Load(reader), culture);
		}

		static IDictionary<string,string> LoadResources(XDocument document, CultureInfo culture) {
			return document.Root.Elements("key")
				.ToDictionary(t => t.Attribute("id").Value, 
				              t => GetSpecificOrNeutralValue(t,culture));
		}

		private static string GetSpecificOrNeutralValue(XElement keyElement, CultureInfo culture) {
			var elements = keyElement.Elements(IdentifierOfCulture(culture));
			if(!culture.IsNeutralCulture) {
				elements = elements.Union(keyElement.Elements(IdentifierOfCulture(culture.Parent)));
			}
			var element = elements.FirstOrDefault();
			return element==null?null:element.Value;
		}

		private static string IdentifierOfCulture(CultureInfo culture) {
			return culture.Name.Replace("-",String.Empty);
		}

		public string Get(string key) {
			string value;
			dictionary.TryGetValue(key, out value);
			return value;
		}
	}
}