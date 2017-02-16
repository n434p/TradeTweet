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
    enum EventType { OrderOpen, OrderClose, PositionOpen, PositionClose }

    class SettingsPanel: Panel
    {
        Label caption;

        Dictionary<EventType, string> names = new Dictionary<EventType, string>()
        {
            { EventType.OrderOpen , "Order Open"},
            { EventType.OrderClose , "Order Close"},
            { EventType.PositionOpen , "Position Open"},
            { EventType.PositionClose , "Position Close"}
        };

        Dictionary<EventType, CheckBox> eventsList;

        public SettingsPanel()
        {
            eventsList = new Dictionary<EventType, CheckBox>();

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
                eventsList[item] = new CheckBox()
                {
                    Tag = item,
                    Text = names[item],
                    ForeColor = Settings.mainFontColor,
                    Location = new Point(10, indent)
                };

                this.Controls.Add(eventsList[item]);
                indent += 20;
            }
        }

        public bool HasEvents { get { return eventsList.Values.Any(c => c.Checked); } }

        public bool this[EventType type]
        {
            get
            {
                return eventsList[type].Checked;
            }
        }       
    }
}
