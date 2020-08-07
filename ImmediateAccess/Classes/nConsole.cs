using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ImmediateAccess
{
    public static class nConsole
    {
        private static TcpListener server;
        private static List<nConsoleConnection> clients = new List<nConsoleConnection>();
        private static string buffer = "";
        private static Queue<string> logs = new Queue<string>();
        private static ConsoleColor cc = ConsoleColor.White;
        private static int logLineLimit = 10000;
        /// <summary>
        /// This method sets up the net console.
        /// </summary>
        public static void Setup()
        {
            Logger.Info("nConsole: Creating net console...");
            try
            {
                server = new TcpListener(IPAddress.Parse("127.0.0.1"), 7362);
                server.Start();
                _ = ConnectLoop();
            }
            catch (IOException e)
            {
                Logger.Warning("nConsole: Failed to create net console: " + e.Message);
            }
        }
        /// <summary>
        /// This method will write text to the Pipe stream.
        /// </summary>
        /// <param name="Text">String: Text to send.</param>
        public static void Write(string Text)
        {
            buffer = buffer + Text;
        }
        /// <summary>
        /// This method will send a line of text to the Pipe client.
        /// </summary>
        /// <param name="Text">String: The line of text to send.</param>
        public static void WriteLine(string Text)
        {
            logs.Enqueue(Text);
            if (logs.Count > logLineLimit)
            {
                logs.Dequeue();
            }
            SendAll(Text + "\r\n").Wait();
        }
        /// <summary>
        /// This method will set the color back to white. (Used for compatibility with Console methods).
        /// </summary>
        public static void ResetColor()
        {
            ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// This getter / setter will send the color that should be set to the Pipe client.
        /// </summary>
        public static ConsoleColor ForegroundColor
        {
            get
            {
                return cc;
            }
            set
            {
                cc = value;
                Write("<#" + value.ToString() + "#>");
            }
        }
        private static async Task SendAll(string Text)
        {
            foreach (nConsoleConnection client in clients)
            {
                await client.SendText(Text);
            }
        }
        private static async Task SendQueue(this nConsoleConnection client)
        {
            foreach(string log in logs)
            {
                await client.SendText(log + "\r\n");
            }
        }
        private static async Task SendText(this nConsoleConnection client, string Text)
        {
            await client.StreamWriter.WriteAsync(Text);
            await client.StreamWriter.FlushAsync();
        }
        private static Task ConnectLoop()
        {
            return Task.Run(async () => {
                while (true)
                {
                    TcpClient tcpClient = server.AcceptTcpClient();
                    StreamWriter sw = new StreamWriter(tcpClient.GetStream());
                    nConsoleConnection client = new nConsoleConnection()
                    {
                        TcpClient = tcpClient,
                        StreamWriter = sw
                    };
                    await client.SendQueue();
                    clients.Add(client);
                }
            });
        }
    }
    class nConsoleConnection
    {
        public TcpClient TcpClient;
        public StreamWriter StreamWriter;
    }
}