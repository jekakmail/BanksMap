using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanksMap.Entity
{
    public class Bank
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public List<Department> Departments { get; set; }

        //public Bank()
        //{
        //    Departments = new List<Department>();
        //}
    }
}
