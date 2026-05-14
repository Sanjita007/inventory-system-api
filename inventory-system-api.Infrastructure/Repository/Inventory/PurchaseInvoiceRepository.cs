using inventory_system_api.Application.IRepository.Invenetory;
using inventory_system_api.Application.Models.Inventory;
using inventory_system_api.Application.Models.System;
using inventory_system_api.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace inventory_system_api.Infrastructure.Repository.Inventory
{
    public class PurchaseInvoiceRepository : BaseRepository<PurchaseInvoiceMaster>, IPurchaseInvoiceRepository
    {
        IDbConnection _dbConnection;

        public PurchaseInvoiceRepository(IDbConnection dbConnection): base(dbConnection) { 
            _dbConnection = dbConnection;
        }

        public async Task<int> AddEdit(PurchaseInvoiceMaster entity, CancellationToken cancellationToken, int userId)
        {
            int res = 0;
            using (_dbConnection as SqlConnection)
            {
                SqlParameter result = new SqlParameter("@return", dbType: SqlDbType.VarChar, 200);
                result.Direction = ParameterDirection.Output;

                SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();
                cmd.CommandText = "[SP_PURCHASE_INVOICE_ADD_EDIT]";
                cmd.Parameters.AddWithValue("@ID", entity.ID);
                cmd.Parameters.AddWithValue("@VOUCHERNO", entity.VoucherNo);
                cmd.Parameters.AddWithValue("@PURCHINVOICEDATE", entity.Date);
                cmd.Parameters.AddWithValue("@SUPPLIERNAME", entity.EntityName);
                cmd.Parameters.AddWithValue("@REMARKS", entity.Remarks);
                cmd.Parameters.AddWithValue("@NETAMOUNT", entity.NetAmount);
                cmd.Parameters.AddWithValue("@SPECIALDISCOUNT", entity.SpecialDiscount);
                cmd.Parameters.AddWithValue("@TOTALAMOUNT", entity.TotalAmount);
                cmd.Parameters.AddWithValue("@GROSSAMOUNT", entity.GrossAmount);
                cmd.Parameters.AddWithValue("@TOTALQTY", entity.TotalQty);
                cmd.Parameters.AddWithValue("@TOTALTCAMOUNT", entity.TotalTCAmount);
                
                cmd.Parameters.AddWithValue("@PURCHDETAILS", entity.Details.ToXml("PURCHINVOICEDETAILS"));
                //cmd.Parameters.AddWithValue("@STATUS", entity.Status.ToString());
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
                cmd.CommandText = "SP_PRODUCT_GROUP_DELETE";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.StoredProcedure;
                _dbConnection.Open();
                return await cmd.ExecuteNonQueryAsync(cancellationToken);

            }
        }

        public async Task<List<PurchaseInvoiceMaster>> Get(CancellationToken cancellationToken)
        {
            return await ExecuteQueryAsync("SP_GET_PURCHASE_INVOICE", MapEntity, [], cancellationToken);

        }

        public override PurchaseInvoiceMaster MapEntity(IDataReader rdr)
        {
            PurchaseInvoiceMaster entity = new PurchaseInvoiceMaster
            {
                ID = Convert.ToInt32(rdr["PurchaseInvoiceID"]),
                VoucherNo = rdr["Voucher_No"].ToString(),
                Date = Convert.ToDateTime(rdr["PurchaseInvoice_Date"]),
                TotalQty = rdr["TotalQty"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["TotalQty"]),
                GrossAmount = rdr["Gross_Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Gross_Amount"]),
                SpecialDiscount = rdr["SpecialDiscount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["SpecialDiscount"]),
                NetAmount = rdr["Net_Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Net_Amount"]),
                //TotalAmount = rdr["Total_Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Total_Amount"]),
                TotalTCAmount = rdr["TotalTCAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["TotalTCAmount"]),
                
                CreatedDate = Convert.ToDateTime(rdr["Created_Date"])
            };

            return entity;
        }

        public PurchaseInvoiceMaster MapEntityDetails(IDataReader rdr)
        {
            PurchaseInvoiceMaster entity = new PurchaseInvoiceMaster
            {
                ID = Convert.ToInt32(rdr["PurchaseInvoiceID"]),
                VoucherNo = rdr["Voucher_No"].ToString(),
                Date = Convert.ToDateTime(rdr["PurchaseInvoice_Date"]),
                TotalQty = rdr["TotalQty"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["TotalQty"]),
                GrossAmount = rdr["Gross_Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Gross_Amount"]),
                SpecialDiscount = rdr["SpecialDiscount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["SpecialDiscount"]),
                NetAmount = rdr["Net_Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Net_Amount"]),
                //TotalAmount = rdr["Total_Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Total_Amount"]),
                TotalTCAmount = rdr["TotalTCAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["TotalTCAmount"]),
                
                CreatedBy = Convert.ToInt32(rdr["Created_By"]),
                CreatedDate = Convert.ToDateTime(rdr["Created_Date"])
            };

            //only get this one for the getByID because it would be heavy otherwise
            while (rdr.NextResult())
                {
                    entity.Details = new List<InvoiceDetail>();
                    while (rdr.Read())
                    {
                        entity.Details.Add(new InvoiceDetail()
                        {
                            ID = Convert.ToInt32(rdr["PurchaseINvoice_DetailID"]),
                            MasterID = Convert.ToInt32(rdr["PurchaseInvoiceID"]),
                            ProductID = Convert.ToInt32(rdr["ProductID"]),
                            ProductName = rdr["ProductName"].ToString()??"",
                            QtyUnitID = Convert.ToInt32(rdr["QtyUnitID"]),
                            DefaultUnitID = Convert.ToInt32(rdr["UnitMaintenanceID"]),
                            DefaultUnitName = rdr["DefaultUnitName"].ToString()??"",
                            DefaultUnitSymbol = rdr["DefaultUnitSymbol"].ToString() ?? "",
                            TaxID = rdr["TaxID"] == DBNull.Value ? null : Convert.ToInt32(rdr["TaxID"]),
                            ProductCode = rdr["Code"].ToString() ?? "",
                            Quantity = Convert.ToInt32(rdr["Quantity"]),
                            Price = Convert.ToDecimal(rdr["PurchaseRate"]),
                            Amount = Convert.ToDecimal(rdr["Amount"]),
                            DiscPercent = Convert.ToDecimal(rdr["DiscPercentage"]),
                            Discount = Convert.ToDecimal(rdr["Discount"]),
                            NetAmount = Convert.ToDecimal(rdr["Net_Amount"]),
                            TaxAmount = Convert.ToDecimal(rdr["TaxAmount"]),
                            //GeneralName = rdr["GeneralName"].ToString(),
                            Remarks = rdr["Description"].ToString(),
                        });
                    }
                }

            return entity;
        }

        public async Task<Navigate> Navigate(int pageNo, int rowPerPage, CancellationToken cancellationToken)
        {
            List<PurchaseInvoiceMaster> listEntity = [];
            int TotalRecords = 0;
            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.Parameters.AddWithValue("@PAGENO", pageNo);
                cmd.Parameters.AddWithValue("@ROWSPERPAGE", rowPerPage);
                cmd.CommandText = "SP_NAVIGATE_PURCHASE_INVOICE";
                
                cmd.CommandType = CommandType.StoredProcedure;
                _dbConnection.Open();
                IDataReader rdr = await cmd.ExecuteReaderAsync(cancellationToken);

                while (rdr.Read())
                {
                    PurchaseInvoiceMaster entity = new()
                    {
                        ID = Convert.ToInt32(rdr["PurchaseInvoiceID"]),
                        VoucherNo = rdr["Voucher_No"].ToString(),
                        Date = Convert.ToDateTime(rdr["PurchaseInvoice_Date"]),
                        TotalQty = rdr["TotalQty"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["TotalQty"]),
                        GrossAmount = rdr["Gross_Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Gross_Amount"]),
                        SpecialDiscount = rdr["SpecialDiscount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["SpecialDiscount"]),
                        NetAmount = rdr["Net_Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Net_Amount"]),
                        //TotalAmount = rdr["Total_Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Total_Amount"]),
                        TotalTCAmount = rdr["TotalTCAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["TotalTCAmount"]),

                        CreatedBy = Convert.ToInt32(rdr["Created_By"]),
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
            return  new Navigate { Entity = listEntity, PageCount = (int)Math.Ceiling((decimal)TotalRecords/rowPerPage), PageNo= pageNo, RowPerPage = rowPerPage };
            
        }

        public async Task<PurchaseInvoiceMaster> Get(int id, CancellationToken cancellationToken)
        {
            
            string query = "SP_GET_PURCHASE_INVOICE";
            SqlParameter[] param = [ new SqlParameter("@id", id) ];

               
            List<PurchaseInvoiceMaster> list = await ExecuteQueryAsync(query, MapEntityDetails, param
                , cancellationToken, commandType: CommandType.StoredProcedure);

            return list.FirstOrDefault();
        }

    }
}
