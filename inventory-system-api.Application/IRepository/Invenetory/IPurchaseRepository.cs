using inventory_system_api.Application.Models.Inventory;
using inventory_system_api.Application.Models.System;

namespace inventory_system_api.Application.IRepository.Invenetory
{
    public interface IPurchaseInvoiceRepository
    {
        /// <summary>
        /// Get all the products or product list
        /// </summary>
        /// <returns></returns>
        public Task<List<PurchaseInvoiceMaster>> Get();

        /// <summary>
        /// Get Product by Product Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<PurchaseInvoiceMaster> Get(int id);
       

        /// <summary>
        /// Add or update the product
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> AddEdit(PurchaseInvoiceMaster entity);

        /// <summary>
        /// Delete a product based on the ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<int> Delete(int id);

        public Task<Navigate> Navigate(int pageNo, int rowPerPage);

    }
}
