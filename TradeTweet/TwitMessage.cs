using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeTweet
{
    class TwitMessage
    {
        internal EventStatus status;
        internal string Message;
        internal Image Image;
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
