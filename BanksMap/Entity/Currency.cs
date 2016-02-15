using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanksMap.Entity
{
    public class Currency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Purchase { get; set; }
        public decimal Sale { get; set; }
        public Department Department { get; set; }
    }
}
