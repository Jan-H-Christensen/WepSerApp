using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WepSerApp.Model
{
    class ServerCommunication :Bindable
    {
        private TcpClient socket = null;
        private NetworkStream ns = null;
        private StreamReader reader = null;
        private string recieveMessage;

        public string RecieveMessage
        {
            get { return recieveMessage; }
            set { recieveMessage = value;}
        }

        private string input;

        public string InputMassage
        {
            get { return input; }
            set { input = value;
                PropertyIsChanged();
            }
        }

        private string outputText;

        public string OutputText
        {
            get { return outputText; }
            set { outputText = value;
                PropertyIsChanged();
            }
        }

        private byte[] sendMessage;

        public byte[] SendMessage
        {
            get { return sendMessage; }
            set {sendMessage = value;}
        }

        public void Connect(MyServerList myServer)
        {
            string bufferText = "Connected to server, the stream is ready\n";
            socket = new TcpClient(myServer.Servername, myServer.Port);

            // (2) Get the streams
            ns = socket.GetStream();

            Console.WriteLine("Connected to server, the stream is ready");

            reader = new StreamReader(ns, Encoding.UTF8);
            RecieveMessage = reader.ReadLine();
            bufferText += RecieveMessage + "\n";
            Console.WriteLine(RecieveMessage);

            
            SendMessage = Encoding.UTF8.GetBytes("authinfo user " + myServer.Username + "\n"); // remember to end the line with newLine!!!!
            ns.Write(SendMessage, 0, SendMessage.Length);

            reader = new StreamReader(ns, Encoding.UTF8);
            RecieveMessage = reader.ReadLine();
            bufferText += RecieveMessage + "\n";
            Console.WriteLine(RecieveMessage);

            SendMessage = Encoding.UTF8.GetBytes("authinfo pass " + myServer.Password + "\n"); // remember to end the line with newLine!!!!
            ns.Write(SendMessage, 0, SendMessage.Length);

            reader = new StreamReader(ns, Encoding.UTF8);
            RecieveMessage = reader.ReadLine();
            bufferText += RecieveMessage + "\n";
            OutputText = bufferText;
        }

        public void SendTextToServer(MyOverview overview)
        {
            string bufferText = OutputText;
            //OutputText = "";
            InputMassage = overview.NamesInList;
            Console.WriteLine(InputMassage);
            SendMessage = Encoding.UTF8.GetBytes(InputMassage + "\n"); // remember to end the line with newLine!!!!
            ns.Write(SendMessage, 0, SendMessage.Length);

            reader = new StreamReader(ns, Encoding.UTF8);
            RecieveMessage = reader.ReadLine();
            bufferText += RecieveMessage + "\n";
            if (RecieveMessage.StartsWith("100") || RecieveMessage.StartsWith("215")
                || RecieveMessage.StartsWith("220") || RecieveMessage.StartsWith("230")
                || RecieveMessage.StartsWith("222") || RecieveMessage.StartsWith("221")
                || RecieveMessage.Contains("211 Article list follows") || RecieveMessage.StartsWith("282")
                || RecieveMessage.StartsWith("224"))
            {
                while (true)
                {
                    if ((RecieveMessage = reader.ReadLine()) == ".")
                    {
                        break;
                    }
                    bufferText += RecieveMessage + "\n";
                    Console.WriteLine(RecieveMessage);
                }
            }
            else
            {
                if (RecieveMessage.StartsWith("205"))
                {
                    Console.WriteLine(RecieveMessage);
                }
                else
                {
                    Console.WriteLine(RecieveMessage);
                }
            }
            OutputText = bufferText;
        }

        public List<MyOverview> GetServerGroups()
        {
            //string bufferText = "";
            List<MyOverview> list = new List<MyOverview>();
            //OutputText = "";
            SendMessage = Encoding.UTF8.GetBytes("list\n"); // remember to end the line with newLine!!!!
            ns.Write(SendMessage, 0, SendMessage.Length);

            reader = new StreamReader(ns, Encoding.UTF8);
            RecieveMessage = reader.ReadLine();
            while (true)
            {
                if ((RecieveMessage = reader.ReadLine()) == ".")
                {
                    break;
                }
                string[] newsGroupsList = Regex.Split(RecieveMessage, @"\s+", RegexOptions.IgnorePatternWhitespace);
                list.Add(new MyOverview { NamesInList = "group "+newsGroupsList[0]});

                Console.WriteLine(RecieveMessage);
            }
            //OutputText = bufferText;
            return list;
        }
        
        public List<MyOverview> GetServerCommands()
        {
            //string bufferText = "";
            List<MyOverview> list = new List<MyOverview>();
            //OutputText = "";
            SendMessage = Encoding.UTF8.GetBytes("help\n"); // remember to end the line with newLine!!!!
            ns.Write(SendMessage, 0, SendMessage.Length);

            reader = new StreamReader(ns, Encoding.UTF8);
            RecieveMessage = reader.ReadLine();
            //bufferText += RecieveMessage + "\n";
            while (true)
            {
                if ((RecieveMessage = reader.ReadLine()) == ".")
                {
                    break;
                }
                var cleanword = Regex.Replace(RecieveMessage.Split()[2], @"[^0-9a-zA-Z\ ]+", "");
                list.Add(new MyOverview { NamesInList = cleanword});
                Console.WriteLine(RecieveMessage);
            }
            //OutputText = bufferText;
            return list;
        }

        public void Clean()
        {
            OutputText = "";
        }
    }
}
