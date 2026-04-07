using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.Models.Reports;
using Microsoft.Data.SqlClient;
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

        public async Task<GrossProfitSummary> GetGrossProfitReport()
        {
            List<GrossProfit> entity = [];
            decimal TotalRev = 0, TotalCost = 0, TotalProfit = 0;
            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();
                cmd.CommandText = "Inv.GROSS_PROFITABILITY_REPORT";

                cmd.CommandType = CommandType.Text;
                _dbConnection.Open();
                IDataReader rdr = await cmd.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    decimal Revenue = rdr["TotalRevenue"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["TotalRevenue"]);
                    decimal Cost = rdr["TotalCost"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["TotalCost"]);
                    decimal Profit = rdr["Profit"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Profit"]);

                    TotalRev += Revenue;
                    TotalCost += Cost;
                    TotalProfit += Profit;

                    entity.Add(new GrossProfit
                    {
                        ProductId = Convert.ToInt32(rdr["ProductID"]),
                        ProductCode = rdr["ProductCode"].ToString()??"",
                        ProductName = rdr["ProductName"].ToString() ?? "",

                        QuantitySold = rdr["QuantitySold"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["QuantitySold"]),
                        TotalRevenue = Revenue,
                        TotalCost = Cost,
                        Profit = Profit,
                        Margin = rdr["Margin"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Margin"]),

                    });

                }

                _dbConnection.Close();
            }

            return new GrossProfitSummary { GrossProfitList = entity , TotalCost= TotalCost, TotalProfit = TotalProfit, TotalRevenue = TotalRev};
        }

        public async Task<InventorySummary> GetInventoryReport()
        {
            List<InventoryDetail> entity = [];
            decimal TotalqtyIn = 0, TotalqtyOut = 0, TotalqtyOnHand = 0, TotalInValue=0;
            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();
                cmd.CommandText = "Inv.INVENTORY_VALUATION_REPORT";

                cmd.CommandType = CommandType.Text;
                _dbConnection.Open();
                IDataReader rdr = await cmd.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    decimal qtyIn = rdr["QUANTITYOUT"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["QUANTITYOUT"]);
                    decimal qtyOut = rdr["QUANTITYIN"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["QUANTITYIN"]);
                    decimal qtyOnHand = rdr["QTYONHAND"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["QTYONHAND"]);
                    decimal qtyValue = rdr["TOTALINVVALUE"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["TOTALINVVALUE"]);

                    TotalqtyIn += qtyIn;
                    TotalqtyOut += qtyOut;
                    TotalqtyOnHand += qtyOnHand;
                    TotalInValue += qtyValue;

                    entity.Add(new InventoryDetail
                    {
                        ProductId = Convert.ToInt32(rdr["ProductID"]),
                        ProductCode = rdr["ProductCode"].ToString()??"",
                        ProductName = rdr["ProductName"].ToString() ?? "",

                        QuantityIn = qtyIn,
                        QuantityOut = qtyOut,
                        QuantityOnHand = qtyOnHand,
                        TotalInValue =  qtyValue

                    });

                }

                _dbConnection.Close();
            }

            return new InventorySummary { InventoryDetail= entity, TotalQuantityIn = TotalqtyIn, TotalQuantityOut = TotalqtyOut, TotalQuantityOnHand = TotalqtyOnHand, TotalInValue = TotalInValue};
        }

    }
}
