using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Net.Sockets;
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
                //var query = from bank in context.Banks
                //    select bank;

                var query = context.Banks.Select(p => p);

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

        public static ObservableCollection<string> GetCurrenciesNames()
        {
            var lstCurrenciesName = new ObservableCollection<string>();
            using (var context = new Context())
            {
                context.Currencies.Load();
                //var query = from currency in context.Currencies
                //    group currency by currency.Name;

                var query = context.Currencies.GroupBy(currency => currency.Name).Select(grouping => grouping.Key);
                
                foreach (var item in query.ToList())
                {
                    lstCurrenciesName.Add(item);
                }

                return lstCurrenciesName;
            }
        }

        public static Bank GetBank(string bank_name, string bank_localLink)
        {
            using (var context = new Context())
            {
                context.Banks.Load();

                var getBank = context.Banks.Where(p => p.Name == bank_name || p.LocalLink == bank_localLink);
                
                Bank bank = getBank.ToList().First();
                
                return bank;
            }
        }
    }
}
