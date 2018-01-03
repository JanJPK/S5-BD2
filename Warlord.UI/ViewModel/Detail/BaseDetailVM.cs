using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;

namespace Warlord.UI.ViewModel.Detail
{
    public abstract class BaseDetailVM : BaseVM, IDetailVM
    {
        #region Fields

        protected readonly IEventAggregator EventAggregator;
        protected readonly IMessageDialogService MessageDialogService;

        private bool hasChanges;
        private string title;

        #endregion

        #region Constructors and Destructors

        protected BaseDetailVM(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            EventAggregator = eventAggregator;
            MessageDialogService = messageDialogService;
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
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
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

        #endregion

        #region Public Methods and Operators

        public abstract Task LoadAsync(int id);

        #endregion

        #region Methods

        protected virtual async void OnCloseDetailViewExecute()
        {
            if (HasChanges)
            {
                var result = await MessageDialogService.ShowOkCancelDialogAsync(
                    "You've made changes. Close this item?", "Question");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            EventAggregator.GetEvent<AfterDetailClosedEvent>()
                .Publish(new AfterDetailClosedEventArgs
                {
                    Id = Id,
                    ViewModelName = GetType().Name
                });
        }

        protected abstract void OnDeleteExecute();
        protected abstract bool OnSaveCanExecute();
        protected abstract void OnSaveExecute();

        protected virtual void RaiseDetailDeletedEvent(int modelId)
        {
            EventAggregator.GetEvent<AfterDetailDeletedEvent>()
                .Publish(new AfterDetailDeletedEventArgs
                {
                    Id = modelId,
                    ViewModelName = GetType().Name
                });
        }

        protected virtual void RaiseDetailSavedEvent(int modelId, string displayMember)
        {
            EventAggregator.GetEvent<AfterDetailSavedEvent>()
                .Publish(new AfterDetailSavedEventArgs
                {
                    Id = modelId,
                    DisplayMember = displayMember,
                    ViewModelName = GetType().Name
                });
        }

        #endregion

        protected virtual void RaiseCollectionSavedEvent()
        {
            EventAggregator.GetEvent<AfterCollectionSavedEvent>()
                .Publish(new AfterCollectionSavedEventArgs
                {
                    ViewModelName = this.GetType().Name
                });
        }

        protected async Task SaveWithOptimisticConcurrencyAsync(Func<Task> saveFunc, Action afterSaveAction)
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
                    MessageDialogService.ShowInfoDialogAsync("The entity has been deleted by another user.");
                    RaiseDetailDeletedEvent(Id);
                    return;
                }

                var result = await MessageDialogService.ShowOkCancelDialogAsync(
                    "The entity has been changed in the meantime by someone else. "
                    + "Click OK to save changes; click Cancel to reload entity from the database.",
                    "Question");

                if (result == MessageDialogResult.OK)
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

            afterSaveAction();
        }
    }
}
