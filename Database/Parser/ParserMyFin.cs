using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using Database.Entity;
using HtmlAgilityPack;

namespace Database.Parser
{
    public class ParserMyFin
    {
        const string ParseSite = @"http://myfin.by";
        const string ParseLink = ParseSite + @"/currency/minsk";
        private Context db;
        private List<Bank> lstBanks;

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
            db = new Context();
            lstBanks = new List<Bank>();
            ParseCity();

            db.Banks.AddRange(lstBanks);
            db.SaveChanges();
            ParseBanks();
            //ParseReviews();
            ParseGeo();

            //db.Banks.Load();
        }

        public void ParseCity()
        {
            foreach (var city in lstCity)
            {
                Parse(city);
            }
        }

        public void ParseReviews()
        {
            foreach (var bank in lstBanks)
            {
                var wG = new HtmlWeb();
                var doc = wG.Load(string.Format("{0}{1}", bank.LocalLink, "otzyvy"));

                var reviews = new List<Review>();

                var reviewsNodes = doc.DocumentNode.SelectNodes("//*[@id='workarea']/div[3]/div/div");

                foreach (var node in reviewsNodes)
                {
                    var review = new Review()
                    {
                        Name = node.SelectSingleNode("div/div/div/div").InnerText,
                        Date = node.SelectSingleNode("div/div/div/time").InnerText,
                        Rate =
                            Convert.ToInt32(
                                Regex.Match(
                                    node.SelectSingleNode("div/div[2]/div/div/div/div")
                                        .GetAttributeValue("mainrating", null), @"\d+")),
                        review = node.SelectSingleNode("div/div[2]/div").InnerText
                    };
                    reviews.Add(review);

                }

                bank.Reviews = reviews;
                db.SaveChangesAsync();
            }
        }

        private void ParseBanks()
        {
            foreach (var bank in lstBanks)
            {
                var wG = new HtmlWeb();
                var doc = wG.Load(bank.LocalLink);


                bank.Phone =
                    doc.DocumentNode.SelectSingleNode("//*[@id='workarea']/div[2]/div/div[1]/div/div/div[1]/div")
                        .InnerText;
                 string str =
                    doc.DocumentNode.SelectSingleNode("//*[@id='workarea']/div[2]/div/div[1]/div/div/div[2]/div")
                        .InnerText;
                bank.Address = Regex.Replace(str, " +", " ");

                bank.Site =
                    doc.DocumentNode.SelectSingleNode("//*[@id='workarea']/div[2]/div/div[1]/div/div/div[3]/a[2]")
                        .InnerText;

                bank.Rank =
                    doc.DocumentNode.SelectSingleNode(
                        "//*[@id='workarea']/div[2]/div/div[1]/div/div/div[3]/div/div/span").InnerText;

                db.SaveChangesAsync();
            }

        }

        private void ParseDepartments()
        { }

        private void ParseGeo()
        {
            foreach (var bank in lstBanks)
            {
                if(bank.Departments != null && bank.Departments.Count > 0)
                {
                    foreach (var depart in bank.Departments)
                    {
                        var wG = new HtmlWeb();
                        var doc = wG.Load(depart.LocalLink);

                        var Node = doc.DocumentNode.SelectSingleNode("//*[@id='workarea']/div[1]/div[2]/script").InnerText;

                        var match = Regex.Match(Node, @"(\d{2}.\d*)");
                        int idx = 0;
                        double lat = 0, lon = 0;
                        while (match.Success)
                        {
                            switch (idx)
                            {
                                case 0:
                                    if (double.TryParse(match.Value.Replace(".", ","), out lat))
                                        idx++;
                                    break;
                                case 1:
                                    if (double.TryParse(match.Value.Replace(".", ","), out lon))
                                        idx++;
                                    break;
                            }
                            match = match.NextMatch();
                        }
                        depart.latitude = lat;
                        depart.longitude = lon;

                        db.SaveChangesAsync();
                    }

                }
                
            }
        }

        private void Parse(string city)
        {
            
            var webGet = new HtmlWeb();
            var document = webGet.Load(city);
            
            
            //Получаем таблицу содержащую все банки и их филиалы
            var banksNodes = document.DocumentNode.SelectNodes("//*[@class='table-body']/tr");
            if (banksNodes == null)
                return;
            Bank bank = new Bank();
            foreach (var bankItem in banksNodes)
            {
                try
                {
                    //Получаем название банка и ссылку
                    var bName = bankItem.SelectSingleNode("td[1]/span/a").InnerText;
                    var bLocalLink = string.Format("{0}{1}", ParseSite,
                        bankItem.SelectSingleNode("td[1]/span/a").GetAttributeValue("href", null));
                    

                    //Такой банк уже добавлен в коллекцию?
                    IEnumerable<Bank> indexBank = null;
                    if (lstBanks.Count != 0)
                    {
                        indexBank = lstBanks.Where(p => p.Name == bName || p.LocalLink == bLocalLink);

                    }
                    if (indexBank !=null && indexBank.Count() != 0 && !string.IsNullOrEmpty(indexBank.First().Name))
                    {
                        int idx = lstBanks.FindIndex(p => p == indexBank.First());
                        bank = lstBanks[idx];
                    }
                    else if (!string.IsNullOrEmpty(bName))
                    {
                        bank = new Bank()
                        {
                            Name = bName,
                            LocalLink = bLocalLink
                        };
                        lstBanks.Add(bank);
                    }
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
                                Purchase = departmentItem.SelectSingleNode("td[3]").InnerText,
                                Sale = departmentItem.SelectSingleNode("td[4]").InnerText
                            });
                             lstCurrencies.Add(new Currency()
                            {
                                Name = departmentItem.SelectSingleNode("td[5]/i").GetAttributeValue("data-c", null),
                                Purchase = departmentItem.SelectSingleNode("td[5]").InnerText,
                                Sale = departmentItem.SelectSingleNode("td[6]").InnerText
                            });
                             lstCurrencies.Add(new Currency()
                            {
                                Name = departmentItem.SelectSingleNode("td[7]/i").GetAttributeValue("data-c", null),
                                Purchase = departmentItem.SelectSingleNode("td[7]").InnerText,
                                Sale = departmentItem.SelectSingleNode("td[8]").InnerText
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
                    //lstBanks[lstBanks.Count - 1].Departments = lstDepartments;
                    if (!string.IsNullOrEmpty(bank.Name) && lstDepartments.Count > 0)
                    {
                        var tBank = lstBanks.Find(p => p == bank);
                        if(tBank.Departments!=null)
                            tBank.Departments.AddRange(lstDepartments);
                        else
                        {
                            tBank.Departments = lstDepartments;
                        }
                    }
                    
                }
            }
            
        }
    }
}
