using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    public sealed class RequestHeaderModel
    {
        public string MobileIPAddress { get; set; }
        public string AppVersion { get; set; }
        public string DeviceType { get; set; }
        public string DeviceId { get; set; }
        public string Manufacturer { get; set; }
        public string IMEI { get; set; }
        public string RequestDateTime { get; set; }
        public string AppPackageName { get; set; }
        public string MacAddress { get; set; }
        public string ChannelName { get; set; }
        public string MobileNumber { get; set; }
        public string Locale { get; set; }
        public string Authorization { get; set; }

    }
}
