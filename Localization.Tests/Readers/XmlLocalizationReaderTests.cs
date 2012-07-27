using System.IO;
using System.Text;
using Mios.Localization.Readers;
using Moq;
using Xunit;

namespace Mios.Localization.Tests.Readers {
  public class XmlLocalizationReaderTests {

    public Stream ToStream(string text) {
      return new MemoryStream(Encoding.UTF8.GetBytes(text));
    }

    [Fact]
    public void ReadsIncludedDictionaries() {
      var resolver = Mock.Of<IResolver>(t =>
        t.Open("file.xml") == 
          ToStream(
            @"<?xml version=""1.0""?>
              <?mios-localization include=""second.xml"" prefix=""pfx""?>
              <dictionary>
                <key id=""first"">
                  <val for=""fi"">abc</val>
                  <val for=""sv-fi"">def</val>
                </key>
              </dictionary>") &&
        t.Open("second.xml") == 
          ToStream(
            @"<?xml version=""1.0""?>
              <dictionary>
                <key id=""second"">
                  <val for=""fi"">ghi</val>
                  <val for=""sv-fi"">jkl</val>
                </key>
              </dictionary>")
      );
      Mock.Get(resolver).Setup(t => t.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns<string, string>((a, b) => b);
      var dictionary = new LocalizationDictionary();
      new XmlLocalizationReader("file.xml") {
        Resolver = resolver
      }.Read(dictionary);
      Assert.Equal("abc", dictionary["fi", "first"]);
      Assert.Equal("ghi", dictionary["fi", "second"]);
    }

    [Fact]
    public void RecordsIncludes() {
      var resolver = Mock.Of<IResolver>(t =>
        t.Open("file.xml") == 
          ToStream(
            @"<?xml version=""1.0"" encoding=""utf-8""?>
              <?mios-localization include=""second.xml""?>
              <dictionary>
                <key id=""test"">
                  <val for=""fi""><![CDATA[Testaus]]></val>
                  <val for=""sv""><![CDATA[Testning]]></val>
                </key>
              </dictionary>")
      );
      var dictionary = new LocalizationDictionary();
      new XmlLocalizationReader("file.xml") {
        Resolver = resolver, Recursive = false
      }.Read(dictionary);
      Assert.Contains(new LocalizationDictionary.Include { Path = "second.xml" }, dictionary.Includes);
    }

    [Fact]
    public void ReadsIncludedDictionariesOnlyIfSpecified() {
      var resolver = Mock.Of<IResolver>(t =>
        t.Open("file.xml") == 
          ToStream(
            @"<?xml version=""1.0""?>
              <?mios-localization include=""second.xml""?>
              <dictionary>
                <key id=""first"">
                  <val for=""fi"">abc</val>
                  <val for=""sv-fi"">def</val>
                </key>
              </dictionary>") &&
        t.Open("second.xml") == 
          ToStream(@"<?xml version=""1.0""?><dictionary/>")
      );
      Mock.Get(resolver).Setup(t => t.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns<string, string>((a, b) => b);
      var dictionary = new LocalizationDictionary();
      new XmlLocalizationReader("file.xml") {
        Resolver = resolver, Recursive = false
      }.Read(dictionary);
      Mock.Get(resolver).Verify(t => t.Open("second.xml"), Times.Never());
    }
    [Fact]
    public void ReadsMultipleIncludedDictionaries() {
      var resolver = Mock.Of<IResolver>(t =>
        t.Open("file.xml") == 
          ToStream(
            @"<?xml version=""1.0""?>
              <?mios-localization include=""second.xml""?>
              <?mios-localization include=""third.xml""?>
              <dictionary xml:ns="""">
                <key id=""first"">
                  <val for=""fi"">abc</val>
                </key>
              </dictionary>") &&
        t.Open("second.xml") == 
          ToStream(
            @"<?xml version=""1.0""?>
              <dictionary>
                <key id=""second"">
                  <val for=""fi"">def</val>
                </key>
              </dictionary>") &&
        t.Open("third.xml") == 
          ToStream(
            @"<?xml version=""1.0""?>
              <dictionary>
                <key id=""third"">
                  <val for=""fi"">ghi</val>
                </key>
              </dictionary>")
      );
      Mock.Get(resolver).Setup(t => t.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns<string, string>((a, b) => b);
      var dictionary = new LocalizationDictionary();
      new XmlLocalizationReader("file.xml") {
        Resolver = resolver
      }.Read(dictionary);
      Assert.Equal("abc", dictionary["fi", "first"]);
      Assert.Equal("def", dictionary["fi", "second"]);
      Assert.Equal("ghi", dictionary["fi", "third"]);
    }

    [Fact]
    public void ReadsRelativeDictionaries() {
      var resolver = Mock.Of<IResolver>(t =>
        t.Combine("a/file.xml","../b/second.xml") == "b/second.xml" &&
        t.Open("a/file.xml") == 
          ToStream(
            @"<?xml version=""1.0""?>
              <?mios-localization include=""../b/second.xml""?>
              <dictionary>
                <key id=""first"">
                  <val for=""fi"">abc</val>
                  <val for=""sv-fi"">def</val>
                </key>
              </dictionary>") &&
        t.Open("b/second.xml") == 
          ToStream(
            @"<?xml version=""1.0""?>
              <dictionary>
                <key id=""second"">
                  <val for=""fi"">ghi</val>
                  <val for=""sv-fi"">jkl</val>
                </key>
              </dictionary>")

      );
      var dictionary = new LocalizationDictionary();
      new XmlLocalizationReader("a/file.xml") {
        Resolver = resolver
      }.Read(dictionary);
      Assert.Equal("abc", dictionary["fi", "first"]);
      Assert.Equal("ghi", dictionary["fi", "second"]);
    }

    [Fact]
    public void ReadsComplexIncludedDictionaries() {
      var resolver = Mock.Of<IResolver>(t =>
        t.Open("file.xml") == 
          ToStream(
            @"<?xml version=""1.0""?>
              <?mios-localization include=""second.xml""?>
              <dictionary>
                <key id=""first"">
                  <val for=""fi"">abc</val>
                  <val for=""sv-fi"">def</val>
                </key>
              </dictionary>") &&
        t.Open("second.xml") == 
          ToStream(
            @"<?xml version=""1.0""?>
              <?mios-localization include=""third.xml""?>
              <dictionary>
                <key id=""second"">
                  <val for=""fi"">ghi</val>
                  <val for=""sv-fi"">jkl</val>
                </key>
              </dictionary>") &&
        t.Open("third.xml") == 
          ToStream(
            @"<?xml version=""1.0""?>
              <dictionary>
                <key id=""third"">
                  <val for=""fi"">mno</val>
                  <val for=""sv-fi"">pqr</val>
                </key>
              </dictionary>")
      );
      Mock.Get(resolver).Setup(t => t.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns<string, string>((a, b) => b);
      var dictionary = new LocalizationDictionary();
      new XmlLocalizationReader("file.xml") {
        Resolver = resolver
      }.Read(dictionary);
      Assert.Equal("abc", dictionary["fi", "first"]);
      Assert.Equal("ghi", dictionary["fi", "second"]);
      Assert.Equal("mno", dictionary["fi", "third"]);
    }

    [Fact]
    public void ReadsSimpleKeyAndValues() {
      var resolver = Mock.Of<IResolver>(t =>
        t.Open(It.IsAny<string>()) == 
          ToStream(
            @"<?xml version=""1.0""?>
              <dictionary>
                <key id=""xyz"">
                  <val for=""fi"">abc</val>
                  <val for=""sv-fi"">def</val>
                </key>
              </dictionary>")
      );
      var dictionary = new LocalizationDictionary();
      new XmlLocalizationReader("file.xml") {
        Resolver = resolver
      }.Read(dictionary);
      Assert.Equal("abc", dictionary["fi", "xyz"]);
      Assert.Equal("def", dictionary["sv-fi", "xyz"]);
    }

    [Fact]
    public void ReadsMultipleKeyAndValues() {
      var resolver = Mock.Of<IResolver>(t =>
        t.Open(It.IsAny<string>()) == 
          ToStream(
            @"<?xml version=""1.0""?>
              <dictionary>
                <key id=""xyz"">
                  <val for=""fi"">abc</val>
                  <val for=""sv-fi"">def</val>
                </key>
                <key id=""pqr"">
                  <val for=""fi"">123</val>
                  <val for=""en"">456</val>
                </key>
              </dictionary>")
      );
      var dictionary = new LocalizationDictionary();
      new XmlLocalizationReader("file.xml") {
        Resolver = resolver
      }.Read(dictionary);
      Assert.Equal("abc", dictionary["fi", "xyz"]);
      Assert.Equal("def", dictionary["sv-fi", "xyz"]);
      Assert.Equal("123", dictionary["fi", "pqr"]);
      Assert.Equal("456", dictionary["en", "pqr"]);
    }

    [Fact]
    public void ReadsCompactFiles() {
      var resolver = Mock.Of<IResolver>(t =>
        t.Open(It.IsAny<string>()) == 
          ToStream(
@"<?xml version=""1.0""?>
<dictionary>
<key id=""xyz""><val for=""fi""></val></key>
<key id=""pqr""><val for=""fi""></val></key>
</dictionary>")
      );
      var dictionary = new LocalizationDictionary();
      new XmlLocalizationReader("file.xml") {
        Resolver = resolver
      }.Read(dictionary);
      Assert.Contains("xyz", dictionary.Keys);
      Assert.Contains("pqr", dictionary.Keys);
      Assert.Equal("", dictionary["fi", "xyz"]);
      Assert.Equal("", dictionary["fi", "pqr"]);
    }

    [Fact]
    public void SkipsUnknownElements() {
      var resolver = Mock.Of<IResolver>(t =>
        t.Open(It.IsAny<string>()) == 
          ToStream(
            @"<?xml version=""1.0""?>
              <dictionary>
                <garbage/>
                <key id=""xyz"">
                  <random>This is text</random>
                  <val for=""fi"">abc</val>
                  <val for=""sv-fi"">def</val>
                </key>
              </dictionary>")
            );
      var dictionary = new LocalizationDictionary();
      new XmlLocalizationReader("file.xml") {
        Resolver = resolver
      }.Read(dictionary);
      Assert.Equal("abc", dictionary["fi", "xyz"]);
      Assert.Equal("def", dictionary["sv-fi", "xyz"]);
    }
  }
}