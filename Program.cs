using System;

namespace arma3_server_starter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Arma3 Server Launcher");

            // init runner
            var runner = new Runner();
            runner.StartServer();
        }
    }
}
