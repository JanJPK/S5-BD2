using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac.Features.Indexed;
using Prism.Commands;
using Prism.Events;
using Warlord.Event;
using Warlord.Service;
using Warlord.Service.Message;
using Warlord.ViewModel.Detail;

namespace Warlord.ViewModel
{
    public class MainVM : BaseVM
    {
        #region Fields

        private readonly IEventAggregator eventAggregator;
        private readonly IMessageService messageService;

        private int nextNewItemId;

        

        #endregion

        #region Constructors and Destructors

        public MainVM(IIndex<string, IDetailVM> detailVMCreator,
            IEventAggregator eventAggregator,
            IMessageService messageService,
            IUserPrivilege userPrivilege)
        {
            this.messageService = messageService;
            this.detailVMCreator = detailVMCreator;
            UserPrivilege = userPrivilege;

            DetailVMs = new ObservableCollection<IDetailVM>();

            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<OnNewDependantDetailOpenedEvent>().Subscribe(OnNewDependantDetailOpened);
            this.eventAggregator.GetEvent<AfterDetailOpenedEvent>().Subscribe(AfterDetailOpened);
            this.eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
            this.eventAggregator.GetEvent<AfterDetailClosedEvent>().Subscribe(AfterDetailClosed);
            this.eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);

            CreateNewDetailViewCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);
            OpenSingleDetailViewCommand = new DelegateCommand<Type>(OnOpenSingleDetailViewExecute);

            CreateVehicleModelBrowseView = new DelegateCommand<Type>(OnCreateNewVehicleModelBrowseView);

            // Debug:
            LogInCommand = new DelegateCommand<Type>(LogIn);
            LogOutCommand = new DelegateCommand<Type>(LogOut);
        }

        #endregion

        #region Public Properties

        public ICommand CreateVehicleModelBrowseView { get; }

        #endregion

        #region Public Methods and Operators

        public Task LoadAsync()
        {
            return null;
        }

        #endregion

        #region Methods

        private void OnCreateNewVehicleModelBrowseView(Type obj)
        {
        }

        #endregion

        #region Detail VM

        #region Fields

        private readonly IIndex<string, IDetailVM> detailVMCreator;

        private IDetailVM selectedDetailVM;

        #endregion

        #region Properties

        public ICommand CreateNewDetailViewCommand { get; }

        public ObservableCollection<IDetailVM> DetailVMs { get; }

        public ICommand OpenSingleDetailViewCommand { get; }

        public IDetailVM SelectedDetailVM
        {
            get => selectedDetailVM;
            set
            {
                selectedDetailVM = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

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
            var detailViewModel = DetailVMs
                .SingleOrDefault(vm => vm.Id == id
                                       && vm.GetType().Name == viewModelName);
            if (detailViewModel != null)
            {
                DetailVMs.Remove(detailViewModel);
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
            var detailViewModel = DetailVMs
                .SingleOrDefault(vm => vm.Id == args.Id
                                       && vm.GetType().Name == args.ViewModelName);

            if (detailViewModel == null)
            {
                detailViewModel = detailVMCreator[args.ViewModelName];
                // Checking if its not deleted by other user.
                try
                {
                    await detailViewModel.LoadAsync(args.Id);
                }
                catch
                {
                    await messageService.ShowInfoDialog("Could not load the entity.");
                    //await SelectedNavigationVM.LoadAsync();
                    return;
                }

                DetailVMs.Add(detailViewModel);
            }

            SelectedDetailVM = detailViewModel;
        }

        private async void OnNewDependantDetailOpened(OnNewDependantDetailOpenedEventArgs args)
        {
            args.Id = nextNewItemId--;
            var detailViewModel = DetailVMs
                .SingleOrDefault(vm => vm.Id == args.Id
                                       && vm.GetType().Name == args.ViewModelName);


            if (detailViewModel == null)
            {
                switch (args.ViewModelName)
                {
                    case nameof(VehicleDetailVM):
                    {
                        detailViewModel = detailVMCreator[args.ViewModelName];

                        ((VehicleDetailVM)detailViewModel).VehicleModelId = args.DependantOnId;
                        await detailViewModel.LoadAsync(args.Id);

                        DetailVMs.Add(detailViewModel);
                        break;
                    }

                    case nameof(OrderDetailVM):
                    {
                        detailViewModel = detailVMCreator[args.ViewModelName];

                        ((OrderDetailVM)detailViewModel).CustomerId = args.DependantOnId;
                        await detailViewModel.LoadAsync(args.Id);

                        DetailVMs.Add(detailViewModel);
                        break;
                    }
                }

            }

            SelectedDetailVM = detailViewModel;
        }

        protected void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            // TODO
            //if (args.ViewModelName == nameof(VehicleDetailVM))
            //{
            //    var detailViewModel = DetailVMs
            //        .SingleOrDefault(vm => vm.Id == args.Id
            //                               && vm.GetType().Name == args.ViewModelName);
            //    detailViewModel?.LoadAsync(args.Id);
            //}
        }

        #endregion

        #endregion

        #region Debug-related

        private IUserPrivilege userPrivilege;
        public IUserPrivilege UserPrivilege
        {
            get => userPrivilege;
            set
            {
                userPrivilege = value;
                OnPropertyChanged();
            }
        }
        public ICommand LogInCommand { get; }
        public ICommand LogOutCommand { get; }

        private void LogIn(Type obj)
        {
            eventAggregator.GetEvent<OnUserLoggedInEvent>().Publish();
        }

        private void LogOut(Type obj)
        {
            eventAggregator.GetEvent<OnUserLoggedOutEvent>().Publish();
        }

        #endregion
    }
}