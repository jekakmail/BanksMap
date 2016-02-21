using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entity
{
    public class Review
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }
        public int Rate { get; set; }
        public string review { get; set; }
        public Bank Bank { get; set; }
    }
}
