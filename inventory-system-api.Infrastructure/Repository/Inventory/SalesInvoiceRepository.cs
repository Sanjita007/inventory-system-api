using Microsoft.Data.SqlClient;
using System.Data;
using inventory_system_api.Shared;
using inventory_system_api.Application.Models.Inventory;
using inventory_system_api.Application.IRepository.Invenetory;
using inventory_system_api.Application.Models.System;

namespace inventory_system_api.Infrastructure.Repository.Inventory
{
    public class SalesInvoiceRepository : BaseRepository<SalesInvoiceMaster>, ISalesInvoiceRepository
    {
        IDbConnection _dbConnection;

        public SalesInvoiceRepository(IDbConnection dbConnection) : base(dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> AddEdit(SalesInvoiceMaster entity, CancellationToken cancellationToken, int userId)
        {
            int res = 0;
            using (_dbConnection as SqlConnection)
            {
                SqlParameter result = new SqlParameter("@return", dbType: SqlDbType.VarChar, 200);
                result.Value = 0;
                result.Direction = ParameterDirection.Output;

                SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();
                cmd.CommandText = "[SP_SALES_INVOICE_ADD_EDIT]";
                cmd.Parameters.AddWithValue("@id", entity.ID);
                cmd.Parameters.AddWithValue("@VOUCHERNO", entity.VoucherNo);
                cmd.Parameters.AddWithValue("@SALESINVOICEDATE", entity.Date);
                cmd.Parameters.AddWithValue("@REMARKS", entity.Remarks);
                cmd.Parameters.AddWithValue("@NETAMOUNT", entity.NetAmount);
                cmd.Parameters.AddWithValue("@SPECIALDISCOUNT", entity.SpecialDiscount);
                cmd.Parameters.AddWithValue("@TOTALAMOUNT", entity.TotalAmount);
                cmd.Parameters.AddWithValue("@GROSSAMOUNT", entity.GrossAmount);
                cmd.Parameters.AddWithValue("@TOTALQTY", entity.TotalQty);
                cmd.Parameters.AddWithValue("@TOTALTCAMOUNT", entity.TotalTCAmount);
                cmd.Parameters.AddWithValue("@TENDERAMT", entity.TenderAmount);
                cmd.Parameters.AddWithValue("@CHANGEAMT", entity.ChangeAmount);
                cmd.Parameters.AddWithValue("@ADJUSTMENTAMT", entity.AdjustmentAmount);
                cmd.Parameters.AddWithValue("@SALESDETAILS", entity.Details.ToXml("SALESINVOICEDETAILS"));
                cmd.Parameters.AddWithValue("@STATUS", entity.Status.ToString());
                cmd.Parameters.AddWithValue("@AUDITLOGCSV", entity.ToJson());
                cmd.Parameters.AddWithValue("@USERID", userId);
                cmd.Parameters.Add(result);

                cmd.CommandType = CommandType.StoredProcedure;
                _dbConnection.Open();
                await cmd.ExecuteNonQueryAsync(cancellationToken);
                res = Convert.ToInt32(result.Value);

            }

            return res;
        }


        public async Task<int> Delete(int id, CancellationToken cancellationToken, int userId)
        {
            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();
                cmd.CommandText = "[SP_SALES_INVOICE_DELETE]";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.StoredProcedure;
                _dbConnection.Open();
                return await cmd.ExecuteNonQueryAsync(cancellationToken);

            }
        }

        public async Task<List<SalesInvoiceMaster>> Get(CancellationToken cancellationToken)
        {
            return await ExecuteQueryAsync("SP_GET_SALES_INVOICE", MapEntity, [], cancellationToken, CommandType.StoredProcedure);
        }

        public override SalesInvoiceMaster MapEntity(IDataReader rdr)
        {
            SalesInvoiceMaster entity = new()
            {
                ID = Convert.ToInt32(rdr["SalesInvoiceID"]),
                VoucherNo = rdr["Voucher_No"].ToString(),
                Date = Convert.ToDateTime(rdr["SalesInvoice_Date"]),
                TotalQty = rdr["TotalQty"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["TotalQty"]),
                GrossAmount = rdr["Gross_Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Gross_Amount"]),
                SpecialDiscount = rdr["SpecialDiscount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["SpecialDiscount"]),
                NetAmount = rdr["Net_Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Net_Amount"]),
                //TotalAmount = rdr["Total_Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Total_Amount"]),
                TotalTCAmount = rdr["TotalTCAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["TotalTCAmount"]),
                TenderAmount = rdr["TenderAmt"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["TenderAmt"]),
                ChangeAmount = rdr["ChangeAmt"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["ChangeAmt"]),
                AdjustmentAmount = rdr["AdjustmentAmt"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["AdjustmentAmt"]),
                //SalesDueDate = Convert.ToDateTime(rdr["SalesInvoice_Date"]),
                CreatedDate = Convert.ToDateTime(rdr["Created_Date"])
            };

            return entity;
        }

        public SalesInvoiceMaster MapEntityDetails(IDataReader rdr)
        {
            SalesInvoiceMaster entity = new()
            {
                ID = Convert.ToInt32(rdr["SalesInvoiceID"]),
                VoucherNo = rdr["Voucher_No"].ToString(),
                Date = Convert.ToDateTime(rdr["SalesInvoice_Date"]),
                TotalQty = rdr["TotalQty"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["TotalQty"]),
                GrossAmount = rdr["Gross_Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Gross_Amount"]),
                SpecialDiscount = rdr["SpecialDiscount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["SpecialDiscount"]),
                NetAmount = rdr["Net_Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Net_Amount"]),
                //TotalAmount = rdr["Total_Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Total_Amount"]),
                TotalTCAmount = rdr["TotalTCAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["TotalTCAmount"]),
                TenderAmount = rdr["TenderAmt"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["TenderAmt"]),
                ChangeAmount = rdr["ChangeAmt"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["ChangeAmt"]),
                AdjustmentAmount = rdr["AdjustmentAmt"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["AdjustmentAmt"]),
                //CreatedBy = Convert.ToInt32(rdr["Created_By"]),
                //SalesDueDate = Convert.ToDateTime(rdr["SalesInvoice_Date"]),
                CreatedDate = Convert.ToDateTime(rdr["Created_Date"])
            };

            //only get this one for the getByID because it would be heavy otherwise
            while (rdr.NextResult())
            {
                entity.Details = [];
                while (rdr.Read())
                {
                    entity.Details.Add(new InvoiceDetail()
                    {
                        ID = Convert.ToInt32(rdr["SalesINvoice_DetailID"]),
                        MasterID = Convert.ToInt32(rdr["SalesInvoiceID"]),
                        ProductID = Convert.ToInt32(rdr["ProductID"]),
                        ProductName = rdr["ProductName"].ToString() ?? "",
                        QtyUnitID = Convert.ToInt32(rdr["QtyUnitID"]),
                        DefaultUnitID = Convert.ToInt32(rdr["UnitMaintenanceID"]),
                        DefaultUnitName = rdr["DefaultUnitName"].ToString() ?? "",
                        DefaultUnitSymbol = rdr["DefaultUnitSymbol"].ToString() ?? "",
                        TaxID = rdr["TaxID"] == DBNull.Value ? null : Convert.ToInt32(rdr["TaxID"]),
                        ProductCode = rdr["Code"].ToString() ?? "",
                        Quantity = Convert.ToInt32(rdr["Quantity"]),
                        Price = Convert.ToDecimal(rdr["SalesRate"]),
                        Amount = Convert.ToDecimal(rdr["Amount"]),
                        DiscPercent = Convert.ToDecimal(rdr["DiscPercentage"]),
                        Discount = Convert.ToDecimal(rdr["Discount"]),
                        NetAmount = Convert.ToDecimal(rdr["Net_Amount"]),
                        TaxAmount = Convert.ToDecimal(rdr["TaxAmount"]),
                        VATAmount = Convert.ToDecimal(rdr["VATAmount"]),
                        GeneralName = rdr["GeneralName"].ToString(),
                        Remarks = rdr["Description"].ToString(),
                    });
                }
            }

            return entity;
        }

        public async Task<Navigate> Navigate(int pageNo, int rowPerPage, CancellationToken cancellationToken)
        {
            List<SalesInvoiceMaster> listEntity = [];
            int TotalRecords = 0;
            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.Parameters.AddWithValue("@pageNo", pageNo);
                cmd.Parameters.AddWithValue("@rowsPerPage", rowPerPage);
                cmd.CommandText = "SP_NAVIGATE_SALES_INVOICE";

                cmd.CommandType = CommandType.StoredProcedure;
                _dbConnection.Open();
                IDataReader rdr = await cmd.ExecuteReaderAsync(cancellationToken);

                while (rdr.Read())
                {
                    SalesInvoiceMaster entity = new()
                    {
                        ID = Convert.ToInt32(rdr["SalesInvoiceID"]),
                        VoucherNo = rdr["Voucher_No"].ToString(),
                        Date = Convert.ToDateTime(rdr["SalesInvoice_Date"]),
                        TotalQty = rdr["TotalQty"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["TotalQty"]),
                        GrossAmount = rdr["Gross_Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Gross_Amount"]),
                        SpecialDiscount = rdr["SpecialDiscount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["SpecialDiscount"]),
                        NetAmount = rdr["Net_Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Net_Amount"]),
                        //TotalAmount = rdr["Total_Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Total_Amount"]),
                        TotalTCAmount = rdr["TotalTCAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["TotalTCAmount"]),
                        TenderAmount = rdr["TenderAmt"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["TenderAmt"]),
                        ChangeAmount = rdr["ChangeAmt"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["ChangeAmt"]),
                        AdjustmentAmount = rdr["AdjustmentAmt"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["AdjustmentAmt"]),
                        CreatedBy = Convert.ToInt32(rdr["Created_By"]),
                        //SalesDueDate = Convert.ToDateTime(rdr["SalesInvoice_Date"]),
                        CreatedDate = Convert.ToDateTime(rdr["Created_Date"])
                    };

                    listEntity.Add(entity);
                }

                if (rdr.NextResult())
                {
                    while (rdr.Read())
                    {
                        TotalRecords = Convert.ToInt32(rdr[0]);
                    }
                }

                _dbConnection.Close();
            }
            return new Navigate { Entity = listEntity, PageCount = (int)Math.Ceiling((decimal)TotalRecords / rowPerPage), PageNo = pageNo, RowPerPage = rowPerPage };

        }

        public async Task<SalesInvoiceMaster> Get(int id, CancellationToken cancellationToken)
        {

            string query = "SP_GET_SALES_INVOICE";
            SqlParameter[] param = [
                new SqlParameter("@id", id) ];


            List<SalesInvoiceMaster> list = await ExecuteQueryAsync(query, MapEntityDetails, param, cancellationToken, CommandType.StoredProcedure);

            return list.FirstOrDefault();
        }

    }
}
