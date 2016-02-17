using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Database.Entity;

namespace Database.Parser
{
    public class PLatLng
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }


        public PLatLng(double lat, double lng)
        {
            Latitude = lat;
            Longitude = lng;
        }
    }


    public class Convertation
    {
        private static PLatLng GeoCoding(string address, string apiKey)
        {
           
            //Запрос к API геокодирования Google.
            var url = string.Format(
                "https://maps.googleapis.com/maps/api/geocode/xml?address={0}&language=ru&key={1}",
                Uri.EscapeDataString(address), apiKey);

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

            if (xmldoc.GetElementsByTagName("status")[0].ChildNodes[0].InnerText != "OK")
                return new PLatLng(latitude, longitude);
            //Получение широты и долготы.
            var nodes = xmldoc.SelectNodes("//location");

            //Получаем широту и долготу.
            foreach (XmlNode node in nodes)
            {
                var selectSingleNode = node.SelectSingleNode("lat");
                if (selectSingleNode != null)
                    latitude =
                        System.Xml.XmlConvert.ToDouble(selectSingleNode.InnerText);
                var singleNode = node.SelectSingleNode("lng");
                if (singleNode != null)
                    longitude =                 
                        System.Xml.XmlConvert.ToDouble(singleNode.InnerText);
            }
            return new PLatLng(latitude, longitude);
        }

        async public void ConvertDepartmentsAddress(string apiConfig)
        {
            var xmldocument = new XmlDocument();
            try
            {
                xmldocument.Load(apiConfig);
            }
            catch (Exception)
            {
                
                return;
                
            }
            
            
            var apiKey = xmldocument.SelectSingleNode("config/api[@service='googleGeo']/key").InnerText;

            using (var context = new Context())
            {
                //Code-First defaults: true, DB/ModelFirst - See EDMX Props                      
                var query = from department in context.Departments
                            select department;
                
                var departmentList = query.ToList();  
                foreach (var department in departmentList)
                {
                    PLatLng point;
                    // На каждой итерации обращение к БД!!!!         
                    if (department.Address != null && (department.latitude == 0 || department.longitude == 0))
                    {
                        point = GeoCoding(department.Address, apiKey);
                        if (point != null)
                        {
                            department.latitude = point.Latitude;
                            department.longitude = point.Longitude;

                            await context.SaveChangesAsync();
                        }

                    }

                }
                
            }

        }
    }
}
