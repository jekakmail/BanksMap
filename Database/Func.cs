using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Entity;

namespace Database
{
    public class Func
    {
        public static ObservableCollection<Bank> GetBanks()
        {
            var lstBanks = new ObservableCollection<Bank>();
            using (var context = new Context())
            {
                context.Banks.Load();
                var query = from bank in context.Banks
                    //where bank.Departments != null
                    select bank;

                var lst = query.ToList();
                foreach (var item in lst)
                {
                    lstBanks.Add(item);
                }
            }
            return lstBanks;
        }

        public static ObservableCollection<Department> GetDepartments(Bank b)
        {
            var lstDepartments = new ObservableCollection<Department>();
            using (var context = new Context())
            {
                context.Departments.Load();
                var query = from department in context.Departments
                    where department.Bank.Id == b.Id
                    select department;
                var lst = query.ToList();

                foreach (var item in lst)
                {
                    lstDepartments.Add(item);
                }
                return lstDepartments;
            }
        } 
    }
}
