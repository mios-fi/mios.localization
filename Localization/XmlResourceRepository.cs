using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Mios.Localization {
	public class XmlResourceRepository : ResourceRepository {
		readonly string basePath;
		readonly IDictionary<string, XmlResourceSet> cache;

		public XmlResourceRepository(string basePath) {
			cache = new Dictionary<string, XmlResourceSet>();
			if(basePath == null) throw new ArgumentNullException("basePath");
			this.basePath = basePath;
		}

		public override string GetString(string resourcePath, CultureInfo culture) {
			var expression = new ResourceExpression(resourcePath);
			var set = GetSet(expression.Path, culture);
			return set!=null ? set.Get(expression.Key) : null;
		}

		private XmlResourceSet GetSet(string path, CultureInfo culture) {
			var cacheKey = path + ":" + culture;
			if(!cache.ContainsKey(cacheKey)) {
				cache[cacheKey]=LoadSet(path,culture);
			}
			return cache[cacheKey];
		}

		protected virtual XmlResourceSet LoadSet(string path, CultureInfo culture) {
			var resourceSetPath = Path.Combine(basePath, path+".xml");
			return File.Exists(resourceSetPath) ? 
				new XmlResourceSet(resourceSetPath, culture) : 
				null;
		}

		struct ResourceExpression {
			public string Path;
			public string Key;
			public ResourceExpression(string pathAndKey) {
				var index = pathAndKey.IndexOf(';');
				if(index<0) {
					throw new ArgumentException("Invalid resource path '" + pathAndKey + "'");
				}
				Path = pathAndKey.Substring(0, index);
				Key = pathAndKey.Substring(index + 1);
			}
		}
	}
}