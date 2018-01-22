using System;
using System.Collections.Generic;
using Warlord.Model;

namespace Warlord.Wrapper
{
    public class VehicleWrapper : BaseWrapper<Vehicle>
    {
        #region Constructors and Destructors

        public VehicleWrapper(Vehicle model) : base(model)
        {
        }

        #endregion

        #region Public Properties

        public string Color
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Condition
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public DateTime DateOfManufacture
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public string Imagepath
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int Id => Model.Id;

        public int? OrderId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int Price
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public bool IsAvailable => Model.Order == null ? true : false;

        public string IsAvailableString
        {
            get
            {
                if (Model.Order == null)
                    return "This vehicle is available for sale.";
                return "This vehicle is not available for sale.";

            }
        }

        #endregion

        #region Methods

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Price):
                {
                    if (Price <= 0)
                    {
                        yield return "Price cannot be negative or zero.";
                    }
                    break;
                }

                case nameof(DateOfManufacture):
                {
                    if (DateOfManufacture > DateTime.Today)
                    {
                        yield return "Vehicle was not manufactured in the future.";
                    }
                    break;
                }
            }
        }

        #endregion
    }
}