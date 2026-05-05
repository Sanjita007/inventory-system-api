using inventory_system_api.Models.Inventory;
using System.Data;

namespace inventory_system_api.Application.IRepository.Invenetory
{
    public interface IProductRepository
    {
        /// <summary>
        /// Get all the products or product list
        /// </summary>
        /// <returns></returns>
        public Task<List<Product>> Get(CancellationToken cancellationToken);

        /// <summary>
        /// Get Product by Product Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Product> Get(int id, CancellationToken cancellationToken);
        public Task<Product> Search(string code, CancellationToken cancellationToken);

        /// <summary>
        /// Add or update the product
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> AddEdit(Product entity, CancellationToken cancellationToken);

        /// <summary>
        /// Delete a product based on the ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<int> Delete (int  id, CancellationToken cancellationToken);
    }
}
