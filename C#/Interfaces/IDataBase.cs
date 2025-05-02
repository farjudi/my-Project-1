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
        public bool CreateTable();

        /// <summary>
        /// Making CRUD
        /// </summary>


        public T GetElementById<T>(int id) where T : IHuman;
        public List<T> GetElementByName<T>(string name) where T : IHuman;
        //For Contact 
        public bool AddRowContact(Contact contact);
        public bool UpdateRowContact(int id);
        public bool DeleteRowContact(int  id);



        //FOR OWNER 
        public bool AddRowOwner(Owner owner);
        public bool UpdateRowOwner(int id );
        public bool DeleteRowOwner(int  id);

    
      


    }
}
