using System;
using Warlord.Model;

namespace Warlord.Wrappers
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

        public string Filename
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int Id => Model.Id;

        public Order Order
        {
            get => GetValue<Order>();
            set => SetValue(value);
        }

        public float Price
        {
            get => GetValue<float>();
            set => SetValue(value);
        }

        public int? VehicleModelId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public VehicleModel VehicleModel
        {
            get => GetValue<VehicleModel>();
            set => SetValue(value);
        }

        #endregion
    }
}