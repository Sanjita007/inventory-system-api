using inventory_system_api.IRepository;
using inventory_system_api.IRepository.Invenetory;
using inventory_system_api.Models;
using inventory_system_api.Models.Inventory;

namespace inventory_system_api.Application.IService
{
    public interface IProductService
    {

        public Task<List<Product>> Get();


        public Task<Product> Get(int id);
        public Task<Product> Search(string code);


        public Task<int> AddEdit(Product entity);
        public Task<List<Tree>> GetProductTrees();

        public Task<int> Delete(int id);

        public Task<List<ProductDetails>> GetProductsWithUnits();

    }
}
