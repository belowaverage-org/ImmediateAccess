using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace ImmediateAccess
{
    class Pipe
    {
        public static NamedPipeServerStream RawPipe;
        public static StreamWriter Writer;
        private static ConsoleColor cc = ConsoleColor.White;
        /// <summary>
        /// This method Starts the Pipe.
        /// </summary>
        public static void Setup()
        {
            Task.Run(() => {
                RawPipe = new NamedPipeServerStream("ImmediateAccess", PipeDirection.Out, 1, PipeTransmissionMode.Byte);
                Writer = new StreamWriter(RawPipe);
                Logger.Info("Pipe: Ready to connect.");
                RawPipe.WaitForConnection();
                AliveTest();
                Logger.Info("Pipe: Client connected.");
            });
        }
        /// <summary>
        /// This method re-starts the Pipe.
        /// </summary>
        public static void Reset()
        {
            RawPipe.Disconnect();
            RawPipe.Dispose();
            Setup();
        }
        /// <summary>
        /// This method will start a loop in a new thread that periodically sends data to the Pipe to check if the Pipe is still functional.
        /// If therer is an Exception, the method will re-start the Pipe.
        /// </summary>
        public static void AliveTest()
        {
            Task.Run(() => {
                try
                {
                    while (RawPipe != null && RawPipe.IsConnected)
                    {
                        RawPipe.Write(new byte[1], 0, 0);
                        RawPipe.Flush();
                        Thread.Sleep(1000);
                    }
                }
                catch (IOException)
                {
                    Reset();
                }
            });
        }
        /// <summary>
        /// This method will write text to the Pipe stream.
        /// </summary>
        /// <param name="Text">String: Text to send.</param>
        public static void Write(string Text)
        {
            try
            {
                if (RawPipe != null && RawPipe.IsConnected)
                {
                    Writer.Write(Text);
                    Writer.Flush();
                }
            }
            catch (IOException)
            {
                Reset();
            }
        }
        /// <summary>
        /// This method will send a line of text to the Pipe client.
        /// </summary>
        /// <param name="Text">String: The line of text to send.</param>
        public static void WriteLine(string Text)
        {
            Write(Text + "\r\n");
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
    }
}