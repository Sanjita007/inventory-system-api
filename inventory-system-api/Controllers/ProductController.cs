using inventory_system_api.Application.IService;
using inventory_system_api.Models.Inventory;
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
        public async Task<IActionResult> Post(Product entity, CancellationToken cancellationToken)
        {
            int res = await _repo.AddEdit(entity, UserId, cancellationToken);
            return OkResponse(new { ID = res });
        }

        [HttpPut]
        public async Task<IActionResult> Put(Product entity, CancellationToken cancellationToken)
        {
            int res = await _repo.AddEdit(entity, UserId, cancellationToken);
            return OkResponse();
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var list = await _repo.Get(cancellationToken);
            return OkResponse(list);
        }

        [HttpGet("Details")]
        public async Task<IActionResult> GetDetails(CancellationToken cancellationToken)
        {
            var list = await _repo.GetProductsWithUnits(cancellationToken);
            return OkResponse(list);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var entity = await _repo.Get(id, cancellationToken);
            return OkResponse(entity!);
        }

        [HttpGet("Tree")]
        public async Task<IActionResult> GetTree(CancellationToken cancellationToken)
        {
            var list = await _repo.GetProductTrees(cancellationToken);
            return OkResponse(list);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string code, string? name, CancellationToken cancellationToken)
        {
            var entity = await _repo.Search(code, cancellationToken);
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
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var res = await _repo.Delete(id, UserId, cancellationToken);
            return OkResponse();
        }
    }
}
