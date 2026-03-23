using inventory_system_api.Application.IQueue;
using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.Models.System;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace inventory_system_api.Controllers
{
    public class TaxController: BaseController
    {
        ITaxRepository _repo;
        IMessageService _queue;

        public TaxController(ITaxRepository repo, IMessageService queue)
        {
            _repo = repo;
            _queue = queue;
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
            //if (entity.ID > 0) ErrorResponse("Cannot update data with id, please add new record");

            //var res = await _repo.AddEdit(entity);
            _queue.Enqueue(JsonSerializer.Serialize(entity));
            return OkResponse();
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
