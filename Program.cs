using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class HTTPServer
    {

        public static string StringToString(List<string> list)
        {
            string FinalString = "";
            for (int i = 0; i < list.Count ;i++)
            {
                FinalString += list[i] + "\n";
            }

            return FinalString;
        }
        public static void DoIt(Socket acceptedSocket, List<string> messages)
        {

            Socket connectionSocket = acceptedSocket;

            Console.WriteLine("Server activated");

            Stream ns = new NetworkStream(connectionSocket);
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true; // enable automatic flushing

            messages.Add(sr.ReadLine());
            while (!String.IsNullOrEmpty(messages[messages.Count - 1]))
            {
                Console.WriteLine("Client: " + messages[messages.Count - 1]);
                try
                {
                    sw.WriteLine(StringToString(messages));
                    messages.Add(sr.ReadLine());
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Exception !");
                    break;
                }
            }
            ns.Close();
            connectionSocket.Close();
        }
        public static void Main(string[] args)
        {
            List<string> messages = new List<string>(0);
            messages.Add("Chat server messages: ");
            TcpListener serverSocket = new TcpListener(6789);
            serverSocket.Start();
            Socket acceptedSocket;
            int i = 0;
            while ((acceptedSocket = serverSocket.AcceptSocket()) != null)
            {
                (new Thread(() => DoIt(acceptedSocket,messages))).Start();
                i++;
            }
            serverSocket.Stop();
        }
    }
}
