using System;
using System.Windows.Forms;

namespace PasteSync
{
    class PasteSync : ApplicationContext
    {
        //private PSMonitor psMonitor = null;
        //private MenuItem printer = null;
        private TrayIcon TrayIcon = new TrayIcon();
        private DevicesForm DevicesWindow = new DevicesForm();
        public LocalDevice Localhost = LocalDevice.SharedDevice;

        public PasteSync() {
            InitializeContext();

            DevicesWindow.Reload();
            DevicesWindow.Show();
            Localhost.LocalStatus = (LocalDeviceStatus) Properties.Settings.Default.Status;
        }

        void Application_ApplicationExit(object sender, EventArgs e) {
            Localhost.Destroy();
        }

        private void InitializeContext() {
            // Bind events to tray icon and menu
            TrayIcon.pastesyncIcon.Click += pastesyncIcon_Click;
            TrayIcon.devices.Click += devices_Click;
            // setup device window
            DevicesWindow.Localhost = Localhost;
            Localhost.DevicesWindow = DevicesWindow;
            DevicesWindow.localhostName.Text = Localhost.Name;

            Application.ApplicationExit += Application_ApplicationExit;
        }

        void pastesyncIcon_Click(object sender, EventArgs e) {
            // toggle form only on left click of tray icon
            if (((MouseEventArgs) e).Button == MouseButtons.Left) {
                DevicesWindow.ToggleVisibility();
            }
        }

        void devices_Click(object sender, EventArgs e) {
            DevicesWindow.Show();
            DevicesWindow.Focus();
        }
    }
}
