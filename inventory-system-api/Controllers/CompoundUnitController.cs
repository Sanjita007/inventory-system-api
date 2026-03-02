using inventory_system_api.Controllers;
using inventory_system_api.IRepository;
using inventory_system_api.Models.System;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace accswift_api.Controllers
{
    [Route("api/v{version:apiVersion}/Unit/Compound")]
    public class CompoundUnitController: BaseController
    {
        ICompoundUnitRepository _repo;
            
        public CompoundUnitController(ICompoundUnitRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Get(CompoundUnit entity)
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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _repo.Delete(id);
            return OkResponse();
        }

       
    }
}
