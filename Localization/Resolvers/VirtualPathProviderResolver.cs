using System.IO;
using System.Web.Hosting;

namespace Mios.Localization.Resolvers {
  public class VirtualPathProviderResolver : IResolver {
    private readonly VirtualPathProvider virtualPathProvider;
    public VirtualPathProviderResolver(VirtualPathProvider virtualPathProvider) {
      this.virtualPathProvider = virtualPathProvider;
    }
    public Stream Open(string path) {
      return virtualPathProvider.FileExists(path)
        ? virtualPathProvider.GetFile(path).Open()
        : null;
    }
    public string Combine(string basePath, string path) {
      var ix = basePath.LastIndexOf('/');
      return virtualPathProvider.CombineVirtualPaths(ix>0 ? basePath.Substring(0,ix+1) : basePath, path);
    }
  }
}