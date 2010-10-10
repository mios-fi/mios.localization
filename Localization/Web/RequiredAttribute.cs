namespace Mios.Localization.Web {
	public class RequiredAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute {
		public RequiredAttribute(string resourceExpression) {
			ErrorMessage = ResourceRepository.Current.GetString(resourceExpression);
		}
	}
}