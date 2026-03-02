using inventory_system_api.Application.IRepository;
using inventory_system_api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace accswift_api.Controllers
{
    public class DahboardController : BaseController
    {
        IDashboardSummaryRepository _repo;
            
        public DahboardController(IDashboardSummaryRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("SalesPurch")]
        public async Task<IActionResult> GetSalesPurch()
        {
            var list = await _repo.GetSalesPurchDashboardSummary();
            return OkResponse(list);
        }

        [HttpGet("Products")]
        public async Task<IActionResult> GetProducts()
        {
            var list = await _repo.GetProductDashboardSummary();
            return OkResponse(list);
        }


        [HttpGet("Recent")]
        public async Task<IActionResult> GetRecent()
        {
            var list = await _repo.GetRecentTransactionSummary();
            return OkResponse(list);
        }

    }
}
