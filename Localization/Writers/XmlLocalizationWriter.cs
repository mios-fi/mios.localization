using System;
using System.IO;
using System.Xml;

namespace Mios.Localization.Writers {
  public class XmlLocalizationWriter {
    public void Write(ILocalizationDictionary dictionary, string path) {
      using(var writer = new StreamWriter(path)) {
        Write(dictionary, writer);
      }
    }

    public void Write(ILocalizationDictionary dictionary, TextWriter writer) {
      using(var xmlWriter = new XmlTextWriter(writer)) {
        xmlWriter.Formatting = Formatting.Indented;
        WriteDictionary(dictionary, xmlWriter);
      }
    }

    private static void WriteDictionary(ILocalizationDictionary dictionary, XmlWriter writer) {
      writer.WriteStartDocument();
      var typedDictionary = dictionary as LocalizationDictionary;
      if(typedDictionary!=null) {
        WriteIncludes(typedDictionary, writer);
      }
      writer.WriteStartElement("dictionary");
      foreach(var key in dictionary.Keys) {
        WriteKey(key, dictionary, writer);
      }
      writer.WriteEndElement();
      writer.WriteEndDocument();
    }

    private static void WriteIncludes(LocalizationDictionary dictionary, XmlWriter writer) {
      foreach(var include in dictionary.Includes) {
        var data = "include=\""+include.Path+"\"";
        if(!String.IsNullOrEmpty(include.Prefix)) {
          data += " prefix=\""+include.Prefix+"\"";
        }
        writer.WriteProcessingInstruction("mios-localization", data);
      }
    }

    private static void WriteKey(string key, ILocalizationDictionary dictionary, XmlWriter writer) {
      writer.WriteStartElement("key");
      writer.WriteAttributeString("id", key);
      foreach(var locale in dictionary.Locales) {
        var value = dictionary[locale, key];
        if(value==null) continue;
        writer.WriteStartElement("val");
        writer.WriteAttributeString("for", locale);
        writer.WriteCData(value);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
  }
}