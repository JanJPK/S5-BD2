using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Warlord.Event;
using Warlord.Service;
using Warlord.Service.Message;

namespace Warlord.ViewModel.Detail
{
    public abstract class BaseDetailVM : BaseVM, IDetailVM
    {
        #region Fields

        protected readonly IEventAggregator EventAggregator;
        protected readonly IMessageService MessageService;

        private bool hasChanges;
        private string title;

        private IUserPrivilege userPrivilege;

        #endregion

        #region Constructors and Destructors

        protected BaseDetailVM(IEventAggregator eventAggregator,
            IMessageService messageService,
            IUserPrivilege userPrivilege)
        {
            EventAggregator = eventAggregator;
            MessageService = messageService;
            UserPrivilege = userPrivilege;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
            CloseDetailViewCommand = new DelegateCommand(OnCloseDetailViewExecute);
        }

        #endregion

        #region Public Properties

        public ICommand CloseDetailViewCommand { get; }

        public ICommand DeleteCommand { get; }

        public bool HasChanges
        {
            get => hasChanges;
            set
            {
                if (hasChanges != value)
                {
                    hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public int Id { get; set; }

        public ICommand SaveCommand { get; }

        public string Title
        {
            get => title;
            protected set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        public IUserPrivilege UserPrivilege
        {
            get => userPrivilege;
            set
            {
                userPrivilege = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Public Methods and Operators

        public abstract Task LoadAsync(int id);

        #endregion

        #region Methods

        protected virtual async void OnCloseDetailViewExecute()
        {
            if (HasChanges)
            {
                var result = await MessageService.ShowConfirmDialog(
                    "You've made changes. Close this item?");
                if (!result)
                {
                    return;
                }
            }

            EventAggregator.GetEvent<AfterDetailViewClosedEvent>()
                .Publish(new AfterDetailClosedEventArgs
                {
                    Id = Id,
                    ViewModelName = GetType().Name
                });
        }

        protected abstract void OnDeleteExecute();
        protected abstract bool OnSaveCanExecute();
        protected abstract void OnSaveExecute();

        protected virtual void RaiseCollectionSavedEvent()
        {
            EventAggregator.GetEvent<AfterCollectionSavedEvent>()
                .Publish(new AfterCollectionSavedEventArgs
                {
                    ViewModelName = GetType().Name
                });
        }

        protected virtual void RaiseDetailViewDeletedEvent(int modelId)
        {
            EventAggregator.GetEvent<AfterDetailViewDeletedEvent>()
                .Publish(new AfterDetailViewDeletedEventArgs
                {
                    Id = modelId,
                    ViewModelName = GetType().Name
                });
        }

        protected virtual void RaiseDetailViewSavedEvent(int modelId, string displayMember)
        {
            EventAggregator.GetEvent<AfterDetailViewSavedEvent>()
                .Publish(new AfterDetailViewSavedEventArgs
                {
                    Id = modelId,
                    DisplayMember = displayMember,
                    ViewModelName = GetType().Name
                });
        }

        protected virtual async Task SaveWithOptimisticConcurrencyAsync(Func<Task> saveFunc)
        {
            try
            {
                await saveFunc();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var databaseValues = ex.Entries.Single().GetDatabaseValues();
                if (databaseValues == null)
                {
                    await MessageService.ShowInfoDialog("The entity has been deleted by another user.");
                    RaiseDetailViewDeletedEvent(Id);
                    return;
                }

                var result = await MessageService.ShowConfirmDialog(
                    "The entity has been changed in the meantime by someone else. "
                    + "Click OK to save changes; click Cancel to reload entity from the database.");

                if (result)
                {
                    // Update entity.
                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    await saveFunc();
                }
                else
                {
                    // Reload entity.
                    await ex.Entries.Single().ReloadAsync();
                    await LoadAsync(Id);
                }
            }

            AfterSaveAction();
        }

        #endregion

        protected abstract void AfterSaveAction();
    }
}