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
				              t => GetRegionalOrNeutralValue(t,culture));
		}

		private static string GetRegionalOrNeutralValue(XElement key, CultureInfo culture) {
			return GetValue(key, culture) ?? GetValue(key, culture.Parent);
		}

		private static string GetValue(XElement key, CultureInfo culture) {
			var localeKey = culture.Name.Replace("-",String.Empty);
			return key
				.Elements(localeKey)
				.Select(t => t.Value)
				.Where(t => !String.IsNullOrEmpty(t))
				.FirstOrDefault();
		}

		public string Get(string key) {
			string value;
			dictionary.TryGetValue(key, out value);
			return value;
		}
	}
}