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

        public AutoSettings()
        {
            InitializeComponent();

            settingsTree.FullRowSelect = false;

            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 1, 5));

            DrawTree();

            Settings.onSettingsChanged += RefreshTree;
        }

        

        internal void RefreshTree()
        {
            foreach (TreeNode rootNode in settingsTree.Nodes)
            {
                EventType type = (EventType)rootNode.Tag;

                int count = 0;

                foreach (TreeNode node in rootNode.Nodes)
                {
                    EventItem item = (EventItem)node.Tag;

                    node.StateImageIndex = (Settings.Set[type].Items[item].Checked) ? 1: 0;

                    if (Settings.Set[type].Items[item].Checked)
                        count++;
                }

                if (count == rootNode.Nodes.Count)
                    rootNode.StateImageIndex = 1;
                else if ( count == 0)
                    rootNode.StateImageIndex = 0;
                else
                    rootNode.StateImageIndex = 2;

            }

            
        }


        void DrawTree()
        {
            settingsTree.Nodes.Clear();

            settingsTree.SuspendLayout();

            foreach (EventType item in Enum.GetValues(typeof(EventType)))
            {
                if (item == EventType.Empty) continue;

                TreeNode rootNode = new TreeNode();
                settingsTree.Nodes.Add(rootNode);

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
                        Name = rootNodeName
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
                    node.Tag = item2;

                    rootNode.Nodes.Add(node);
                }

                rootNode.Name = Settings.Set[item].Name;
                rootNode.Tag = item;
                rootNode.Checked = Settings.Set[item].Active;
            }

            settingsTree.ResumeLayout();

            //RefreshTree();
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
    }


}
