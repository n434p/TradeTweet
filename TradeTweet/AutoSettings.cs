using System;
using System.Collections.Generic;
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
            settingsTree.CheckBoxes = false;

            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 1, 5));

            DrawTree();

            Settings.onSettingsChanged += RefreshTree;
        }

        internal void RefreshTree()
        {
            settingsTree.SuspendLayout();
            settingsTree.IgnoreClickAction++;

            foreach (TreeNode rootNode in settingsTree.Nodes)
            {
                EventType type = (EventType)rootNode.Tag;

                int count = 0;

                foreach (TreeNode node in rootNode.Nodes)
                {
                    EventItem item = (EventItem)node.Tag;

                    node.Checked = Settings.Set[type].Items[item].Checked;
                    node.StateImageIndex = (node.Checked) ? 1: 0;

                    if (Settings.Set[type].Items[item].Checked)
                        count++;
                }

                rootNode.Checked = count > 0;

                if (count == rootNode.Nodes.Count)
                    rootNode.StateImageIndex = 1;
                else if (count == 0)
                    rootNode.StateImageIndex = 0;
                else
                    rootNode.StateImageIndex = 2;

            }
            settingsTree.IgnoreClickAction--;
            settingsTree.ResumeLayout();

        }

        void DrawTree()
        {
            settingsTree.Nodes.Clear();

            foreach (EventType item in Enum.GetValues(typeof(EventType)))
            {
                if (item == EventType.Empty) continue;

                var list = new List<TreeNode>();

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

                if (!Settings.Set.ContainsKey(item))
                {
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

                    if (!Settings.Set[item].Items.ContainsKey(item2))
                    {
                        EventOperationItem eventItem = new EventOperationItem()
                        {
                            Checked = false,
                            Name = nodeName
                        };

                        Settings.Set[item].Items[item2] = eventItem;
                    }

                    TreeNode node = new TreeNode(Settings.Set[item].Items[item2].Name);
                    //node.Checked = Settings.Set[item].Items[item2].Checked;
                    node.Tag = item2;

                    list.Add(node);
                }

                TreeNode rootNode = new TreeNode(Settings.Set[item].Name, list.ToArray());
                rootNode.Tag = item;

                //rootNode.Checked = Settings.Set[item].Active;

                settingsTree.Nodes.Add(rootNode);
            }

            RefreshTree();
        }

        internal class TriStateTreeView : System.Windows.Forms.TreeView
        {
            // <remarks>
            // CheckedState is an enum of all allowable nodes states
            // </remarks>
            public enum CheckedState : int { /*UnInitialised = -1,*/ UnChecked, Checked, Mixed };

            // <remarks>
            // IgnoreClickAction is used to ingore messages generated by setting the node.Checked flag in code
            // Do not set <c>e.Cancel = true</c> in <c>OnBeforeCheck</c> otherwise the Checked state will be lost
            // </remarks>
            internal int IgnoreClickAction = 0;

            // <summary>
            // Constructor.  Create and populate an image list
            // </summary>
            public TriStateTreeView() : base()
            {
                StateImageList = new System.Windows.Forms.ImageList();

                StateImageList.Images.Add(Properties.Resources.unselect);
                StateImageList.Images.Add(Properties.Resources.select);
                StateImageList.Images.Add(Properties.Resources.middle);
            }

            // <summary>
            // Called once before window displayed.  Disables default Checkbox functionality and ensures all nodes display an 'unchecked' image.
            // </summary>
            protected override void OnCreateControl()
            {
                base.OnCreateControl();
                CheckBoxes = false;         // Disable default CheckBox functionality if it's been enabled

                // Give every node an initial 'unchecked' image
                //IgnoreClickAction++;    // we're making changes to the tree, ignore any other change requests
                //UpdateChildState(this.Nodes, (int)CheckedState.UnChecked, false, true);
                //IgnoreClickAction--;
            }

            // <summary>
            // Called after a node is checked.  Forces all children to inherit current state, and notifies parents they may need to become 'mixed'
            // </summary>
            protected override void OnAfterCheck(System.Windows.Forms.TreeViewEventArgs e)
            {
                base.OnAfterCheck(e);

                if (IgnoreClickAction > 0)
                {
                    return;
                }

                IgnoreClickAction++;    // we're making changes to the tree, ignore any other change requests

                // the checked state has already been changed, we just need to update the state index

                // node is either ticked or unticked.  ignore mixed state, as the node is still only ticked or unticked regardless of state of children
                System.Windows.Forms.TreeNode tn = e.Node;
                tn.StateImageIndex = tn.Checked ? (int)CheckedState.Checked : (int)CheckedState.UnChecked;

                // force all children to inherit the same state as the current node
                UpdateChildState(e.Node.Nodes, e.Node.StateImageIndex, e.Node.Checked, false);

                // populate state up the tree, possibly resulting in parents with mixed state
                UpdateParentState(e.Node.Parent);

                IgnoreClickAction--;
            }

            internal void RefreshSettings()
            {
                foreach (TreeNode rootNode in Nodes)
                {
                    EventType type = (EventType)rootNode.Tag;

                    foreach (TreeNode node in rootNode.Nodes)
                    {
                        EventItem item = (EventItem)node.Tag;
                        Settings.Set[type].Items[item].Checked = node.Checked;
                    }

                    bool rootChecked = rootNode.StateImageIndex > 0;

                    if (rootChecked != Settings.Set[type].Active)
                    {
                        Settings.Set[type].Active = rootChecked;
                    }
                }

                Settings.OnSettingsChange();
            }

            // <summary>
            // Called after a node is expanded.  Ensures any new nodes display an 'unchecked' image
            // </summary>
            protected override void OnAfterExpand(System.Windows.Forms.TreeViewEventArgs e)
            {
                // If any child node is new, give it the same check state as the current node
                // So if current node is ticked, child nodes will also be ticked
                base.OnAfterExpand(e);

                IgnoreClickAction++;    // we're making changes to the tree, ignore any other change requests
                UpdateChildState(e.Node.Nodes, e.Node.StateImageIndex, e.Node.Checked, true);
                IgnoreClickAction--;
            }

            // <summary>
            // Helper function to replace child state with that of the parent
            // </summary>
            internal void UpdateChildState(System.Windows.Forms.TreeNodeCollection Nodes, int StateImageIndex, bool Checked, bool ChangeUninitialisedNodesOnly)
            {
                foreach (System.Windows.Forms.TreeNode tnChild in Nodes)
                {
                    if (!ChangeUninitialisedNodesOnly) // || tnChild.StateImageIndex == -1)
                    {
                        tnChild.StateImageIndex = StateImageIndex;
                        tnChild.Checked = Checked;  // override 'checked' state of child with that of parent

                        if (tnChild.Nodes.Count > 0)
                        {
                            UpdateChildState(tnChild.Nodes, StateImageIndex, Checked, ChangeUninitialisedNodesOnly);
                        }
                    }
                }
            }

            // <summary>
            // Helper function to notify parent it may need to use 'mixed' state
            // </summary>
            internal void UpdateParentState(System.Windows.Forms.TreeNode tn)
            {
                // Node needs to check all of it's children to see if any of them are ticked or mixed
                if (tn == null)
                    return;

                int OrigStateImageIndex = tn.StateImageIndex;

                int UnCheckedNodes = 0, CheckedNodes = 0, MixedNodes = 0;

                // The parent needs to know how many of it's children are Checked or Mixed
                foreach (System.Windows.Forms.TreeNode tnChild in tn.Nodes)
                {
                    if (tnChild.StateImageIndex == (int)CheckedState.Checked)
                        CheckedNodes++;
                    else if (tnChild.StateImageIndex == (int)CheckedState.Mixed)
                    {
                        MixedNodes++;
                        break;
                    }
                    else
                        UnCheckedNodes++;
                }


                    // In Installer mode, if all child nodes are checked then parent is checked
                    // If at least one child is unchecked, then parent is unchecked
                    if (MixedNodes == 0)
                    {
                        if (UnCheckedNodes == 0)
                        {
                            // all children are checked, so parent must be checked
                            tn.Checked = true;
                        }
                        else
                        {
                            // at least one child is unchecked, so parent must be unchecked
                            tn.Checked = false;
                        }
                    }


                // Determine the parent's new Image State
                if (MixedNodes > 0)
                {
                    // at least one child is mixed, so parent must be mixed
                    tn.StateImageIndex = (int)CheckedState.Mixed;
                }
                else if (CheckedNodes > 0 && UnCheckedNodes == 0)
                {
                    // all children are checked
                    if (tn.Checked)
                        tn.StateImageIndex = (int)CheckedState.Checked;
                    else
                        tn.StateImageIndex = (int)CheckedState.Mixed;
                }
                else if (CheckedNodes > 0)
                {
                    // some children are checked, the rest are unchecked
                    tn.StateImageIndex = (int)CheckedState.Mixed;
                }
                else
                {
                    // all children are unchecked
                    if (tn.Checked)
                        tn.StateImageIndex = (int)CheckedState.Mixed;
                    else
                        tn.StateImageIndex = (int)CheckedState.UnChecked;
                }

                if (OrigStateImageIndex != tn.StateImageIndex && tn.Parent != null)
                {
                    // Parent's state has changed, notify the parent's parent
                    UpdateParentState(tn.Parent);
                }
            }

            // <summary>
            // Called on keypress.  Used to change node state when Space key is pressed
            // Invokes OnAfterCheck to do the real work
            // </summary>
            protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
            {
                base.OnKeyDown(e);

                // is the keypress a space?  If not, discard it
                if (e.KeyCode == System.Windows.Forms.Keys.Space)
                {
                    // toggle the node's checked status.  This will then fire OnAfterCheck
                    SelectedNode.Checked = !SelectedNode.Checked;

                    RefreshSettings();
                }
            }

            // <summary>
            // Called when node is clicked by the mouse.  Does nothing unless the image was clicked
            // Invokes OnAfterCheck to do the real work
            // </summary>
            protected override void OnNodeMouseClick(System.Windows.Forms.TreeNodeMouseClickEventArgs e)
            {
                base.OnNodeMouseClick(e);

                // is the click on the checkbox?  If not, discard it
                System.Windows.Forms.TreeViewHitTestInfo info = HitTest(e.X, e.Y);
                if (info == null || info.Location != System.Windows.Forms.TreeViewHitTestLocations.StateImage)
                {
                    return;
                }

                // toggle the node's checked status.  This will then fire OnAfterCheck
                System.Windows.Forms.TreeNode tn = e.Node;
                tn.Checked = !tn.Checked;

                RefreshSettings();
            }
        }
    }


}
