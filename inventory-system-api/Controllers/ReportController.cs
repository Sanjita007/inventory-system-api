using inventory_system_api.Controllers;
using inventory_system_api.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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
