namespace Mios.Localization {
	public interface ILocalizationReader {
		ILocalizationDictionary Read(string path);
	}
}