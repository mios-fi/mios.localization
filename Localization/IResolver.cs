using System.IO;

namespace Mios.Localization {
  public interface IResolver {
    Stream Open(string path);
    string Combine(string basePath, string path);
  }
}