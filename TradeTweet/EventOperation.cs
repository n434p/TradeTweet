using PTLRuntime.NETScript;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TradeTweet
{
    enum EventType { Empty, OrderPlaced, OrderCancelled, PositionOpened, PositionClosed }

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

    static class EventBuilder
    {
        public const string DELIMITER = "_";
        public const string PTMC_CAPTION = "#PTMC_platform\n";

        internal static EventOperation Create(EventType evType)
        {
            EventOperation op = null;

            switch (evType)
            {
                case EventType.Empty:
                    break;
                case EventType.OrderPlaced:
                    return new OrderPlacedEventOperation();
                    break;
                case EventType.OrderCancelled:
                    break;
                case EventType.PositionOpened:
                    break;
                case EventType.PositionClosed:
                    break;
                default:
                    break;
            }

            return op;
        }
    }

    [DataContract]
    abstract class EventOperation
    {
        [DataMember]
        internal bool Active;
        [DataMember]
        internal string Name;

        internal abstract string GetMessage(object obj);
        internal virtual System.DateTime GetTime<T>(T obj)
        {
            return System.DateTime.UtcNow;
        }
    }

    [DataContract]
    class EventOperationItem   
    {
        [DataMember]
        internal string Name;
        [DataMember]
        internal bool Checked;

        internal Func<object,string> MessageText;

    }

    class OrderPlacedEventOperation : EventOperation
    {
        public OrderPlacedEventOperation()
        {
            Name = "Order placed";
            PopulateItems();
        }

        internal Dictionary<string, EventOperationItem> Items = new Dictionary<string, EventOperationItem>();

        internal override string GetMessage(object o)
        {
            Order order = o as Order;

            if (order == null)
                return string.Empty;

            string text = EventBuilder.PTMC_CAPTION + order.Type.ToString() + " "+Name+":\n";

            List<string> list = new List<string>();

            foreach (EventOperationItem item in Items.Values)
            {
                if (!item.Checked) continue;
                string part = item.MessageText(order);
                list.Add(part);
            }

            return text + string.Join(EventBuilder.DELIMITER, list);
        }

        internal override DateTime GetTime<Order>(Order order)
        {
            return (order != null)? order.Time: DateTime.UtcNow;
        }

        void PopulateItems()
        {
            SetItem("Side", (o) => { return (o as Order).Side.ToString(); });
            SetItem("Quantity", (o) => { return o.Amount.ToString(); });
            SetItem("Symbol", (o) => { return o.Instrument.Symbol.ToString(); });
            SetItem("Type", (o) => { return o.Type.ToString(); });
            SetItem("Price", (o) => { return o.Instrument.FormatPrice(o.Price); });
            SetItem("SL", (o) => { return (o.StopLossOrder != null) ? ("SL@" + o.Instrument.FormatPrice(o.StopLossOrder.Price)) : ""; });
            SetItem("TP", (o) => { return (o.TakeProfitOrder != null) ? ("TP@" + o.Instrument.FormatPrice(o.TakeProfitOrder.Price)) : ""; });
            SetItem("Id", (o) => { return "#" + o.Id; });
        }

        internal void SetItem(string name, Func<object, string> text)
        {
            Items[name] = new EventOperationItem() { Name = name, MessageText = text };
        }
    }


}
