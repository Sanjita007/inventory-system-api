using inventory_system_api.Models;
using inventory_system_api.Models.Inventory;
using System.Data;

namespace inventory_system_api.IRepository.Invenetory
{
    public interface IProductGroupRepository
    {
        /// <summary>
        /// Get all the products or product list
        /// </summary>
        /// <returns></returns>
        public Task<List<ProductGroup>> Get();

        /// <summary>
        /// Get Product by Product Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ProductGroup> Get(int id);

        /// <summary>
        /// Add or update the product
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> AddEdit(ProductGroup entity);

        /// <summary>
        /// Delete a product based on the ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<int> Delete(int id);

        public Task<List<Tree>> GetProductTrees();
    }
}
