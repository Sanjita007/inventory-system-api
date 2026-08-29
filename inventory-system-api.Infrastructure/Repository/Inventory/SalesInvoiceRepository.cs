using Azure;
using Dapper;
using inventory_system_api.Application.IRepository.Invenetory;
using inventory_system_api.Application.Models.Inventory;
using inventory_system_api.Application.Models.System;
using inventory_system_api.Shared;
using Microsoft.Data.SqlClient;
using System.Data;
using static Dapper.SqlMapper;

namespace inventory_system_api.Infrastructure.Repository.Inventory
{
    public class SalesInvoiceRepository : ISalesInvoiceRepository
    {
        IDbConnection _dbConnection;

        public SalesInvoiceRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> AddEdit(SalesInvoiceMaster entity, CancellationToken cancellationToken, int userId)
        {
            using (_dbConnection as SqlConnection)
            {
                var parameters = new DynamicParameters(new
                {
                    entity.ID,
                    entity.VoucherNo,
                    entity.Date,
                    entity.Remarks,
                    entity.NetAmount,
                    entity.SpecialDiscount,
                    entity.TotalAmount,
                    entity.GrossAmount,
                    entity.TotalQty,
                    entity.TotalTCAmount,
                    entity.TenderAmount,
                    entity.ChangeAmount,
                    entity.AdjustmentAmount,
                    entity.EntityName,
                    Details = entity.Details.ToXml("SALESINVOICEDETAILS"),
                    entity.Status,
                    userId
                });

                parameters.Add("return", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _dbConnection.ExecuteAsync(new CommandDefinition("SP_SALES_INVOICE_ADD_EDIT", parameters,
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

                return parameters.Get<int>("return");
            }
        }


        public async Task<int> Delete(int id, CancellationToken cancellationToken, int userId)
        {
            using (_dbConnection as SqlConnection)
            {
                string commandText = "SP_SALES_INVOICE_DELETE";

                _dbConnection.Open();
                return await _dbConnection.ExecuteAsync(new CommandDefinition(commandText, new { id },
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

            }
        }

        public async Task<List<SalesInvoiceMaster>> Get(CancellationToken cancellationToken)
        {
            using (_dbConnection as SqlConnection)
            {
                string commandText = "SP_GET_SALES_INVOICE";
                _dbConnection.Open();

                return await _dbConnection.QueryAsync<SalesInvoiceMaster>(commandText,
                    commandType: CommandType.StoredProcedure).ContinueWith(t => t.Result.ToList(), cancellationToken);
            }
        }
        public async Task<Navigate> Navigate(int pageNo, int rowPerPage, CancellationToken cancellationToken)
        {
            IEnumerable<SalesInvoiceMaster> listEntity;
            int totalRecords;

            using (var multi = await _dbConnection.QueryMultipleAsync(
                    new CommandDefinition(
                        "[dbo].[SP_NAVIGATE_SALES_INVOICE]",
                        new DynamicParameters(new
                        {
                            pageNo,
                            rowPerPage,
                        }),
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken
        )))
            {

                // first read the list of records
                listEntity = await multi.ReadAsync<SalesInvoiceMaster>();

                // get the total number of records 
                totalRecords = await multi.ReadSingleAsync<int>();
            }

            return new Navigate
            {
                Entity = listEntity,
                PageCount = (int)Math.Ceiling((decimal)totalRecords / rowPerPage),
                PageNo = pageNo,
                RowPerPage = rowPerPage
            };

        }

        public async Task<SalesInvoiceMaster?> Get(int id, CancellationToken cancellationToken)
        {
            using var multi = await _dbConnection.QueryMultipleAsync(
                  new CommandDefinition(
                      "[dbo].[SP_GET_SALES_INVOICE]",
                      new DynamicParameters(new
                      {
                          id
                      }),
          commandType: CommandType.StoredProcedure,
          cancellationToken: cancellationToken
      ));
            // first read the list of records
            SalesInvoiceMaster? entity = await multi.ReadFirstOrDefaultAsync<SalesInvoiceMaster?>();

            // get the details of the sales invoice

            var details = await multi.ReadAsync<InvoiceDetail>();
            entity!.Details = [.. details];

            return entity;

        }

    }
}
