using System;

namespace Mios.Localization {
	public class FilteringLocalizationReader : ILocalizationReader {
		private readonly ILocalizationReader filtered;
		private readonly Predicate<string> filter;

		public FilteringLocalizationReader(ILocalizationReader filtered, Predicate<string> filter) {
			this.filtered = filtered;
			this.filter = filter;
		}
		public ILocalizationDictionary Read(string path) {
			return filter(path)
				? filtered.Read(path)
				: null;
		}
	}
}