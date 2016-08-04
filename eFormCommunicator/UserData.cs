using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFormDll
{
    class UserData
    {
        #region var
        private string request;
        private string api_id;
        private string user_name;
        private string user_uuid;
        private string token;
        private string server_address;
        #endregion

        #region con
        public UserData(string request, string api_id, string user_name, string user_uuid, string token, string server_address)
        {
            this.request = request;
            this.api_id = api_id;
            this.user_name = user_name;
            this.user_uuid = user_uuid;
            this.token = token;
            this.server_address = server_address;
        }
        #endregion

        #region get
        public string Request { get; }
        public string Api_id { get; }
        public string User_name { get; }
        public string User_uuid { get; }
        public string Token { get; }
        public string Server_address { get; }
        #endregion
    }
}
