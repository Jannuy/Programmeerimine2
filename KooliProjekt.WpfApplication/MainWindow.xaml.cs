using System.Windows;

namespace KooliProjekt.WpfApplication
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new MainWindowViewModel();
            DataContext = viewModel;
            Loaded += async (s, e) => await viewModel.LoadData();
        }
    }
}