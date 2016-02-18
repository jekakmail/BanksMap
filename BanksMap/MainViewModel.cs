using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BanksMap.Markers;
using Database.Entity;
using Database.Parser;
using GMap.NET.WindowsPresentation;

namespace BanksMap
{
    public class MainViewModel
    {
        private ParserMyFin _p;

        public ObservableCollection<Bank> LstBanks { get; set; }
        public ObservableCollection<string> LstCurrencies { get; set; }

        public ICommand GMapControlClick { get; set; }

        public MainViewModel()
        {
            GMapControlClick = new RelayCommand(arg => GMapControl_MouseClick());
            
            LstBanks = Database.Func.GetBanks();
            LstCurrencies = Database.Func.GetCurrenciesNames();

            //_p = new ParserMyFin();
            Convertation conv = new Convertation();
            conv.ConvertDepartmentsAddress("GoogleAPI.xml");
        }

        public void GMapControl_MouseClick()
        {
            
        }
    }
}
