using inventory_system_api.Application.IService;
using inventory_system_api.Application.Models.Inventory;
using inventory_system_api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace accswift_api.Controllers
{
    public class PurchaseInvoiceController : BaseController
    {
        IPurchaseInvoiceMasterService _repo;

        public PurchaseInvoiceController(IPurchaseInvoiceMasterService repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Post(PurchaseInvoiceMaster entity, CancellationToken cancellationToken)
        {
            if(entity.ID > 0)
            {
                return ErrorResponse("Cannot have value of ID for post request");
            }
            int res = await _repo.AddEdit(entity, cancellationToken, UserId);
            return OkResponse(new { ID = res });
        }

        [HttpPut]
        public async Task<IActionResult> Put(PurchaseInvoiceMaster entity, CancellationToken cancellationToken)
        {
            int res = await _repo.AddEdit(entity, cancellationToken, UserId);
            return OkResponse();
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var list = await _repo.Get(cancellationToken);
            return OkResponse(list);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var entity = await _repo.Get(id, cancellationToken);
            return OkResponse(entity);
        }

        [HttpGet("Navigate")]
        public async Task<IActionResult> Navigate(int pageNo, int rowPerPage, CancellationToken cancellationToken)
        {
            var entity = await _repo.Navigate(pageNo, rowPerPage, cancellationToken);
            return OkResponse(entity);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var res = await _repo.Delete(id, cancellationToken, UserId);
            return OkResponse();
        }
    }
}
