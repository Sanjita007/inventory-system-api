using inventory_system_api.Application.Models;
using inventory_system_api.Models;

namespace inventory_system_api.Application.IRepository
{
    public interface IErrorLogRepository
    {
        Task<int> AddErrorLog(ErrorLog error);
        Task<List<ErrorLog>> Get();
    }
}
