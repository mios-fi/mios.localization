using System.Web;

namespace Mios.Localization {
	public struct LocalizedString : IHtmlString {
		public string Localization { get; private set; }
		public string String { get; private set; }

		public LocalizedString(string @string, string localization) : this() {
			Localization = localization;
			String = @string;
		}
		public override string ToString() {
			return Localization??String;
		}

	  public string ToHtmlString() {
      return ToString();
	  }

	  public static implicit operator string(LocalizedString d) {
			return d.ToString();
		}
	}
}