//using inventory_system_api.Application.IRepository.Invenetory;
//using inventory_system_api.Application.Models.Inventory;
//using inventory_system_api.Controllers;
//using Microsoft.AspNetCore.Mvc;

//namespace accswift_api.Controllers
//{
//    public class DepotController: BaseController
//    {
//        IDepotRepository _repo;
            
//        public DepotController(IDepotRepository repo)
//        {
//            _repo = repo;
//        }

//        [HttpPost]
//        public async Task<IActionResult> Get(Depot entity, CancellationToken cancellationToken)
//        {
//            int res = await _repo.AddEdit(entity, cancellationToken);
//            return OkResponse();
//        }

//        [HttpGet]
//        public async Task<IActionResult> Get(CancellationToken cancellationToken)
//        {
//            var list = await _repo.Get(cancellationToken);
//            return OkResponse(list);
//        }

//        [HttpGet("{id:int}")]
//        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
//        {
//            var entity = await _repo.Get(id, cancellationToken);
//            return OkResponse(entity);
//        }

//        [HttpDelete("{id:int}")]
//        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
//        {
//            var res = await _repo.Delete(id, cancellationToken);
//            return OkResponse();
//        }
//    }
//}
