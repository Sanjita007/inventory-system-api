using inventory_system_api.Application.IRepository;
using inventory_system_api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace accswift_api.Controllers
{
    public class ReportController : BaseController
    {
        IReportRepository _repo;
            
        public ReportController(IReportRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("GrossProfit")]
        public async Task<IActionResult> GetGrossProfitReport()
        {
            var list = await _repo.GetGrossProfitReport();
            return OkResponse(list);
        }

        [HttpGet("Inventory")]
        public async Task<IActionResult> GetInventoryReport()
        {
            var list = await _repo.GetInventoryReport();
            return OkResponse(list);
        }

    }
}
