using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Northwind.ViewModels;

namespace Northwind.Controllers
{
	public class ResourcesController : Controller
	{
		private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

		public ResourcesController(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
		{
			_actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
		}

		[HttpGet]
		public IActionResult Scopes()
		{
			var controllerInfos = _actionDescriptorCollectionProvider.ActionDescriptors.Items
				.GroupBy(ad => ad.RouteValues["controller"])
				.Select(g => new ControllerVm
				{
					Name = g.Key,
					ActionMethods =
						g.Select(ad => new ActionInfoVm { ActionMethodName = ad.RouteValues["action"] }).ToList()
				})
				.ToList();

			return View(controllerInfos);
		}
	}
}