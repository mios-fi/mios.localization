using System.Collections.Generic;

namespace Mios.Localization {
  public interface ILocalizationDictionary {
    string this[string locale, string key] { get; set; }
    IEnumerable<string> Keys { get; }
    IEnumerable<string> Locales { get; }
    IDictionary<string, string> InLocale(string locale);
  }
}