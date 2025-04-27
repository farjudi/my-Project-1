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
        public bool CreateTableContact();
        public bool CreateTableOwner();
        /// <summary>
        /// Making CRUD
        /// </summary>

        public bool AddRowContact(Contact contact);
        public bool UpdateRowContact(int id);
        public bool DeleteRowContact(int  id);



        //FOR OWNER 
        public bool AddRowOwner(Owner owner);
        public bool UpdateRowOwner(int id );
        public bool DeleteRowOwner(int  id);

        /// <summary>
        /// Getting information by making a bet
        /// </summary>

        public bool GetRowContact(int id);
        public bool GetRowContact(string name);

        //for owner 
        public bool GetRowOwner(int id);
        public bool GetRowOwner(string name);



    }
}
