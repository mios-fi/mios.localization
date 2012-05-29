using System.IO;
using System.Web.Hosting;
using Mios.Localization.Readers;

namespace Mios.Localization.Resolvers {
  public class VirtualPathProviderResolver : IResolver {
    private readonly VirtualPathProvider virtualPathProvider;
    public VirtualPathProviderResolver(VirtualPathProvider virtualPathProvider) {
      this.virtualPathProvider = virtualPathProvider;
    }
    public Stream Open(string path) {
      return virtualPathProvider.GetFile(path).Open();
    }
    public string Combine(string basePath, string path) {
      var ix = basePath.LastIndexOf('/');
      return virtualPathProvider.CombineVirtualPaths(ix>0 ? basePath.Substring(0,ix+1) : basePath, path);
    }
  }
}