using System.Web.Hosting;

namespace Mios.Localization.Implementations.Readers {
	public class VirtualPathLocalizationReader : ILocalizationReader {
		private readonly ILocalizationReader wrapped;
		public VirtualPathLocalizationReader(ILocalizationReader wrapped) {
			this.wrapped = wrapped;
		}
		public ILocalizationDictionary Read(string path) {
			return wrapped.Read(HostingEnvironment.MapPath(path));
		}
	}
}