using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Mvc
{
    public enum MessageBoxTip
    {
        Success = 1,
        Failure = 2,
        Exception = 3
    }

    public class MessageBoxModel
    {
        public string No { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public bool IsPopup { get; set; }

        public MessageBoxTip Type { get; set; }

        public string ErrorStackTrace { get; set; }

        public bool IsTop { get; set; }
    }
}
