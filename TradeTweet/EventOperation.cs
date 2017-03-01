using System.Collections.Generic;

namespace TradeTweet
{
    enum EventItem
    {
        side,
        qty,
        symbol,
        type,
        price,
        sl,
        tp,
        id
    }

    class EventOperation
    {
        internal bool Active;

        internal string Name;

        internal Dictionary<EventItem, EventOperationItem> Items = new Dictionary<EventItem, EventOperationItem>();
    }

    class EventOperationItem
    {
        internal string Name;
        internal bool Checked;
    }
}
