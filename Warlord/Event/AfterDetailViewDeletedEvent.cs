using Prism.Events;

namespace Warlord.Event
{
    /// <summary>
    ///     Published when entity is deleted using view.
    /// </summary>
    public class AfterDetailViewDeletedEvent : PubSubEvent<AfterDetailViewDeletedEventArgs>
    {
    }

    public class AfterDetailViewDeletedEventArgs
    {
        #region Public Properties

        public int Id { get; set; }
        public string ViewModelName { get; set; }

        #endregion
    }
}