namespace Mios.Localization {
	public delegate LocalizedString Localizer(string key, params object[] args);

	public static class LocalizationExtensions {
		public static Localizer In(this Localizer localizer, string ns) {
			return (str, args) => localizer(ns+"."+str, args);
		}
	}
}
