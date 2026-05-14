using inventory_system_api.Application.Models.Inventory;
using inventory_system_api.Application.Models.System;

namespace inventory_system_api.Application.IService
{
    public interface IPurchaseInvoiceMasterService
    {
        /// <summary>
        /// Get all the products or product list
        /// </summary>
        /// <returns></returns>
        public Task<List<PurchaseInvoiceMaster>> Get(CancellationToken cancellationToken);

        /// <summary>
        /// Get Product by Product Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<PurchaseInvoiceMaster> Get(int id, CancellationToken cancellationToken);


        /// <summary>
        /// Add or update the product
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> AddEdit(PurchaseInvoiceMaster entity, CancellationToken cancellationToken);

        /// <summary>
        /// Delete a product based on the ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<int> Delete(int id, CancellationToken cancellationToken);

        public Task<Navigate> Navigate(int pageNo, int rowPerPage, CancellationToken cancellationToken);

    }


}
