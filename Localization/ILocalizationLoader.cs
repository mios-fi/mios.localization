namespace Mios.Localization {
	public interface ILocalizationLoader {
		LocalizationDictionary Load(string path);
	}
}