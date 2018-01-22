using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Autofac.Features.Indexed;
using Prism.Commands;
using Prism.Events;
using Warlord.Event;
using Warlord.Service;
using Warlord.Service.Message;
using Warlord.ViewModel.Detail;
using Warlord.ViewModel.Detail.Browse;

namespace Warlord.ViewModel
{
    public class MainVM : BaseVM
    {
        #region Fields

        private readonly IIndex<string, IDetailVM> detailVMCreator;
        private readonly IEventAggregator eventAggregator;
        private readonly IMessageService messageService;
        private int nextNewItemId;
        private IDetailVM selectedDetailVM;

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
            this.eventAggregator.GetEvent<OnNewDependantDetailViewOpenedEvent>().Subscribe(OnNewDependantDetailOpened);
            this.eventAggregator.GetEvent<OnDetailViewOpenedEvent>().Subscribe(OnDetailViewOpened);
            this.eventAggregator.GetEvent<OnBrowseViewFilteredOpenedEvent>().Subscribe(OnBrowseViewFilteredOpened);
            this.eventAggregator.GetEvent<AfterDetailViewDeletedEvent>().Subscribe(AfterDetailViewDeleted);
            this.eventAggregator.GetEvent<AfterDetailViewClosedEvent>().Subscribe(AfterDetailViewClosed);

            CreateNewDetailViewCommand = new DelegateCommand<Type>(OnCreateNewDetailViewExecute);
            OpenSingleDetailViewCommand = new DelegateCommand<Type>(OnOpenSingleDetailViewExecute);

            LogInCommand = new DelegateCommand<Type>(LogIn);
            LogOutCommand = new DelegateCommand<Type>(LogOut);
        }

        #endregion

        #region Public Properties

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

        /// <summary>
        ///     Removes ViewModel from DetailVMs.
        /// </summary>
        private void AfterDetailViewClosed(AfterDetailClosedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        /// <summary>
        ///     Removes ViewModel from DetailVMs.
        /// </summary>
        private void AfterDetailViewDeleted(AfterDetailViewDeletedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        /// <summary>
        ///     Creates a new browse view with filters applied - for example Vehicle browse view for specific model only.
        /// </summary>
        private async void OnBrowseViewFilteredOpened(OnBrowseViewFilteredOpenedEventArgs args)
        {
            // Finding view or creating it.
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

            // Applying filter.
            ((BaseBrowseVM) detailViewModel).FilterReset();
            if (!string.IsNullOrEmpty(args.FilterId))
            {
                ((BaseBrowseVM) detailViewModel).FilterId = args.FilterId;
                ((BaseBrowseVM) detailViewModel).FilterById();
            }
            else
            {
                ((BaseBrowseVM) detailViewModel).FilterDisplayMember = args.FilterDisplayMember;
                ((BaseBrowseVM) detailViewModel).FilterByDisplayMember();
            }

            // Selecting as currently viewed.
            SelectedDetailVM = detailViewModel;
        }

        /// <summary>
        ///     Publishes an event calling for creation of new VM of given type.
        /// </summary>
        private void OnCreateNewDetailViewExecute(Type viewModelType)
        {
            //OnOpenDetailView(new OpenDetailViewEventArgs { ViewModelName = nameof(FriendDetailViewModel)});
            // More versatile approach:
            OnDetailViewOpened(new OnDetailViewOpenedEventArgs
            {
                Id = nextNewItemId--,
                ViewModelName = viewModelType.Name
            });
        }

        /// <summary>
        ///     Creates new VM (or selects it if tis already created) then sets it as currently viewed.
        /// </summary>
        private async void OnDetailViewOpened(OnDetailViewOpenedEventArgs args)
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

        /// <summary>
        ///     Used when new Order or Vehicle is created through button on Customer/VehicleModel Views.
        /// </summary>
        private async void OnNewDependantDetailOpened(OnNewDependantDetailViewOpenedEventArgs args)
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

                        ((VehicleDetailVM) detailViewModel).VehicleModelId = args.DependantOnId;
                        await detailViewModel.LoadAsync(args.Id);

                        DetailVMs.Add(detailViewModel);
                        break;
                    }

                    case nameof(OrderDetailVM):
                    {
                        detailViewModel = detailVMCreator[args.ViewModelName];

                        ((OrderDetailVM) detailViewModel).CustomerId = args.DependantOnId;
                        await detailViewModel.LoadAsync(args.Id);

                        DetailVMs.Add(detailViewModel);
                        break;
                    }
                }
            }

            SelectedDetailVM = detailViewModel;
        }

        /// <summary>
        ///     Publishes an event calling for creation of new VM of given type. Id is -1 => only one instance of this View type
        ///     might exist at any time.
        /// </summary>
        private void OnOpenSingleDetailViewExecute(Type viewModelType)
        {
            OnDetailViewOpened(new OnDetailViewOpenedEventArgs
            {
                Id = -1,
                ViewModelName = viewModelType.Name
            });
        }

        /// <summary>
        ///     Removes ViewModel from DetailVMs.
        /// </summary>
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

        #region Debug-related

        // LogIn system placeholder.
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
            UserPrivilege.LogIn();
        }

        private void LogOut(Type obj)
        {
            UserPrivilege.LogOut();
        }

        #endregion
    }
}