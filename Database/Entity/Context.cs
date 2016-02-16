using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entity
{
    public class Context : DbContext
    {
        public Context() : base("ConnectionStr")
        { }

        public Context(string connectionStr) : base(connectionStr)
        { }

        public DbSet<Bank> Banks { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Department> Departments { get; set; }
        
    }
}
