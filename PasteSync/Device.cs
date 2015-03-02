using Bonjour;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PasteSync
{
    public delegate void DeviceEventHandler(object sender);

    public enum DeviceStatus {
        DeviceOfflineStatus,
        DeviceOnlineStatus,
        DeviceConnectedStatus
    }

    public class Device
    {
        protected DNSSDService Service { get; set; }
        protected DNSSDEventManager EventManager = new DNSSDEventManager();
        public string Name { get; set; }
        public List<string> Ips = new List<string>();
        public ushort Port { get; set; }
        public string Hostname { get; set; }
        public DeviceStatus Status = DeviceStatus.DeviceOfflineStatus;
        public uint IfIndex { get; set; }
        protected Socket Sock { get; set; }
        protected IPEndPoint LocalEP { get; set; }
        private IPEndPoint RemoteEP { get; set; }
        private NetworkStream Stream { get; set; }
        private byte[] ReadBuffer = new byte[2048];
        private StringBuilder ReadString = new StringBuilder();

        public event DeviceEventHandler Resolved;

        public Device(string name) {
            Name = name;
            EventManager.ServiceResolved += eventManager_ServiceResolved;
            EventManager.AddressFound += eventManager_AddressFound;
        }

        public Device(string name, uint ifIndex) : this(name) {
            IfIndex = ifIndex;
        }

        public void Destroy() {
            Console.WriteLine("Destroying device: {0}", Name);
            //if (Stream != null) {
            //    Stream.Dispose();
            //}

            if (Sock != null) {
                if (Sock.Connected) {
                    Sock.Disconnect(false);
                }
                Sock.Close(2);
            }
        }

        public void Resolve() {
            Service = new DNSSDService();
            Service.Resolve(0, IfIndex, Name, LocalDevice.PS_SERVICE_TYPE, LocalDevice.PS_SERVICE_DOMAIN, EventManager);
        }

        public string IP {
            get {
                return string.Join(";", Ips.ToArray());
            }
        }

        public Image StatusImage {
            get {
                switch (Status) {
                    case DeviceStatus.DeviceConnectedStatus:
                        return Properties.Resources.status_online;
                    case DeviceStatus.DeviceOnlineStatus:
                        return Properties.Resources.status_invisible;
                    case DeviceStatus.DeviceOfflineStatus: default:
                        return Properties.Resources.status_offline;
                }
            }
        }

        public void Connect() {
            string[] ar = Ips.ToArray();
            for (int i = 0; i < ar.Length; i++) {
                try {
                    Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    RemoteEP = new IPEndPoint(IPAddress.Parse(ar[i]), Port);
                    Sock.Connect(RemoteEP);

                    if (!Sock.Connected) {
                        Console.WriteLine("connection failed");
                    }
                }
                catch (SocketException e) {
                    Console.WriteLine("Socket exception: {0} - {1}", e.Source, e.Message);
                    continue;
                }
                catch (Exception e) {
                    Console.WriteLine("Exception: {0} - {1}", e.Source, e.Message);
                    continue;
                }

                break;
            }

            if (Sock != null && Sock.Connected) {
                LocalEP = (IPEndPoint) Sock.LocalEndPoint;
                try {
                    Stream = new NetworkStream(Sock);
                    Console.WriteLine("Setup stream on interface {0} port {1} to remote {2} on port {3}", LocalEP.Address, LocalEP.Port, RemoteEP.Address, RemoteEP.Port);
                    Send("Hello world!");
                    Stream.BeginRead(ReadBuffer, 0, ReadBuffer.Length, new AsyncCallback(Receive_Callback), Stream);
                    Status = DeviceStatus.DeviceConnectedStatus;
                }
                catch (ArgumentNullException e) {
                    Console.WriteLine("Argument NULL exception: {0} - {1}", e.Source, e.Message);
                }
                catch (IOException e) {
                    Console.WriteLine("IO exception: {0} - {1}", e.Source, e.Message);
                }
                catch (Exception e) {
                    Console.WriteLine("Exception: {0} - {1}", e.Source, e.Message);
                }
            }
            else {
                Console.WriteLine("Setup failed");
            }
        }

        public void Disconnect() {
            try {
                if (Stream != null) Stream.Close(2);
                if (Sock != null) Sock.Close(2);
            }
            catch (Exception e) {
                Console.WriteLine("Exception: {0} - {1}", e.Source, e.Message);
            }
            Status = DeviceStatus.DeviceOfflineStatus;
        }

        private void Receive_Callback(IAsyncResult ar) {
            Console.WriteLine("read stream");
            try {
                int bytesRead = Stream.EndRead(ar);
                if (bytesRead > 0) {
                    ReadString.Append(Encoding.UTF8.GetString(ReadBuffer, 0, bytesRead));
                    Stream.BeginRead(ReadBuffer, 0, ReadBuffer.Length, new AsyncCallback(Receive_Callback), Stream);
                }
                else {
                    Console.WriteLine("read all");
                    if (ReadString.Length > 1) {
                        Console.WriteLine("handle data: {0}", ReadString.ToString());
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine("Exception:\n{0}\n{1}", e.Source, e.Message);
            }
        }

        private void Send(String str) {
            if (Stream.CanWrite) {
                try {
                    byte[] buffer = Encoding.UTF8.GetBytes(str);
                    Stream.Write(buffer, 0, buffer.Length);
                    Console.WriteLine("Sent: {0}", str);
                }
                catch (Exception e) {
                    Console.WriteLine("Exception:\n{0}\n{1}", e.Source, e.Message);
                }
            }
            else {
                Console.WriteLine("Stream can't write");
            }
        }

        void eventManager_ServiceResolved(DNSSDService service, DNSSDFlags flags, uint ifIndex, string fullname, string hostname, ushort port, TXTRecord record) {
            //Console.WriteLine("Service resolved: {0} - {1} - {2}", fullname, hostname, port);
            Hostname = hostname;
            Port = port;
            service.GetAddrInfo(0, ifIndex, DNSSDAddressFamily.kDNSSDAddressFamily_IPv4, hostname, EventManager);
        }

        void eventManager_AddressFound(DNSSDService service, DNSSDFlags flags, uint ifIndex, string hostname, DNSSDAddressFamily addressFamily, string address, uint ttl) {
            //Console.WriteLine("Adress found: {0} - {1}", this.name, address);
            Ips.Add(address);
            if ((flags & DNSSDFlags.kDNSSDFlagsMoreComing) == 0) {
                Resolved(this);
            }
        }
    }
}
