using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.IRepository.Invenetory;
using inventory_system_api.Application.IService;
using inventory_system_api.Application.Models;
using inventory_system_api.Application.Models.Inventory;
using inventory_system_api.Models;
using inventory_system_api.Models.Inventory;

namespace inventory_system_api.Infrastructure.Service
{

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IUnitRepository _unitRepo;
        private readonly IProductGroupRepository _prodGroupRepo;
        public ProductService(IProductRepository productRepo, IUnitRepository unitRepo, IProductGroupRepository prodGroupRepo)
        {
            _productRepo = productRepo;
            _unitRepo = unitRepo;
            _prodGroupRepo = prodGroupRepo;
        }

        public async Task<int> AddEdit(Product entity)
        {
            return await _productRepo.AddEdit(entity);
        }

        public async Task<int> Delete(int id)
        {
            return await _productRepo.Delete(id);
        }

        public async Task<List<Product>> Get()
        {
            return await _productRepo.Get();
        }

        public async Task<Product> Get(int id)
        {
            return await _productRepo.Get(id);
        }

        public async Task<List<ProductDetails>> GetProductsWithUnits()
        {
            List<Product> products = await _productRepo.Get();
            //List<ProductDetails> productDetails = [];

            List<int> units = products.Select(r => r.UnitID).Distinct().ToList();

            List<UnitDetails> details = await _unitRepo.GetMultipleRelatedUnit(String.Join(",", units));


            var unitDetailsLookup = details
                .GroupBy(d => d.DefaultUnitID)
                .ToDictionary(g => g.Key, g => g.ToList());

            List<ProductDetails> productDetails = products
                .Select(product =>
                {

                    // Find units by UnitID. If the key doesn't exist, GetValueOrDefault provides null.
                    List<UnitDetails> relatedUnits;
                    unitDetailsLookup.TryGetValue(product.UnitID, out relatedUnits);

                    // Create the final object, assigning the found units or an empty list.
                    return new ProductDetails(product)
                    {
                        UnitDetails = relatedUnits ?? new List<UnitDetails>()
                    };
                })
                .ToList();
            //foreach (Product product in products) {

            //    List<UnitDetails> u = [.. details.Where(r => r.DefaultUnitID == product.UnitID)];
            //    productDetails.Add(new ProductDetails(product)
            //    {
            //        UnitDetails = u
            //    });
            //}

            return productDetails;
        }

        public async Task<Product> Search(string code)
        {
            return await _productRepo.Search(code);
        }

        public async Task<List<Tree>> GetProductTrees()
        {
            List<Product> products = await Get();
            List<ProductGroup> productGroups = await _prodGroupRepo.Get();

            List<Tree> tree = TreeMethod(productGroups, products, 0, 0);

            return tree;
        }

        public List<Tree> TreeMethod(List<ProductGroup> groups, List<Product> products, int level, int id)
        {
            List<Tree> children = new List<Tree>();

            List<ProductGroup> subGroups = groups.Where(r => r.ParentGroupID == id).ToList();
            List<Product> currentProducts = products.Where(r => r.GroupID == id).ToList();

            int currentLevel = level + 1;

            // ADD PRODUCTS
            children.AddRange(
                currentProducts.Select(p => new Tree
                {
                    Id = p.ID,
                    Name = p.EngName,

                    Level = currentLevel,
                    ParentID = id,
                    IsProduct = true,
                    Children = new List<Tree>()
                })
            );

            // ADD GROUPS (RECURSION)
            foreach (ProductGroup l in subGroups)
            {
                Tree t = new()
                {
                    Id = l.ID,
                    ParentID = l.ParentGroupID,
                    Level = currentLevel,
                    Name = l.EngName,
                    IsProduct = false,
                    Children = TreeMethod(groups, products, currentLevel, l.ID)
                };
                children.Add(t);
            }

            return children;
        }
    }
}
