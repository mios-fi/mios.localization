using System.IO;
using System.Linq;
using System.Xml.Linq;
using Mios.Localization.Writers;
using Xunit;

namespace Mios.Localization.Tests.Writers {
  public class XmlDictionaryWriterTests {
    [Fact]
    public void WritesKeysToStream() {
      var dic = new LocalizationDictionary();
      dic["fi", "first"] = "ensimmäinen";
      dic["sv", "first"] = "första";
      dic["sv-fi", "first"] = "först";
      var writer = new StringWriter();
      new XmlLocalizationWriter().Write(dic, writer);
      var doc = XDocument.Parse(writer.ToString());
      Assert.Equal("dictionary", doc.Root.Name);
      var e = doc.Root.Element("key");
      Assert.Equal("first", e.Attribute("id").Value);

      var elements = e.Elements("val").ToArray();
      Assert.Equal("fi", elements[0].Attribute("for").Value);
      Assert.Equal("ensimmäinen", elements[0].Value);
      Assert.Equal("sv", elements[1].Attribute("for").Value);
      Assert.Equal("första", elements[1].Value);
      Assert.Equal("sv-fi", elements[2].Attribute("for").Value);
      Assert.Equal("först", elements[2].Value);
    }

    [Fact]
    public void WritesIncludesToStream() {
      var dic = new LocalizationDictionary {
        Includes = { 
          new LocalizationDictionary.Include { Path = "second.xml", Prefix = "pfx" },
          new LocalizationDictionary.Include { Path = "third.xml" }
        }
      };
      dic["fi", "first"] = "ensimmäinen";
      var writer = new StringWriter();
      new XmlLocalizationWriter().Write(dic, writer);
      var doc = XDocument.Parse(writer.ToString());
      var pi = doc.Nodes().OfType<XProcessingInstruction>().First();
      Assert.Equal("mios-localization", pi.Target);
      Assert.Equal("include=\"second.xml\" prefix=\"pfx\"", pi.Data);
      pi = doc.Nodes().OfType<XProcessingInstruction>().Skip(1).First();
      Assert.Equal("mios-localization", pi.Target);
      Assert.Equal("include=\"third.xml\"", pi.Data);
    }
  }
}