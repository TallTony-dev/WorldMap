using System;
using System.Collections.Generic;
using System.Text;

namespace DriverLib.Transport
{
    internal struct DeviceCommand
    {
        public string CommandName { get; set; }
        public string[] Args {  get; set; }

        public DeviceCommand(string commandName, string[] args)
        { 
            CommandName = commandName;
            Args = args;
        }
    }
}
