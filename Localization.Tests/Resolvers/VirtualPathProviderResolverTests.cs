using System.Web.Hosting;
using Mios.Localization.Resolvers;
using Moq;
using Xunit;

namespace Mios.Localization.Tests.Resolvers {
  public class VirtualPathProviderResolverTests {
    [Fact]
    public void ShouldCombinePaths() {
      var virtualPathProvider = Mock.Of<VirtualPathProvider>();
      var resolver = new VirtualPathProviderResolver(virtualPathProvider);
      resolver.Combine("/root/alice.xml", "../bob.xml");
      Mock.Get(virtualPathProvider).Verify(t => t.CombineVirtualPaths("/root/", "../bob.xml"));
    }
  }
}