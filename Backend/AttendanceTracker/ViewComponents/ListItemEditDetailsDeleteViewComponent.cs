using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.ViewComponents
{
	public class ListItemEditDetailsDeleteViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync(object Model, string ObjectType, string DetailsUrl)
		{
			ViewData["ObjectType"] = ObjectType;
			if (!string.IsNullOrEmpty(DetailsUrl))
			{
				ViewData["DetailsUrl"] = DetailsUrl;
			}
			return View(Model);
		}
	}
}
