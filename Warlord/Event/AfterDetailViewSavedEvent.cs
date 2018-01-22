using Prism.Events;

namespace Warlord.Event
{
    /// <summary>
    ///     Published when entity is saved using view.
    /// </summary>
    public class AfterDetailViewSavedEvent : PubSubEvent<AfterDetailViewSavedEventArgs>
    {
    }

    public class AfterDetailViewSavedEventArgs
    {
        #region Public Properties

        public string DisplayMember { get; set; }
        public int Id { get; set; }
        public string ViewModelName { get; set; }

        #endregion
    }
}