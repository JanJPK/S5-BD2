using Prism.Events;

namespace Warlord.Event
{
    /// <summary>
    ///     Published after saving collection is saved so listboxes can be updated with changed collection contents.
    /// </summary>
    public class AfterCollectionSavedEvent : PubSubEvent<AfterCollectionSavedEventArgs>
    {
    }

    public class AfterCollectionSavedEventArgs
    {
        #region Public Properties

        public string ViewModelName { get; set; }

        #endregion
    }
}