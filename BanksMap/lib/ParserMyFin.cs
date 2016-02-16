using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BanksMap.Entity;
using HtmlAgilityPack;

namespace BanksMap.lib
{
    public class ParserMyFin
    {
        const string ParseSite = @"http://myfin.by";
        const string ParseLink = ParseSite + @"/currency/minsk";
        Context db = new Context();
        List<Bank> lstBanks = new List<Bank>();

        private List<string> lstCity = new List<string>()
        {
            ParseSite + @"/currency/minsk",
            ParseSite + @"/currency/brest",
            ParseSite + @"/currency/vitebsk",
            ParseSite + @"/currency/gomel",
            ParseSite + @"/currency/grodno",
            ParseSite + @"/currency/mogilev"
        }; 

        public ParserMyFin()
        {
            //ParseCity();
        }

        public void ParseCity()
        {
            foreach (var city in lstCity)
            {
                Parse(city);
            }
            db.Banks.AddRange(lstBanks);
            db.SaveChanges();
        }

        private void ParseBanks()
        { }

        private void ParseDepartments()
        { }


        private void Parse(string city)
        {
            
            var webGet = new HtmlWeb();
            var document = webGet.Load(city);
            

            //Получаем таблицу содержащую все банки и их филиалы
            var banksNodes = document.DocumentNode.SelectNodes("//*[@class='table-body']/tr");
            if (banksNodes == null)
                return;
            
            foreach (var bankItem in banksNodes)
            {

                var bank = new Bank();
                try
                {
                    //Получаем название банка и ссылку
                    bank.Name = bankItem.SelectSingleNode("td[1]/span/a").InnerText;
                    bank.LocalLink = string.Format("{0}{1}", ParseSite,
                        bankItem.SelectSingleNode("td[1]/span/a").GetAttributeValue("href", null));
                    if (!string.IsNullOrEmpty(bank.Name))
                        lstBanks.Add(bank);
                }
                catch (Exception)
                {
                    //Получаем таблицу отделений банка
                    var departmentNodes = bankItem.SelectNodes("td/div/div/div[2]/table/tbody/tr");
                    var lstDepartments = new List<Department>();

                    if (departmentNodes == null)
                        continue;

                    foreach (var departmentItem in departmentNodes)
                    {
                        var department = new Department();
                        var lstCurrencies = new List<Currency>();
                        try
                        {
                            //Получаем название отделения, ссылку, адрес и телефон
                            department.Name =
                                departmentItem.SelectSingleNode("td/div[@class='ttl']/a").InnerText;
                            department.LocalLink = string.Format("{0}{1}", ParseSite,
                                departmentItem.SelectSingleNode("td/div[@class='ttl']/a")
                                    .GetAttributeValue("href", null));
                            department.Phone =
                                departmentItem.SelectSingleNode("td/div[@class='tel']").InnerText;
                            department.Address =
                                departmentItem.SelectSingleNode("td/div[@class='address']/a").InnerText;

                            //Получаем курсы валют по отделению
                             lstCurrencies.Add(new Currency()
                            {
                                Name = departmentItem.SelectSingleNode("td[3]/i").GetAttributeValue("data-c", null),
                                Purchase = Convert.ToDecimal(departmentItem.SelectSingleNode("td[3]").InnerText),
                                Sale = Convert.ToDecimal(departmentItem.SelectSingleNode("td[4]").InnerText)
                            });
                             lstCurrencies.Add(new Currency()
                            {
                                Name = departmentItem.SelectSingleNode("td[5]/i").GetAttributeValue("data-c", null),
                                Purchase = Convert.ToDecimal(departmentItem.SelectSingleNode("td[5]").InnerText),
                                Sale = Convert.ToDecimal(departmentItem.SelectSingleNode("td[6]").InnerText)
                            });
                             lstCurrencies.Add(new Currency()
                            {
                                Name = departmentItem.SelectSingleNode("td[7]/i").GetAttributeValue("data-c", null),
                                Purchase = Convert.ToDecimal(departmentItem.SelectSingleNode("td[7]").InnerText),
                                Sale = Convert.ToDecimal(departmentItem.SelectSingleNode("td[8]").InnerText)
                            });

                            department.Currencies = lstCurrencies;

                            if (!string.IsNullOrEmpty(department.Name))
                            {
                                lstDepartments.Add(department);
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    //Привязываем коллекцию отделений к банку.
                    lstBanks[lstBanks.Count - 1].Departments = lstDepartments;

                }
            }
            
        }
    }
}
