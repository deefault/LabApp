using System;

namespace LabApp.Shared.EventConsistency
{
    public enum StoreType
    {
        Ef = 1,
    }

    public partial class EventConsistencyOptions
    {
        public const string Key = "EventConsistency";

        public bool EnableInbox { get; set; }
        public bool EnableOutbox { get; set; }
        public StoreType InboxStoreType { get; set; } = StoreType.Ef;
        public StoreType OutboxStoreType { get; set; } = StoreType.Ef;
        
        public static EventConsistencyOptions Default =>
            new EventConsistencyOptions()
            {
                EnableInbox = true,
                EnableOutbox = true
            };

        public static readonly Action<EventConsistencyOptions> DefaultConfiguration = o =>
        {
            o.EnableInbox = Default.EnableInbox;
            o.EnableOutbox = Default.EnableOutbox;
            o.InboxStoreType = StoreType.Ef;
            o.OutboxStoreType = StoreType.Ef;
        };
    }
}