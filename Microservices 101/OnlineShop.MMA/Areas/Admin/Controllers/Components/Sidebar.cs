using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Website.Presentation.Areas.Admin.Models.Components;

namespace Website.Presentation.Areas.Admin.Controllers.Components
{
    [ViewComponent]
    public class SideBar : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sidebarModel = new SidebarModel
            {
                IsPostMenuSelected = (string)ViewContext.RouteData.Values["Controller"] == "Post",
                IsCommentMenuSelected = (string)ViewContext.RouteData.Values["Controller"] == "Comment",
                IsCategoryMenuSelected = (string)ViewContext.RouteData.Values["Controller"] == "Category",
                IsTagMenuSelected = (string)ViewContext.RouteData.Values["Controller"] == "Tag",
                IsContactsMenuSelected = (string)ViewContext.RouteData.Values["Controller"] == "Contact",
                IsCVMenuSelected = (string)ViewContext.RouteData.Values["Controller"] == "CV",
                IsServiceMenuSelected = (string)ViewContext.RouteData.Values["Controller"] == "Service"
            };

            return View(sidebarModel);
        }
    }
}
