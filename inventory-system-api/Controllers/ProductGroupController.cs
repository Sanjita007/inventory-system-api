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
        public async Task<IActionResult> Post(ProductGroup entity)
        {
            int res = await _repo.AddEdit(entity);
            return OkResponse();
        }

        [HttpPut]
        public async Task<IActionResult> Put(ProductGroup entity)
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

        [HttpGet("Tree")]
        public async Task<IActionResult> GetTree()
        {
            var list = await _repo.GetProductTrees();
            return OkResponse(list);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _repo.Get(id);
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
