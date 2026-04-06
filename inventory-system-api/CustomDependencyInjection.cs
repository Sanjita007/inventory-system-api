using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.IRepository.Invenetory;
using inventory_system_api.Application.IService;
using inventory_system_api.Infrastructure.Repository;
using inventory_system_api.Infrastructure.Repository.Inventory;
using inventory_system_api.Infrastructure.Repository.Reports;
using inventory_system_api.Infrastructure.Service;

namespace inventory_system_api
{
    public static class CustomDependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddScoped<ISalesInvoiceRepository, SalesInvoiceRepository>();
            services.AddScoped<IPurchaseInvoiceRepository, PurchaseInvoiceRepository>();
            services.AddScoped<ISalesInvoiceService, SalesInvoiceService>();
            services.AddScoped<IPurchaseInvoiceMasterService,PurchaseInvoiceMasterService>();
            services.AddScoped<IProductGroupRepository, ProductGroupRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IUnitRepository, UnitRepository>();
            services.AddTransient<ICompoundUnitRepository, CompoundUnitRepository>();
            services.AddTransient<IDashboardSummaryRepository, DashboardSummaryRepository>();
            services.AddScoped<IDepotRepository, DepotRepository>();
            services.AddScoped<ITaxRepository, TaxRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddSingleton<IErrorLogRepository, ErrorLogRepositoy>();
            return services;
        }
    }
}
