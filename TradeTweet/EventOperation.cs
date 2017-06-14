using PTLRuntime.NETScript;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace TradeTweet
{
    static class EventManager
    {
        public const string DELIMITER = "_";
        public const string PTMC_CAPTION = "#PTMC_platform\n";

        internal static Dictionary<string,EventOperation> EventsList = new Dictionary<string, EventOperation>()
        {
            {OrderPlacedEventOperation.NAME, new OrderPlacedEventOperation() },
            {PositionOpenedEventOperation.NAME, new PositionOpenedEventOperation() }
        };

        internal static void RefreshListStates(Dictionary<string,EventOperation> list)
        {
            foreach (var operation in list.Keys)
            {
                if (!EventsList.Keys.Contains(operation))
                    continue;

                EventsList[operation].rootItem.Checked = list[operation].rootItem.Checked;

                foreach (var key in EventsList[operation].Items.Keys)
                {
                    EventsList[operation].Items[key].Checked = list[operation].Items[key].Checked;
                } 
            }
        }

        internal static Action<object, EventOperation> eventInvoke = TweetMessenger.SendAutoTweet;
    }

    [DataContract]
    [KnownType("DerivedTypes")]
    abstract class EventOperation
    {
        [DataMember]
        internal EventOperationItem rootItem;

        [DataMember]
        internal Dictionary<string, EventOperationItem> Items = new Dictionary<string, EventOperationItem>();

        internal virtual void OnEvent(object obj)
        {
            if (EventManager.eventInvoke != null)
                EventManager.eventInvoke.Invoke(obj, this);
        }

        public EventOperation()
        {
            PopulateItems();
        }

        internal abstract void PopulateItems();

        internal abstract void Subscribe();

        internal abstract void Unsubscribe();

        private static Type[] DerivedTypes()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(_ => _.IsSubclassOf(typeof(EventOperation))).ToArray();
        }

        internal virtual Image GetImage(EventStatus status)
        {
            Image img = null;
            switch (status)
            {
                case EventStatus.Info:
                    img = Properties.Resources.TradeTweet_09;
                    break;
                case EventStatus.Error:
                    img = Properties.Resources.TradeTweet_10;
                    break;
                case EventStatus.Success:
                    img = Properties.Resources.TradeTweet_11;
                    break;
                default:
                    break;
            }

            return img;
        }

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

            return text + string.Join(EventManager.DELIMITER, list);
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

        internal Func<object, string> MessageText;

        internal Action<bool> Checking;

        public EventOperationItem(string name, bool check = false)
        {
            Name = name;
            Checked = check;
            Checking = (b) => { Checked = b; };
        }
    }

    [DataContract]
    class OrderPlacedEventOperation : EventOperation
    {
        internal const string NAME = "Order placed";

        internal override void Subscribe()
        {
            TweetManager.PlatformEngine.Orders.OrderAdded += OnEvent;
        }

        internal override void Unsubscribe()
        {
            TweetManager.PlatformEngine.Orders.OrderAdded -= OnEvent;
        }

        internal override void PopulateItems()
        {
            rootItem = GetItem(NAME, (o) => { return EventManager.PTMC_CAPTION + o.Type.ToString() + " " + NAME + ":\n"; });
            rootItem.Checking = (b) => { rootItem.Checked = b; };

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

        internal override DateTime GetTime(object o)
        {
            return (o != null) ? (o as Order).Time : base.GetTime(o);
        }

        internal override Image GetImage(EventStatus status)
        {
                Image img = null;
                switch (status)
                {
                    case EventStatus.Info:
                        img = Properties.Resources.open_order_yellow;
                        break;
                    case EventStatus.Error:
                        img = Properties.Resources.open_order_red;
                        break;
                    case EventStatus.Success:
                        img = Properties.Resources.open_order_green;
                        break;
                    default:
                        break;
                }

                return img;
        }
    }

    [DataContract]
    class PositionOpenedEventOperation : EventOperation
    {
        internal const string NAME = "Position opened";

        internal override void Subscribe()
        {
            TweetManager.PlatformEngine.Positions.PositionAdded += OnEvent;
        }

        internal override void Unsubscribe()
        {
            TweetManager.PlatformEngine.Positions.PositionAdded -= OnEvent;
        }

        internal override void PopulateItems()
        {
            rootItem = GetItem(NAME, (o) => { return EventManager.PTMC_CAPTION + NAME + ":\n"; });
            rootItem.Checking = (b) => { rootItem.Checked = b; };

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
            var op = new EventOperationItem(name);
            op.MessageText = (o) => { return text(o as Position); };
            return op;
        }

        internal override DateTime GetTime(object o)
        {
            return (o != null) ? (o as Position).OpenTime : base.GetTime(o);
        }

        internal override Image GetImage(EventStatus status)
        {
                Image img = null;
                switch (status)
                {
                    case EventStatus.Info:
                        img = Properties.Resources.open_pos_yellow;
                        break;
                    case EventStatus.Error:
                        img = Properties.Resources.open_pos_red;
                        break;
                    case EventStatus.Success:
                        img = Properties.Resources.open_pos_green;
                        break;
                    default:
                        break;
                }

                return img;
        }
    }
}