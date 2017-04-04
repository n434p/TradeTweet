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
        internal static Dictionary<EventItem, EventOperationItem> GetDefaultDictionary()
        {
            return new Dictionary<EventItem, EventOperationItem>
            {
                {EventItem.id, new EventOperationItem() { Name = "Id"} },
                {EventItem.price, new EventOperationItem() { Name = "Price"} },
                {EventItem.qty, new EventOperationItem() { Name = "Quantity"} },
                {EventItem.side, new EventOperationItem() { Name = "Side"} },
                {EventItem.sl, new EventOperationItem() { Name = "Stoploss"} },
                {EventItem.symbol, new EventOperationItem() { Name = "Symbol"} },
                {EventItem.tp, new EventOperationItem() { Name = "TP"} },
                {EventItem.type, new EventOperationItem() { Name = "Type"} }
            };
        }

        internal static EventOperation Create(EventType evType)
        {
            EventOperation op = null;

            switch (evType)
            {
                case EventType.Empty:
                    break;
                case EventType.OrderPlaced:
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
        public const string DELIMITER = "_";
        public const string PTMC_CAPTION = "#PTMC_platform\n";

        [DataMember]
        internal bool Active;
        [DataMember]
        internal string Name;
        [DataMember]
        internal Dictionary<EventItem, EventOperationItem> Items = new Dictionary<EventItem, EventOperationItem>();

        internal abstract string GetMessage();
        internal abstract System.DateTime GetTime();
    }

    [DataContract]
    class EventOperationItem
    {
        [DataMember]
        internal string Name;
        [DataMember]
        internal bool Checked;
    }

    class OrderPlacedEventOperation : EventOperation
    {
        internal Order order;

        public OrderPlacedEventOperation()
        {
            Name = "Order placed";
            Items = EventBuilder.GetDefaultDictionary();
        }

        internal override string GetMessage()
        {
            if (order == null)
                return string.Empty;

            string text = EventOperation.PTMC_CAPTION + order.Type.ToString() + " Order placed:\n";

            List<string> list = new List<string>();

            foreach (EventItem item in Items.Keys)
            {
                if (!Items[item].Checked) continue;

                string part = "";

                switch (item)
                {
                    case EventItem.side:
                        part = order.Side.ToString();
                        break;
                    case EventItem.qty:
                        part = order.Amount.ToString();
                        break;
                    case EventItem.symbol:
                        part = order.Instrument.Symbol.ToString();
                        break;
                    case EventItem.type:
                        part = order.Type.ToString();
                        break;
                    case EventItem.price:
                        part = order.Instrument.FormatPrice(order.Price);
                        break;
                    case EventItem.sl:
                        part = (order.StopLossOrder != null) ? ("SL@" + order.Instrument.FormatPrice(order.StopLossOrder.Price)) : "";
                        break;
                    case EventItem.tp:
                        part = (order.TakeProfitOrder != null) ? ("TP@" + order.Instrument.FormatPrice(order.TakeProfitOrder.Price)) : "";
                        break;
                    case EventItem.id:
                        part = "#" + order.Id;
                        break;
                    default:
                        break;
                }

                list.Add(part);
            }

            return text + string.Join(EventOperation.DELIMITER, list);
        }

        internal override DateTime GetTime()
        {
            return (order != null)? order.Time: DateTime.UtcNow;
        }
    }


}
