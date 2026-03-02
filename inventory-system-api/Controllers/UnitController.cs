using inventory_system_api.Controllers;
using inventory_system_api.IRepository;
using inventory_system_api.Models.Inventory;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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
        public async Task<IActionResult> Get(Unit entity)
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

        [HttpGet("Convert")]
        public async Task<IActionResult> Convert(int defaultUnitID, int currentUnitID, decimal valueToConvert)
        {
            var res = await _repo.ConvertUnit(defaultUnitID, currentUnitID, valueToConvert);
            return OkResponse(res);
        }

        [HttpGet("Related")]
        public async Task<IActionResult> Related(int baseUnitID)
        {
            var res = await _repo.GetRelatedUnit(baseUnitID);
            return OkResponse(res);
        }

    }
}
