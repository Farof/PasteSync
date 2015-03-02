namespace PasteSync
{
    partial class TrayIcon
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrayIcon));
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.quit = new System.Windows.Forms.ToolStripMenuItem();
            this.devices = new System.Windows.Forms.ToolStripMenuItem();
            this.pastesyncIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // trayMenu
            // 
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.devices,
            this.quit});
            this.trayMenu.Name = "contextMenuStrip1";
            this.trayMenu.Size = new System.Drawing.Size(115, 48);
            // 
            // quit
            // 
            this.quit.Name = "quit";
            this.quit.Size = new System.Drawing.Size(114, 22);
            this.quit.Text = "Quit";
            this.quit.Click += new System.EventHandler(this.quit_Click);
            // 
            // devices
            // 
            this.devices.Name = "devices";
            this.devices.Size = new System.Drawing.Size(114, 22);
            this.devices.Text = "Devices";
            // 
            // pastesyncIcon
            // 
            this.pastesyncIcon.ContextMenuStrip = this.trayMenu;
            this.pastesyncIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("pastesyncIcon.Icon")));
            this.pastesyncIcon.Text = "PasteSync";
            this.pastesyncIcon.Visible = true;
            this.trayMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.NotifyIcon pastesyncIcon;
        private System.Windows.Forms.ContextMenuStrip trayMenu;
        private System.Windows.Forms.ToolStripMenuItem quit;
        public System.Windows.Forms.ToolStripMenuItem devices;
    }
}
