namespace Mios.Localization {
	public static class LocalizerExtensions {
		public static Localizer Scope(this Localizer localizer, string scope) {
			return (key, p) => localizer(scope + key, p);
		}
	}
}
