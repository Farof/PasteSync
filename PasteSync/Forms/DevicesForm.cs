using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasteSync
{
    public partial class DevicesForm : Form
    {
        public LocalDevice Localhost { get; set; }
        
        public DevicesForm()
        {
            InitializeComponent();
            devicesGrid.AutoGenerateColumns = false;
        }

        public void Reload()
        {
            devicesGrid.DataSource = new BindingSource(new BindingList<Device>(Localhost.Remotes.Values.ToList()), null);
        }

        public void ToggleVisibility()
        {
            if (Visible) Hide();
            else Show(); Focus();
        }

        public void UpdateStatus() {
            switch (Localhost.LocalStatus) {
                case LocalDeviceStatus.LocalDeviceOfflineStatus:
                    statusImage.Image = Properties.Resources.status_offline;
                    statusMenu.SelectedIndex = 0;
                    break;
                case LocalDeviceStatus.LocalDeviceInvisibleStatus:
                    statusImage.Image = Properties.Resources.status_invisible;
                    statusMenu.SelectedIndex = 1;
                    break;
                case LocalDeviceStatus.LocalDeviceVisibleStatus:
                    statusImage.Image = Properties.Resources.status_visible;
                    statusMenu.SelectedIndex = 2;
                    break;
                case LocalDeviceStatus.LocalDeviceOnlineStatus:
                    statusImage.Image = Properties.Resources.status_online;
                    statusMenu.SelectedIndex = 3;
                    break;
            }
        }

        private void Devices_Shown(object sender, EventArgs e)
        {
            Focus();
        }

        private void Devices_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void localhostName_SizeChanged(object sender, EventArgs e) {
            statusMenu.Left = localhostName.Right + 6;
        }

        private void statusMenu_SelectionChangeCommitted(object sender, EventArgs e) {
            Localhost.LocalStatus = (LocalDeviceStatus) ((ComboBox) sender).SelectedIndex;
        }
    }
}
