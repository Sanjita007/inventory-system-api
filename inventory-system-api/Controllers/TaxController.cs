using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.Models.System;
using Microsoft.AspNetCore.Mvc;

namespace inventory_system_api.Controllers
{
    public class TaxController: BaseController
    {
        ITaxRepository _repo;
            
        public TaxController(ITaxRepository repo)
        {
            _repo = repo;
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

        [HttpPost]
        public async Task<IActionResult> Post(Tax entity, CancellationToken cancellationToken)
        {
            if (entity.ID > 0) ErrorResponse("Cannot update data with id, please add new record");

            var res = await _repo.AddEdit(entity, cancellationToken);
            return OkResponse(new { ID = res });        
        }

        [HttpPut]
        public async Task<IActionResult> Put(Tax entity, CancellationToken cancellationToken)
        {
            if (entity.ID == 0) ErrorResponse("Cannot add data with id, please update the record");
            var list = await _repo.AddEdit(entity, cancellationToken);
            return OkResponse(list);
        }

        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var res = await _repo.Delete(id, cancellationToken);
            return OkResponse();
        }
    }
}
