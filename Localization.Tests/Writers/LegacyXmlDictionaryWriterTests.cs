using System.IO;
using System.Xml.Linq;
using Mios.Localization.Writers;
using Xunit;

namespace Mios.Localization.Tests.Writers {
  public class LegacyXmlDictionaryWriterTests {
    [Fact]
    public void WritesKeysToStream() {
      var dic = new LocalizationDictionary();
      dic["fi", "first"] = "ensimm�inen";
      dic["sv", "first"] = "f�rsta";
      dic["sv-fi", "first"] = "f�rst";
      var writer = new StringWriter();
      new LegacyXmlLocalizationWriter().Write(dic, writer);
      var doc = XDocument.Parse(writer.ToString());
      var e = doc.Root.Element("key");
      Assert.Equal("first", e.Attribute("id").Value);
      Assert.Equal("ensimm�inen", e.Element("fi").Value);
      Assert.Equal("f�rsta", e.Element("sv").Value);
      Assert.Equal("f�rst", e.Element("svFI").Value);
    }
  }
}