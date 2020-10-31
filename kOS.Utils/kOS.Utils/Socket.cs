using kOS.AddOns;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Suffixed;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using kOS.Safe.Utilities;
using LibNoise.Modifiers;
using Debug = UnityEngine.Debug;

namespace kOS.Utils
{
    [kOSAddon("SOCK")]
    [Safe.Utilities.KOSNomenclature("SOCKAddon")]
    public class SocketAddon : Addon
    {
        public SocketAddon(SharedObjects shared) : base(shared)
        {
            InitializeSuffixes();
        }

        private void InitializeSuffixes()
        {
            AddSuffix("CONNECT", new TwoArgsSuffix<kOSSocket, StringValue, ScalarValue>(CreateSocket));
        }

        private kOSSocket CreateSocket(StringValue ipAddress, ScalarValue port)
        {
            kOSSocket socket = new kOSSocket(ipAddress, (int)port, shared);
            return socket;
        }

        public override BooleanValue Available()
        {
            return true;
        }
    }

    [KOSNomenclature("kOSSocket")]
    public class kOSSocket : Structure
    {
        private Socket socket;
        private byte[] bytes = new byte[1024];
        private QueueValue<StringValue> messageQueue = new QueueValue<StringValue>();
        private StringValue receivedMessage = "";
        private BooleanValue isempty;

        public kOSSocket(string ipAddress, int port, SharedObjects shared)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(ipAddress);
                IPEndPoint endPoint = new IPEndPoint(ip, port);
                socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    socket.Connect(endPoint);
                } catch (Exception e) {
                    Debug.LogError("Could not connect to server");
                    Debug.LogException(e);
                    shared.Window.Print("Could not connect to server");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            InitializeSuffixes();
            Receive();
        }

        private void InitializeSuffixes()
        {
            AddSuffix("SEND", new OneArgsSuffix<StringValue>(SendString));
            //AddSuffix("RECEIVE", new Suffix<StringValue>(ReceiveString));
            AddSuffix("QUEUE", new Suffix<QueueValue<StringValue>>(() => messageQueue));
            AddSuffix("CLOSE", new NoArgsVoidSuffix(EndConnection));
        }
        
        public void SendString(StringValue message)
        {
            byte[] msg = Encoding.UTF8.GetBytes(message);
            socket.Send(msg);
        }

        /*public StringValue ReceiveString()
        {
            int bytesRec = socket.Receive(bytes);
            return Encoding.UTF8.GetString(bytes, 0, bytesRec);
        }*/

        public void Receive()
        {
            socket.BeginReceive(bytes, 0, 1024, 0, new AsyncCallback(ReceiveCallback), null);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            int bytesRead = socket.EndReceive(ar);
            messageQueue.Push(Encoding.UTF8.GetString(bytes, 0, bytesRead));
            bytes = new byte[1024];
            socket.BeginReceive(bytes, 0, 1024, 0, new AsyncCallback(ReceiveCallback), null);
        }

        public void EndConnection()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}