using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Nest;
using Newtonsoft.Json;
using AdvertApi.Models.BS.Messages;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace WedbAdvert_SearchWorker
{
    public class SearchWorker
    {
        private readonly IElasticClient _client;

        public SearchWorker() : this(ElasticSearchHelper.GetInstance())
        {
        }

        public SearchWorker(IElasticClient client)
        {
            _client = client;
        }

        public async Task Function(SNSEvent snsEvent, ILambdaContext context)
        {            
            foreach (var record in snsEvent.Records)
            {
                context.Logger.LogLine(record.Sns.Message);

                AdvertConfirmedMessage message = JsonConvert.DeserializeObject<AdvertConfirmedMessage>(record.Sns.Message);
                AdvertType advertDocument = MappingHelper.Map(message);

                IndexResponse response = await _client.IndexDocumentAsync(advertDocument);

                context.Logger.LogLine("Result is " + response.Result.ToString());
                context.Logger.LogLine("Exception: " + response.OriginalException?.Message);
                context.Logger.LogLine("Server error: " + response.ServerError);
            }
        }
    }
}
