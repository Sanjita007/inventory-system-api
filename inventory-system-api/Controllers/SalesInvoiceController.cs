using inventory_system_api.Application.IService;
using inventory_system_api.Controllers;
using inventory_system_api.Models.Inventory;
using inventory_system_api.Service;
using Microsoft.AspNetCore.Mvc;

namespace accswift_api.Controllers
{
    public class SalesInvoiceController : BaseController
    {
        ISalesInvoiceService _repo;

        public SalesInvoiceController(ISalesInvoiceService repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Post(SalesInvoiceMaster entity)
        {
            if(entity.ID > 0)
            {
                return ErrorResponse("Cannot have value of ID for post request");
            }
            int res = await _repo.AddEdit(entity);
            return OkResponse();
        }

        [HttpPut]
        public async Task<IActionResult> Put(SalesInvoiceMaster entity)
        {
            int res = await _repo.AddEdit(entity);
            return OkResponse();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var list = await _repo.Get();
            return OkResponse(list);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _repo.Get(id);
            return OkResponse(entity);
        }

        [HttpGet("Navigate")]
        public async Task<IActionResult> Navigate(int pageNo, int rowPerPage)
        {
            var entity = await _repo.Navigate(pageNo, rowPerPage);
            return OkResponse(entity);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _repo.Delete(id);
            return OkResponse();
        }
    }
}
