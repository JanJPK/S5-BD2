namespace Warlord.Service.Lookups
{
    public class LookupItem
    {
        #region Public Properties

        public string DisplayMember { get; set; }
        public int Id { get; set; }

        #endregion
    }

    public class NullLookupItem : LookupItem
    {
        public new int? Id => null;
    }
}
