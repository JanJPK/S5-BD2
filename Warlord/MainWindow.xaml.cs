using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Warlord.ViewModel;

namespace Warlord
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
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
