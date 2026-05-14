using inventory_system_api.Application.Models;
using inventory_system_api.Application.Models.Inventory;

namespace inventory_system_api.Application.IRepository.Invenetory
{
    public interface IProductGroupRepository
    {
        /// <summary>
        /// Get all the products or product list
        /// </summary>
        /// <returns></returns>
        public Task<List<ProductGroup>> Get(CancellationToken cancellationToken);

        /// <summary>
        /// Get Product by Product Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ProductGroup> Get(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Add or update the product
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> AddEdit(ProductGroup entity, CancellationToken cancellationToken, int userId);

        /// <summary>
        /// Delete a product based on the ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<int> Delete(int id, CancellationToken cancellationToken, int userId);

        public Task<List<Tree>> GetProductTrees(CancellationToken cancellationToken);
    }
}
