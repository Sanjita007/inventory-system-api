using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory_system_api.Application.IQueue
{
    public interface IMessageService
    {
        Task<bool> Enqueue(string message);
    }

}
