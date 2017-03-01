using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        internal EventType Type;

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
