using System;
using System.IO;
using System.Xml.Linq;

namespace Mios.Localization.Readers {
  public class LegacyXmlLocalizationReader {
    private readonly string path;

    public LegacyXmlLocalizationReader() {
    }
    public LegacyXmlLocalizationReader(string path) {
      this.path = path;
    }

    public void Read(TextReader reader, ILocalizationDictionary dictionary) {
      Read(XDocument.Load(reader), dictionary);
    }
    public void Read(ILocalizationDictionary dictionary) {
      if(path==null || !File.Exists(path)) return;
      Read(XDocument.Load(path), dictionary);
    }

    private static void Read(XDocument document, ILocalizationDictionary dictionary) {
      var root = document.Root;
      if(root == null) throw new ArgumentException("Document does not contain required root element");
      foreach(var keyElement in root.Elements("key")) {
        var attribute = keyElement.Attribute("id");
        if(attribute==null) throw new ArgumentException("Element does not contain required 'id' attribute");
        var id = attribute.Value;
        foreach(var entryElement in keyElement.Elements()) {
          var locale = entryElement.Name.LocalName;
          dictionary[FromCompressedLocale(locale), id] = entryElement.Value;
        }
      }
    }

    private static string FromCompressedLocale(string locale) {
      if(locale.Length!=4) return locale.ToLowerInvariant();
      return
        locale.Substring(0, 2).ToLowerInvariant() + 
          "-" + 
            locale.Substring(2, 2).ToLowerInvariant();
    }
  }
}