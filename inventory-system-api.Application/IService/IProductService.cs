using inventory_system_api.Application.Models;
using inventory_system_api.Models.Inventory;

namespace inventory_system_api.Application.IService
{
    public interface IProductService
    {

        public Task<List<Product>> Get(CancellationToken cancellationToken);

        public Task<Product> Get(int id, CancellationToken cancellationToken);
        public Task<Product> Search(string code, CancellationToken cancellationToken);      

        public Task<int> AddEdit(Product entity, CancellationToken cancellationToken, int userId);
        public Task<List<Tree>> GetProductTrees(CancellationToken cancellationToken);

        public Task<int> Delete(int id, CancellationToken cancellationToken, int userId);

        public Task<List<ProductDetails>> GetProductsWithUnits(CancellationToken cancellationToken);

    }
}
