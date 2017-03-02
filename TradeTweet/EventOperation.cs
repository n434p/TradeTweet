using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TradeTweet
{
    [DataContract]
    enum EventItem
    {
        [EnumMember]
        side,
        [EnumMember]
        qty,
        [EnumMember]
        symbol,
        [EnumMember]
        type,
        [EnumMember]
        price,
        [EnumMember]
        sl,
        [EnumMember]
        tp,
        [EnumMember]
        id
    }

    [DataContract]
    class EventOperation
    {
        [DataMember]
        internal bool Active;
        [DataMember]
        internal string Name;
        [DataMember]
        internal Dictionary<EventItem, EventOperationItem> Items = new Dictionary<EventItem, EventOperationItem>();
    }

    [DataContract]
    class EventOperationItem
    {
        [DataMember]
        internal string Name;
        [DataMember]
        internal bool Checked;
    }
}
