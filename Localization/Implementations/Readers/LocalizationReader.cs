using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mios.Localization.Implementations.Readers {
	public class LocalizationReader : ILocalizationReader, IEnumerable<ILocalizationReader> {
		private readonly List<ILocalizationReader> loaders;
		public LocalizationReader() {
			loaders = new List<ILocalizationReader>();
		}
		public void Add(ILocalizationReader loader) {
			loaders.Add(loader);
		}
		public void Remove(ILocalizationReader loader) {
			loaders.Remove(loader);
		}
		public ILocalizationDictionary Read(string path) {
			return loaders
			  .Select(t => t.Read(path))
        .FirstOrDefault(t => t != null);
		}
		public IEnumerator<ILocalizationReader> GetEnumerator() {
			return loaders.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}