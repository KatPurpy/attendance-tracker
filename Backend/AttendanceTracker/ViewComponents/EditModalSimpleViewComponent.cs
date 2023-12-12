using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.ViewComponents
{
	public class EditModalSimpleViewComponent : ViewComponent
	{
		public IViewComponentResult ViewResult { get; set; }

		public async Task<IViewComponentResult> InvokeAsync(object Model, string ObjectType)
		{
			ViewData["ObjectType"] = ObjectType;
			return View(Model);
		}
	}
}
