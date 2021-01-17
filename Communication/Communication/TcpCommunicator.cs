using Communication.MessageClasses;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

namespace Communication
{
    public class TcpCommunicator
    {

        public int ProcessId { get; set; }

        private TcpListener _myTcpListener;
        private TcpClient _myTcpClient;
        private NetworkStream _myNwStream;
        private Thread _receiveThread;
        private bool _started;
        private int repeatCount;
        private int sleepTime;
        private static readonly object StartStopLock = new object();
        private readonly ConcurrentQueue<Envelope> _incomingEnvelopes = new ConcurrentQueue<Envelope>();
        private readonly AutoResetEvent _waitHandle = new AutoResetEvent(false);

        public int MinPort { get; set; } = 1024;
        public int MaxPort { get; set; } = 49151;
        public int TCPPort { get; set; }
        public int Timeout { get; set; }
        public int Port => ((IPEndPoint)_myTcpClient?.Client.LocalEndPoint)?.Port ?? 0;


        public void Start()
        {
            if (TCPPort == -1)
            {
                return;
            }

            repeatCount = 3;
            sleepTime = 500;
            

            ValidPorts();
            lock (StartStopLock)
            {
                if (_started) Stop();
                var portToTry = FindAvailablePort(MinPort, MaxPort);
                if (portToTry > 0)
                {
                    try
                    {
                        IPEndPoint localEp;
                        if (TCPPort != 0)
                        {
                            localEp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), TCPPort);
                        }
                        else
                        {
                            localEp = new IPEndPoint(IPAddress.Any, portToTry);
                        }
                        _myTcpListener = new TcpListener(localEp);
                        _myTcpListener.Start();

                        _started = true;
                    }
                    catch (SocketException ex)
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
            if (TCPPort == -1) return;
            lock (StartStopLock)
            {
                _started = false;
                _receiveThread?.Join(Timeout * 2);
                _receiveThread = null;

                if (_myTcpClient != null)
                {
                    _myTcpClient.Close();
                    _myTcpClient = null;
                }
            }
        }


        public bool Send(Envelope outgoingEnvelope)
        {
            try
            {
                if (TCPPort == -1) throw new ApplicationException("Can't send on a TCP Communicator that hasn't been started");


                if (_myTcpClient == null || !_myTcpClient.Connected)
                {
                    _myTcpClient = new TcpClient();
                    _myTcpClient.Connect(outgoingEnvelope.EndPoint);
                }
                Tuple<byte[], int> MessageData = outgoingEnvelope.Encode();
                _myNwStream = _myTcpClient.GetStream();


                _myNwStream.Write(MessageData.Item1, 0, MessageData.Item2);
                Thread.Sleep(sleepTime);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception when sending with TCP: " + ex.Message);
                return false;
            }
            
            Console.WriteLine("---> (" + outgoingEnvelope.EndPoint.ToString() + ") : " + outgoingEnvelope.Message.JSON());
            return true;
        }

        public bool Reply(Envelope outgoingEnvelope)
        {
            if (TCPPort == -1) throw new ApplicationException("Can't send on a TCP Communicator that hasn't been started");
            
            Tuple<byte[], int> MessageData = outgoingEnvelope.Encode();

            try
            {
                _myNwStream.Write(MessageData.Item1, 0, MessageData.Item2);
                Thread.Sleep(sleepTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception when sending with TCP: " + ex.Message);
                return false;
            }
            
            Console.WriteLine("---> (" + outgoingEnvelope.EndPoint.ToString() + ") : " + outgoingEnvelope.Message.JSON());
            _myTcpClient.Close();
            _myNwStream = null;
            return true;
        }

        private const int BUFFER_LENGTH = 1024;
        //Receives message and packages it into an envelope
        private Envelope InternalReceive()
        {
            try
            {
                Envelope MyEnvelope = new Envelope();
                if (_myTcpListener.Pending())
                {
                    _myTcpClient = _myTcpListener.AcceptTcpClient();
                    _myNwStream = _myTcpClient.GetStream();
                }
                if (_myTcpClient == null || !_myTcpClient.Connected)
                {
                    return null;
                }

                var buffer = new byte[BUFFER_LENGTH];
                var stayConnected = true;

                while (stayConnected)
                {
                    if (_myNwStream == null) continue;
                    try
                    {
                        var ReadLength = _myNwStream.Read(buffer, 0, buffer.Length);
                        if (ReadLength >= 0) break;
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine(ex.Message);
                        stayConnected = false;
                    }
                    catch (NullReferenceException ex)
                    {
                        continue;
                    }
                }

                IPEndPoint remoteEp = ((IPEndPoint)_myTcpClient.Client.RemoteEndPoint);
                MyEnvelope.Decode(buffer);
                MyEnvelope.EndPoint = remoteEp;

                if(MyEnvelope.Message is PublicKeysResponse)
                {
                    _myTcpClient.Close();
                    _myNwStream = null;
                }
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
                Thread.Sleep(100);
            }
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
    }
}

