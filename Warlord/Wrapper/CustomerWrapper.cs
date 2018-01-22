using Warlord.Model;

namespace Warlord.Wrapper
{
    public class CustomerWrapper : BaseWrapper<Customer>
    {
        #region Constructors and Destructors

        public CustomerWrapper(Customer model) : base(model)
        {
        }

        #endregion

        #region Public Properties

        public string Address
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string City
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Country
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Email
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int Id => Model.Id;

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Phone
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string PostalCode
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        #endregion
    }
}