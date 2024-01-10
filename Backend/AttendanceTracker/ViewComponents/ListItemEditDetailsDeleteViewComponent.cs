using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.ViewComponents
{
	public class ListItemEditDetailsDeleteViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync(object Model, string ObjectType, string DetailsUrl, string DetailsText)
		{
			ViewData["ObjectType"] = ObjectType;
			if (!string.IsNullOrEmpty(DetailsUrl))
			{
				ViewData["DetailsUrl"] = DetailsUrl;
				ViewData["DetailsText"] = DetailsText;
			}
			return View(Model);
		}
	}
}
