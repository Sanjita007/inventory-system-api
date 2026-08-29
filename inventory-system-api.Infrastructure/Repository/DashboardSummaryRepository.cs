using Dapper;
using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.Models.Reports;
using Microsoft.Data.SqlClient;
using System.Data;

namespace inventory_system_api.Infrastructure.Repository.Reports
{
    public class DashboardSummaryRepository : IDashboardSummaryRepository
    {
        IDbConnection _dbConnection;

        public DashboardSummaryRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<DashboardSummary> GetDashboardSummary(CancellationToken cancellationToken)
        {
            using var multi = await _dbConnection.QueryMultipleAsync(
            new CommandDefinition(
                    commandText: "DASHBOARD_SALES_PURCH_SUMMARY",
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken
                )
            );

            var dashboardSummary = new DashboardSummary
            {
                SalesPurch = new SalesPurchSummary(),
                Product = new List<ProductSummary>()
            };

            // Sales & Purchase Summary
            var salesPurchData = await multi.ReadAsync<dynamic>();
            foreach (var row in salesPurchData)
            {
                dashboardSummary.SalesPurch.Months.Add(row.DATE?.ToString() ?? "");
                dashboardSummary.SalesPurch.PurchAmounts.Add(row.PURCHASE == null ? 0m : Convert.ToDecimal(row.PURCHASE));
                dashboardSummary.SalesPurch.SalesAmounts.Add(row.SALES == null ? 0m : Convert.ToDecimal(row.SALES));
            }

            // Product Summary
            var productData = await multi.ReadAsync<dynamic>();
            foreach (var row in productData)
            {
                dashboardSummary.Product.Add(new ProductSummary
                {
                    ImageByte = row.IMAGE == null ? null : (byte[])row.IMAGE,
                    ProductName = row.ENGNAME?.ToString() ?? "",
                    SalesPrice = row.SALESRATE == null ? 0m : Convert.ToDecimal(row.SALESRATE)
                });
            }

            return dashboardSummary;
        }

        public async Task<List<ProductSummary>> GetProductDashboardSummary(CancellationToken cancellationToken)
        {
            using (_dbConnection as SqlConnection)
            {
                string commandText = "SP_GET_PRODUCT_DASHBOARD_SUMMARY";
                _dbConnection.Open();

                return await _dbConnection.QueryAsync<ProductSummary>(
                commandText, commandType: CommandType.StoredProcedure).ContinueWith(t => t.Result.ToList(), cancellationToken);
            }
        }


        public async Task<SalesPurchSummary?> GetSalesPurchDashboardSummary(CancellationToken cancellationToken)
        {
            SalesPurchSummary salesPurchSummary = new()
            {
                Months = [],
                SalesAmounts = [],
                PurchAmounts = []
            };

            using (_dbConnection as SqlConnection)
            {
                string commandText = "DASHBOARD_SALES_PURCH_SUMMARY";
                _dbConnection.Open();

                var salesPurchData = await _dbConnection.QueryAsync<SalesPurchInitial>(
                commandText, commandType: CommandType.StoredProcedure).ContinueWith(t => t.Result.ToList(), cancellationToken);

                foreach (var row in salesPurchData)
                {
                    salesPurchSummary.Months.Add(row.Date.ToString() ?? "");
                    salesPurchSummary.PurchAmounts.Add(Convert.ToDecimal(row.Purchase));
                    salesPurchSummary.SalesAmounts.Add(Convert.ToDecimal(row.Sales));
                }
            }
            return salesPurchSummary;
        }


        public async Task<List<RecentTransactionSummary>> GetRecentTransactionSummary(CancellationToken cancellationToken)
        {
            using (_dbConnection as SqlConnection)
            {
                string commandText = "SP_RECENT_TRANSACTION_SUMMARY";
                _dbConnection.Open();

                return await _dbConnection.QueryAsync<RecentTransactionSummary>(
                commandText, commandType: CommandType.StoredProcedure).ContinueWith(t => t.Result.ToList(), cancellationToken);
                
            }
        }

    }
}
