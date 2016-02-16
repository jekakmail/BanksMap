using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entity
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string LocalLink { get; set; }

        public double latitude { get; set; }
        public double longitude { get; set; }


        public Bank Bank { get; set; } 
        public List<Currency> Currencies { get; set; }
        
    }
    
}
