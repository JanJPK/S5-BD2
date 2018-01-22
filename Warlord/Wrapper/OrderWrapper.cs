using System;
using Warlord.Model;

namespace Warlord.Wrapper
{
    public class OrderWrapper : BaseWrapper<Order>
    {
        #region Constructors and Destructors

        public OrderWrapper(Order model) : base(model)
        {
        }

        #endregion

        #region Public Properties

        public bool Completed
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public Customer Customer
        {
            get => GetValue<Customer>();
            set => SetValue(value);
        }

        public DateTime Date
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public int Id => Model.Id;

        public int TotalPrice
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        #endregion
    }
}