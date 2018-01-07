using System.Globalization;
using System.Threading;
using System.Windows;
using Warlord.UI.ViewModel;

namespace Warlord.UI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        private readonly MainVM viewModel;

        #endregion

        #region Constructors and Destructors

        public MainWindow(MainVM viewModel)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            InitializeComponent();
            this.viewModel = viewModel;
            DataContext = this.viewModel;
            //Loaded += MainWindow_Loaded;
        }

        #endregion

        #region Methods

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //await viewModel.LoadAsync();
        }

        #endregion
    }
}