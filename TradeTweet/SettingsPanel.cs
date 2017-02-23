using PTLRuntime.NETScript;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TradeTweet
{
    public enum EventType { OrderOpen, OrderClose, PositionOpen, PositionClose }

    class SettingsPanel: Panel
    {
        Label caption;
        Button applyBtn;

        public Action OnApply;

        Dictionary<EventType, string> names = new Dictionary<EventType, string>()
        {
            { EventType.OrderOpen , "Order Open"},
            { EventType.OrderClose , "Order Close"},
            { EventType.PositionOpen , "Position Open"},
            { EventType.PositionClose , "Position Close"}
        };

        Dictionary<EventType, CheckBox> eventsList;
        Dictionary<EventType, bool> set = Settings.Set;

        public SettingsPanel()
        {
            eventsList = new Dictionary<EventType, CheckBox>();

            Width = 150;
            Dock = DockStyle.Left;
            BackColor = Color.DimGray;
            Visible = false;

            BackgroundImageLayout = ImageLayout.Tile;
            BackgroundImage = Properties.Resources.settingsPanelBack;

            caption = new Label()
            {
                Text = "AutoTweet",
                BackColor = Color.Transparent,
                Image = Properties.Resources.settingsPanelBack,
                Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold),
                ForeColor = Settings.mainFontColor,
                Location = new System.Drawing.Point(0,10),
                Height = 20,
                Width = 150,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };

            this.Controls.Add(caption);

            int indent = 40;

            foreach (EventType item in Enum.GetValues(typeof(EventType)))
            {
                set[item] =  (set.Count == Enum.GetValues(typeof(EventType)).Length) ? set[item] : false;

                eventsList[item] = new CheckBox()
                {
                    Tag = item,
                    Checked = set[item],
                    Text = names[item],
                    ForeColor = Settings.mainFontColor,
                    Location = new Point(10, indent)
                };

                eventsList[item].CheckedChanged += SettingsPanel_CheckedChanged;

                this.Controls.Add(eventsList[item]);
                indent += 20;
            }

            applyBtn = new Button()
            {
                FlatStyle = FlatStyle.Flat,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = Settings.mainFont,
                DialogResult = DialogResult.OK,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Text = "Apply",
                Height = Settings.btnHeight,
                Width = 150-2,
                Location = new Point(0,this.Height - Settings.btnHeight),
                BackColor = Color.Gray,
                ForeColor = Color.Black,
                Enabled = false,
            };

            applyBtn.Click += (o, e) =>
            {
                foreach (EventType item in Enum.GetValues(typeof(EventType)))
                {
                    set[item] = eventsList[item].Checked;
                }

                applyBtn.Enabled = false;
                applyBtn.BackColor = Color.Gray;

                if (OnApply != null)
                    OnApply.Invoke();
            };

            this.Controls.Add(applyBtn);
        }

        private void SettingsPanel_CheckedChanged(object sender, EventArgs e)
        {
            bool check = false;

            foreach (EventType type in Enum.GetValues(typeof(EventType)))
            {
                if (eventsList[type].Checked != set[type])
                {
                    check = true;
                    break;
                }
            }

            applyBtn.Enabled = check;
            applyBtn.BackColor = (check) ? Settings.mainFontColor : Color.Gray;
        }

        public void ShowSet()
        {


            foreach (EventType item in Enum.GetValues(typeof(EventType)))
            {
                bool check = (set != null) ? set[item] : false;
                eventsList[item].Checked = check;
            }

            this.Visible = !this.Visible;
        }

        public bool HasEvents { get { return set.Values.Any(v => v); } }

        public bool this[EventType type]
        {
            get
            {
                return set[type];
            }
        }       
    }
}
