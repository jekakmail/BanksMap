using System.Collections.ObjectModel;
using System.Windows;
using Catel.Data;
using Database.Entity;
using MapBanks.Views;

namespace MapBanks.ViewModels
{
    using Catel.MVVM;
    using System.Threading.Tasks;

    public class AddBankViewModel : ViewModelBase
    {
        
        public Bank SelectedBank
        {
            get { return GetValue<Bank>(SelectedBankProperty); }
            set { SetValue(SelectedBankProperty, value); }
        }
        public static readonly PropertyData SelectedBankProperty = RegisterProperty("SelectedBank", typeof(Bank), null);

        public bool? Result
        {
            get { return GetValue<bool?>(resultProperty); }
            set { SetValue(resultProperty, value); }
        }
        public static readonly PropertyData resultProperty = RegisterProperty("Result", typeof(bool?),null);

        public ObservableCollection<Bank> LstBanks
        {
            get { return GetValue<ObservableCollection<Bank>>(LstBanksProperty); }
            set { SetValue(LstBanksProperty, value); }
        }
        public static readonly PropertyData LstBanksProperty = RegisterProperty("LstBanks", typeof(ObservableCollection<Bank>));

        
        public string bankName
        {
            get { return GetValue<string>(bankNameProperty); }
            set { SetValue(bankNameProperty, value); }
        }
        public static readonly PropertyData bankNameProperty = RegisterProperty("bankName", typeof(string));

        
        public string bankSite
        {
            get { return GetValue<string>(bankSiteProperty); }
            set { SetValue(bankSiteProperty, value); }
        }
        public static readonly PropertyData bankSiteProperty = RegisterProperty("bankSite", typeof(string));

        
        public string bankPhones
        {
            get { return GetValue<string>(bankPhonesProperty); }
            set { SetValue(bankPhonesProperty, value); }
        }
        public static readonly PropertyData bankPhonesProperty = RegisterProperty("bankPhones", typeof(string));

        
        public string bankAddress
        {
            get { return GetValue<string>(bankAddressProperty); }
            set { SetValue(bankAddressProperty, value); }
        }
        public static readonly PropertyData bankAddressProperty = RegisterProperty("bankAddress", typeof(string));

        public AddBankViewModel(ObservableCollection<Bank> lstBanks)
        {
            LstBanks = lstBanks;
            OkCommand = new Command(OnOkCommandExecute);
        }

        /// <summary>
            /// Gets the name command.
            /// </summary>
        public Command OkCommand { get; private set; }
        
        private void OnOkCommandExecute()
        {
            Result = true;
        }

        public override string Title { get { return "Добавление банка"; } }

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
            if (Result == true)
                Result = Database.Func.AddBank(bankName, bankSite, bankAddress, bankPhones);
            else
                Result = null;
            
            
            await base.CloseAsync();
        }
    }
}
