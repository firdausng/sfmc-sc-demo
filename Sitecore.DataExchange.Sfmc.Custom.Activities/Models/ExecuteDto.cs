using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.DataExchange.Sfmc.Custom.Activities.Models
{
    public class ExecuteDto
    {
        public List<InArgument> inArguments { get; set; } = new List<InArgument>();
        public Guid activityObjectID { get; set; }
        public Guid journeyId { get; set; }
        public Guid activityId { get; set; }
        public Guid definitionInstanceId { get; set; }
        public Guid activityInstanceId { get; set; }
        public string keyValue { get; set; }
        public int mode { get; set; }
    }

    public class InArgument
    {
        public string contactKey { get; set; }
        public string planState { get; set; }
        public bool userExit { get; set; }
    }

    public class PublishDto
    {
        public Guid activityObjectID { get; set; }
        public Guid interactionId { get; set; }
        public Guid originalDefinitionId { get; set; }
        public Guid interactionKey { get; set; }
        public int interactionVersion { get; set; }
        public bool isPublished { get; set; }
    }
}