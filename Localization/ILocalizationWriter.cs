namespace Mios.Localization {
	public interface ILocalizationWriter {
		void Write(ILocalizationDictionary dictionary, string path);
	}
}