using inventory_system_api.Application.IService;
using inventory_system_api.IRepository.Invenetory;
using inventory_system_api.Models.Inventory;
using inventory_system_api.Service;
using Microsoft.AspNetCore.Mvc;

namespace inventory_system_api.Controllers
{
    public class ProductController : BaseController
    {
        IProductService _repo;

        public ProductController(IProductService repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Product entity)
        {
            int res = await _repo.AddEdit(entity);
            return OkResponse();
        }

        [HttpPut]
        public async Task<IActionResult> Put(Product entity)
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

        [HttpGet("Details")]
        public async Task<IActionResult> GetDetails()
        {
            var list = await _repo.GetProductsWithUnits();
            return OkResponse(list);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _repo.Get(id);
            return OkResponse(entity);
        }

        [HttpGet("Tree")]
        public async Task<IActionResult> GetTree()
        {
            var list = await _repo.GetProductTrees();
            return OkResponse(list);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string code, string? name)
        {
            var entity = await _repo.Search(code);
            return OkResponse(entity);
        }

        [HttpGet("IsSquare/{num:int}")]
        public async Task<IActionResult> IsSquare(int num)
        {
            if (num <= 0) return ErrorResponse("No");
            int divNo = 2;
            while (num > 1)
            {
                if (num % divNo == 0)
                {
                    num = num / divNo;
                    if (num % divNo == 0)
                    {
                        num = num / divNo;
                    }
                    else { return ErrorResponse("No"); }
                }
                else { divNo++; }
            }
            return OkResponse("Yes");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _repo.Delete(id);
            return OkResponse();
        }
    }
}
