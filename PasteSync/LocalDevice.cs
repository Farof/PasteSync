using Bonjour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PasteSync
{
    public enum LocalDeviceStatus {
        LocalDeviceOfflineStatus = 0,
        LocalDeviceInvisibleStatus = 1,
        LocalDeviceVisibleStatus = 2,
        LocalDeviceOnlineStatus = 3
    }

    public class LocalDevice : Device
    {
        private static LocalDevice sharedDevice;
        // Bonjour configuration
        static public String PS_SERVICE_TYPE = "_pastesync._tcp.";
        static public String PS_SERVICE_DOMAIN = "local.";
        static public Boolean PS_SERVICE_IGNORE_SELF = false;

        public DevicesForm DevicesWindow { get; set; }
        public LocalDeviceStatus localStatus = LocalDeviceStatus.LocalDeviceOfflineStatus;
        private int Backlog;
        private DNSSDService Browser { get; set; }
        private Boolean IgnoreSelf = PS_SERVICE_IGNORE_SELF;
        public Dictionary<string, Device> Remotes = new Dictionary<string,Device>();

        public static LocalDevice SharedDevice {
            get {
                if (sharedDevice == null) {
                    sharedDevice = new LocalDevice();
                }

                return sharedDevice;
            }
        }

        public LocalDevice() : base(System.Net.Dns.GetHostName()) {
            // http://www.opensource.apple.com/source/mDNSResponder/mDNSResponder-320.10/Clients/SimpleChat.NET/SimpleChat.cs
            // _

            // socket creation and configuration
            LocalEP = new IPEndPoint(System.Net.IPAddress.Any, 0);
            Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try {
                Sock.Bind(LocalEP);
                Sock.Listen(Backlog);
                Sock.BeginAccept(new AsyncCallback(AcceptCallback), Sock);
            }
            catch (Exception e) {
                Console.WriteLine("Exception: {0} - {1}", e.Source, e.Message);
            }
            LocalEP = (IPEndPoint) Sock.LocalEndPoint;
            Ips.Add(LocalEP.Address.ToString());
            Port = (ushort) LocalEP.Port;
            Console.WriteLine("Opened socket on interface {0} and port {1}", LocalEP.Address, LocalEP.Port);

            EventManager.ServiceRegistered += eventManager_ServiceRegistered;
            EventManager.ServiceFound += eventManager_ServiceFound;
            EventManager.ServiceLost += eventManager_ServiceLost;
            EventManager.OperationFailed += eventManager_OperationFailed;

            if (Properties.Settings.Default.Remotes == null) {
                Properties.Settings.Default.Remotes = new System.Collections.Specialized.StringCollection();
            }
            Console.WriteLine("known remotes: {0} - {1}", Properties.Settings.Default.Remotes.Count, string.Join("; ", Properties.Settings.Default.Remotes.Cast<string>().ToList()));

            foreach (string remote in Properties.Settings.Default.Remotes) {
                Remotes.Add(remote, new Device(remote));
            }
        }

        new public void Destroy() {
            Hide();
            StopScan();
            ClearRemotes();

            base.Destroy();
        }

        public void Show() {
            Service = new DNSSDService();
            Service.Register(0, 0, Name, PS_SERVICE_TYPE, PS_SERVICE_DOMAIN, null, (ushort) LocalEP.Port, null, EventManager);
        }

        public void Hide() {
            Service.Stop();
        }

        public void Scan() {
            Browser = new DNSSDService();
            Browser.Browse(0, 0, PS_SERVICE_TYPE, PS_SERVICE_DOMAIN, EventManager);
        }

        private void ClearRemotes() {
            Device[] ar = Remotes.Values.ToArray();
            for (int i = 0; i < ar.Length; i++) {
                ar[i].Destroy();
            }
            Remotes.Clear();
            DevicesWindow.Reload();
        }

        public void StopScan() {
            Browser.Stop();
            Device[] ar = Remotes.Values.ToArray();
            for (int i = 0; i < ar.Length; i++) {
                ar[i].Disconnect();
            }
        }

        public LocalDeviceStatus LocalStatus {
            get { return localStatus; }
            set {
                switch (value) {
                    case LocalDeviceStatus.LocalDeviceOfflineStatus:
                        if (localStatus == LocalDeviceStatus.LocalDeviceOnlineStatus || localStatus == LocalDeviceStatus.LocalDeviceVisibleStatus) {
                            Hide();
                        }
                        if (localStatus == LocalDeviceStatus.LocalDeviceOnlineStatus || localStatus == LocalDeviceStatus.LocalDeviceInvisibleStatus) {
                            StopScan();
                        }
                        break;
                    case LocalDeviceStatus.LocalDeviceInvisibleStatus:
                        if (localStatus == LocalDeviceStatus.LocalDeviceOnlineStatus || localStatus == LocalDeviceStatus.LocalDeviceVisibleStatus) {
                            Hide();
                        }
                        if (localStatus == LocalDeviceStatus.LocalDeviceOfflineStatus || localStatus == LocalDeviceStatus.LocalDeviceVisibleStatus) {
                            Scan();
                        }
                        break;
                    case LocalDeviceStatus.LocalDeviceVisibleStatus:
                        if (localStatus == LocalDeviceStatus.LocalDeviceOfflineStatus || localStatus == LocalDeviceStatus.LocalDeviceInvisibleStatus) {
                            Show();
                        }
                        if (localStatus == LocalDeviceStatus.LocalDeviceOnlineStatus || localStatus == LocalDeviceStatus.LocalDeviceInvisibleStatus) {
                            StopScan();
                        }
                        break;
                    case LocalDeviceStatus.LocalDeviceOnlineStatus:
                        if (localStatus == LocalDeviceStatus.LocalDeviceOfflineStatus || localStatus == LocalDeviceStatus.LocalDeviceInvisibleStatus) {
                            Show();
                        }
                        if (localStatus == LocalDeviceStatus.LocalDeviceOfflineStatus || localStatus == LocalDeviceStatus.LocalDeviceVisibleStatus) {
                            Scan();
                        }
                        break;
                }
                localStatus = value;
                Properties.Settings.Default.Status = (int) value;
                Properties.Settings.Default.Save();
                DevicesWindow.UpdateStatus();
            }
        }

        private void AcceptCallback(IAsyncResult ar) {
            Console.WriteLine("Accept connection");
            Socket listener = (Socket) ar.AsyncState;
            try {
                Socket handler = listener.EndAccept(ar);
                IPEndPoint remoteEP = (IPEndPoint) handler.RemoteEndPoint;
                Console.WriteLine("accepted connection to {0} on port {1}", remoteEP.Address, remoteEP.Port);
                Sock.BeginAccept(new AsyncCallback(AcceptCallback), Sock);
            }
            catch (Exception e) {
                Console.WriteLine("Exception: {0} - {1}", e.Source, e.Message);
            }
        }

        private void eventManager_OperationFailed(DNSSDService service, DNSSDError error) {
            Console.WriteLine("Service error: {0}", error);
        }

        private void eventManager_ServiceRegistered(DNSSDService service, DNSSDFlags flags, string name, string regtype, string domain) {
            Console.WriteLine("Service published: {0}", name);
        }

        private void eventManager_ServiceFound(DNSSDService browser, DNSSDFlags flags, uint ifIndex, string serviceName, string regtype, string domain) {
            if (!IgnoreSelf || (serviceName != Name)) {
                Console.WriteLine("Service found: {0}", serviceName);
                Device device;

                if (Remotes.ContainsKey(serviceName)) {
                    Remotes.TryGetValue(serviceName, out device);
                    device.IfIndex = ifIndex;
                }
                else {
                    device = new Device(serviceName, ifIndex);
                    Remotes.Add(serviceName, device);
                    if (!Properties.Settings.Default.Remotes.Contains(serviceName)) {
                        Properties.Settings.Default.Remotes.Add(serviceName);
                        Properties.Settings.Default.Save();
                    }
                }

                device.Resolved += device_Resolved;
                device.Status = DeviceStatus.DeviceOnlineStatus;
                device.Resolve();
                DevicesWindow.Reload();
            }
        }

        private void eventManager_ServiceLost(DNSSDService browser, DNSSDFlags flags, uint ifIndex, string serviceName, string regtype, string domain) {
            if (!IgnoreSelf || (serviceName != Name)) {
                Console.WriteLine("Service removed: {0}", serviceName);
                Device device;
                Remotes.TryGetValue(serviceName, out device);
                device.Status = DeviceStatus.DeviceOfflineStatus;
                //Remotes.Remove(serviceName);
                // binding is reset each time, find out how to make it persist
                DevicesWindow.Reload();
            }
        }

        void device_Resolved(object sender) {
            Device device = (Device) sender;
            DevicesWindow.Reload();
            device.Connect();
        }
    }
}
