﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Events;
using Warlord.Event;
using Warlord.Service;
using Warlord.Service.Lookups;
using Warlord.Service.Message;

namespace Warlord.ViewModel.Detail.Browse
{
    public class CustomerBrowseVM : BaseBrowseVM
    {
        #region Fields

        private readonly ICustomerLookupService lookupService;

        #endregion

        #region Constructors and Destructors

        public CustomerBrowseVM(IEventAggregator eventAggregator, IMessageService messageService,
            IUserPrivilege userPrivilege,
            ICustomerLookupService lookupService)
            : base(eventAggregator, messageService, userPrivilege)
        {
            Title = "Customers";
            this.lookupService = lookupService;
        }

        #endregion

        #region Public Methods and Operators

        public override async Task LoadAsync(int id)
        {
            Id = id;
            BrowseItems.Clear();

            var lookupItems = await lookupService.GetCustomerLookupAsync();
            foreach (var item in lookupItems)
            {
                BrowseItems.Add(new BrowseItem(
                    item.Id,
                    item.DisplayMember,
                    EventAggregator,
                    nameof(CustomerDetailVM)
                ));
            }
        }

        #endregion

        #region Event-related

        protected override void AfterDetailDeleted(AfterDetailViewDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(CustomerDetailVM))
            {
                AfterDetailDeleted(BrowseItems, args);
            }
        }

        protected override void AfterDetailSaved(AfterDetailViewSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(CustomerDetailVM))
            {
                AfterDetailSaved(BrowseItems, args);
            }
        }

        #endregion
    }
}