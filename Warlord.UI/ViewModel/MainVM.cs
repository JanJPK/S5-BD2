using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac.Features.Indexed;
using Prism.Commands;
using Prism.Events;
using Warlord.UI.Event;
using Warlord.UI.Service;
using Warlord.UI.Service.Message;
using Warlord.UI.ViewModel.Detail;
using Warlord.UI.ViewModel.Navigation;

namespace Warlord.UI.ViewModel
{
    public class MainVM : BaseVM
    {
        #region Fields

        private readonly IIndex<string, IDetailVM> detailVMCreator;

        private readonly IEventAggregator eventAggregator;
        private readonly IMessageService messageService;

        private int nextNewItemId;
        private IDetailVM selectedDetailViewModel;

        #endregion

        #region Constructors and Destructors

        public MainVM(INavigationVM navigationVM,
            IIndex<string, IDetailVM> detailVMCreator,
            IEventAggregator eventAggregator,
            IMessageService messageService)
        {
            this.messageService = messageService;
            this.detailVMCreator = detailVMCreator;

            DetailVMs = new ObservableCollection<IDetailVM>();

            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<AfterDetailOpenedEvent>().Subscribe(AfterDetailOpened);
            this.eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
            this.eventAggregator.GetEvent<AfterDetailClosedEvent>().Subscribe(AfterDetailClosed);

            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);
            OpenSingleDetailViewCommand = new DelegateCommand<Type>(OnOpenSingleDetailViewExecute);
            NavigationVM = navigationVM;
        }

        #endregion

        #region Public Properties

        public ICommand CreateNewDetailCommand { get; }

        public ObservableCollection<IDetailVM> DetailVMs { get; }

        public INavigationVM NavigationVM { get; }

        public ICommand OpenSingleDetailViewCommand { get; }

        public IDetailVM SelectedDetailViewModel
        {
            get => selectedDetailViewModel;
            set
            {
                selectedDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Public Methods and Operators

        public async Task LoadAsync()
        {
            await NavigationVM.LoadAsync();
        }

        #endregion

        #region Detail VM

        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            //OnOpenDetailView(new OpenDetailViewEventArgs { ViewModelName = nameof(FriendDetailViewModel)});
            // More versatile approach:
            AfterDetailOpened(new AfterDetailOpenedEventArgs
            {
                Id = nextNewItemId--,
                ViewModelName = viewModelType.Name
            });
        }



        private void OnOpenSingleDetailViewExecute(Type viewModelType)
        {
            AfterDetailOpened(new AfterDetailOpenedEventArgs
            {
                Id = -1,
                ViewModelName = viewModelType.Name
            });
        }

        private void RemoveDetailViewModel(int id, string viewModelName)
        {
            var detailViewModel = DetailViewModels
                .SingleOrDefault(vm => vm.Id == id
                                       && vm.GetType().Name == viewModelName);
            if (detailViewModel != null)
            {
                DetailViewModels.Remove(detailViewModel);
            }
        }

        #endregion

        #region Event subscriptions

        private void AfterDetailClosed(AfterDetailClosedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        private async void AfterDetailOpened(AfterDetailOpenedEventArgs args)
        {
            var detailViewModel = DetailViewModels
                .SingleOrDefault(vm => vm.Id == args.Id
                                       && vm.GetType().Name == args.ViewModelName);

            if (detailViewModel == null)
            {
                detailViewModel = detailViewModelCreator[args.ViewModelName];
                // Checking if its not deleted by other user.
                try
                {
                    await detailViewModel.LoadAsync(args.Id);
                }
                catch
                {
                    messageService.ShowInfoDialogAsync("Could not load the entity.");
                    await NavigationViewModel.LoadAsync();
                    return;
                }

                DetailViewModels.Add(detailViewModel);
            }

            SelectedDetailViewModel = detailViewModel;
        }
        #endregion
    }
}
