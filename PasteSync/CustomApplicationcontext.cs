using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PasteSync
{
    class CustomApplicationcontext : ApplicationContext
    {
        private System.ComponentModel.IContainer components = null;
        private NotifyIcon notifyIcon = null;

        public CustomApplicationcontext() {
            InitializeContext();            
        }

        private void InitializeContext()
        {
            components = new System.ComponentModel.Container();
            notifyIcon = new NotifyIcon(components)
            {
                ContextMenu = new ContextMenu(),
                Visible = true
            };
            notifyIcon.Icon = Properties.Resources.PasteSyncIcon;

            //ContextMenuStrip contextMenu = new ContextMenuStrip();
            MenuItem printer = new MenuItem("Print", Print_Click);
            MenuItem preferences = new MenuItem("Preferences", Preferences_Click);
            preferences.Enabled = false;
            MenuItem quit = new MenuItem("Quit", Quit_Click);

            notifyIcon.ContextMenu.MenuItems.Add(printer);
            notifyIcon.ContextMenu.MenuItems.Add(preferences);
            notifyIcon.ContextMenu.MenuItems.Add(quit);

            //notifyIcon.ContextMenu.Popup += ContextMenu_Popup;
        }

        void ContextMenu_Popup(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Print_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Print");
        }

        private void Preferences_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
