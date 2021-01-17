using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

using Communication;
using Communication.MessageClasses;

namespace Communication
{
    public class UdpCommunicator
    {

        private UdpClient _myUdpClient;
        private Thread _receiveThread;
        private bool _started;
        private static readonly object StartStopLock = new object();
        private readonly ConcurrentQueue<Envelope> _incomingEnvelopes = new ConcurrentQueue<Envelope>();
        private readonly AutoResetEvent _waitHandle = new AutoResetEvent(false);

        public int MinPort { get; set; } = 1024;
        public int MaxPort { get; set; } = 49151;
        public int Timeout { get; set; }
        public int Port => ((IPEndPoint)_myUdpClient?.Client.LocalEndPoint)?.Port ?? 0;
        public static IPEndPoint ListenEndPoint { get; set; }

        public void Start()
        {
            ValidPorts();
            lock (StartStopLock)
            {
                if (_started) Stop();
                var portToTry = FindAvailablePort(MinPort, MaxPort);
                if (portToTry > 0)
                {
                    try
                    {
                        var localEp = new IPEndPoint(IPAddress.Any, portToTry);
                        _myUdpClient = new UdpClient(localEp);
                        _started = true;
                    }
                    catch (SocketException)
                    {
                    }
                }

                if (!_started)
                    throw new ApplicationException($"Cannot bind the socket to a port {portToTry}");
                else
                {
                    _receiveThread = new Thread(Run);
                    _receiveThread.Start();
                }
            }
        }


        public void Stop()
        {
            lock (StartStopLock)
            {
                _started = false;
                _receiveThread?.Join(Timeout * 2);
                _receiveThread = null;

                if (_myUdpClient != null)
                {
                    _myUdpClient.Close();
                    _myUdpClient = null;
                }
            }
        }


        public bool Send(Envelope outgoingEnvelope)
        {
            bool success = false;

            Console.WriteLine("Sending with UDP");
            if (outgoingEnvelope == null)
            {
                Console.WriteLine("Not Implemented 1");

            }
            else if (!outgoingEnvelope.IsValidToSend)
            {
                Console.WriteLine("Not Implemented 2");
            }
            else
            {
                try
                {
                    Tuple<byte[], int> MessageData = outgoingEnvelope.Encode();
                    _myUdpClient.Send(MessageData.Item1, MessageData.Item2, outgoingEnvelope.EndPoint);
                    Console.WriteLine("---> (" + outgoingEnvelope.EndPoint.ToString() + ") : " + outgoingEnvelope.Message.JSON());
                    success = true;
                }
                catch (Exception err)
                {
                    Console.WriteLine("Could not send message");
                    Console.WriteLine(err.ToString());
                    success = false;
                }
            }

            return true;
        }

        //Receives message and packages it into an envelope
        private Envelope InternalReceive()
        {
            try
            {
                Envelope MyEnvelope = new Envelope();

                IPEndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);
                var Message = _myUdpClient.Receive(ref remoteEp);

                MyEnvelope.Decode(Message);
                //MyEnvelope.EndPoint = (IPEndPoint)_myUdpClient.Client.RemoteEndPoint;
                MyEnvelope.EndPoint = remoteEp;
                return MyEnvelope;
            }
            catch (Exception err)
            {
                return null;
            }
        }

        // Public function that returns the next envelope or times out
        public Envelope Receive(int timeout)
        {
            Envelope env = null;
            var startTime = DateTime.Now;
            while (env == null && DateTime.Now.Subtract(startTime).TotalMilliseconds < timeout)
            {
                if (_incomingEnvelopes.IsEmpty)
                    _waitHandle.WaitOne(timeout);

                if (!_incomingEnvelopes.TryDequeue(out env))
                    env = null;
            }
            if (env != null)
            {
                Console.WriteLine("<--- (" + env.EndPoint.ToString() + ") : " + env?.Message?.JSON());
            }
            return env;
        }

        private int FindAvailablePort(int minPort, int maxPort)
        {
            var availablePort = -1;
            for (var possiblePort = minPort; possiblePort <= maxPort; possiblePort++)
            {
                if (!IsUsed(possiblePort))
                {
                    availablePort = possiblePort;
                    break;
                }
            }
            return availablePort;
        }

        private static bool IsUsed(int port)
        {
            var properties = IPGlobalProperties.GetIPGlobalProperties();
            var endPoints = properties.GetActiveUdpListeners();
            return endPoints.Any(ep => ep.Port == port);
        }

        private void ValidPorts()
        {
            if ((MinPort != 0 && (MinPort < IPEndPoint.MinPort || MinPort > IPEndPoint.MaxPort)) ||
                (MaxPort != 0 && (MaxPort < IPEndPoint.MinPort || MaxPort > IPEndPoint.MaxPort)))
                throw new ApplicationException("Invalid port specifications");
        }

        private void Run()
        {
            while (_started)
            {
                var env = InternalReceive();
                if (env != null)
                {
                    _incomingEnvelopes.Enqueue(env);
                    _waitHandle.Set();
                }
            }
        }
    }
}