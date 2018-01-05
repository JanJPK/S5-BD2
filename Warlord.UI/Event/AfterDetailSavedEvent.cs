using Prism.Events;

namespace Warlord.UI.Event
{
    /// <summary>
    ///     Published when entity is saved using view.
    /// </summary>
    public class AfterDetailSavedEvent : PubSubEvent<AfterDetailSavedEventArgs>
    {
    }

    public class AfterDetailSavedEventArgs
    {
        #region Public Properties

        public string DisplayMember { get; set; }
        public int Id { get; set; }
        public string ViewModelName { get; set; }

        #endregion
    }
}