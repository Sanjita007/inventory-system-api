using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.Models.Reports;
using inventory_system_api.Shared;
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

        public async Task<DashboardSummary> GetDashboardSummary()
        {
            DashboardSummary dashboardSummary = new DashboardSummary();

            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();
                cmd.CommandText = "DASHBOARD_SALES_PURCH_SUMMARY";

                cmd.CommandType = CommandType.StoredProcedure;
                _dbConnection.Open();
                IDataReader rdr = await cmd.ExecuteReaderAsync();
                dashboardSummary.SalesPurch = new SalesPurchSummary();
                dashboardSummary.Product = new List<ProductSummary>();

                while (rdr.Read())
                {
                    dashboardSummary.SalesPurch.Months.Add(rdr["DATE"].ToString()??"");
                    dashboardSummary.SalesPurch.PurchAmounts.Add(rdr["PURCHASE"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["PURCHASE"]));
                    dashboardSummary.SalesPurch.SalesAmounts.Add(rdr["SALES"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["SALES"]));

                }
                if (rdr.NextResult())
                {
                    while (rdr.Read())
                    {
                        dashboardSummary.Product.Add(new ProductSummary()
                        {
                            Image = rdr["IMAGE"] == DBNull.Value ? null : ((byte[])rdr["IMAGE"]).ToBase64(),
                            ProductName = rdr["ENGNAME"].ToString() ?? "",
                            SalesPrice = rdr["SALESRATE"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["SALESRATE"]),

                        });
                      
                    }
                }

                _dbConnection.Close();
            }

            return dashboardSummary;
        }

        public async Task<List<ProductSummary>> GetProductDashboardSummary()
        {

            List<ProductSummary> products = new List<ProductSummary>();
            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();
                cmd.CommandText = "SELECT TOP 4 ENGNAME, IMAGE, SALESRATE  FROM TBLPRODUCT WHERE IMAGE IS NOT NULL";

                cmd.CommandType = CommandType.Text;
                _dbConnection.Open();
                IDataReader rdr = await cmd.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    
                        products.Add(new ProductSummary()
                        {
                            Image = rdr["IMAGE"] == DBNull.Value ? null : ((byte[])rdr["IMAGE"]).ToBase64(),
                            ProductName = rdr["ENGNAME"].ToString() ?? "",
                            SalesPrice = rdr["SALESRATE"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["SALESRATE"]),

                        });

                    }
                

                _dbConnection.Close();
            }

            return products;
        }

       
        public async Task<SalesPurchSummary> GetSalesPurchDashboardSummary()
        {
            SalesPurchSummary   SalesPurch = new SalesPurchSummary();

            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();
                cmd.CommandText = "DASHBOARD_SALES_PURCH_SUMMARY";

                cmd.CommandType = CommandType.Text;
                _dbConnection.Open();
                IDataReader rdr = await cmd.ExecuteReaderAsync();

                while (rdr.Read())
                {
                   SalesPurch.Months.Add(rdr["DATE"].ToString()?? "");
                   SalesPurch.PurchAmounts.Add(rdr["PURCHASE"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["PURCHASE"]));
                   SalesPurch.SalesAmounts.Add(rdr["SALES"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["SALES"]));

               
                }

                _dbConnection.Close();
            }

            return SalesPurch;
        }

        public async Task<List<RecentTransactionSummary>> GetRecentTransactionSummary()
        {

            List<RecentTransactionSummary> products = new List<RecentTransactionSummary>();
            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();
                cmd.CommandText = "SELECT TOP 5 CONVERT(DATE, SALESINVOICE_DATE, 111) SALES_DATE, CONCAT('SOLD $', M.NET_AMOUNT , ' WORTH OF PRODUCTS TO ', CUSTOMERNAME, ' - ', PRODUCTS ) DETAIL " +
                    "FROM TBLSALESINVOICEMASTER M INNER JOIN (SELECT  SALESINVOICEID, STRING_AGG(ENGNAME, ', ') PRODUCTS" +
                    "   FROM TBLSALESINVOICEDETAILS T INNER JOIN TBLPRODUCT P ON T.PRODUCTID = P.PRODUCTID GROUP BY SALESINVOICEID) DET ON M.SALESINVOICEID = DET.SALESINVOICEID " +
                    "ORDER BY SALESINVOICE_DATE DESC";

                cmd.CommandType = CommandType.Text;
                _dbConnection.Open();
                IDataReader rdr = await cmd.ExecuteReaderAsync();

                while (rdr.Read())
                {

                    products.Add(new RecentTransactionSummary()
                    {
                        Date = rdr["SALES_DATE"].ToString() ?? "",
                        Details = rdr["DETAIL"].ToString() ?? "",

                    });

                }


                _dbConnection.Close();
            }

            return products;
        }

    }
}
