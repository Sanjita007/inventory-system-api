using inventory_system_api.IRepository;
using inventory_system_api.IRepository.Invenetory;
using inventory_system_api.Models.Inventory;
using inventory_system_api.Models.System;

namespace inventory_system_api.Application.IService
{
    public interface ISalesInvoiceService
    {
        /// <summary>
        /// Get all the products or product list
        /// </summary>
        /// <returns></returns>
        public Task<List<SalesInvoiceMaster>> Get();

        /// <summary>
        /// Get Product by Product Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<SalesInvoiceMaster> Get(int id);


        /// <summary>
        /// Add or update the product
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> AddEdit(SalesInvoiceMaster entity);

        /// <summary>
        /// Delete a product based on the ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<int> Delete(int id);

        public Task<Navigate> Navigate(int pageNo, int rowPerPage);

    }

   
}
