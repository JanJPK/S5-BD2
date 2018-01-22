using Warlord.Model;

namespace Warlord.Wrapper
{
    public class ManufacturerWrapper : BaseWrapper<Manufacturer>
    {
        #region Constructors and Destructors

        public ManufacturerWrapper(Manufacturer model) : base(model)
        {
        }

        #endregion

        #region Public Properties

        public string Country
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string FullName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int Id => Model.Id;

        public string ShortName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        #endregion
    }
}