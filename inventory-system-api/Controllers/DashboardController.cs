using inventory_system_api.Application.IRepository;
using inventory_system_api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace accswift_api.Controllers
{
    public class DashboardController : BaseController
    {
        IDashboardSummaryRepository _repo;
            
        public DashboardController(IDashboardSummaryRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("SalesPurch")]
        public async Task<IActionResult> GetSalesPurch(CancellationToken cancellationToken)
        {
            var list = await _repo.GetSalesPurchDashboardSummary(cancellationToken);
            return OkResponse(list!);
        }

        [HttpGet("Products")]
        public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
        {
            var list = await _repo.GetProductDashboardSummary(cancellationToken);
            return OkResponse(list);
        }


        [HttpGet("Recent")]
        public async Task<IActionResult> GetRecent(CancellationToken cancellationToken)
        {
            var list = await _repo.GetRecentTransactionSummary(cancellationToken);
            return OkResponse(list);
        }

    }
}
