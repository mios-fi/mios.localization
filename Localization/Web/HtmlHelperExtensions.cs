using System;
using System.Web.Mvc;

namespace Mios.Localization.Web {
	public static class HtmlHelperExtensions {
		public static string Localize(this HtmlHelper htmlhelper, string resourceExpression, params object[] args) {
			return String.Format(ResourceRepository.Current.GetString(resourceExpression)??String.Empty,args);
		}
	}
}