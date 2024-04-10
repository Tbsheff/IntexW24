using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;

namespace Intex.Infrastructure
{
	public static class HtmlHelper
	{
		public static string IsActive(this IHtmlHelper html, string action, string controller)
		{
			var routeData = html.ViewContext.RouteData;

			var routeAction = (string)routeData.Values["action"];
			var routeController = (string)routeData.Values["controller"];

			// Both must match.
			var returnActive = controller == routeController && action == routeAction;

			return returnActive ? "active-menu" : "";
		}
	}
}
