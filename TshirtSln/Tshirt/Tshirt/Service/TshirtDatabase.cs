using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Tshirt.Models;
using Tshirt.Service.Interfaces;

namespace Tshirt.Service
{
    public class TshirtDatabase : IDatabase
    {
        readonly SQLiteAsyncConnection database;

        public TshirtDatabase()
        {
            string dbPath = GetDbPath();
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<TshirtProperties>().Wait();
        }

        private string GetDbPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "products.db3");
        }

        public Task<List<TshirtProperties>> GetItemsAsync()
        {
            return database.Table<TshirtProperties>().ToListAsync();
        }

        public Task<List<TshirtProperties>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<TshirtProperties>("SELECT * FROM [Tshirt] WHERE [Done] = 0");
        }

        public Task<TshirtProperties> GetItemAsync(int id)
        {
            return database.Table<TshirtProperties>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(TshirtProperties item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(TshirtProperties item)
        {
            return database.DeleteAsync(item);
        }

        public Task<List<TshirtProperties>> GetUnSubmittedOrders()
        {
            return database.Table<TshirtProperties>().Where(x => x.Posted == false).ToListAsync();
        }
    }
}

