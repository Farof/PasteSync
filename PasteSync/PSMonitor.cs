using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasteSync
{
    public class DataChangedEvent : EventArgs
    {
        public String data = null;

        public DataChangedEvent(String _data)
        {
            data = _data;
        }
    }

    public delegate void DataChangedEventHandler(object sender, DataChangedEvent e);

    class PSMonitor
    {
        public String latestData = null;
        private Timer timer = new Timer();
        private int timerLooptime = 1000;
        public event DataChangedEventHandler DataChanged;

        public PSMonitor()
        {
            latestData = this.getCurrentData();

            timer.Interval = timerLooptime;
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
                latestData = newData;
                DataChanged(this, new DataChangedEvent(newData));
            }
        }

        private String getCurrentData()
        {
            return Clipboard.GetText();
        }
    }
}
