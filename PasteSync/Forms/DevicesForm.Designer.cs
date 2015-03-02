namespace PasteSync
{
    partial class DevicesForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DevicesForm));
            this.devicesGrid = new System.Windows.Forms.DataGridView();
            this.status = new System.Windows.Forms.DataGridViewImageColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.port = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusMenu = new System.Windows.Forms.ComboBox();
            this.localhostName = new System.Windows.Forms.Label();
            this.statusImage = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.devicesGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusImage)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // devicesGrid
            // 
            this.devicesGrid.AllowUserToAddRows = false;
            this.devicesGrid.AllowUserToDeleteRows = false;
            this.devicesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.devicesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.devicesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.status,
            this.name,
            this.ip,
            this.port});
            this.devicesGrid.Location = new System.Drawing.Point(0, 22);
            this.devicesGrid.Name = "devicesGrid";
            this.devicesGrid.ReadOnly = true;
            this.devicesGrid.Size = new System.Drawing.Size(344, 239);
            this.devicesGrid.TabIndex = 0;
            // 
            // status
            // 
            this.status.DataPropertyName = "StatusImage";
            this.status.Frozen = true;
            this.status.HeaderText = "Status";
            this.status.Image = global::PasteSync.Properties.Resources.status_offline;
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Width = 45;
            // 
            // name
            // 
            this.name.DataPropertyName = "name";
            this.name.FillWeight = 93.26638F;
            this.name.Frozen = true;
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Width = 70;
            // 
            // ip
            // 
            this.ip.DataPropertyName = "ip";
            this.ip.FillWeight = 186.7065F;
            this.ip.HeaderText = "IP";
            this.ip.Name = "ip";
            this.ip.ReadOnly = true;
            this.ip.Width = 141;
            // 
            // port
            // 
            this.port.DataPropertyName = "port";
            this.port.FillWeight = 53.58194F;
            this.port.HeaderText = "Port";
            this.port.Name = "port";
            this.port.ReadOnly = true;
            this.port.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.port.Width = 45;
            // 
            // statusMenu
            // 
            this.statusMenu.FormattingEnabled = true;
            this.statusMenu.Items.AddRange(new object[] {
            "Offline",
            "Invisible",
            "Visible",
            "Online"});
            this.statusMenu.Location = new System.Drawing.Point(89, 3);
            this.statusMenu.Name = "statusMenu";
            this.statusMenu.Size = new System.Drawing.Size(121, 21);
            this.statusMenu.TabIndex = 1;
            this.statusMenu.SelectionChangeCommitted += new System.EventHandler(this.statusMenu_SelectionChangeCommitted);
            // 
            // localhostName
            // 
            this.localhostName.AutoSize = true;
            this.localhostName.Location = new System.Drawing.Point(30, 6);
            this.localhostName.Name = "localhostName";
            this.localhostName.Size = new System.Drawing.Size(53, 13);
            this.localhostName.TabIndex = 2;
            this.localhostName.Text = "hostname";
            this.localhostName.SizeChanged += new System.EventHandler(this.localhostName_SizeChanged);
            // 
            // statusImage
            // 
            this.statusImage.Image = global::PasteSync.Properties.Resources.status_offline;
            this.statusImage.InitialImage = null;
            this.statusImage.Location = new System.Drawing.Point(3, 3);
            this.statusImage.Name = "statusImage";
            this.statusImage.Size = new System.Drawing.Size(21, 21);
            this.statusImage.TabIndex = 3;
            this.statusImage.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.statusMenu);
            this.panel1.Controls.Add(this.statusImage);
            this.panel1.Controls.Add(this.localhostName);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(344, 27);
            this.panel1.TabIndex = 4;
            // 
            // DevicesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 261);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.devicesGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DevicesForm";
            this.Text = "Devices";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Devices_FormClosing);
            this.Shown += new System.EventHandler(this.Devices_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.devicesGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusImage)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView devicesGrid;
        public System.Windows.Forms.ComboBox statusMenu;
        public System.Windows.Forms.Label localhostName;
        public System.Windows.Forms.PictureBox statusImage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewImageColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn ip;
        private System.Windows.Forms.DataGridViewTextBoxColumn port;
    }
}