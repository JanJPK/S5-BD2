using System.Collections.Generic;
using Warlord.Model;

namespace Warlord.Wrapper
{
    public class VehicleModelWrapper : BaseWrapper<VehicleModel>
    {
        #region Constructors and Destructors

        public VehicleModelWrapper(VehicleModel model) : base(model)
        {
        }

        #endregion

        #region Public Properties

        public int Crew
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public string Engine
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int EnginePower
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int Id => Model.Id;

        public string MainArmament
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int ManufacturerId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public Manufacturer Manufacturer
        {
            get => GetValue<Manufacturer>();
            set => SetValue(value);
        }

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string SecondaryArmament
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public float Weight
        {
            get => GetValue<float>();
            set => SetValue(value);
        }

        #endregion

        #region Methods

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Crew):
                {
                    if (Crew < 1)
                    {
                        yield return "There must be at least one crew member.";
                    }
                    break;
                }

                case nameof(EnginePower):
                {
                    if (EnginePower < 1)
                    {
                        yield return "Engine cannot have zero or negative horsepower.";
                    }
                    break;
                }

                case nameof(ManufacturerId):
                {
                    if (ManufacturerId < 1)
                    {
                        yield return "Correct manufacturer must be selected.";
                    }
                    break;
                }
            }
        }

        #endregion
    }
}