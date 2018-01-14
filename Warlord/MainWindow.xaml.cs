using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Warlord.ViewModel;

namespace Warlord
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
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
            UIScaleSlider.MouseDoubleClick += new MouseButtonEventHandler(RestoreScalingFactor);
        }

        #endregion

        #region Methods

        //private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    //await viewModel.LoadAsync();
        //}

        void RestoreScalingFactor(object sender, MouseButtonEventArgs args)
        {
            ((Slider) sender).Value = 1.0;
        }

        #endregion
    }
}