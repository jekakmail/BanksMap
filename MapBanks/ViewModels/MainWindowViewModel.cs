using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Catel;
using Catel.Collections;
using Catel.Data;
using Catel.Threading;
using Database.Entity;
using Database.Parser;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using MapBanks.Markers;

namespace MapBanks.ViewModels
{
    using Catel.MVVM;
    using System.Threading.Tasks;

    public class MainWindowViewModel : ViewModelBase
    {
        
        
        public Bank SelectedBank
        {
            get { return GetValue<Bank>(SelectedBankProperty); }
            set
            {
                SetValue(SelectedBankProperty, value);
                LstDepartments = Database.Func.GetDepartments(value);
                InitializeMarkersAsync();
            }
        }
        public static readonly PropertyData SelectedBankProperty = RegisterProperty("SelectedBank", typeof(Bank), null);

        
        public ObservableCollection<Bank> LstBanks
        {
            get { return GetValue<ObservableCollection<Bank>>(LstBanksProperty); }
            set { SetValue(LstBanksProperty, value); }
        }
        public static readonly PropertyData LstBanksProperty = RegisterProperty("LstBanks", typeof(ObservableCollection<Bank>), null);


        public ObservableCollection<Department> LstDepartments
        {
            get { return GetValue<ObservableCollection<Department>>(LstDepartmentsProperty); }
            set { SetValue(LstDepartmentsProperty, value); }
        }
        public static readonly PropertyData LstDepartmentsProperty = RegisterProperty("LstDepartments", typeof(ObservableCollection<Department>), null);

        
        public ObservableCollection<string> CureenciesList
        {
            get { return GetValue<ObservableCollection<string>>(CureenciesListProperty); }
            set { SetValue(CureenciesListProperty, value); }
        }
        public static readonly PropertyData CureenciesListProperty = RegisterProperty("CureenciesList", typeof(ObservableCollection<string>), null);

        
        public string SelectedCurrency
        {
            get { return GetValue<string>(SelectedCurrencyProperty); }
            set { SetValue(SelectedCurrencyProperty, value); }
        }
        public static readonly PropertyData SelectedCurrencyProperty = RegisterProperty("SelectedCurrency", typeof(string), null);

        private ParserMyFin _p;

        public ObservableCollection<GMapMarker> LstMarkers
        {
            get { return GetValue<ObservableCollection<GMapMarker>>(LstMarkersProperty); }
            set { SetValue(LstMarkersProperty, value); }
        }
        public static readonly PropertyData LstMarkersProperty = RegisterProperty("LstMarkers", typeof(ObservableCollection<GMapMarker>), null);

        public MainWindowViewModel()
        {
            //_p = new ParserMyFin();
            LstBanks = Database.Func.GetBanks();
            SelectedBank = LstBanks.First();
            CureenciesList = Database.Func.GetCurrenciesNames();
            LstDepartments = Database.Func.GetDepartments(LstBanks.First());
            LstMarkers = new ObservableCollection<GMapMarker>();
            
        }
        

        protected async Task InitializeMarkersAsync()
        {
            await InitializeMarkers();
        }

        protected Task InitializeMarkers()
        {
            
            //GMapCtrl.Markers.Clear();
            LstMarkers.Clear();

                foreach (var depart in LstDepartments)
                {
                    GMapMarker point = new GMapMarker(new PointLatLng(depart.latitude, depart.longitude));

                    point.Shape = new DepartmentMarker(point, depart.Name);
                    //point.Offset = new Point(-16, -32);
                    point.ZIndex = int.MaxValue;

                    LstMarkers.Add(point);

                }
            return CloseAsync();
        }

        public override string Title { get { return "MapBanks"; } }

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets
        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
