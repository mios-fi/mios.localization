namespace Mios.Localization.Web {
	public class DisplayNameAttribute : System.ComponentModel.DisplayNameAttribute {
		private readonly string resourceExpression;
		public DisplayNameAttribute(string resourceExpression) {
			this.resourceExpression = resourceExpression;
		}
		public override string DisplayName {
			get { return ResourceRepository.Current.GetString(resourceExpression); }
		}
	}
}