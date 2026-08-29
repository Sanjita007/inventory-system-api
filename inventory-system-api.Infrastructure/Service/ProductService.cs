using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.IRepository.Invenetory;
using inventory_system_api.Application.IService;
using inventory_system_api.Application.Models;
using inventory_system_api.Application.Models.Inventory;
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

        public async Task<int> AddEdit(Product entity, CancellationToken cancellationToken, int userId)
        {
            return await _productRepo.AddEdit(entity, cancellationToken, userId);
        }

        public async Task<int> Delete(int id, CancellationToken cancellationToken, int userId)
        {
            return await _productRepo.Delete(id, cancellationToken, userId);
        }

        public async Task<List<Product>> Get(CancellationToken cancellationToken)
        {
            return await _productRepo.Get(cancellationToken);
        }

        public async Task<Product?> Get(int id, CancellationToken cancellationToken)
        {
            return await _productRepo.Get(id, cancellationToken);
        }

        public async Task<List<ProductDetails>> GetProductsWithUnits(CancellationToken cancellationToken)
        {
            List<Product> products = await _productRepo.Get(cancellationToken);
            //List<ProductDetails> productDetails = [];

            List<int> units = products.Select(r => r.UnitID).Distinct().ToList();

            List<UnitDetails> details = await _unitRepo.GetMultipleRelatedUnit(String.Join(",", units), cancellationToken);


            var unitDetailsLookup = details
                .GroupBy(d => d.DefaultUnitID)
                .ToDictionary(g => g.Key, g => g.ToList());

            List<ProductDetails> productDetails = products
                .Select(product =>
                {

                    // Find units by UnitID. If the key doesn't exist, GetValueOrDefault provides null.
                    List<UnitDetails> relatedUnits;
                    unitDetailsLookup.TryGetValue(product.UnitID, out relatedUnits!);

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

        public async Task<List<Product>> Search(string code, CancellationToken cancellationToken)
        {
            return await _productRepo.Search(code, cancellationToken);
        }

        public async Task<List<Tree>> GetProductTrees(CancellationToken cancellationToken)
        {
            List<Product> products = await Get(cancellationToken);
            List<ProductGroup> productGroups = await _prodGroupRepo.Get(cancellationToken);

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
