using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using GMap.NET;

namespace BanksMap.lib
{
    public class Convertation
    {
        private static PointLatLng GeoCoding(string address)
        {
            const string apiKey = "AIzaSyDXpU0jJ0CaihQ3PZwF3J5qjtJfrpY5raY";
            //Запрос к API геокодирования Google.
            var url = string.Format(
                "https://maps.googleapis.com/maps/api/geocode/xml?address={0}&language=ru&key={1}",
                Uri.EscapeDataString(address),apiKey);

            //Выполняем запрос к универсальному коду ресурса (URI).
            var request = (HttpWebRequest)WebRequest.Create(url);

            //Получаем ответ от интернет-ресурса.
            var response = request.GetResponse();

            //Экземпляр класса System.IO.Stream
            //для чтения данных из интернет-ресурса.
            var dataStream = response.GetResponseStream();

            //Инициализируем новый экземпляр класса
            //System.IO.StreamReader для указанного потока.
            var sreader = new StreamReader(dataStream);

            //Считывает поток от текущего положения до конца.           
            var responsereader = sreader.ReadToEnd();

            //Закрываем поток ответа.
            response.Close();

            var xmldoc = new XmlDocument();

            xmldoc.LoadXml(responsereader);

            //Переменные широты и долготы.
            double latitude = 0;
            double longitude = 0;

            if (xmldoc.GetElementsByTagName("status")[0].ChildNodes[0].InnerText == "OK")
            {
                //Получение широты и долготы.
                var nodes = xmldoc.SelectNodes("//location");

                //Получаем широту и долготу.
                foreach (XmlNode node in nodes)
                {
                    latitude =
                       System.Xml.XmlConvert.ToDouble(node.SelectSingleNode("lat").InnerText);
                    longitude =                 
                       System.Xml.XmlConvert.ToDouble(node.SelectSingleNode("lng").InnerText);
                }
            }
            return new PointLatLng(latitude, longitude);
        }

        async public void ConvertDepartmentsAddress()
        {
            using (var context = new Context())
            {
                //Code-First defaults: true, DB/ModelFirst - See EDMX Props                      
                var query = from department in context.Departments
                            select department;
                
                var departmentList = query.ToList();  
                foreach (var department in departmentList)
                {
                    PointLatLng point;
                    // На каждой итерации обращение к БД!!!!         
                    if (department.Address != null)
                    {
                        point = GeoCoding(department.Address);
                        if (point != null)
                        {
                            department.latitude = point.Lat;
                            department.longitude = point.Lng;

                            context.SaveChangesAsync();
                        }

                    }

                }
                
            }

        }
    }
}
