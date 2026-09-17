using System;
using System.Collections.Generic;
using System.Text;

namespace DriverLib.Transport
{
    internal struct RecievedDeviceData
    {
        public string DataType { get; set; }
        public byte[] Data { get; set; }
    }
}
