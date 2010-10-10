using System.Globalization;
using System.Threading;

namespace Mios.Localization {
	public class ResourceRepository {
		public static ResourceRepository Current { get; set; }
		static ResourceRepository() {
			Current = new ResourceRepository();
		}
		public string GetString(string resourcePath) {
			return GetString(resourcePath, Thread.CurrentThread.CurrentUICulture);
		}
		public virtual string GetString(string resourcePath, CultureInfo culture) {
			return string.Format("{0}[{1}]", resourcePath, culture);
		}
	}
}