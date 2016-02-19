using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BanksMap.Markers;
using Database.Entity;
using Database.Parser;
using GMap.NET;
using GMap.NET.WindowsPresentation;

namespace BanksMap
{
    public class MainViewModel
    {
        private ParserMyFin _p;
        private Bank _selectedBank;
        private ObservableCollection<Department> _lstDepartments;

        public ObservableCollection<Bank> LstBanks { get; set; }

        public ObservableCollection<Department> LstDepartments
        {
            get { return _lstDepartments; }
            set { _lstDepartments = value; }
        }

        

        public ObservableCollection<string> LstCurrencies { get; set; }

        public Bank SelectedBank
        {
            get { return _selectedBank; }
            set
            {
                _selectedBank = value;
                LstDepartments.Clear();

                foreach (var item in Database.Func.GetDepartments(value))
                {
                    LstDepartments.Add(item);
                }
                

            }
        }

        public Department SelectedDepartment { get; set; }
        public string SelectedCurrency { get; set; }

        public ICommand BankSelectedCommand { get; set; }

        public MainViewModel()
        {
            BankSelectedCommand = new RelayCommand(arg => cbBank_SelectedItem());
            

            LstBanks = Database.Func.GetBanks();
            LstCurrencies = Database.Func.GetCurrenciesNames();
            LstDepartments = Database.Func.GetDepartments(LstBanks.First());
            //_p = new ParserMyFin();
            Convertation conv = new Convertation();
            conv.ConvertDepartmentsAddress("GoogleAPI.xml");

            LstDepartments.CollectionChanged += (sender, args) =>
            {
                ObservableCollection<GMapMarker> lstMarkers = new ObservableCollection<GMapMarker>();
                foreach (var depart in LstDepartments)
                {
                    GMapMarker point = new GMapMarker(new PointLatLng(depart.latitude, depart.longitude));

                    point.Shape = new DepartmentMarker(point, depart.Name);
                    //point.Offset = new Point(-16, -32);
                    point.ZIndex = int.MaxValue;

                    //GMapCtrl.Markers.Add(point);
                    lstMarkers.Add(point);
                }
                (Application.Current.MainWindow as MainView).GMapCtrl.LstMarkers = lstMarkers;
            };
        }

        public void cbBank_SelectedItem()
        {
            
            
          
        }

        //public void gMapCtrl_LoadPickers(ObservableCollection<Department> lstDepartments)
        //{
        //    GMapCtrl.Markers.Clear();

        //    foreach (var depart in lstDepartments)
        //    {
        //        GMapMarker point = new GMapMarker(new PointLatLng(depart.latitude, depart.longitude));

        //        point.Shape = new DepartmentMarker(point, depart.Name);
        //        //point.Offset = new Point(-16, -32);
        //        point.ZIndex = int.MaxValue;

        //        GMapCtrl.Markers.Add(point);

        //    }
        //}
        
    }
}
