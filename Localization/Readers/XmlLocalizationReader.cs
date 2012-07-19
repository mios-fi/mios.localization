using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Mios.Localization.Resolvers;

namespace Mios.Localization.Readers {
  public class XmlLocalizationReader {
    private readonly string path;
    public string Prefix { get; set; }
    public bool Recursive { get; set; }
    public IResolver Resolver { get; set; }

    public XmlLocalizationReader(string path) {
      this.path = path;
      Recursive = true;
      Prefix = String.Empty;
      Resolver = new FileSystemResolver();
    }

    public void Read(ILocalizationDictionary dictionary) {
      using(var stream = Resolver.Open(path)) {
        if(stream==null) return;
        using(var reader = XmlReader.Create(stream)) {
          while(reader.Read()) {
            if(reader.NodeType == XmlNodeType.ProcessingInstruction) {
              ReadProcessingInstruction(reader, dictionary);
            } else if(reader.NodeType == XmlNodeType.Element && reader.Name == "dictionary") {
              ReadPageElement(reader, dictionary);
              return;
            }
          }
        }
      }
    }

    private static readonly Regex AttributesPattern = new Regex(@"(?:\s|^)(\w+)=""([^""]*)""");
    private void ReadProcessingInstruction(XmlReader reader, ILocalizationDictionary dictionary) {
      if(reader.Name != "mios-localization") return;
      var attributes = AttributesPattern.Matches(reader.Value).Cast<Match>()
        .ToDictionary(t => t.Groups[1].Value, t => t.Groups[2].Value);
      string includePath;
      if(!attributes.TryGetValue("include", out includePath)) return;

      string prefix;
      attributes.TryGetValue("prefix", out prefix);

      var typedDictionary = dictionary as LocalizationDictionary;
      if(typedDictionary!=null) {
        typedDictionary.Includes.Add(
          new LocalizationDictionary.Include { Path = includePath, Prefix = prefix }
        );
      }
      if(!Recursive) return;
      ReadNestedDictionary(includePath, prefix, dictionary);
    }

    private void ReadNestedDictionary(string path, string prefix, ILocalizationDictionary dictionary) {
      var combinedPath = Resolver.Combine(this.path, path);
      var nestedReader = new XmlLocalizationReader(combinedPath) {
        Resolver = Resolver,
        Prefix = Prefix + prefix
      };
      nestedReader.Read(dictionary);
    }

    private static void ReadPageElement(XmlReader reader, ILocalizationDictionary dictionary) {
      if(!reader.ReadToDescendant("key")) return;
      do {
        ReadKeyElement(reader,dictionary);
      } while(reader.ReadToNextSibling("key"));
    }

    private static void ReadKeyElement(XmlReader reader, ILocalizationDictionary dictionary) {
      var id = reader.GetAttribute("id");
      if(!reader.ReadToDescendant("val")) return;
      do {
        var locale = reader.GetAttribute("for");
        if(locale==null) {
          throw new XmlLocalizationReaderException("Missing required attribute 'for' on element 'val'");
        }
        var val = reader.ReadElementContentAsString();
        dictionary[locale, id] = val;
      } while(reader.ReadToNextSibling("val"));
    }
  }
}