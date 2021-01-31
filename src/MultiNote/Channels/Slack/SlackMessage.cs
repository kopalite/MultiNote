using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MultiNote.Channels.Slack
{
    public class SlackMessage
    {
        public string Text { get; set; }
        
        public string AsJson()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() },
                NullValueHandling = NullValueHandling.Ignore
            });
        }

    }
}