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
        /// <summary>
        /// For database connections
        /// </summary>

        public bool OpenConnection();
        public bool CloseConnection();

        /// <summary>
        /// Making CRUD
        /// </summary>

        public bool AddRow<T>();
        public bool UpdateRow<T>();
        public bool DeleteRow<T>(T entity);

        /// <summary>
        /// Getting information by making a bet
        /// </summary>

        public bool GetRow<T>(int id);
        public bool GetRow<T>(string name);
        public Contact GetAllRows<T>();


    }
}
