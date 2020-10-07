using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Sitecore.XConnect.Schema;
using Sitecore.Xdb.Common.Web;
using Sitecore.XConnect.Client.WebApi;
using Sitecore.DataExchange.Sfmc.Model;
using SampleXConnectCustomModel;
using SfmcCustomModel;
using Sitecore.DataExchange.Sfmc.Custom.Activities.Models;

namespace Sitecore.DataExchange.Sfmc.Custom.Activities.Controllers
{
    public class SfmcController : ApiController
    {
        [HttpPost]
        [ActionName("Execute")]
        public async Task<IHttpActionResult> Execute(ExecuteDto data)
        {
            SfmcInfo sfmcInfo = new SfmcInfo();
            sfmcInfo.PlanId = data.journeyId.ToString();
            var arg = data.inArguments.FirstOrDefault();
            sfmcInfo.PlanState = arg.planState;

            //var status = await UpdateXconnectContactAsync(data.keyValue, sfmcInfo);

            return Ok("success");
        }

        //[HttpPost]
        //[ActionName("Execute")]
        //public async Task<IHttpActionResult> ExecuteJwt(Object data1)
        //{
        //    ExecuteDto data = data1 as ExecuteDto;
        //    SfmcInfo sfmcInfo = new SfmcInfo();
        //    sfmcInfo.PlanId = data.journeyId.ToString();
        //    var arg = data.inArguments.FirstOrDefault();
        //    sfmcInfo.PlanState = arg.planState;

        //    //var status = await UpdateXconnectContactAsync(data.keyValue, sfmcInfo);

        //    return Ok("success");
        //}

        [HttpPost]
        [ActionName("Publish")]
        public async Task<IHttpActionResult> Publish(Object data)
        {

            return Ok("success");
        }

        private async Task<string> UpdateXconnectContactAsync(string id, SfmcInfo sfmcInfo)
        {
            var xconnectServerPath = "10sfmc_xconnect";
            // Valid certificate thumbprints must be passed in
            CertificateHttpClientHandlerModifierOptions options =
            CertificateHttpClientHandlerModifierOptions.Parse("StoreName=My;StoreLocation=LocalMachine;FindType=FindByThumbprint;FindValue=07218C193380E43E6EE46516BD09E62CE219794B");

            // Optional timeout modifier
            var certificateModifier = new CertificateHttpClientHandlerModifier(options);

            List<IHttpClientModifier> clientModifiers = new List<IHttpClientModifier>();
            var timeoutClientModifier = new TimeoutHttpClientModifier(new TimeSpan(0, 0, 20));
            clientModifiers.Add(timeoutClientModifier);

            // This overload takes three client end points - collection, search, and configuration
            var collectionClient = new CollectionWebApiClient(new Uri($"https://{xconnectServerPath}/odata"), clientModifiers, new[] { certificateModifier });
            var searchClient = new SearchWebApiClient(new Uri($"https://{xconnectServerPath}/odata"), clientModifiers, new[] { certificateModifier });
            var configurationClient = new ConfigurationWebApiClient(new Uri($"https://{xconnectServerPath}/configuration"), clientModifiers, new[] { certificateModifier });

            var cfg = new XConnectClientConfiguration(
                new XdbRuntimeModel(SfmcInfoListModel.Model), collectionClient, searchClient, configurationClient);

            try
            {
                await cfg.InitializeAsync();

            }
            catch (XdbModelConflictException ce)
            {
                Console.WriteLine("ERROR:" + ce.Message);
                return null;
            }


            //var id = "eeb64cd7-48d4-0000-0000-05f30b74a775";
            using (var client = new XConnectClient(cfg))
            {
                try
                {
                    var reference = new ContactReference(Guid.Parse(id));

                    Contact existingContact = await client.GetAsync(reference, new ContactExpandOptions(PersonalInformation.DefaultFacetKey, SfmcInfoList.DefaultFacetKey) { });

                    if (existingContact != null)
                    {
                        // Retrieve facet by name
                        var facet = existingContact.GetFacet<SfmcInfoList>(SfmcInfoList.DefaultFacetKey);

                        if (facet == null)
                        {
                            facet = new SfmcInfoList();
                        }

                        var plan = facet.List.SingleOrDefault(f => f.PlanId.Equals(sfmcInfo.PlanId));

                        if (plan != null)
                        {

                            // Update plan data
                            plan.PlanState = sfmcInfo.PlanState;
                            if (plan.PlanState.Equals("exit"))
                            {
                                plan.PlanExitDate = DateTime.Now.ToString();
                            }

                            // Set the updated facet
                            client.SetFacet(existingContact, SfmcInfoList.DefaultFacetKey, facet);
                        }
                        else
                        {
                            // Plan is new, add to the facet list
                            plan = new SfmcInfo();
                            plan.PlanId = sfmcInfo.PlanId;
                            plan.PlanState = sfmcInfo.PlanState;
                            if (plan.PlanState.Equals("exit"))
                            {
                                plan.PlanExitDate = DateTime.Now.ToString();
                            }

                            facet.List.Add(plan);
                            client.SetFacet(existingContact, SfmcInfoList.DefaultFacetKey, facet);
                        }

                        await client.SubmitAsync();
                        
                    }
                }
                catch (XdbExecutionException ex)
                {
                    // Handle exception
                    return null;
                }

                return "success";
            }
        }
    }

}
