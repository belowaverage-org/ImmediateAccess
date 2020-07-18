using System;
using System.IO;
using System.IO.Pipes;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Threading.Tasks;

namespace ImmediateAccess
{
    class Pipe
    {
        //public static NamedPipeServerStream RawPipe;
        private static StreamWriter Writer;
        private static MemoryMappedFile mConsole;
        private static MemoryMappedViewStream mConsoleStream;
        private static ConsoleColor cc = ConsoleColor.White;
        /// <summary>
        /// This method sets up the virtual memory console.
        /// </summary>
        public static void Setup()
        {
            Logger.Info("mConsole: Creating memory console...");
            try
            {
                mConsole = MemoryMappedFile.CreateOrOpen("ImmediateAccessConsole", 5 * 1024 * 1024);  //Create 5MB Memory File.
                mConsoleStream = mConsole.CreateViewStream();
                Writer = new StreamWriter(mConsoleStream);
            }
            catch (IOException e)
            {
                Logger.Warning("mConsole: Failed to create memory console: " + e.Message);
            }
        }
        /// <summary>
        /// This method re-starts the Pipe.
        /// </summary>
        /*public static void Reset()
        {
            RawPipe.Disconnect();
            RawPipe.Dispose();
            Setup();
        }*/
        /// <summary>
        /// This method will start a loop in a new thread that periodically sends data to the Pipe to check if the Pipe is still functional.
        /// If therer is an Exception, the method will re-start the Pipe.
        /// </summary>
        /*public static void AliveTest()
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
        }*/
        /// <summary>
        /// This method will write text to the Pipe stream.
        /// </summary>
        /// <param name="Text">String: Text to send.</param>
        public static void Write(string Text)
        {
            if (Writer == null) return;
            try
            {
                Writer.Write(Text);
                Writer.Flush();
            }
            catch (IOException)
            {
                //Reset();
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