using inventory_system_api.Application.Models.System;

namespace inventory_system_api.Application.IRepository
{
    public interface ITaxRepository
    {
        /// <summary>
        /// Get all the products or product list
        /// </summary>
        /// <returns></returns>
        public Task<List<Tax>> Get(CancellationToken cancellationToken);

        /// <summary>
        /// Get Product by Product Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Tax> Get(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Add or update the product
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> AddEdit(Tax entity, CancellationToken cancellationToken);

        /// <summary>
        /// Delete a product based on the ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<int> Delete(int id, CancellationToken cancellationToken);
    }
}
