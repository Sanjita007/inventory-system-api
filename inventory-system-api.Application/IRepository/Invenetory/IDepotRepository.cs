using inventory_system_api.Application.Models.Inventory;
using inventory_system_api.Models.Inventory;
using System.Data;

namespace inventory_system_api.Application.IRepository.Invenetory
{
    public interface IDepotRepository 
    {
        /// <summary>
        /// Get all the products or product list
        /// </summary>
        /// <returns></returns>
        public Task<List<Depot>> Get(CancellationToken cancellationToken);

        /// <summary>
        /// Get Product by Product Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Depot> Get(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Add or update the product
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> AddEdit(Depot entity, CancellationToken cancellationToken);

        /// <summary>
        /// Delete a product based on the ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> Delete(int id, CancellationToken cancellationToken);
    }
}
