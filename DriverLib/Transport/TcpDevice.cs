//using System;
//using System.Collections.Generic;
//using System.Net.Sockets;
//using System.Text;

//namespace DriverLib
//{

//    //If you plan to reimplement this properly, try using a framing library to make it better and easier, for now http should be fine.


//    internal class TcpDevice : IDeviceTransport
//    {
//        public bool IsConnected => _tcpClient.Connected;

//        private SemaphoreSlim _writeLock = new SemaphoreSlim(1);
//        private SemaphoreSlim _readLock = new SemaphoreSlim(1);
//        private byte[] readBuffer;
//        private TcpClient _tcpClient;
//        private string _targetIp;
//        private int _port;


//        public TcpDevice(string targetIp, int port, int readBufferSize = 1024)
//        {
//            _targetIp = targetIp;
//            _port = port;
//            _tcpClient = new TcpClient();
//            _tcpClient.ConnectAsync(_targetIp, _port);
//            readBuffer = new byte[readBufferSize];
//        }

//        public void TransferToDevice(byte[] data)
//        {
//            //TODO: use readonlyspan to transfer data here, don't allocate a new buffer every transfer
//            _writeLock.Wait();
//            GetDeviceStream().BeginWrite(data, 0, data.Length, (IAsyncResult e) => 
//            { 
//                _writeLock.Release();
//                Console.WriteLine($"Transferred data"); 
//            }, null);
//        }

//        public Task<byte[]> GetRecievedData(int )
//        {
//            _readLock.Wait();
//            var stream = GetDeviceStream();
//            int ind = 0;
//            while (stream.DataAvailable)
//            {
//                stream.Read(readBuffer, ind, 1);
//            }
//            stream.Read 
//        }

//        private NetworkStream GetDeviceStream()
//        {
//            return _tcpClient.GetStream();
//        }

//    }
//}
