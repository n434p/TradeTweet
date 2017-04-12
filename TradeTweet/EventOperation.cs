using PTLRuntime.NETScript;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TradeTweet
{
    static class EventBuilder
    {
        public const string DELIMITER = "_";
        public const string PTMC_CAPTION = "#PTMC_platform\n";

        internal static List<EventOperation> Create()
        {
            List<EventOperation> list = new List<EventOperation>();

            list.Add(new OrderPlacedEventOperation());
            list.Add(new PositionOpenedEventOperation());

            return list;
        }
    }

    [DataContract]
    abstract class EventOperation
    {
        internal EventOperationItem rootItem;

        internal Dictionary<string, EventOperationItem> Items = new Dictionary<string, EventOperationItem>();

        internal virtual string GetMessage(object o)
        {
            if (o == null)
                return string.Empty;

            string text = rootItem.MessageText(o);

            List<string> list = new List<string>();

            foreach (EventOperationItem item in Items.Values)
            {
                if (!item.Checked) continue;
                string part = item.MessageText(o);
                list.Add(part);
            }

            return text + string.Join(EventBuilder.DELIMITER, list);
        }

        internal virtual System.DateTime GetTime(object obj)
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

        internal Action<bool> Checking;

        public EventOperationItem(string name, bool check = false)
        {
            Name = name;
            Checked = check;
            Checking = (b) => { Checked = b; };
        }
    }

    class OrderPlacedEventOperation : EventOperation
    {
        const string NAME = "Order placed";

        public OrderPlacedEventOperation()
        {
            rootItem = GetItem(NAME, (o) => { return EventBuilder.PTMC_CAPTION + o.Type.ToString() + " " + NAME + ":\n"; });
            rootItem.Checking = (b) => { rootItem.Checked = b; };
            PopulateItems();
        }

        internal override DateTime GetTime(object o)
        {
            return (o != null)? (o as Order).Time: base.GetTime(o);
        }

        void PopulateItems()
        {
            Items["Side"] = GetItem("Side", (o) => { return o.Side.ToString(); });
            Items["Quantity"] = GetItem("Quantity", (o) => { return o.Amount.ToString(); });
            Items["Symbol"] = GetItem("Symbol", (o) => { return o.Instrument.Symbol.ToString(); });
            Items["Type"] = GetItem("Type", (o) => { return o.Type.ToString(); });
            Items["Price"] = GetItem("Price", (o) => { return o.Instrument.FormatPrice(o.Price); });
            Items["SL"] = GetItem("SL", (o) => { return (o.StopLossOrder != null) ? ("SL@" + o.Instrument.FormatPrice(o.StopLossOrder.Price)) : ""; });
            Items["TP"] = GetItem("TP", (o) => { return (o.TakeProfitOrder != null) ? ("TP@" + o.Instrument.FormatPrice(o.TakeProfitOrder.Price)) : ""; });
            Items["Id"] = GetItem("Id", (o) => { return "#" + o.Id; });
        }
         
        internal EventOperationItem GetItem(string name, Func<Order, string> text)
        {
            var op = new EventOperationItem(name);
            op.MessageText = (o) => { return text(o as Order); };
            return op;
        }
    }

    class PositionOpenedEventOperation : EventOperation
    {
        const string NAME = "Position opened";

        public PositionOpenedEventOperation()
        {
            rootItem = GetItem(NAME, (o) => { return EventBuilder.PTMC_CAPTION + NAME + ":\n"; });
            PopulateItems();
        }

        internal override DateTime GetTime(object o)
        {
            return (o != null) ? (o as Position).OpenTime : base.GetTime(o);
        }

        void PopulateItems()
        {
            Items["Side"] = GetItem("Side", (o) => { return o.Side.ToString(); });
            Items["Quantity"] = GetItem("Quantity", (o) => { return o.Amount.ToString(); });
            Items["Symbol"] = GetItem("Symbol", (o) => { return o.Instrument.Symbol.ToString(); });
            Items["Price"] = GetItem("Price", (o) => { return o.Instrument.FormatPrice(o.OpenPrice); });
            Items["SL"] = GetItem("SL", (o) => { return (o.StopLossOrder != null) ? ("SL@" + o.Instrument.FormatPrice(o.StopLossOrder.Price)) : ""; });
            Items["TP"] = GetItem("TP", (o) => { return (o.TakeProfitOrder != null) ? ("TP@" + o.Instrument.FormatPrice(o.TakeProfitOrder.Price)) : ""; });
            Items["Id"] = GetItem("Id", (o) => { return "#" + o.Id; });
        }

        internal EventOperationItem GetItem(string name, Func<Position, string> text)
        {
            return new EventOperationItem() { Name = name, MessageText = (o) => { return text(o as Position); } };
        }
    }
}
