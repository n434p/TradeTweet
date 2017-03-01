using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TradeTweet
{
    public partial class AutoSettings : UserControl
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );

        void DrawTree()
        {
            settingsTree.Nodes.Clear();

            foreach (EventType item in Enum.GetValues(typeof(EventType)))
            {
                if (item == EventType.Empty) continue;

                var list = new List<TreeNode>();

                if (!Settings.Set.ContainsKey(item))
                {
                    string rootNodeName = "";

                    switch (item)
                    {
                        case EventType.OrderPlaced:
                            rootNodeName = "Order placed";
                            break;
                        case EventType.OrderCancelled:
                            rootNodeName = "Order cancelled";
                            break;
                        case EventType.PositionOpened:
                            rootNodeName = "Position opened";
                            break;
                        case EventType.PositionClosed:
                            rootNodeName = "Position closed";
                            break;
                        default:
                            break;
                    }

                    var evenOperation = new EventOperation()
                    {
                        Active = false,
                        Items = new Dictionary<EventItem, EventOperationItem>(),
                        Name = rootNodeName,
                        Type = item
                    };

                    Settings.Set[item] = evenOperation;
                }

                foreach (EventItem item2 in Enum.GetValues(typeof(EventItem)))
                {
                    if (!Settings.Set[item].Items.ContainsKey(item2))
                    {
                        string nodeName = "";

                        switch (item2)
                        {
                            case EventItem.side:
                                nodeName = "Side";
                                break;
                            case EventItem.qty:
                                nodeName = "Quantity";
                                break;
                            case EventItem.symbol:
                                nodeName = "Symbol";
                                break;
                            case EventItem.type:
                                nodeName = "Order type";
                                break;
                            case EventItem.price:
                                nodeName = "Open price";
                                break;
                            case EventItem.sl:
                                nodeName = "Stop loss";
                                break;
                            case EventItem.tp:
                                nodeName = "Take profit";
                                break;
                            case EventItem.id:
                                nodeName = "Order Id";
                                break;
                            default:
                                break;
                        }

                        // skip order type for Position's cases
                        if ((item == EventType.PositionOpened || item == EventType.PositionClosed) && (item2 == EventItem.type))
                            continue;

                        EventOperationItem eventItem = new EventOperationItem()
                        {
                            Checked = false,
                            Name = nodeName
                        };

                        Settings.Set[item].Items[item2] = eventItem;
                    }

                    TreeNode node = new TreeNode(Settings.Set[item].Items[item2].Name);
                    node.Checked = Settings.Set[item].Items[item2].Checked;

                    list.Add(node);
                }

                TreeNode rootNode = new TreeNode(Settings.Set[item].Name, list.ToArray());
                rootNode.Tag = Settings.Set[item];

                settingsTree.Nodes.Add(rootNode);
            }
        }



        //private void GenerateNewTree()
        //{
        //    foreach (EventType item in Enum.GetValues(typeof(EventType)))
        //    {
        //        string rootNodeName = "";

        //        switch (item)
        //        {
        //            case EventType.OrderPlaced:
        //                rootNodeName = "Order placed";
        //                break;
        //            case EventType.OrderCancelled:
        //                rootNodeName = "Order cancelled";
        //                break;
        //            case EventType.PositionOpened:
        //                rootNodeName = "Position opened";
        //                break;
        //            case EventType.PositionClosed:
        //                rootNodeName = "Position closed";
        //                break;
        //            default:
        //                break;
        //        }

        //        var evenOperation = new EventOperation()
        //        {
        //            Active = false,
        //            Items = new Dictionary<EventItem, EventOperationItem>(),
        //            Name = rootNodeName,
        //            Type = item
        //        };

        //        var list = new List<TreeNode>();

        //        foreach (EventItem item2 in Enum.GetValues(typeof(EventItem)))
        //        {
        //            string nodeName = "";

        //            switch (item2)
        //            {
        //                case EventItem.side:
        //                    nodeName = "Side";
        //                    break;
        //                case EventItem.qty:
        //                    nodeName = "Quantity";
        //                    break;
        //                case EventItem.symbol:
        //                    nodeName = "Symbol";
        //                    break;
        //                case EventItem.type:
        //                    nodeName = "Order type";
        //                    break;
        //                case EventItem.price:
        //                    nodeName = "Open price";
        //                    break;
        //                case EventItem.sl:
        //                    nodeName = "Stop loss";
        //                    break;
        //                case EventItem.tp:
        //                    nodeName = "Take profit";
        //                    break;
        //                case EventItem.id:
        //                    nodeName = "Order Id";
        //                    break;
        //                default:
        //                    break;
        //            }

        //            // skip order type for Position's cases
        //            if ((item == EventType.PositionOpened || item == EventType.PositionClosed) && (item2 == EventItem.type))
        //                continue;

        //            TreeNode node = new TreeNode(nodeName);
        //            node.Checked = false;

        //            EventOperationItem eventItem = new EventOperationItem()
        //            {
        //                Checked = false,
        //                Name = nodeName
        //            };

        //            evenOperation.Items[item2] = eventItem;
        //            list.Add(node);
        //        }

        //        Settings.Set[item] = evenOperation;
        //        settingsTree.Nodes.Add(new TreeNode(Settings.Set[item].Name, list.ToArray()));
        //    }
        //}

        public AutoSettings()
        {
            InitializeComponent();

            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 1, 5));

            DrawTree();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }

    
}
