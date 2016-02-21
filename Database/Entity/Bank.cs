using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entity
{
    public class Bank
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LocalLink { get; set; }
        public string Site { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Rank { get; set; }

        public List<Department> Departments { get; set; }
        public List<Review> Reviews { get; set; } 
    }
}
