using LibraNovel.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraNovel.WebAPI.Controllers
{
    public class DashboardController : BaseApiController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("/get-data")]
        public async Task<IActionResult> GetData()
        {
            var userID = User.FindFirstValue("UserID");
            if(userID == null)
            {
                return Unauthorized();
            }

            return Ok(await _dashboardService.GetDashboardData(Guid.Parse(userID)));
        }
    }
}
