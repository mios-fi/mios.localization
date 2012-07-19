using System;
using System.IO;

namespace Mios.Localization.Resolvers {
  public class FileSystemResolver : IResolver {
    public Stream Open(string path) {
      return File.Exists(path) 
        ? File.OpenRead(path)
        : null;
    }
    public string Combine(string basePath, string path) {
      return new Uri(new Uri("file:///"+basePath.TrimStart('/')), path).LocalPath;
    }
  }
}