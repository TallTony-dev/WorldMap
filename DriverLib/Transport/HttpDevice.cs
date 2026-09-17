using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace DriverLib.Transport
{
    internal class HttpDevice : IDeviceTransport
    {
        private HttpClient _httpClient;
        private string _targetIp;

        private const string CommandSendSubUri = "/commands";
        private const string GetCommandSubUri = "/get";

        public HttpDevice(string targetIp, int readBufferSize = 1024)
        {
            _targetIp = targetIp;
            _httpClient = new HttpClient();
            Console.WriteLine($"Http client created with base address {_httpClient.BaseAddress}");
        }


        public async Task TransferToDevice(DeviceCommand command)
        {
            await _httpClient.PostAsJsonAsync(_targetIp + CommandSendSubUri, command);
        }

        public async Task<RecievedDeviceData?> GetFromDevice(DeviceCommand getCommand)
        {
            return await _httpClient.GetFromJsonAsync<RecievedDeviceData>(_targetIp + GetCommandSubUri);
        }
    }
}
