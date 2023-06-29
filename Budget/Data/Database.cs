using Budget.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Budget.Models.budget_fields;

namespace Budget.Data
{
    internal class Database
    {
        private readonly SQLiteAsyncConnection _connection;

        public Database()
        {
            var dataDir = FileSystem.AppDataDirectory;
            var databasePath = Path.Combine(dataDir, "budget.db");

            var dbstringconnection = new SQLiteConnectionString(databasePath);
            _connection = new SQLiteAsyncConnection(dbstringconnection);

            var response = Initialize_DB(); 
        }
        public async Task Initialize_DB() 
        {
            await _connection.CreateTableAsync<budget_item>();
        }

        public async Task<int> AggiungiBugetItem(budget_item item)
        {
            return await _connection.InsertAsync(item);
        }
        public async Task<List<budget_item>> LeggiBugetItem()
        {
            return await _connection.Table<budget_item>().ToListAsync();
        }


    }
}
