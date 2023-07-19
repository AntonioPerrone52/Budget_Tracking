using Budget.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private async Task Initialize_DB()
        {
            await _connection.CreateTableAsync<budget_fields>();
        }

        public async Task<int> AggiungiBugetItem(budget_fields item)
        {
            return await _connection.InsertAsync(item);
        }
        public async Task<List<budget_fields>> LeggiBugetItem()
        {
            return await _connection.Table<budget_fields>().ToListAsync();
        }
        public async Task<List<budget_fields>> Leggi_by_name(string s)
        {
            return await _connection.Table<budget_fields>().Where(t => t.name.Contains(s)).ToListAsync();
        }
        public async Task<List<budget_fields>> Leggi_by_type(string s)
        {
            return await _connection.Table<budget_fields>().Where(t => t.type.Contains(s)).ToListAsync();
        }
        public async Task<List<budget_fields>> Leggi_by_id(int s)
        {
            return await _connection.Table<budget_fields>().Where(t => t.id.Equals(s)).ToListAsync();
        }
        public async Task<int> rimuovi_tupla(int id)
        {
            return await _connection.Table<budget_fields>().Where(t => t.id.Equals(id)).DeleteAsync();
        }

    }
}
