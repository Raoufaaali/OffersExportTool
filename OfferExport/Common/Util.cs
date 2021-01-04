using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace OfferExport.Common
{
    /// <summary>
    /// Utilty Class. Provides shared functionality. 
    /// </summary>
    class Util
    {

        public Util()
        {

        }

        /// <summary>
        /// Accept a generic list List<T> and fileName and writes the list to a csv file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="filename"></param>
        /// <returns>true if success and false if not</returns>
        public bool WriteListToCsv<T>(List<T> list, string filename)
        {
            bool isSuccess = false;
            try
            {
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fullPath = desktop + "/Offers";
                Directory.CreateDirectory(fullPath);
                using (var writer = new StreamWriter($"{fullPath}/{filename}"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.TypeConverterOptionsCache.GetOptions<DateTime>().Formats = new[] { "MM/dd/yyyy" };
                    csv.Configuration.HasHeaderRecord = true;
                    csv.WriteRecords(list);
                }
                isSuccess = true;

            }
            catch (Exception x)
            {
                LogColoredMessage(x.Message, SuccessStatus.FAILURE);   
            }            
            return isSuccess;
        }

        /// <summary>
        /// Logs a colored message to the console. 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="status"></param>
        public void LogColoredMessage(string message, SuccessStatus status)
        {
            switch (status)
            {
                case SuccessStatus.SUCCESS:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case SuccessStatus.FAILURE:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    break;
            }

            Console.WriteLine(message);
            Console.ResetColor();
        }
        
        public enum SuccessStatus 
        {
            SUCCESS,
            FAILURE
        }

    }
}
