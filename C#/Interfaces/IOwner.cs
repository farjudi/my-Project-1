using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ph_Bo_Interfaces
{
    public interface IOwner:IHuman
    {
        /// <summary>
        /// Represents an owner with details including Human information.  
        /// </summary>
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

    }
}
