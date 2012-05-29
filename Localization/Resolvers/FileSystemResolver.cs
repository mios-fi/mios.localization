using System;
using System.IO;
using Mios.Localization.Readers;

namespace Mios.Localization.Resolvers {
  public class FileSystemResolver : IResolver {
    public Stream Open(string path) {
      return File.OpenRead(path);
    }
    public string Combine(string basePath, string path) {
      return new Uri(new Uri("file:///"+basePath.TrimStart('/')), path).LocalPath;
    }
  }
}