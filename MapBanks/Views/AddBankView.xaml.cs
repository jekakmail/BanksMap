namespace MapBanks.Views
{
    using Catel.Windows;
    using ViewModels;

    public partial class AddBankView
    {
        public AddBankView()
            : this(null)
        { }

        public AddBankView(AddBankViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
