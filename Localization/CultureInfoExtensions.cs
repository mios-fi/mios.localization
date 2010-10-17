using System.Globalization;

namespace Mios.Localization.Tests {
	public static class CultureInfoExtensions {
		public static string GetRegion(this CultureInfo cultureInfo) {
			return cultureInfo.IsNeutralCulture?null:cultureInfo.Name.Substring(3, 2).ToLowerInvariant();
		}
	}
}