using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ph_Bo_Interfaces
{
    public interface IContact:IHuman
    {
        /// <summary>
        /// Represents an Contact with details including Human information.  
        /// </summary>
        public string PhoneNumber { set; get; }
    }
}
