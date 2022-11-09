using System;

namespace RobotApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                try
                {
                    Coordinator.Run(args[0]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                } 
            }
            else
            {
                Console.WriteLine("No filename supplied for processing.");
            }

            //Console.WriteLine("Press any key to exit...");
            //Console.ReadKey();
        }
    }
}
