using OfferExport.Common;
using System;

namespace OfferExport
{
    class Program
    {
        static void Main(string[] args)
        {
            Util util = new Util();
            OasisOfferProcessor processor = new OasisOfferProcessor(util);
            Settings configs = new Settings();

            if (args == null || args.Length == 0)
            {
                try
                {
                    processor.ProcessOasisOffers();
                }
                catch (Exception e)
                {
                    util.LogColoredMessage($"an error has occurred: {e.Message}", Util.SuccessStatus.FAILURE);
                }
            }
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToLower() == "setup")
                {
                    try
                    {
                        configs.Reset();
                    }
                    catch (Exception x)
                    {
                        util.LogColoredMessage($"Error writing to app settings: {x.Message}", Util.SuccessStatus.FAILURE);
                    }
                }
                else
                {
                    util.LogColoredMessage("Unknown option! For a list of commands run help", Util.SuccessStatus.FAILURE);
                }
            }
            Console.WriteLine("\n \n \nPress any key to exit..");
            Console.ReadKey();
        }
    }
}
