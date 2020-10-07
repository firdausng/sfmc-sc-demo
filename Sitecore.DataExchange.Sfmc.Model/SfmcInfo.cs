using Sitecore.XConnect;
using Sitecore.XConnect.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.DataExchange.Sfmc.Model
{
    [Serializable]
    [FacetKey(DefaultFacetKey)]
    public class SfmcInfoList : Facet
    {
        public const string DefaultFacetKey = "SfmcFacetInfoList";
        public List<SfmcInfo> List { get; set; } = new List<SfmcInfo>();
    }

    public class SfmcInfo
    {
        public string PlanId { get; set; }
        public string PlanName { get; set; }
        public string PlanState { get; set; }
        public string PlanDateOfEntry { get; set; }
        public string PlanExitDate { get; set; }
    }

    public static class SfmcInfoListModel
    {
        public static XdbModel Model { get; } = CreateModel();

        private static XdbModel CreateModel()
        {
            XdbModelBuilder builder = new XdbModelBuilder("SfmcInfoListModel", new XdbModelVersion(1, 0));

            builder.DefineFacet<Contact, SfmcInfoList>(SfmcInfoList.DefaultFacetKey);

            return builder.BuildModel();

        }

    }
}
