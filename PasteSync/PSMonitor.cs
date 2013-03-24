using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasteSync
{
    class PSMonitor
    {
        private String latestData = null;

        public PSMonitor()
        {
            this.latestData = this.getCurrentData();
        }

        private String getCurrentData()
        {
            return Clipboard.GetText();
        }
    }
}
