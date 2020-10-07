using Sitecore.DataExchange.Sfmc.Model;
using Sitecore.XConnect.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XconnectModelSerializer
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = XdbModelWriter.Serialize(SfmcInfoListModel.Model);
            var fileName = SfmcInfoListModel.Model.FullName + ".json";
            //var json = XdbModelWriter.Serialize(CustomerModel.Model);
            File.WriteAllText(fileName, model);
            Console.WriteLine("Completed");
            //Console.Read();
        }
    }
}
