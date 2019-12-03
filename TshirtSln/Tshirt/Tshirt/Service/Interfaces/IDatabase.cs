using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tshirt.Models;

namespace Tshirt.Service.Interfaces
{
    public interface IDatabase
    {
        Task<List<TshirtProperties>> GetItemsAsync();
        Task<List<TshirtProperties>> GetItemsNotDoneAsync();
        Task<TshirtProperties> GetItemAsync(int id);
        Task<int> SaveItemAsync(TshirtProperties item);
        Task<int> DeleteItemAsync(TshirtProperties item);
        Task<List<TshirtProperties>> GetUnSubmittedOrders();


    }
}
