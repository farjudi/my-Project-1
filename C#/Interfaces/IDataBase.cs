using Ph_Bo_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ph_Bo_Interfaces
{
    public interface IDataBase
    {
        public Task<bool> OpenConnectionAsync();
        public Task<bool> CloseConnectionAsync();
        public Task<bool> CreateTableAsync();
        public  Task<T> UpdataDatabaseAsync<T>(T entity) where T : IHuman;
        public Task<bool> InsertDataAsync<T>(T entity) where T : IHuman;
        public Task<T> GetElementByIdAsync<T>(int id) where T : IHuman;
        public Task<List<T>> GetElementByNameAsync<T>(string name) where T : IHuman;
        public Task<bool> DeleteRowAsync(int id);

    }
}
