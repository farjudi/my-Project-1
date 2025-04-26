using Ph_Bo_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ph_Bo_Model
{
    public class Contact : IContact
    {
        /// <summary>
        /// Represents a contact with personal details such as id , full name and phone number.  
        /// </summary>
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
