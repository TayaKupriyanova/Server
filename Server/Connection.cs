using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets; // для сокетов

namespace Server
{
    internal class Connection
    {
       public static int port = 1111; // порт для приема входящих запросов

        // получаем адреса для запуска сокета
        public IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

        // создаем сокет
        public Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public Connection() { }

        public void bind()
        {
            listenSocket.Bind(ipPoint);
        }

        public void listen()
        {
            listenSocket.Listen(10);
        }

        public string getFromClient(Socket client)
        {
            StringBuilder builder = new StringBuilder();
            byte[] data = new byte[256]; // буфер для ответа
            int bytes = 0; // количество полученных байт
            do
            {
                bytes = client.Receive(data, data.Length, 0);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (client.Available > 0);
            return builder.ToString();
        }

        public void sendToClient(string msg, Socket client)
        {
            byte[] data;
            data = Encoding.Unicode.GetBytes(msg);
            client.Send(data);
        }

        public void sendKeyToClient(byte[] key, Socket client)
        {
            client.Send(key);
        }

    }
}
