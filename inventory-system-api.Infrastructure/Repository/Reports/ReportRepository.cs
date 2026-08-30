using Dapper;
using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.Models.Reports;
using System.Data;

namespace inventory_system_api.Infrastructure.Repository.Reports
{
    public class ReportRepository : IReportRepository
    {
        IDbConnection _dbConnection;

        public ReportRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<GrossProfitSummary> GetGrossProfitReport(CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(
                commandText: "GROSS_PROFITABILITY_REPORT",
                commandType: CommandType.StoredProcedure, 
                cancellationToken: cancellationToken
            );

            var entity = (await _dbConnection.QueryAsync<GrossProfit>(command)).ToList();

            return new GrossProfitSummary
            {
                GrossProfitList = entity,
                TotalRevenue = entity.Sum(x => x.TotalRevenue),
                TotalCost = entity.Sum(x => x.TotalCost),
                TotalProfit = entity.Sum(x => x.Profit)
            };
        }

        public async Task<InventorySummary> GetInventoryReport(CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(
                commandText: "INVENTORY_VALUATION_REPORT",
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var entity = (await _dbConnection.QueryAsync<InventoryDetail>(command)).ToList();

            return new InventorySummary
            {
                InventoryDetail = entity,
                TotalQuantityIn = entity.Sum(x => x.QuantityIn),
                TotalQuantityOut = entity.Sum(x => x.QuantityOut),
                TotalQuantityOnHand = entity.Sum(x => x.QuantityOnHand),
                TotalInValue = entity.Sum(x => x.TotalInValue)
            };
        }

    }
}
