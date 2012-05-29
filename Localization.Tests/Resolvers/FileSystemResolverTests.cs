using Mios.Localization.Resolvers;
using Xunit;

namespace Mios.Localization.Tests.Resolvers {
  public class FileSystemResolverTests {
    [Fact]
    public void ShouldCombinePaths() {
      var resolver = new FileSystemResolver();
      Assert.Equal("/bob.xml", resolver.Combine("/root/alice.xml", "../bob.xml"));
      
    }
  }
}