using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ph_Bo_Interfaces
{
    public interface IHuman
    {/// <summary>
     /// Represents a human with basic personal information.
     /// </summary>
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
