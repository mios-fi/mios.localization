using System.IO;
using System.Xml.Linq;
using Mios.Localization.Writers;
using Xunit;

namespace Mios.Localization.Tests.Writers {
  public class LegacyXmlDictionaryWriterTests {
    [Fact]
    public void WritesKeysToStream() {
      var dic = new LocalizationDictionary();
      dic["fi", "first"] = "ensimmäinen";
      dic["sv", "first"] = "första";
      dic["sv-fi", "first"] = "först";
      var writer = new StringWriter();
      new LegacyXmlLocalizationWriter().Write(dic, writer);
      var doc = XDocument.Parse(writer.ToString());
      var e = doc.Root.Element("key");
      Assert.Equal("first", e.Attribute("id").Value);
      Assert.Equal("ensimmäinen", e.Element("fi").Value);
      Assert.Equal("första", e.Element("sv").Value);
      Assert.Equal("först", e.Element("svFI").Value);
    }
  }
}