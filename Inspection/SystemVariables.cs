using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspection
{
    public class SystemVariables
    {
        public SystemVariables(string token, string server_address)
        {
            this.Token = token;
            this.ServerAddress = server_address;
        }

        public string Token { get; set; }
        public string ServerAddress { get; set; }
    }
}
