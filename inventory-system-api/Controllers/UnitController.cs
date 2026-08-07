using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.Models.Inventory;
using inventory_system_api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace accswift_api.Controllers
{
    public class UnitController: BaseController
    {
        IUnitRepository _repo;
            
        public UnitController(IUnitRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Unit entity, CancellationToken cancellationToken)
        {
            int res = await _repo.AddEdit(entity, cancellationToken, UserId);
            return OkResponse(new { ID = res });
        }

        [HttpPut]
        public async Task<IActionResult> Put(Unit entity, CancellationToken cancellationToken)
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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var res = await _repo.Delete(id, cancellationToken, UserId);
            return OkResponse();
        }

        [HttpGet("Convert")]
        public async Task<IActionResult> Convert(int defaultUnitID, int currentUnitID, decimal valueToConvert, CancellationToken cancellationToken)
        {
            var res = await _repo.ConvertUnit(defaultUnitID, currentUnitID, valueToConvert, cancellationToken);
            return OkResponse(res);
        }

        [HttpGet("Related")]
        public async Task<IActionResult> Related(int baseUnitID, CancellationToken cancellationToken)
        {
            var res = await _repo.GetRelatedUnit(baseUnitID, cancellationToken);
            return OkResponse(res);
        }

    }
}
