using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeTweet
{
    class TwitMessage
    {
        internal string Message;
        internal NoticeType NoticeType = NoticeType.Info;
        internal EventType EventType = EventType.Empty;
        internal DateTime Time;

        public string FormattedTime
        {
            get
            { 
                return Time.ToLocalTime().ToString("H:mm (dd.MM.yyyy)");
            }
        } 
    }
}
