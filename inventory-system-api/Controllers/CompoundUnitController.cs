using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.Models.System;
using inventory_system_api.Controllers;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPut]
        public async Task<IActionResult> Put(CompoundUnit entity, CancellationToken cancellationToken)
        {
            var res = await _repo.AddEdit(entity, UserId, cancellationToken);
            return OkResponse();
        }

        [HttpPost]
        public async Task<IActionResult> Post(CompoundUnit entity, CancellationToken cancellationToken)
        {
            int res = await _repo.AddEdit(entity, UserId, cancellationToken);
            return OkResponse(new { ID = res });
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
