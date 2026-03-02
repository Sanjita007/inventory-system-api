using inventory_system_api.IRepository;
using inventory_system_api.Models.Inventory;
using inventory_system_api.Models.System;
using inventory_system_api.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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

        [HttpPost]
        public async Task<IActionResult> Post(Tax entity)
        {
            if (entity.ID > 0) ErrorResponse("Cannot update data with id, please add new record");

            var res = await _repo.AddEdit(entity);
            return OkResponse(res);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Tax entity)
        {
            if (entity.ID == 0) ErrorResponse("Cannot add data with id, please update the record");
            var list = await _repo.AddEdit(entity);
            return OkResponse(list);
        }

        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _repo.Delete(id);
            return OkResponse();
        }
    }
}
