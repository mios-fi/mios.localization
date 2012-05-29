using System.IO;
using Mios.Localization.Readers;
using Xunit;

namespace Mios.Localization.Tests {
  public class LegacyXmlDictionaryReaderTests {
    [Fact]
    public void RemapsLocaleStrings() {
      const string source = @"<?xml version=""1.0""?><page><key id=""first""><fi>ensimmäinen</fi><sv>första</sv><svFI>först</svFI></key></page>";
      var dic = new LocalizationDictionary();
      new LegacyXmlLocalizationReader().Read(new StringReader(source),dic);
      Assert.Equal("först", dic["sv-fi", "first"]);
    }
  }
}
