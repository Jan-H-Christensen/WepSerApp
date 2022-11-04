using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WepSerApp.Model
{
    class MyServerList
    {
        private string servername;

        public string Servername
        {
            get { return servername; }
            set { servername = value; }
        }

        private int port;

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

    }
}
