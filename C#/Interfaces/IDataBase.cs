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
        public bool OpenConnection();
        public bool CloseConnection();
        public bool CreateTable();

        public bool InsertData<T>(T entity) where T : IHuman;
        public T GetElementById<T>(int id) where T : IHuman;
        public List<T> GetElementByName<T>(string name) where T : IHuman;
        public bool DeleteRow(int id);

    }
}
