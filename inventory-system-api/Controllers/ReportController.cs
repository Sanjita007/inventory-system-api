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
        public async Task<IActionResult> GetGrossProfitReport(CancellationToken cancellationToken)
        {
            var list = await _repo.GetGrossProfitReport(cancellationToken);
            return OkResponse(list);
        }

        [HttpGet("Inventory")]
        public async Task<IActionResult> GetInventoryReport(CancellationToken cancellationToken)
        {
            var list = await _repo.GetInventoryReport(cancellationToken);
            return OkResponse(list);
        }

    }
}
