using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Catel;
using Catel.Collections;
using Catel.Data;
using Catel.Services;
using Catel.Threading;
using Database.Entity;
using Database.Parser;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using MapBanks.Markers;
using MapBanks.Views;
using Department = Database.Entity.Department;

namespace MapBanks.ViewModels
{
    using Catel.MVVM;
    using System.Threading.Tasks;

    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _visualizerService;
        private readonly IMessageService _messageService;

        public Command AddBankCommand { get; private set; }

        public MainWindowViewModel(IUIVisualizerService visualizerService, IMessageService messageService)
        {
            _visualizerService = visualizerService;
            _messageService = messageService;

            AddBankCommand = new Command(OnAddBankCommandExecute);
        }

        private void OnAddBankCommandExecute()
        {
            var dlg = new AddBankViewModel(LstBanks);

            if (_visualizerService.ShowDialog(dlg) == true)
            {
                _messageService.ShowInformationAsync("Банк успешно добавлен!");
                LstBanks = Database.Func.GetBanks();
            }
            else
            {
                if (dlg.Result == false)
                    _messageService.ShowWarningAsync("Что то пошло не так...  :(", "Внимание");
            }
        }
        
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
        public static readonly PropertyData SelectedBankProperty = RegisterProperty("SelectedBank", typeof(Bank));

        
        public ObservableCollection<Bank> LstBanks
        {
            get { return GetValue<ObservableCollection<Bank>>(LstBanksProperty); }
            set { SetValue(LstBanksProperty, value); }
        }
        public static readonly PropertyData LstBanksProperty = RegisterProperty("LstBanks", typeof(ObservableCollection<Bank>));


        public ObservableCollection<Department> LstDepartments
        {
            get { return GetValue<ObservableCollection<Department>>(LstDepartmentsProperty); }
            set { SetValue(LstDepartmentsProperty, value); }
        }
        public static readonly PropertyData LstDepartmentsProperty = RegisterProperty("LstDepartments", typeof(ObservableCollection<Department>));

        
        public ObservableCollection<string> CureenciesList
        {
            get { return GetValue<ObservableCollection<string>>(CureenciesListProperty); }
            set { SetValue(CureenciesListProperty, value); }
        }
        public static readonly PropertyData CureenciesListProperty = RegisterProperty("CureenciesList", typeof(ObservableCollection<string>));

        
        public string SelectedCurrency
        {
            get { return GetValue<string>(SelectedCurrencyProperty); }
            set { SetValue(SelectedCurrencyProperty, value); }
        }
        public static readonly PropertyData SelectedCurrencyProperty = RegisterProperty("SelectedCurrency", typeof(string));

        private ParserMyFin _p;

        public static ObservableCollection<GMapMarker> LstMarkers = new ObservableCollection<GMapMarker>();


        private async void InitializeMarkersAsync()
        {
            await InitializeMarkers;
        }

        private Task InitializeMarkers
        {
            get
            {
                if (LstMarkers != null && LstMarkers.Count > 0)
                {
                    //GMapCtrl.Markers.Clear();
                    LstMarkers.Clear();
                }
                foreach (var depart in LstDepartments)
                    {
                        GMapMarker point = new GMapMarker(new PointLatLng(depart.latitude, depart.longitude));

                        point.Shape = new DepartmentMarker(point, depart.Name);
                        //point.Offset = new Point(-16, -32);
                        point.ZIndex = int.MaxValue;

                        if (LstMarkers != null) LstMarkers.Add(point);
                    }
                
                return CloseAsync();
            }
        }

        public override string Title { get { return "MapBanks"; } }

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets
        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            // TODO: subscribe to events here
            //
            //!!!!Раскомментировать для парсинга базы!!!!!
            //
            //_p = new ParserMyFin();


            LstBanks = Database.Func.GetBanks();
            if (LstBanks.Count > 0)
            {
                SelectedBank = LstBanks.First();
                //LstDepartments = Database.Func.GetDepartments(LstBanks.First());
            }
            
            CureenciesList = Database.Func.GetCurrenciesNames();
            
            if (CureenciesList.Count > 0)
                SelectedCurrency = CureenciesList.First();
            
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here
            
            await base.CloseAsync();
        }
    }
}
