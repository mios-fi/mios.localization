using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;

namespace Mios.Localization {
	public class LocalizationLoader : ILocalizationLoader, IEnumerable<ILocalizationLoader> {
		private List<ILocalizationLoader> loaders;
		public LocalizationLoader() {
			loaders = new List<ILocalizationLoader>();
		}
		public void Add(ILocalizationLoader loader) {
			loaders.Add(loader);
		}
		public void Remove(ILocalizationLoader loader) {
			loaders.Remove(loader);
		}
		public LocalizationDictionary Load(string path) {
			return loaders
				.Select(t => t.Load(path))
				.Where(t => t != null)
				.FirstOrDefault();
		}
		public IEnumerator<ILocalizationLoader> GetEnumerator() {
			return loaders.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
	public static class LocalizationLoaderExtensions {
		public static ILocalizationLoader Filter(this ILocalizationLoader loader, Predicate<string> filter) {
			return new FilteringLocalizationLoader(loader, filter);
		}
		public static ILocalizationLoader WithVirtualPathsMapped(this ILocalizationLoader loader) {
			return new VirtualPathLocalizationLoader(loader);
		}
	}
	public class FilteringLocalizationLoader : ILocalizationLoader {
		private readonly ILocalizationLoader filtered;
		private readonly Predicate<string> filter;

		public FilteringLocalizationLoader(ILocalizationLoader filtered, Predicate<string> filter) {
			this.filtered = filtered;
			this.filter = filter;
		}
		public LocalizationDictionary Load(string path) {
			return filter(path)
				? filtered.Load(path)
				: null;
		}
	}
	public class VirtualPathLocalizationLoader : ILocalizationLoader {
		private readonly ILocalizationLoader wrapped;
		public VirtualPathLocalizationLoader(ILocalizationLoader wrapped) {
			this.wrapped = wrapped;
		}
		public LocalizationDictionary Load(string path) {
			return wrapped.Load(HostingEnvironment.MapPath(path));
		}
	}
}