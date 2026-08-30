using inventory_system_api.Application.IRepository.Invenetory;
using inventory_system_api.Application.Models.Inventory;
using inventory_system_api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace accswift_api.Controllers
{
    public class ProductGroupController: BaseController
    {
        IProductGroupRepository _repo;
            
        public ProductGroupController(IProductGroupRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductGroup entity, CancellationToken cancellationToken)
        {
            int res = await _repo.AddEdit(entity, UserId, cancellationToken);
            return OkResponse(new { ID = res });
        }

        [HttpPut]
        public async Task<IActionResult> Put(ProductGroup entity, CancellationToken cancellationToken)
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

        [HttpGet("Tree")]
        public async Task<IActionResult> GetTree(CancellationToken cancellationToken)
        {
            var list = await _repo.GetProductTrees(cancellationToken);
            return OkResponse(list);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var entity = await _repo.Get(id, cancellationToken);
            return OkResponse(entity!);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var res = await _repo.Delete(id, UserId, cancellationToken);
            return OkResponse();
        }
    }
}
