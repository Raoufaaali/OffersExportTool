using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using ATI.EncryptionTool.Core;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace OfferExport
{
    /// <summary>
    /// Settings Class. Reads from appSettings.json
    /// </summary>
    public class Settings
    {
        private AppData _appData;

        /// <summary>
        /// constructor. Builds config source 
        /// </summary>
        public Settings()
        {
            VerifyConnectionString();
        }

        public void Reset()
        {
            Write(null);
            VerifyConnectionString();
        }
        private void VerifyConnectionString()
        {
            _appData = Read();
            if (_appData != null)
            {
                return;
            }
            _appData = new AppData();

            Console.WriteLine("WinOasis Offer Export Tool");
            Console.WriteLine("Enter SQL Server Name:");
            Console.Write("> ");
            _appData.Server = Console.ReadLine();

            Console.WriteLine("Database Name (Enter for WinOasis):");
            Console.Write("> ");
            _appData.Name = Console.ReadLine();
            if (string.IsNullOrEmpty(_appData.Name))
            {
                _appData.Name = "winoasis";
            }

            Console.WriteLine("Integrated Security (Y/N)");
            Console.Write("> ");
            _appData.IntegratedSecurity = Console.ReadLine().ToLower().StartsWith("y");

            if (_appData.IntegratedSecurity)
            {
                _appData.Username = string.Empty;
                _appData.Password = string.Empty;
            }
            else
            {
                Console.WriteLine("Username");
                Console.Write("> ");
                _appData.Username = Console.ReadLine();

                Console.WriteLine("Password");
                Console.Write("> ");
                _appData.Password = Console.ReadLine();
            }
            Write(_appData);
        }

        private void Write(AppData appData)
        {
            if (appData == null)
            {
                File.Delete(DataFileName());
                return;
            }
            string data = JsonSerializer.Serialize(appData);
            data = EncryptAse256.EncryptString(data);
            File.WriteAllText(DataFileName(), data);
        }

        private AppData Read()
        {
            try
            {
                string data = File.ReadAllText(DataFileName());
                data = EncryptAse256.DecryptString(data);
                return JsonSerializer.Deserialize<AppData>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private string DataFileName()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            return Path.Combine(directory, "PlayerMaxOfferData.settings");
        }

        /// <summary>
        /// return the configured connection string
        /// </summary>
        /// <returns></returns>
        public string GetOasisConnectionString()
        {
            if (_appData.IntegratedSecurity)
            {
                return $"data source={_appData.Server}; initial catalog={_appData.Name}; Integrated Security=true; Min Pool Size = 10; Max Pool Size = 100; multipleactiveresultsets=True; Application Name= PlayerMax.OfferExportTool";
            }
            else
            {
                return $"data source={_appData.Server}; initial catalog={_appData.Name}; user id={_appData.Username}; password={_appData.Password}; Min Pool Size = 10; Max Pool Size = 100; multipleactiveresultsets=True; Application Name= PlayerMax.OfferExportTool";
            }
        }


    }
}
