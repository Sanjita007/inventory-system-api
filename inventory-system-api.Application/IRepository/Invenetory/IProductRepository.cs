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
        public Task<List<Product>> Get();

        /// <summary>
        /// Get Product by Product Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Product> Get(int id);
        public Task<Product> Search(string code);

        /// <summary>
        /// Add or update the product
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> AddEdit(Product entity);

        /// <summary>
        /// Delete a product based on the ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<int> Delete (int  id);
    }
}
