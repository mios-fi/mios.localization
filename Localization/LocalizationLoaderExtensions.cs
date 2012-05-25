using System;
using Mios.Localization.Implementations.Readers;

namespace Mios.Localization {
	public static class LocalizationLoaderExtensions {
		public static ILocalizationReader Filter(this ILocalizationReader loader, Predicate<string> filter) {
			return new FilteringLocalizationReader(loader, filter);
		}
		public static ILocalizationReader WithVirtualPathsMapped(this ILocalizationReader loader) {
			return new VirtualPathLocalizationReader(loader);
		}
	}
}