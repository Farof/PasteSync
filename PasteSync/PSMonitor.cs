using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasteSync
{
    class PSMonitor
    {
        public String latestData = null;
        private Timer timer = new Timer();

        public PSMonitor()
        {
            latestData = this.getCurrentData();

            timer.Interval = 3000;
            timer.Tick += timer_Tick;
            startMonitor();
        }

        private void startMonitor()
        {
            if (!timer.Enabled)
            {
                timer.Start();
            }
        }

        private void stopMonitor()
        {
            if (timer.Enabled)
            {
                timer.Stop();
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            String newData = getCurrentData();
            if (newData != latestData)
            {
                Console.WriteLine("Data changed: " + newData);
                latestData = newData;
            }
        }

        private String getCurrentData()
        {
            return Clipboard.GetText();
        }
    }
}
