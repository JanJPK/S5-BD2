using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warlord.Model;

namespace Warlord.UI.Wrappers
{
    public class VehicleModelWrapper : BaseWrapper<VehicleModel>
    {
        public VehicleModelWrapper(VehicleModel model) : base(model)
        {
        }

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

        public string SecondaryArmament
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public float Weight
        {
            get => GetValue<float>();
            set => SetValue(value);
        }

        public Manufacturer Manufacturer
        {
            get => GetValue<Manufacturer>();
            set => SetValue(value);
        }

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Crew):
                {
                    if (Crew <= 0)
                    {
                        yield return "There must be at least one crew member.";
                    }
                    break;
                }

                case nameof(EnginePower):
                {
                    if (EnginePower <= 0)
                    {
                        yield return "Engine cannot have zero or negative horsepower.";
                    }
                    break;
                }
            }
        }
    }
}
