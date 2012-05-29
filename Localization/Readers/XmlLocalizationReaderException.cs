using System;
using System.Runtime.Serialization;

namespace Mios.Localization.Readers {
  [Serializable]
  public class XmlLocalizationReaderException : Exception {
    public XmlLocalizationReaderException() { }
    public XmlLocalizationReaderException(string message) : base(message) { }
    public XmlLocalizationReaderException(string message, Exception inner) : base(message, inner) { }
    protected XmlLocalizationReaderException(
      SerializationInfo info,
      StreamingContext context) : base(info, context) {
    }
  }
}
