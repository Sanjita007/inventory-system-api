using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.IRepository.Invenetory;
using inventory_system_api.Application.IService;
using inventory_system_api.Application.Models.Inventory;
using inventory_system_api.Application.Models.System;

namespace inventory_system_api.Infrastructure.Service
{
    public class PurchaseInvoiceMasterService : IPurchaseInvoiceMasterService
    {
        private readonly IUnitRepository _unitRepo;
        private readonly IPurchaseInvoiceRepository _salesInvoiceRepo;

        public PurchaseInvoiceMasterService(IPurchaseInvoiceRepository salesInvoiceRepo, IUnitRepository unitRepo)
        {
            _unitRepo = unitRepo;
            _salesInvoiceRepo = salesInvoiceRepo;
        }

        public Task<int> AddEdit(PurchaseInvoiceMaster entity, CancellationToken cancellationToken, int userId)
        {
            return _salesInvoiceRepo.AddEdit(entity, cancellationToken, userId);
        }

        public Task<int> Delete(int id, CancellationToken cancellationToken, int userId)
        {
            return _salesInvoiceRepo.Delete(id, cancellationToken, userId);
        }

        public Task<List<PurchaseInvoiceMaster>> Get(CancellationToken cancellationToken)
        {
            return _salesInvoiceRepo.Get(cancellationToken);
        }

        public async Task<PurchaseInvoiceMaster?> Get(int id, CancellationToken cancellationToken)
        {
            PurchaseInvoiceMaster? entity = await _salesInvoiceRepo.Get(id, cancellationToken);


            List<int> units = entity!.Details.Select(r => r.DefaultUnitID).Distinct().ToList();

            List<UnitDetails> details = await _unitRepo.GetMultipleRelatedUnit(string.Join(",", units), cancellationToken);


            var unitDetailsLookup = details
                .GroupBy(d => d.DefaultUnitID)
                .ToDictionary(g => g.Key, g => g.ToList());

            List<InvoiceDetail> productDetails = [.. entity.Details
                .Select(product =>
                {

                    // Find units by UnitID. If the key doesn't exist, GetValueOrDefault provides null.
                    unitDetailsLookup.TryGetValue(product.DefaultUnitID, out var relatedUnits);

                    // Create the final object, assigning the found units or an empty list.
                    product.UnitDetails = relatedUnits!;
                    return product;
                })];


            entity.Details = productDetails;
            return entity;
        }

        public Task<Navigate> Navigate(int pageNo, int rowPerPage, CancellationToken cancellationToken)
        {
            return _salesInvoiceRepo.Navigate(pageNo, rowPerPage, cancellationToken);
        }
    }
}
